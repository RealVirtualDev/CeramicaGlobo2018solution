using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Language
    {
        [Key]
        public int id { get; set; }
        public string lang { get; set; }
        public bool abilitata { get; set; }
        public string label { get; set; }
        public string commento { get; set; }
        public bool isdefault { get; set; }
        public string paypaluicode { get; set; }

    }
}