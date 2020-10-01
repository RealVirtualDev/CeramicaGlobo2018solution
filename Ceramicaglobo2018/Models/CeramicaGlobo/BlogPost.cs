using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Infrastructure;

namespace WebSite.Models
{
    public class BlogPost:iMappableResource
    {
        public int itemgroup { get; set; }
        public string titolo { get; set; }
        public string img { get; set; }
        public string imgmin { get; set; }
        public string urlname { get; set;}
        public string descrizione { get; set;}
        public string categoria { get; set;}
        public string content { get; set;}
        public string tag { get; set;}
        public string urlvideo { get; set;}
        public int numvisite { get; set;}
        public string metatitle { get; set;}
        public string metadescription { get; set;}
        public string visualizza { get; set;}
        public int numcommenti { get; set;}
        public DateTime data { get; set;}

        
    }
}