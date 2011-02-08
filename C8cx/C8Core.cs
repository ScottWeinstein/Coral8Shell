using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using C8cx.DTO;

namespace C8cx
{
    public class C8Core
    {
        public C8Core(string root)
        {
            UriBuilder ub = new UriBuilder(root);
            ub.Scheme = "http";
            ub.Path = "/Manager";
            ManagerUri = ub.Uri;
        }
        
        public C8Core(Uri managerUrl)
        {
        	ManagerUri = managerUrl;
        }

        public Uri ManagerUri { get; set; }

        #region GetStatus and info 
        public Status GetManagerStatus()
        {
            return Status.Deserialize(GetSOAPResponse("GetManagerStatus", ""));
        }

        public Status GetWorkspaceStatus(string workspace, bool needCCX)
        {
            string req = string.Format("<provide-ccx-info>{0}</provide-ccx-info>", needCCX);
            var resp = GetSOAPResponseWS("GetWorkspaceStatus", workspace, req);
            return Status.Deserialize(resp);

        }

        public Status GetApplicationStatus(string workspace, string app, bool needCCX)
        {
            string req = string.Format("<program-name>{0}</program-name><provide-ccx-info>{1}</provide-ccx-info>", app, needCCX);
            string resp = GetSOAPResponseWS("GetApplicationStatus", workspace, req);
            return Status.Deserialize(resp);
        }
        public IEnumerable<string> GetWorkspaces()
        {
            var ms = GetManagerStatus();
            return (from cwi in ms.CclWorkspaceInfo select cwi.Name);
        }

        public IEnumerable<string> GetApplications(string workspace)
        {
            var ms = GetWorkspaceStatus(workspace, true);
            var apps = from appi in ms.CclApplicationInfo select appi.Name.Split('/')[1];
            return apps;
        }

        public bool IsLoadedApplication(string workspace, string app)
        {
            var qry = (from a in GetApplications(workspace) where a == app select true);
            return qry.SingleOrDefault();
        }

        #endregion

        #region cmdletsupport

        public void StopApplication(string workspace, string app)
        {
            var req = MakeCmdCpx("<UnregisterList><NamedObject Name='{0}'/></UnregisterList>", app);
            GetSOAPResponseWS("ExecuteCommand", workspace, req);
        }
        public void RemoveApplication(string workspace, string app)
        {
            var req = MakeCmdCpx("<UnloadList><NamedObject Name='{0}'/></UnloadList>", app);
            GetSOAPResponseWS("ExecuteCommand", workspace, req);
        }
        public void AddApplication(string workspace, string ccxFile,string app, bool startApplication)
        {
            var xrdr = XmlReader.Create(File.OpenRead(ccxFile));
            xrdr.Read();
            xrdr.Read();
            xrdr.Read();
            xrdr.Read();
            xrdr.ReadOuterXml();
            var ccx = xrdr.ReadOuterXml();
            string req;
            
            if (startApplication)
            {
                req = MakeCmdCpx("<RegisterList>{0}</RegisterList>,<StartProgramsList><NamedObject Name='{1}'/></StartProgramsList>",ccx, app);
            }
            else
            {
                req = MakeCmdCpx("<RegisterList>{0}</RegisterList>", ccx);
            }
            var resp = GetSOAPResponseWS("ExecuteCommand", workspace, req);
            
        }

        private static string MakeCmdCpx(string innerCommand, params object[] args)
        {
            innerCommand = string.Format(innerCommand, args);
            string req = string.Format("<CpxCommand xmlns='http://www.coral8.com/cpx/2004/03/'>{0}</CpxCommand>", innerCommand);
            req = string.Format(@"<command>{0}</command>", HttpUtility.HtmlEncode(req));
            return req;
        }
        #endregion

        #region SOAP support
        public string GetSOAPResponseWS(string element, string workspace, string args)
        {
            string req = String.Format("<workspace-name>{0}</workspace-name>{1}", workspace, args);
            return GetSOAPResponse(element, req);
        }

        public  string GetSOAPResponse(string action, string args)
        {
            const string SOAP_Header = @"application/soap+xml; charset=utf-8; action='urn:coral8.com:SOAPAction'";
            const string actionNamespaces = @"xmlns='urn:coral8.com:Manager' s:encodingStyle='http://www.w3.org/2003/05/soap-encoding'";
            const string soapEnvBodyStart = @"<s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope'><s:Body>";

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", SOAP_Header);
                string soapMsg = String.Format("{0}<{1} {2}>{3}</{1}></s:Body></s:Envelope>",
                                     soapEnvBodyStart,
                                     action,
                                     actionNamespaces,
                                     args);
                var resp = wc.UploadString(ManagerUri, soapMsg);
                var xrdr = XmlReader.Create(new StringReader(resp));
                xrdr.MoveToContent();
                xrdr.Read();
                xrdr.Read();
                xrdr.Read();
                xrdr.Read();
                var value = xrdr.Value;
                return value;
            }
        }
        #endregion

