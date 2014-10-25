using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace LocknessClient
{
    public class OldNetworkInterface
    {
        private DatagramSocket _socket;
 
        public OldNetworkInterface()
        {
            _socket = new DatagramSocket();
            _socket.MessageReceived += _socket_MessageReceived;
        }
 
        public async void Connect(HostName remoteHostName, string remoteServiceNameOrPort)
        {
            await _socket.ConnectAsync(remoteHostName, remoteServiceNameOrPort);
        }
 
        public async void SendMessage(string message)
        {
            var stream = _socket.OutputStream;
 
            var writer = new DataWriter(stream);
 
            writer.WriteString(message);
 
            await writer.StoreAsync();

        }

        void _socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

    }
}
