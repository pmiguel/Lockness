using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace LocknessClient
{
    public class NetworkInterface
    {
        private DatagramSocket mSocket;
        private ConcurrentQueue<string> messageQueue;

        public NetworkInterface()
        {
            mSocket = new DatagramSocket();
            messageQueue = new ConcurrentQueue<string>();
        }

        public async void Connect(HostName host, string port, bool joinMulticast = false)
        {
            await mSocket.ConnectAsync(host, port);
            mSocket.MessageReceived += this.ReceiveInternal;
            if (joinMulticast)
                mSocket.JoinMulticastGroup(host);
        }

        private void ReceiveInternal(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            DataReader reader = args.GetDataReader();
            uint unconsumed = reader.UnconsumedBufferLength;
            messageQueue.Enqueue(reader.ReadString(unconsumed));
        }

        public string PollReceive()
        {
            string output = "";
            if(messageQueue.TryDequeue(out output))
            {
                return output;
            }
            return null;
        }

        public async void Send(string message)
        {
            DataWriter writer = new DataWriter(mSocket.OutputStream);
            writer.WriteString(message);
            await writer.StoreAsync();
        }

        public bool HasMessages()
        {
            if (messageQueue.IsEmpty)
                return false;
            return true;
        }
    }
}
