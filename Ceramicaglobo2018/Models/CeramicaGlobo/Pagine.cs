using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Pagine
    {
        public int id { get; set; }
        public string pagina { get; set; }

        public string testo { get; set; }
        public string testo_en { get; set; }
        public string testo_fr { get; set; }
        public string testo_es { get; set; }
        public string testo_de { get; set; }
        public string testo_ru { get; set; }
        public string testo_ch { get; set; }

        public string pagetitle { get; set; }
        public string pagetitle_en { get; set; }
        public string pagetitle_fr { get; set; }
        public string pagetitle_es { get; set; }
        public string pagetitle_de { get; set; }
        public string pagetitle_ru { get; set; }
        public string pagetitle_ch { get; set; }

        public string metadescription { get; set; }
        public string metadescription_en { get; set; }
        public string metadescription_fr { get; set; }
        public string metadescription_es { get; set; }
        public string metadescription_de { get; set; }
        public string metadescription_ru { get; set; }
        public string metadescription_ch { get; set; }
        
    }
}