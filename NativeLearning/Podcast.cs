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



        public Podcast(string name, string url)
        {
            this.name = name;
            this.url = url;
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


        public PodcastViewModel()
        {

            this.podcasts.Add(new Podcast("Darknet diaries", this.testUrl));
            this.podcasts.Add(new Podcast("Political Gabfest", this.test2));

            

            




        }
    }
}
