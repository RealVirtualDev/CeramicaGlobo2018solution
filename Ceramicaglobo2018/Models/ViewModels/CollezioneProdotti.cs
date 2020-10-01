using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class CollezioneProdotti
    {
        public Collezioni Collezione { get; set; }
        public List<Prodotti> Prodotti { get; set; }
        // IG e SOTTOCATEGORIA
        public Dictionary<int,string> Sottocategorie { get; set; }
        public string CategoriaName { get; set; }
        public string CategoriaUrlName { get; set; }
        public string CollezioneLink { get; set; }
    }
}