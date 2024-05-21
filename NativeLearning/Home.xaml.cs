using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using Windows.Storage;
using Windows.Security.Cryptography.Certificates;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {

        public PodcastViewModel ViewModel { get; set; }

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public CStorage podStore;

        public Home()
        {
            this.InitializeComponent();
            this.ViewModel = new PodcastViewModel();
            //this.loadConfig();
            

            
        }

        

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(podContent.SelectedIndex != -1)
            {
                Podcast pod = ViewModel.Podcasts[podContent.SelectedIndex];

                String title = pod.name;
                Frame.Navigate(typeof(Content),pod);
            }
        }

        private void toAdd(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Add));
        }


        public async void loadConfig()
        {

            StorageFile st = await localFolder.GetFileAsync("config.json");

            String data =  await FileIO.ReadTextAsync(st);

            if(data != "")
            {
                CStorage? content = JsonSerializer.Deserialize<CStorage>(data);

                this.podStore = content;
            }

           
            

        }
    }
}
