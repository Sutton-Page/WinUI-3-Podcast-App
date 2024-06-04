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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NativeLearning
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Add : Page
    {
        private static string podcastSearchUrl = "https://itunes.apple.com/search?term={0}&media=podcast&limit=5";

        private ObservableCollection<PodResult> searchResults;

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        private readonly DispatcherQueue _dispatcherQueue;

        public ArrayList addItems = new ArrayList();

        public Add()
        {
            this.InitializeComponent();

            searchResults = new ObservableCollection<PodResult>();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        private String wrapTitle(String title, int lineLength)
        {

            title = Regex.Replace(title, "(.{" + lineLength + "})", "$1" + Environment.NewLine);

            return title;
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
                    searchResults.Add(temp);

                });

                //searchResults.Add(temp);
            }


        }

        private void goHome(object sender, RoutedEventArgs e)
        {
            this.writeToConfig();

            Frame.Navigate(typeof(Home));
        }

        
        

        private void searchTerm(object sender, RoutedEventArgs e)
        {

            searchResults.Clear();


            string t = input.Text;

            Task.Run(() => pullData(t));

             //await pullData(t);

            


        }

        public async void writeToConfig()
        {

            StorageFile st = await localFolder.GetFileAsync("config.json");

            String data = await FileIO.ReadTextAsync(st);

            CStorage store = new CStorage();


            if (data != "")
            {
                CStorage? content = JsonSerializer.Deserialize<CStorage>(data);

                for(int i = 0; i < content.podcasts.Length; i++)
                {

                    this.addItems.Add(content.podcasts[i]);
                }

                Podcast[] totalItems = new Podcast[this.addItems.Count];

                for(int i = 0; i < totalItems.Length; i++)
                {

                    totalItems[i] = (Podcast) this.addItems[i];
                }


                store.podcasts = totalItems;

                

                
            }
            else
            {

                
                Podcast[] temp = new Podcast[this.addItems.Count];

                for(int i = 0; i <  this.addItems.Count; i++)
                {
                    temp[i] = (Podcast) this.addItems[i];
                }

                store.podcasts = temp;
  

            }

            var handle = await st.OpenStreamForWriteAsync();

            await JsonSerializer.SerializeAsync(handle, store);

           
        }

        private void addPodcast(object sender, RoutedEventArgs e)
        {
            if(resultView.SelectedIndex != -1)
            {
                PodResult item = searchResults[resultView.SelectedIndex];

                Podcast add = new Podcast(item.name, item.imageUrl, item.feedUrl);

                addItems.Add(add);

            }
        }
    }
}
