using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Provider;
using System.Data;


namespace C8cx
{
    #region Output objects -C8DriveInfo, Application, WorkSpace, C8Stream
    public class C8DriveInfo : PSDriveInfo
    {
        public C8DriveInfo(PSDriveInfo drive)
            : base(drive)
        {
            Init();
        }
        public C8DriveInfo(string name, ProviderInfo pi, string root, string desc, PSCredential cred)
            : base(name, pi, root, desc, cred)
        {
            Init();
        }
        public Uri ManagerUri { get; set; }
        internal C8Core C8core { get; set; }
        private void Init()
        {
            C8core = new C8Core(this.Root);
            ManagerUri = C8core.ManagerUri;
        }
    }


    public abstract class C8BaseOutputObject
    {
        public string Name { get; set; }
    }

    public class Application : C8BaseOutputObject
    {
    }


    public class WorkSpace : C8BaseOutputObject
    {
    }
    public class C8Stream : C8BaseOutputObject
    {
    }
    #endregion

    [CmdletProvider("Coral8", ProviderCapabilities.None)]
    public class Coral8Provider : NavigationCmdletProvider
    {
        public C8DriveInfo C8Drive
        {
            get { return PSDriveInfo as C8DriveInfo; }
        }
        public C8Core C8core
        {
            get { return C8Drive.C8core; }
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            var c8drive = new C8DriveInfo(drive);
            return c8drive;
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            C8DriveInfo newC8DriveInfo;
            try
            {
                newC8DriveInfo = new C8DriveInfo("C8Local", this.ProviderInfo, "ccl://localhost:6789", "", null);
            }
            catch (Exception)
            {
                return null;
            }
            Collection<PSDriveInfo> drives = new Collection<PSDriveInfo>() { newC8DriveInfo };
            return drives;
        }

        protected override bool IsValidPath( string path )
        {
            if ( String.IsNullOrEmpty( path ) )
            {
                return false;
            }

             string pathSeparator = @"\/";
             char[] pathSeparatorToCharArray = pathSeparator.ToCharArray();
             path = path.TrimEnd( pathSeparatorToCharArray );
             path = path.TrimStart( pathSeparatorToCharArray );
             string[] pathChunks = path.Split( pathSeparatorToCharArray );
             foreach ( string pathChunk in pathChunks )
             { 
                 if ( pathChunk.Length == 0 )
                 {
                     return false;
                 }
             }

            return true;

        } 

        protected override bool ItemExists(string path)
        {
            return true;
        } 

        protected override void GetItem( string path )
        {
            foreach (string wss in C8Drive.C8core.GetWorkspaces())
            {
                WriteItemObject(new WorkSpace() { Name = wss }, path, true);

            }
        }


        PSObject Extracted(C8Tuple tuple)
        {
            PSObject pso = new PSObject(tuple);
            for (int ii = 0; ii < tuple.columnNames.Length; ii++)
            {
                string name = tuple.columnNames[ii];
                string getter = String.Format("$this[\"{0}\"]", name);
                string setter = String.Format("$this[\"{0}\"] = $value", name);
                pso.Properties.Add(new PSScriptProperty(name, this.InvokeCommand.NewScriptBlock(getter), this.InvokeCommand.NewScriptBlock(setter)));
            }
            return pso;
        }
        protected override void GetChildItems(string path, bool recurse)
        {
            var relPath = path.Substring(C8Drive.Root.Length + 1);
            var parts = relPath.Split(@"\/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            switch (parts.Length)
            {
                case 0:
                    IEnumerable<string> workspaces = C8core.GetWorkspaces();
                    workspaces.ForEach(wss => WriteItemObject(new WorkSpace() { Name = wss }, path, true));
                    if (recurse)
                    {
                        workspaces.ForEach(wss => GetChildItems(Path.Combine(path, wss), recurse));
                    }
                    break;
                case 1:
                    var wsstatus = C8core.GetWorkspaceStatus(parts[0], true);
                    var apps = C8core.GetApplications(parts[0]);
                    apps.ForEach(app => WriteItemObject(new Application() { Name = app }, Path.Combine(path, app), true));
                    if (recurse)
                    {
                        apps.ForEach(app => GetChildItems(Path.Combine(path, app), recurse));
                    }
                    break;
                case 2:
                    string workspace = parts[0];
                    string program = parts[1];
                    var urib = new UriBuilder(path.Replace(@"\", "/"));
                    urib.Path = string.Format("Status/Workspace/{0}", workspace);
                    IEnumerable<C8Tuple> res = C8core.OpenStream(urib.ToString(),true);
                    GetStreamNames(res, workspace, program).ForEach(streamname => WriteItemObject(new C8Stream() { Name = streamname }, Path.Combine(path, streamname), true));
                    break;
                case 3:
                    urib = new UriBuilder(path.Replace(@"\", "/"));
                    urib.Path = "/Stream" + urib.Path;
                    string streamurl = urib.ToString();
                    IEnumerable<C8Tuple> resStream = C8core.OpenStream(streamurl);
                    resStream.ForEach(pso => WriteItemObject(Extracted(pso), path, false));
                    break;
                default:

                    throw new Exception("not yet");

            }
        }

        // open stream 
        // http://localhost:6789/status/stream/{workspace}
        // loop till 
        //                    MessageGroup,MessageName
        //                   CclApplicationInfo,RunningTime
        // or 
        // ObjectID,ObjectID2,MessageGroup
        //ccl://sw-V:6789/Stream/Default/Treadstone/DO_ByCustomer/outAugmentedDO,ccl://sw-V:6789/Stream/Default/Treadstone/outCustomerMovementAlert,CclStreamPairInfo
        // item count > 1

        private IEnumerable<string> GetStreamNames(IEnumerable<C8Tuple> res, string workspace, string program)
        {
            int markerCount = 0;
            int containerMessageCount = 0;
            string match = "/Stream/" + workspace + "/" + program + "/";
            Dictionary<string, bool> found = new Dictionary<string, bool>();

            var psoTransform = from pso in res
                               select new
                               {
                                   MessageGroup = (string)pso["MessageGroup"],
                                   MessageName = (string)pso["MessageName"],
                                   ObjectId = (string)pso["ObjectID"],
                                   ObjectId2 = (string)pso["ObjectID2"],
                               };

            foreach (var psoT in psoTransform)
            {
                if (psoT.MessageGroup == "ContainerInfo" && psoT.MessageName == "CPUTime")
                {
                    if (containerMessageCount++ > 10 && markerCount == 0)
                        yield break;
                }
                if (psoT.MessageGroup == "CclApplicationInfo" && psoT.MessageName == "RunningTime")
                {
                    if (markerCount++ > 1)
                        yield break;
                }
                if (psoT.MessageGroup == "CclStreamPairInfo")
                {
                    string[] objecIDPair = { psoT.ObjectId, psoT.ObjectId2 };
                    foreach (string objid in objecIDPair)
                    {
                        var uri = new Uri(objid);
                        if (uri.PathAndQuery.StartsWith(match) && !found.ContainsKey(uri.PathAndQuery))
                        {
                            found[uri.PathAndQuery] = true;
                            yield return uri.PathAndQuery.Substring(match.Length);
                        }
                    }
                }

            }
        }
        
        protected override bool HasChildItems( string path )
        {
            //TODO HasChildItems();
            return true;
        } 


        protected override bool IsItemContainer( string path )
        {
            //TODO IsItemContainer
            return true; 
        } 

    }

}
