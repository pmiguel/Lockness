using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MockSenderClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            string input = "";
            byte[] sendBuffer;
            do
            {
                input = Console.ReadLine().Trim();
                if(input!="exit")
                {
                    string[] strs = input.Split();
                    IPEndPoint rep = new IPEndPoint(IPAddress.Parse(strs[0]), Convert.ToInt32(strs[1]));
                    sendBuffer = Encoding.UTF8.GetBytes(strs[2]);
                    s.SendTo(sendBuffer, (EndPoint)rep);
                }
            } while (input != "exit");
        }
    }
}
