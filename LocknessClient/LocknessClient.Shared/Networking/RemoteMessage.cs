using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LocknessClient.Networking
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
}
