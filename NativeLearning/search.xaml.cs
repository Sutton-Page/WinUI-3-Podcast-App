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
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Collections;
using Windows.Storage;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.UI.Dispatching;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Search : Page
    {
        private static string podcastSearchUrl = "https://itunes.apple.com/search?term={0}&media=podcast";

        private ObservableCollection<PodResult> searchResults;

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        private readonly DispatcherQueue _dispatcherQueue;

        private string settingKey = "searchTerm";

        private string searchStoreFile = "search.json";

        private string podStoreFile = "pod.json";

        private string podcastSearchString;

        private StateService stateService; 

        public Search()
        {
            this.InitializeComponent();

            searchResults = new ObservableCollection<PodResult>();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            this.stateService = new StateService();
        }

        private String wrapTitle(String title, int lineLength)
        {

            title = Regex.Replace(title, "(.{" + lineLength + "})", "$1" + Environment.NewLine);

            return title;
        }

        private async void addPodcast(Podcast pod)
        {

            Podcast [] currentPodState = await stateService.LoadStateAsync<Podcast[]>(this.podStoreFile);
            
            ArrayList temp = new ArrayList(currentPodState);

            

            temp.Add(pod);

            Podcast[] final = new Podcast[temp.Count];

            temp.CopyTo(final, 0);

            await stateService.SaveStateAsync<Podcast[]>(this.podStoreFile, final);


            string message =  pod.name + ": was added to your saved podcasts!";

            _dispatcherQueue.TryEnqueue(() =>
            {

                podcastSavedNotification.Message = message;
                podcastSavedNotification.IsOpen = true;

            });

            Thread.Sleep(3000);

            _dispatcherQueue.TryEnqueue(() =>
            {

                podcastSavedNotification.IsOpen = false;

            });







        }

        public void SaveSearchTerm()
        {

            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[settingKey] = podcastSearchString;
        }

        public void LoadSearchTerm()
        {

            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(settingKey))
            {
                this.podcastSearchString = (string)localSettings.Values[settingKey];

                input.Text = this.podcastSearchString;
            }

        }

        private async void saveState()
        {

            PodResult[] items = searchResults.ToArray();

            await stateService.SaveStateAsync(searchStoreFile, items);


        }

        private async void RestoreData()
        {

            PodResult[] result = await stateService.LoadStateAsync<PodResult[]>(searchStoreFile);

            if(result != null)
            {

                _dispatcherQueue.TryEnqueue(() =>
                {

                    foreach (var item in result)
                    {
                        searchResults.Add(item);
                    }

                });
                
            }
        }


        public async Task pullData(string term)
        {
            
            using HttpClient client = new HttpClient();

            term = term.Replace(" ", "%20");

            string item = string.Format(podcastSearchUrl, term);

            string result = await client.GetStringAsync(item);

            Searchobject? contentData = JsonSerializer.Deserialize<Searchobject>(result);


            foreach (var value in contentData.results)
            {
                String title = this.wrapTitle(value.collectionName,25);
                PodResult temp = new PodResult(title, value.artworkUrl600, value.feedUrl);

                _dispatcherQueue.TryEnqueue(() =>
                {
                    progress.IsActive = false;
                    progress.Width = 0;
                    progress.Height = 0;

                    progress.Visibility = Visibility.Collapsed;


                    searchResults.Add(temp);

                });

                //searchResults.Add(temp);
            }


        }

        private void goHome(object sender, RoutedEventArgs e)
        {
          

            Frame.Navigate(typeof(Home));
        }

        
        

        private void searchTerm(object sender, RoutedEventArgs e)
        {

            searchResults.Clear();

            progress.IsActive = true;
            progress.Visibility = Visibility.Visible;
            progress.Width = 100;
            progress.Height = 100;
            

            string t = input.Text;

            this.podcastSearchString = t;

            Task.Run(() => pullData(t));

             

            


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
                var dataContext = menuItem.DataContext as PodResult;
                if (dataContext != null)
                {

                    Podcast pod = new Podcast(dataContext.name,dataContext.imageUrl,dataContext.feedUrl);

                    Frame.Navigate(typeof(Content), pod);

                    
                }
            }
        }


        private void resultView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as PodResult;

            if(clicked != null)
            {

                Podcast pod = new Podcast(clicked.name, clicked.imageUrl, clicked.feedUrl);

                Frame.Navigate(typeof(Content), pod);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //this.RestoreData();

            Task.Run(() => this.RestoreData());


            this.LoadSearchTerm();
 

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.saveState();
            this.SaveSearchTerm();
        }

        private void podcastRightClickSave(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem != null)
            {
                var dataContext = menuItem.DataContext as PodResult;
                if (dataContext != null)
                {

                    Podcast pod = new Podcast(dataContext.name, dataContext.imageUrl, dataContext.feedUrl);

                    Task.Run(() => this.addPodcast(pod));



                }
            }
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            if(textBox.Text.Length == 0)
            {
                searchResults.Clear();

            }
        }

        private void searchBoxKeyPressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                searchTerm(searchButton, new RoutedEventArgs());

                


            }
        }

    }
}
