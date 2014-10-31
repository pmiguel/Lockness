using LocknessClient.Networking.Discovery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace LocknessClient
{
    public class MainPageViewModel
    {
        public DatagramSocket Socket { get; set; }
        public DiscoveryClient Discovery { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public ObservableCollection<string> Hosts { get { return Discovery.DiscoveredClients; } }

        public MainPageViewModel()
        {
            Discovery = new DiscoveryClient(new HostName("239.0.0.2"), "7135", true);
            Socket = new DatagramSocket();
        }

        public async Task SendLock()
        {
            HostName host = new HostName(Host);
            await Socket.ConnectAsync(host, Port);
            DataWriter dw = new DataWriter(Socket.OutputStream);
            dw.WriteString("lock");
            await dw.StoreAsync();
        }
    }
}
