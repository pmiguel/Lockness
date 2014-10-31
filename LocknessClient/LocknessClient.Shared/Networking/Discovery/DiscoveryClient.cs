using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;

namespace LocknessClient.Networking.Discovery
{
    public class RemoteMessage : IEquatable<RemoteMessage>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string _host;
        private string _port;
        private string _msg;
        public string Host
        {
            get { return _host; }
            set { SetField<string>(ref _host, value, "Host"); }
        }
        public string Port
        {
            get { return _port; }
            set { SetField<string>(ref _port, value, "Port"); }
        }

        public string Message
        {
            get { return _msg; }
            set { SetField<string>(ref _msg, value, "Message"); }
        }

        public RemoteMessage(string host, string port, string msg = "")
        {
            _host = host;
            _port = port;
            _msg = msg;
        }

        public bool Equals(RemoteMessage other)
        {
            return _host.Equals(other._host) && _port.Equals(other._port);
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", _host, _port);
        }
    }
    public class DiscoveryClient
    {
        private DatagramSocket socket;
        public HostName MulticastHost { get; set; }
        public string LocalServiceName { get; set; }
        public ObservableCollection<RemoteMessage> DiscoveredClients { get; set; }
        
        public DiscoveryClient(HostName multicastHost, string multicastPort, bool joinGroup = false)
        {
            this.MulticastHost = multicastHost;
            this.LocalServiceName = multicastPort;
            DiscoveredClients = new ObservableCollection<RemoteMessage>();

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
            RemoteMessage hp = new RemoteMessage(args.RemoteAddress.DisplayName, args.RemotePort);
            if(!DiscoveredClients.Contains(hp))
            {
                string msg = args.GetDataReader().ReadString(args.GetDataReader().UnconsumedBufferLength);
                hp.Message = msg.Split(':')[1];
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => DiscoveredClients.Add(hp));
            }      
        }

    }
}
