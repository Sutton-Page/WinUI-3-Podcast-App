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
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Windows.Media.Core;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.UI.Dispatching;
using Windows.Media.Playback;
using System.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Content : Page
    {
        Podcast podcastData;

        int mediaPlayingIndex = -1;

        private ObservableCollection<PodcastItem> podItems;

       private readonly DispatcherQueue _dispatcherQueue;

        private string podStoreFile = "pod.json";

        private string settingKey = "searchTerm";

        private string podcastSearchString;

        private StateService stateService;

        private Podcast[] currentPodState;



        public Content()
        {
            this.InitializeComponent();

            podItems = new ObservableCollection<PodcastItem>();

            podView.SelectedIndex = 0;

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            this.stateService = new StateService();

            


        }

        

        //String lg = "Explore true stories of the dark side of the Internet with host Jack Rhysider \n as he takes you on a journey through the chilling world of hacking, \n data breaches, and cyber crime.";


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is  Podcast)
            {
                podcastData = e.Parameter as Podcast;

                //this.pullFeed(podcastData.feedUrl);
            }

            
            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = null;
            mediaPlayingIndex = -1;

            Frame.Navigate(typeof(Home));
        }


        private async Task pullFeed(String url)
        {



            XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

            XElement root = XElement.Load(url);

            var podDes = root.Element("channel").Element("description").Value;

            podDes = this.cleanUpDescription(podDes, 40);

            if(podDes.Length > 100)
            {

                podDes = podDes.Substring(0, 100);
            }


            var items = from item in root.Descendants("item")

                        select new
                        {




                            Title = (string)item.Element("title"),
                            Description = (string)item.Element("description"),

                            url = (string)item.Element("enclosure").Attribute("url"),

                            episodeImageUrl = (String)item.Element(itunes + "image")?.Attribute("href")




                        };


            // setting header podcast description
            //desc.Text = podDes;


            _dispatcherQueue.TryEnqueue(() =>
            {
                desc.Text = podDes;

            });

            foreach (var item in items)
            {

                //String cleanedDescription = this.cleanUpDescriptionF(item.Description,32);
                String cleanedDescription = this.cleanUpDescriptionFullLength(item.Description, 32);
                //String cleanedTitle = this.cleanUpDescription(item.Title, 20);

                String cleanedTitle = item.Title;

                cleanedTitle = this.cleanUpDescriptionFullLength(cleanedTitle, 32);

                if(cleanedTitle.Length > 60)
                {

                    cleanedTitle = cleanedTitle.Substring(0,60);
                }
                

                

                String checkNull = item.episodeImageUrl;

                if (checkNull is null)
                {

                    checkNull = podcastData.url;

                }


                PodcastItem temp = new PodcastItem(cleanedTitle, cleanedDescription, item.url, checkNull);

                _dispatcherQueue.TryEnqueue(() =>
                {
                    progress.IsActive = false;
                    progress.Width = 0;
                    progress.Height = 0;

                    podItems.Add(temp);

                });

                
            }


            





        }

        public String cleanUpDescriptionFullLength(String description, int lineLength)
        {
            description = description.Replace("/n", "");

            description = Regex.Replace(description, "(.{" + lineLength + "})", "$1" + Environment.NewLine);

            
            return description;


        }


        public String cleanUpDescription(String description, int lineLength)
        {
            description = description.Replace("/n", "");

            description=  Regex.Replace(description, "(.{" + lineLength + "})", "$1" + Environment.NewLine);

            int maxLength = 300;

            if (description.Length > maxLength)
            {

                description = description.Substring(0, maxLength);
            }



            return description;


        }


        


        private async void savePodcast()
        {

            if(currentPodState != null)
            {

                ArrayList holder = new ArrayList(currentPodState);

                holder.Add(this.podcastData);

                Podcast[] final = new Podcast[holder.Count];

                holder.CopyTo(final, 0);


                await stateService.SaveStateAsync<Podcast[]>(this.podStoreFile, final);




            }
            else
            {

                Podcast[] final = new Podcast[1];
                final[0] = this.podcastData;

                await stateService.SaveStateAsync<Podcast[]>(this.podStoreFile, final);
            }


            _dispatcherQueue.TryEnqueue(() =>
            {

                this.saveButton.IsEnabled = false;
                this.saveButton.Content = "Podcast Saved";

            });
            
        }

        private async void loadContent()
        {

            currentPodState = await stateService.LoadStateAsync<Podcast[]>(this.podStoreFile);


            if (currentPodState != null)
            {


                for (int i = 0; i < currentPodState.Length; i++)
                {

                    if(currentPodState[i].name == this.podcastData.name)
                    {

                        _dispatcherQueue.TryEnqueue(() =>
                        {

                            this.saveButton.Content = "Podcast saved";
                            this.saveButton.IsEnabled = false;

                        });

                        break;

                    }
                }
               
                
            }

           
        }

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(podcastData is not null)
            {


                Task.Run(() => this.pullFeed(podcastData.feedUrl));

                Task.Run(() => this.loadContent());


                

                //await this.pullFeed(podcastData.feedUrl);

                // Thread test = new Thread(o => this.pullFeed(podcastData.feedUrl));

                //test.Start();


            }
        }

        

        private void podView_ItemClick(object sender, ItemClickEventArgs e)
        {

            var clicked = e.ClickedItem as PodcastItem;

            if (clicked != null)
            {

                MediaPlayer mp = mediaPlayer.MediaPlayer;
                IMediaPlaybackSource source = mediaPlayer.Source;

                if (mp.PlaybackSession.CanPause)
                {

                    mp.Pause();
                }

                mediaPlayer.Source = null;
                mediaPlayer.SetMediaPlayer(null);

                if (source is MediaSource mediaSource)
                {
                    mediaSource.Dispose();
                }

                mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(clicked.url));


                mediaPlayingIndex = podView.SelectedIndex;

            }

        }




        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => this.savePodcast());

            
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

           
        }
    }
}
