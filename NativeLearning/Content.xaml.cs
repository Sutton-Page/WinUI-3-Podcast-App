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

       


        public Content()
        {
            this.InitializeComponent();

            podItems = new ObservableCollection<PodcastItem>();

            podView.SelectedIndex = 0;

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            








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

                String cleanedDescription = this.cleanUpDescription(item.Description, 45);
                String cleanedTitle = this.cleanUpDescription(item.Title, 30);

                String checkNull = item.episodeImageUrl;

                if (checkNull is null)
                {

                    checkNull = podcastData.url;

                }


                PodcastItem temp = new PodcastItem(cleanedTitle, cleanedDescription, item.url, checkNull);

                _dispatcherQueue.TryEnqueue(() =>
                {
                    podItems.Add(temp);

                });

                
            }


            





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

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(podcastData is not null)
            {


                Task.Run(() => this.pullFeed(podcastData.feedUrl));
                

                //await this.pullFeed(podcastData.feedUrl);

                // Thread test = new Thread(o => this.pullFeed(podcastData.feedUrl));

                //test.Start();


            }
        }

        private async void playAudio(object sender, RoutedEventArgs e)
        {
            if (podView.SelectedIndex != -1)
            {

                if (mediaPlayingIndex != podView.SelectedIndex)
                {

                    
                    mediaPlayer.Source = null;

                    

                    PodcastItem pod = podItems[podView.SelectedIndex];
                    mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(pod.url));

                    mediaPlayingIndex = podView.SelectedIndex;

                }

            }
        }
    }
}
