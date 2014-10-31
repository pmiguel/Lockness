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
            int Port = 7130;

            DiscoveryServer ds = new DiscoveryServer("239.0.0.2", 7135, "locknessService-Port:"+Port, 2);
            ds.StartAnnounce();

            Parser p = new Parser();
            MessageReceiver mr = new MessageReceiver(ref p, Port);
            mr.Start();

            Console.ReadKey();
            ds.StopAnnounce();
        }
    }
}
