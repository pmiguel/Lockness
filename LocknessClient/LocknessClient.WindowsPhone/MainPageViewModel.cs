using LocknessClient.Networking.Discovery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace LocknessClient
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region --NotifyChanges--
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
        #endregion
        #region --Private data--
        private DatagramSocket socket;
        private DiscoveryClient discoveryClient;
        private RemoteMessage messagePacket;
        #endregion

        #region --Binding properties--
        public RemoteMessage Selected
        {
            get { return messagePacket; }
            set { SetField<RemoteMessage>(ref messagePacket, value, "Selected");}
        }
           
        public ObservableCollection<RemoteMessage> Hosts { get { return discoveryClient.DiscoveredClients; } }
        #endregion
        public MainPageViewModel()
        {
            discoveryClient = new DiscoveryClient(new HostName("239.0.0.2"), "7135", true);
            socket = new DatagramSocket();
            Selected = new RemoteMessage("", "", "");
        }

        public async Task SendLock()
        {
            HostName host = new HostName(messagePacket.Host);
            await socket.ConnectAsync(host, messagePacket.Message);
            DataWriter dw = new DataWriter(socket.OutputStream);
            dw.WriteString("lock");
            await dw.StoreAsync();
        }

    }
}
