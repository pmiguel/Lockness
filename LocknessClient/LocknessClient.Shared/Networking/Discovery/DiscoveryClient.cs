using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;

namespace LocknessClient.Networking.Discovery
{
    public class DiscoveryClient
    {
        private DatagramSocket socket;
        public HostName MulticastHost { get; set; }
        public string LocalServiceName { get; set; }
        public ObservableCollection<string> DiscoveredClients { get; set; }
        
        public DiscoveryClient(HostName multicastHost, string multicastPort, bool joinGroup = false)
        {
            this.MulticastHost = multicastHost;
            this.LocalServiceName = multicastPort;
            DiscoveredClients = new ObservableCollection<string>();

            socket = new DatagramSocket();
            socket.MessageReceived += MessageHandler;
            if (joinGroup)
                JoinMulticast();
        }

        private async void JoinMulticast()
        {
            await socket.BindServiceNameAsync(LocalServiceName);
            socket.JoinMulticastGroup(MulticastHost);
        }

        private async void MessageHandler(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            string str = String.Format("{0}:{1}", args.RemoteAddress.DisplayName, args.RemotePort);
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => DiscoveredClients.Add(str));

            
        }

    }
}
