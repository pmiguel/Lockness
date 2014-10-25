using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockNess.Networking.Discovery;

namespace DiscoveryClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscoveryServer ds = new DiscoveryServer("239.0.0.2", 7135, "locknessService", 2);
            ds.StartAnnounce();
            Console.ReadKey();
            ds.StopAnnounce();
        }
    }
}
