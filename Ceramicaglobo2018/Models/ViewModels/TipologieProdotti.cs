using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class TipologieProdotti
    {
        public string TipologiaName { get; set; }
        public string TipologieLink { get; set; }
        public string TipologiaUrlname { get; set; }
        public string Metatitle { get; set; }
        public string Metadescription { get; set; }
        public List<Prodotti> Prodotti { get; set; }

    }
}