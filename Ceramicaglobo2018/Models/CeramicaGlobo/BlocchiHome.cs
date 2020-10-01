using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class BlocchiHome
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public bool visibile { get; set; }

        public string titolo { get; set; }
        public string descrizione { get; set; }
        public string content { get; set; }
        public bool showcontent { get; set; }
        public bool usaimmagineinterna { get; set; }

        public string link { get; set; }
        public string urlvideo { get; set; }
        public string metatitle { get; set; }
        public string metadescription { get; set; }
        public string urlname { get; set; }
        public string ico { get; set; }
        public string img { get; set; }
        public string template { get; set; }
        public string buttonlabel { get; set; }

    }
}