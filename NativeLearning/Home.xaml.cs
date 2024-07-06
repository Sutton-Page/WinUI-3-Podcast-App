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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    
    public sealed partial class Home : Page
    {

        private string podStoreFile = "pod.json";

        private StateService StateService;
        

        private ObservableCollection<Podcast> podcastItems;

        private readonly DispatcherQueue _dispatcherQueue;


        public Home()
        {
            this.InitializeComponent();
            
            this.StateService = new StateService();
            this.podcastItems = new ObservableCollection<Podcast>();
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();




        }

        
        private async void loadPodcasts(string podStoreFile)
        {

            Podcast[] savedPodcast = await this.StateService.LoadStateAsync<Podcast[]>(podStoreFile);


            if(savedPodcast != null)
            {

                _dispatcherQueue.TryEnqueue(() =>
                {

                    foreach (var item in savedPodcast)
                    {

                        podcastItems.Add(item);
                    }

                });

            }

            
            


        }

        private void toAdd(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Search));
        }


        

        private void podContent_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as Podcast;

            if(clicked != null)
            {
                Frame.Navigate(typeof(Content), clicked);

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            Task.Run(() => this.loadPodcasts(this.podStoreFile));
        }
    }
}
