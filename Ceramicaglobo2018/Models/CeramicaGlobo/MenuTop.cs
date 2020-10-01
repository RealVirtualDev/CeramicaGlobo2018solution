using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Models;
using WebSite.Helpers;
using WebSite.Infrastructure;

namespace WebSite.Models
{
    public class MenuTop : iMappableResource
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public string img { get; set; }
        public string areakey { get; set; }
        public string vocekey { get; set; }
        public string area { get; set; }
        public string voce { get; set; }
        public string url { get; set; }
        public string pname { get; set; }
        public string breadcrumb { get; set; }
        public bool attivo { get; set; }
        
    }
}