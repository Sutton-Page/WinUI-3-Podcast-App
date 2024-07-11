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
using System.Threading;

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

                if(savedPodcast.Length > 0)
                {

                    

                    _dispatcherQueue.TryEnqueue(() =>
                    {

                        emptyUI.Visibility = Visibility.Collapsed;

                        foreach (var item in savedPodcast)
                        {

                            podcastItems.Add(item);
                        }

                    });
                }
                else
                {
                    _dispatcherQueue.TryEnqueue(() =>
                    {

                        emptyUI.Visibility = Visibility.Visible;

                    });
                    
                }
                

                

                

            }
            else
            {

                _dispatcherQueue.TryEnqueue(() =>
                {

                    emptyUI.Visibility = Visibility.Visible;

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

        private void directToSearch(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Search));
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void podcast_right_click(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            if (senderElement != null)
            {

                removeFly.ShowAt(senderElement, e.GetPosition(senderElement));

                
            }
        }



        private void menuflyoutToPodcast(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem != null)
            {
                var dataContext = menuItem.DataContext as Podcast;
                if (dataContext != null)
                {

                    Frame.Navigate(typeof(Content), dataContext);

                    //YourItemCollection.Remove(dataContext);
                }
            }
        }


        private async void updatePodcastState()
        {

            Podcast[] state = podcastItems.ToArray();

            await StateService.SaveStateAsync<Podcast[]>(this.podStoreFile,state);

            

        }

        private void removePodcast(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem != null)
            {
                var dataContext = menuItem.DataContext as Podcast;
                if (dataContext != null)
                {

                    podcastItems.Remove(dataContext);

                    if(podcastItems.Count == 0)
                    {

                        emptyUI.Visibility = Visibility.Visible;
                    }


                    Task.Run(() => this.updatePodcastState());


                    
                }
            }
        }
    }
}
