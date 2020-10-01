using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class resource_content
    {

        public int id { get; set; }
        public virtual string parentname { get; set; }
        public int idrisorsa { get; set; }
        public int itemgroup { get; set; }
        public string tipo { get; set; }
        public string name { get; set; }
        public string val { get; set; }
        public string lang { get; set; }
        public int ordinamento { get; set; }
        
    }
}