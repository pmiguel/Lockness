using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace LocknessClient
{
    public class NetworkInterface
    {
        private DatagramSocket _socket;
 
        public NetworkInterface()
        {
            _socket = new DatagramSocket();
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
    }
}
