using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using C8cx;
using System.ServiceModel.Channels;
using System.Diagnostics;
using System.IO;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
//            var ServerUrl = "http://localhost:6789";
//            var Workspace = "Default";

//            C8Core core = new C8Core(ServerUrl);
//            string app = "BufferDO";

//            var r2 = core.GetManagerStatus();
//            var res =  core.GetWorkspaceStatus(Workspace, false);
//            var r3 = core.GetApplicationStatus(Workspace, app, false);
////           var  ress = core.StopApplication("Default", app);
//            //core.StartApplication(Workspace,"BufferDO");


//            var r4 = core.GetTD("http://localhost:6789/Status/Workspace/Default");
////            core.AddApplication(Workspace,@" V:\Dev\CEP\C8projects\BufferDO.ccx");


//        var r5=    core.OpenStream("ccl://localhost:6789/Stream/WS2/P1/outBin");
//        r5.ToList();

            //var rdr = new AsyncStreamReader(File.OpenRead(@"C:\dev\coral8shell\C8Core.cs"), Encoding.UTF8);

            //rdr.LineRead += (_, line) =>
            //{
            //    Console.WriteLine(line);
            //};

            //rdr.EOFReached += obj =>
            //{
            //    Console.WriteLine("EOF");
            //};
            //rdr.ReadLineAsync();

            var s1 = new outTest1C8Stream("ccl://localhost:6789/Stream/Default/TestC8Shell/outTest1");
            s1.DataReceived += (tpl) => Console.WriteLine(tpl.v1);
            s1.Connect();
            Console.ReadKey();

        }




    }

    
}
