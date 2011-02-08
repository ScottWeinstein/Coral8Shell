using System;
using System.Management.Automation;
using System.Linq;
using System.IO;

namespace C8cx
{
    #region Base classes
    public abstract class C8BaseCmdLet : Cmdlet
    {
        public C8BaseCmdLet()
        {
            ServerUrl = "http://localhost:6789";
            Workspace = "Default";
        }
        [Parameter]
        public string ServerUrl { get; set; }
        [Parameter]
        public string Workspace { get; set; }

        internal const string C8AppNoun = "C8App";
        internal const string C8StatusNoun = "C8Status"; 
    }

    public abstract class C8AppCmdLet : C8BaseCmdLet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Application { get; set; }
    }

    #endregion

   [Cmdlet(VerbsCommon.Get, C8BaseCmdLet.C8StatusNoun, SupportsShouldProcess = false)]
    public class GetC8Status : C8BaseCmdLet
    {
       public GetC8Status()
       {
           Workspace = "";
       }

        protected override void BeginProcessing()
        {
            C8Core core = new C8Core(ServerUrl);
            if (Workspace != "")
            {
                var wsstatus = core.GetWorkspaceStatus(Workspace, false);

                var apps = from ap in wsstatus.CclApplicationInfo
                           from itm in ap.Value 
                           select new { Application = ap.Name, itm.Name,itm.Value};
                WriteObject(apps, true);
            }
            else
            {
                var mgstatus = core.GetManagerStatus();
                foreach (var item in mgstatus.ManagerInfo.Object.Value)
                {
                    WriteObject(item);
                }
            }
        }
    }


    [Cmdlet(VerbsCommon.Add, C8BaseCmdLet.C8AppNoun, SupportsShouldProcess = false)]
    public class AddAppCmdlet : C8BaseCmdLet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string CcxFile { get; set; }

        protected override void ProcessRecord()
        {
            if (!File.Exists(CcxFile))
            {
                WriteWarning("Specified CCX file does not exist");
                return;
            }

            C8Core core = new C8Core(ServerUrl);
            core.AddApplication(Workspace, CcxFile,"",false);
        }
    }


    [Cmdlet(VerbsCommon.Remove, C8BaseCmdLet.C8AppNoun, SupportsShouldProcess = false)]
    public class RemoveAppCmdlets : C8AppCmdLet
    {
        protected override void ProcessRecord()
        {
            C8Core core = new C8Core(ServerUrl);
            core.RemoveApplication(Workspace, Application);
        }
    }

    [Cmdlet(VerbsCommon.Get, C8BaseCmdLet.C8AppNoun, SupportsShouldProcess = false)]
    public class GetAppCmdlet : C8BaseCmdLet
    {
        [Parameter(Position = 0)]
        public string Application { get; set; }

        protected override void ProcessRecord()
        {
            C8Core core = new C8Core(ServerUrl);
            if (string.IsNullOrEmpty(Application))
            {
                foreach (var item in core.GetApplications(Workspace))
                {
                    var apps = from stat in core.GetApplicationStatus(Workspace, item, false).CclApplicationInfo[0].Value
                               where stat.Name == "State"
                               select new { Application = item, State = stat.Value };
                    WriteObject(apps, true);
                }
                return;
            }


            if (!core.IsLoadedApplication(Workspace,Application))
            {
                WriteWarning("Application " + Application + " does not exist");
                return;
            }
            var res = core.GetApplicationStatus(Workspace, Application, false);


            WriteObject((from stat in res.CclApplicationInfo[0].Value
                         select new { stat.Name, stat.Value }), true);

            WriteObject((from stat in res.CclCompilerInfo[0].Value
                         select new { stat.Name, stat.Value }), true);

        }
    }

    [Cmdlet(VerbsLifecycle.Stop, C8BaseCmdLet.C8AppNoun)]
    public class StopAppCmdlet : C8AppCmdLet
    {
        protected override void ProcessRecord()
        {
            if (ShouldProcess(Application, VerbsLifecycle.Stop))
            {
                C8Core core = new C8Core(ServerUrl);
                core.StopApplication(Workspace, Application);
                WriteProgress(new ProgressRecord(0, "Stopping", Application));
            }
        }
    }

    [Cmdlet(VerbsLifecycle.Start, C8BaseCmdLet.C8AppNoun, SupportsShouldProcess = false)]
    public class StartAppCmdlet : C8AppCmdLet
    {
        protected override void ProcessRecord()
        {
            C8Core core = new C8Core(ServerUrl);
            if (!core.IsLoadedApplication(Workspace, Application))
            {
                WriteWarning("Application " + Application + " does not exist");
                return;
            }
            var status = core.GetApplicationStatus(Workspace, Application, false);
            var state = (from item in status.CclApplicationInfo[0].Value where item.Name == "State" select item.Value).Single();
            if (state == "Started")
            {
                WriteWarning("Application " + Application + " is running");
                return;
            }
            string CcxFile = (from item in status.CclCompilerInfo[0].Value where item.Name == "CcxFile" select item.Value).Single();
            string RepositoryPath = "";
            if (!Path.IsPathRooted(CcxFile))
            {
                RepositoryPath = (from item in status.CclCompilerInfo[0].Value where item.Name == "RepositoryPath" select item.Value).Single();
                CcxFile = Path.Combine(RepositoryPath, CcxFile);
            }
            core.AddApplication(Workspace,CcxFile, Application,true);
            WriteProgress(new ProgressRecord(0, "Started", Application));
        }
    }

}
