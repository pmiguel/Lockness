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
            DiscoveryServer ds = new DiscoveryServer("239.0.0.2", 7135, "locknessService", 2);
            ds.StartAnnounce();

            Parser p = new Parser();
            MessageReceiver mr = new MessageReceiver(ref p, 7130);
            mr.Start();

            Console.ReadKey();
            ds.StopAnnounce();
        }
    }
}
