using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Phone.Speech.Synthesis;
using BluetoothClientWP8.Model;
using BluetoothClientWP8.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BluetoothClientWP8.Resources;
using Windows.Networking.Sockets;
using Windows.Networking.Proximity;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using BluetoothConnectionManager;
using System.Windows.Media;
using Microsoft.Practices.ServiceLocation;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace BluetoothClientWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ConnectionManager connectionManager;

        private StateManager stateManager;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            connectionManager = new ConnectionManager();
            connectionManager.MessageReceived += connectionManager_MessageReceived;
            stateManager = new StateManager();

        }

        async void connectionManager_MessageReceived(string message)
        {
            var main = ServiceLocator.Current.GetInstance<MainViewModel>();

            Debug.WriteLine("Message received:" + message);
            //string[] messageArray = message.Split(':');
            Dispatcher.BeginInvoke(delegate()
            {
                BodyDetectionStatus.Text = message;
                BodyDetectionStatus.Foreground = new SolidColorBrush(Colors.White);
            });

            Dispatcher.BeginInvoke(delegate()
            {
                var accelItem = new AccelerationItem() {Message = message};
                main.Items.Add(accelItem);

                main.CheckHit();
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            connectionManager.Initialize();
            stateManager.Initialize();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            connectionManager.Terminate();
        }

        private void ConnectAppToDeviceButton_Click_1(object sender, RoutedEventArgs e)
        {
            AppToDevice();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void AppToDevice()
        {
            ConnectAppToDeviceButton.Content = "Connecting...";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var pairedDevices = await PeerFinder.FindAllPeersAsync();

            if (pairedDevices.Count == 0)
            {
                SpeakText("Не обнаружено связанное устрйство.");
                Debug.WriteLine("No paired devices were found.");
            }
            else
            { 
                foreach (var pairedDevice in pairedDevices)
                {
                    if (pairedDevice.DisplayName == DeviceName.Text)
                    {
                        bool result = await connectionManager.Connect(pairedDevice.HostName);
                        if (result)
                        {
                            ConnectAppToDeviceButton.Content = "Connected";
                            DeviceName.IsReadOnly = true;
                            ConnectAppToDeviceButton.IsEnabled = false;

                            SpeakText("Приложение подключено к устройству");
                        }
                        else
                        {
                            ConnectAppToDeviceButton.Content = "Connect";
                            SpeakText("Приложению не удалось подключиться к устройству");
                        }

                        continue;
                    }
                }
            }
        }

        private async void RedButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.RedLightOn ? "TURN_OFF_RED" : "TURN_ON_RED";
            await connectionManager.SendCommand(command);
        }

        private async void GreenButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.GreenLightOn ? "TURN_OFF_GREEN" : "TURN_ON_GREEN";
            await connectionManager.SendCommand(command);
        }

        private async void YellowButton_Click_1(object sender, RoutedEventArgs e)
        {
            string command = stateManager.YellowLightOn ? "TURN_OFF_YELLOW" : "TURN_ON_YELLOW";
            await connectionManager.SendCommand(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutTile_OnTap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("Armor control app");
        }

        private void ConnectionTile_OnTap(object sender, GestureEventArgs e)
        {
            //throw new NotImplementedException();
            //MainPanorama.SelectedIndex = 1;
            MainPanorama.DefaultItem = MainPanorama.Items[1];
        }

        private void EventsTile_OnTap(object sender, GestureEventArgs e)
        {
            //throw new NotImplementedException();
            MainPanorama.DefaultItem = MainPanorama.Items[2];
        }

        private void GraphTile_OnTap(object sender, GestureEventArgs e)
        {
            //throw new NotImplementedException();
            MainPanorama.DefaultItem = MainPanorama.Items[3];
        }

        private void StatisticsTile_OnTap(object sender, GestureEventArgs e)
        {
            //throw new NotImplementedException();
            MainPanorama.DefaultItem = MainPanorama.Items[4];
        }

        private void ResultsContracts_OnItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            SpeakText("Товарищ, приложение запущено");

            /*var main = ServiceLocator.Current.GetInstance<MainViewModel>();
            main.Items.Add(new AccelerationItem() { Message = "10 140 460"});
            main.Items.Add(new AccelerationItem() { Message = "210 420 620", RecivedDateTime = DateTime.Now.AddDays(-1) });*/
        }

        private async void SpeakText(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync(text);
        }
    }
}