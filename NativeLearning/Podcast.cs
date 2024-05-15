using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeLearning
{
    public class Podcast
    {
        public String name { get; set; }
        public String url { get; set; }

        public String feedUrl { get; set; }



        public Podcast(string name, string url, string feedUrl)
        {
            this.name = name;
            this.url = url;

            this.feedUrl = feedUrl;

        }

        public String summary
        {
            get
            {
                return $"Podcast name {this.name} with image url {this.url}";
            }
        }


    }

    public class PodcastViewModel
    {

        private ObservableCollection<Podcast> podcasts = new ObservableCollection<Podcast>();
        public ObservableCollection<Podcast> Podcasts { get { return this.podcasts; } }

        String testUrl = "https://megaphone.imgix.net/podcasts/29bed80a-d8cc-11e8-b199-aba552a0bbdf/image/uploads_2F1562951997273-pdd2keiryql-99f75240ab90a579e25720d85d3057b2_2Fdarknet-diaries-rss.jpg?ixlib=rails-4.3.1&fit=crop&auto=format,compress";
        String test2 = "https://megaphone.imgix.net/podcasts/310f8cfe-8289-11e5-b42a-c78a2a468812/image/uploads_2F1516104874974-9xwrsh4ccl-5e5f7491cf0582599bb2341880f5eff8_2F01_Slate_Redux_Podcast_Cover_Political-Gabfest.jpg?ixlib=rails-4.3.1&fit=crop&auto=format,compress";
        String test3 = "https://image.simplecastcdn.com/images/2be48404-a43c-4fa8-a32c-760a3216272e/fab1584e-8ea9-4efa-9650-5e595861b2cd/3000x3000/image.jpg?aid=rss_feed";



        String podUrl1 = "https://feeds.megaphone.fm/darknetdiaries";
        String podUrl2 = "https://feeds.megaphone.fm/slatespoliticalgabfest";

        String podUrl3 = "https://feeds.simplecast.com/Y8lFbOT4";
        public PodcastViewModel()
        {

            this.podcasts.Add(new Podcast("Darknet diaries", this.testUrl,podUrl1));
            this.podcasts.Add(new Podcast("Political Gabfest", this.test2,podUrl2));

            this.podcasts.Add(new Podcast("Freakonomics", this.test3, podUrl3));

            

            




        }
    }
}
