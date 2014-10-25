using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Networking.Discovery
{
    public class DiscoveryClient
    {
        public MulticastSocket Socket { get; set; }
        public string ServiceMessageCode { get; set; }

        public int LookupInterval {get; set;}

        public DiscoveryClient(string multicast, int port, string message, int lookupInterval)
        {
            if (port < 0 || port > 65535)
                throw new Exception("Port out of bounds");
            Socket = new MulticastSocket(multicast, port, 4);
            ServiceMessageCode = message;
            LookupInterval = lookupInterval;
        }

        public List<IPEndPoint> FindServices()
        {
            List<IPEndPoint> list = new List<IPEndPoint>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while(true)
            {
                if (watch.ElapsedMilliseconds >= LookupInterval * 1000)
                    break;
                EndPoint ep = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
                byte[] buff = Socket.Receive(ref ep);
                string str = Encoding.UTF8.GetString(buff).Replace("\0", String.Empty).Trim();
                if (str.Equals(ServiceMessageCode))
                {
                    IPEndPoint ep0 = (IPEndPoint)ep;
                    Console.WriteLine(str + ep0.Address);
                    //list.Add(new IPEndPoint(ep0.Address, ep0.Port));
                }
            }
            watch.Stop();
            return list;
        }
    }
}
