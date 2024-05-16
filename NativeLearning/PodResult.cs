using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeLearning
{
    public class PodResult
    {
        public String name {  get; set; }
        public String imageUrl { get; set; }

        public String feedUrl { get; set; }

        public PodResult(String name, String imageUrl, String feedUrl) {

            this.name = name;
            this.imageUrl = imageUrl;
            this.feedUrl = feedUrl;
        
        }
    }
}
