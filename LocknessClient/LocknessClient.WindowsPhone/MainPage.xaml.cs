using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LocknessClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        OldNetworkInterface nic;
        DatagramSocket socket;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            nic = new OldNetworkInterface();
            socket =  new DatagramSocket();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HostName host = new HostName(this.HostBox.Text);
            nic.Connect(host, this.PortBox.Text);
            nic.SendMessage("lock");
        }

        private async void btnDiscover_Click(object sender, RoutedEventArgs e)
        {
            HostName host = new HostName(this.HostBox.Text);
            //await socket.ConnectAsync(host, this.PortBox.Text);
            socket.MessageReceived += socket_MessageReceived;
            await socket.BindServiceNameAsync("7135");
            socket.JoinMulticastGroup(host);            
        }

        async void socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            DataReader reader = args.GetDataReader();
            uint num = reader.UnconsumedBufferLength;
            string str = reader.ReadString(num);

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => lstBox.Items.Add(str));
            //lstBox.Items.Add(str);
        }
    }
}
