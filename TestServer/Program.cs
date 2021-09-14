using org.acplt.oncrpc;
using org.acplt.oncrpc.apps.jportmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Console.Out.WriteLine("Starting demoServer...");
            try
            {
                Vxi11Server server = new Vxi11Server();
                server.run();
            }
            catch (System.Exception e)
            {
                Console.Out.WriteLine("demoServer oops:");
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
            }
            //server.stopRpcProcessing();//停止服务
            Console.Out.WriteLine("demoServer stopped.");
        }
    }
}
