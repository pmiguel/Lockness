using LockNess.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Networking
{
    public class MessageReceiver
    {
        private Socket socket;
        private IPEndPoint endpoint;
        private byte[] buffer;
        private Parser parser;

        private EndPoint remoteEndPoint;
        private bool running = false; 
       
        public MessageReceiver(ref Parser p, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endpoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(endpoint);
            buffer = new byte[1024];

            Console.WriteLine("Local host: " + socket.LocalEndPoint);
            remoteEndPoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            parser = p;
        }

        public MessageReceiver(ref Parser p) : this(ref p, 0)
        {
        }

        public void Start()
        {
            running = true;
            //Start listening for a new message.
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Waiting for commands...");
            while(running)
            {
                Array.Clear(buffer, 0, buffer.Length);
                socket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref remoteEndPoint);
                string remoteCommand = Encoding.UTF8.GetString(buffer).Replace("\0", String.Empty).Trim();
                Console.WriteLine("COMMAND > " + remoteEndPoint + " | " + remoteCommand);

                try
                {
                    parser.Parse(remoteCommand);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[!]\tCommand not found");
                } 
            }
        }
    }
}
