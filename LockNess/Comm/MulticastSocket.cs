using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LockNess.Comm
{
    public class MulticastSocket
    {
        private Socket mSocket;

        private IPEndPoint mMulticastEP;
        public IPEndPoint MulticastEndpoint { get { return mMulticastEP; } set { mMulticastEP = value; } }
        public IPEndPoint LocalEndpoint { get; set; }
        public int Ttl { get; set; }

        private byte[] RecvBuffer;

        public Socket Socket
        {
            get
            {
                return this.mSocket;
            }
        }

        public MulticastSocket(
            string MulticastAddress, int MulticastPort, int LocalPort, 
            int TimeToLive, bool ReuseAddress, bool Loopback, int bufferSize)
        {
            if(MulticastPort < 0 || MulticastPort > 65535 || LocalPort < 0 || LocalPort > 65535)
                throw new Exception("Port range out of bounds.");

            mMulticastEP = new IPEndPoint(IPAddress.Parse(MulticastAddress), MulticastPort);
            LocalEndpoint = new IPEndPoint(IPAddress.Any, LocalPort);
            InitSocket(ReuseAddress, Loopback, TimeToLive);
            RecvBuffer = new byte[bufferSize];
        }

        public MulticastSocket(string MulticastAddress, int MulticastPort, int TimeToLive) 
            : this(MulticastAddress, MulticastPort, 0, TimeToLive, true, true, 1024)
        {

        }

        private void InitSocket(bool reuseAddr, bool loopback, int ttl)
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, reuseAddr);
            mSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);
            mSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, loopback);
            mSocket.Bind(LocalEndpoint as EndPoint);

            mSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(mMulticastEP.Address));
        }

        public int Send(byte[] sendBuffer)
        {
            int sentBytes = mSocket.SendTo(sendBuffer, mMulticastEP as EndPoint);
            return sentBytes;
        }

        public byte[] Receive()
        {
            EndPoint ep = mMulticastEP as EndPoint;
            int recv = mSocket.ReceiveFrom(RecvBuffer, ref ep);
            return RecvBuffer;
        }

        public void Close()
        {
            mSocket.Close();
        }
    }
}
