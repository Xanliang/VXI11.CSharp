using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {

        static void Main(string[] args)
        {
            byte[] recive = new byte[4096];
            Vxi11Client client = new Vxi11Client();
            client.connect("127.0.0.1", "inst0");
            //for (; ; )
            //{
            int count = client.send(Encoding.Default.GetBytes("*IDN?\n"), recive);
            //    System.Threading.Thread.Sleep(500);
            //}

            Console.ReadKey();
        }
    }
}
