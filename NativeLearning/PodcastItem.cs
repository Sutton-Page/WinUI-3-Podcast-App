using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeLearning
{
    public class PodcastItem
    {

        public String title { get; set; }
        public String description { get; set; }

        public String url { get; set; }

        public String imageUrl { get; set; }


        public PodcastItem(string title, string description, string url, String imageUrl)
        {
            this.title = title;
            this.description = description;
            this.url = url;
            this.imageUrl = imageUrl;
            
        }


    }
}
