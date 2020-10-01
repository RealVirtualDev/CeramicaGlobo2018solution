using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{ 
    public class Comunicazione
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public bool visibile { get; set; }
        public string titolo { get; set; }
        public string sottotitolo { get; set; }
        public DateTime data { get; set; }
        public string img { get; set; }
        public string pdf { get; set; }
        public string urlname { get; set; }
        public string datastr {
            get
                {
                    return data.ToShortDateString();
                }
            }
   
    }
}