        #region stream reader
        public C8xSchema GetTD(string streamUrl)
        {

            
            var resp = GetSOAPResponse("GetTD", string.Format("<uri>{0}</uri>", streamUrl));
            resp = resp.Replace(@"xsi:type=""", @"C8Type=""");
            var tdesc = TupleDescriptor.Deserialize(resp);

            int ii = 0;
            var schema = new C8xSchema();
            schema.ColumnNames = tdesc.Field.Select(f => f.Name).ToArray();
            schema.ColumnPositionMap = tdesc.Field.ToDictionary(f => f.Name, f => ii++);
            schema.ColumnTypes = tdesc.Field.Select(f=>ConvertC8StringTypeToCLRType(f.C8Type)).ToArray();
            return schema;
        }

        public static Type ConvertC8StringTypeToCLRType(string c8type)
        {
            switch (c8type)
            {
                case "TimeFieldType":
                    return typeof(DateTime);
                case "XmlFieldType":
                case "StringFieldType":
                    return typeof(string);
                case "LongFieldType":
                    return typeof(long);
                case "FloatFieldType":
                    return typeof(double);
                case "IntegerFieldType":
                    return typeof(int);
                case "BooleanFieldType":
                    return typeof(bool);
                default:
                    throw new ArgumentException(String.Format("Unknown Coral8 type '{0}'", c8type));
            }
        }
        public static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public IEnumerable<C8Tuple> OpenStream(string streamUrl, bool needsUrlResolve)
        {
            string rurl = (needsUrlResolve) ? ResolveUrl(streamUrl) : streamUrl;
            C8xSchema tbl = GetTD(streamUrl);
            var req = WebRequest.Create(rurl);
            req.Headers.Add("X-C8-StreamFormat", "CSV");
            req.Headers.Add("X-C8-StreamFormatOptions", "TitleRow=false");
            req.Method = "GET";
            using (var resp = req.GetResponse())
            using (var rdr = new StreamReader(resp.GetResponseStream()))
                do
                {
                    var buff = rdr.ReadLine();
                    yield return GetRow(tbl, buff);
                } while (true);
        
            
        }
        public IEnumerable<C8Tuple> OpenStream(string streamUrl)
        {
            return OpenStream(streamUrl, true);
        }

        private static C8Tuple GetRow(C8xSchema schema, string buff)
        {
            var fields = buff.Split(',');
            if (fields.Length != schema.Count + 1 )
            {
                throw new ArgumentException("Column count error");
            }
            var msgTimeStamp = Epoch.AddTicks(long.Parse(fields[0]) * 10).ToLocalTime();
            var row = new C8Tuple(schema,msgTimeStamp);

            for (int ii = 0; ii < schema.Count; ii++)
            {
                var t = schema.ColumnTypes[ii];
                string item = fields[ii+1]; // one extra for Timestamp that we skip here
                object val = item;
                if (t == typeof(DateTime))
                {
                    val = Epoch.AddTicks(long.Parse(item) * 10).ToLocalTime();
                }
                else if (t == typeof(bool))
                {
                    val = bool.Parse(item);
                }
                else if (t == typeof(int))
                {
                    val = int.Parse(item);
                }
                else if (t == typeof(double))
                {
                    val = double.Parse(item);
                }
                else if (t == typeof(long))
                {
                    val = long.Parse(item);
                }
                row[ii] = val;
            }
            return row;
        }

        public static string ResolveUrl(string url)
        {
            var ub = new UriBuilder(url);
            ub.Scheme = "http";
            ub.Path = "/Manager/ResolveUri";
            using (var wc = new WebClient())
            {
                var resp = wc.UploadString(ub.Uri, url);
                return resp;
            }
        }
        #endregion

        public static CCXDataSet LoadCCX(string fileName)
        {
            var ds = new CCXDataSet();
            var ccxtext = File.ReadAllText(fileName);
            using (var srdr = new StringReader(ccxtext.Replace("xsi:type=", "C8Type=")))
            {
                ds.ReadXml(srdr);
            }
            return ds;
        }
        public static string StreamNameToClassName(string streamName)
        {
            if (streamName.Contains(":"))
            {
                return streamName.Split(":".ToCharArray())[0];
            }

            if (streamName.EndsWith("_1"))
            {
                return streamName.Substring(0, streamName.Length - 2);
            } 
            return streamName;
        }
        public void MethodName()
        {
            var C8ProgDS = new CCXDataSet();
            var CCX = C8ProgDS;
            C8ProgDS.ReadXml("sdf");
            var tdref = new HashSet<string>();
            foreach (var c8strm in  C8ProgDS.Stream )
            {
                if (!tdref.Contains(c8strm.TupleDescriptorRef))
                {
                   
                    tdref.Add(c8strm.TupleDescriptorRef);
                }
            }

            //Type tp;
            //tp.
            //var tdesc = CCX.TupleDescriptor.Where(tdrow => tdrow.Name == "sdf").Single();
            //foreach (C8cx.CCXDataSet.FieldRow fr in tdesc.GetFieldRows())
            //{
            //    fieldRow.Name
            //}
            
            
        }
    }
}
