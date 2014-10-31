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
        private Action receiveAction;

        public NetworkInterface()
        {
            mSocket = new DatagramSocket();
        }

        public async void Connect(HostName host, string port, bool joinMulticast = false)
        {
            mSocket.MessageReceived += this.ReceiveInternal;
            if (joinMulticast)
            {
                mSocket.JoinMulticastGroup(host);
                await mSocket.BindServiceNameAsync("7135");
            }
            else
                await mSocket.ConnectAsync(host, port);
        }

        private void ReceiveInternal(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            if (receiveAction != null)
                receiveAction.BeginInvoke(null, args);
        }

        public async void Send(string message)
        {
            DataWriter writer = new DataWriter(mSocket.OutputStream);
            writer.WriteString(message);
            await writer.StoreAsync();
        }

        public void Receive(Action action)
        {
            this.receiveAction = action;
        }
    }
}
