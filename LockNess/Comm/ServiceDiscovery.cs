using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LockNess.Comm
{
    public class ServiceDiscovery
    {
        private MulticastSocket socket;
        private string multicastAddress;
        private int multicastPort;

        private int frequency;
        public bool IsRunning = false;
        private Timer timer;

        public string Message;

        public bool IsServer { get; }

        public ServiceDiscovery(string multicast, int port, string message, int frequency, bool IsServer)
        {
            this.multicastAddress = multicast;
            this.multicastPort = port;

            this.Message = message;

            this.frequency = frequency;
            timer = new Timer();
            socket = new MulticastSocket(multicast, port, 4);
        }
        public void StartAnnounce()
        {
            if(IsServer)
            {
                this.IsRunning = true;
                timer.Elapsed += SendAnnounce; // Send callback
                timer.Interval = this.frequency * 1000; // seconds to millisseconds
                timer.Enabled = IsRunning;
            }
            // TODO else throw exception
        }

        public void StopAnnounce()
        {
            if(IsServer && IsRunning)
            {
                this.IsRunning = false;
                timer.Enabled = IsRunning;
                socket.Close();
            }
            // TODO else throw exception
        }

        private void SendAnnounce(object sender, ElapsedEventArgs e)
        {
            socket.Send(Encoding.UTF8.GetBytes(Message));
        }
    }
}
