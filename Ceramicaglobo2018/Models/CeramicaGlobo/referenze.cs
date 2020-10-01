using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Referenze
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public string titolo { get; set; }
        public string img { get; set; }
        public string gruppo { get; set; }
        public string urlname { get; set; }
    }
}