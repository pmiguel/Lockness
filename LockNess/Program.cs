using LockNess.Networking;
using LockNess.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LockNess.Networking.Discovery;

namespace LockNess
{
    class Program
    {
        static List<IPEndPoint> eps;
        static void Main(string[] args)
        {
            //Parser p = new Parser();
            //MessageReceiver mr = new MessageReceiver(ref p);
            //mr.Start();
            DiscoveryClient dc = new DiscoveryClient("239.0.0.2", 7135, "locknessService", 5);
            try
            {
                eps = dc.FindServices();

            } catch(Exception ex)
            {
                Console.WriteLine("No hosts");
            }
            Console.ReadKey();
        }
    }
}
