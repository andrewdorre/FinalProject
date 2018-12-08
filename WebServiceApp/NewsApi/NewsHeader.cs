using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApi
{
    // Class created to hold the relevant info from the web service
    // This way I could create a list of the objects. Each object would
    // be an individual news story.
    class NewsHeader
    {
        public string NewsTitle { get; set; }
        public string NewsSource { get; set; }
        public string NewsLink { get; set; }
        public string NewsDate { get; set; }        
    }
}
