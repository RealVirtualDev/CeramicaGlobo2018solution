using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class CollezioneCategorie
    {
        public Collezioni Collezione { get; set; }
        public List<CategoriaLink> Categorie { get; set; }
    }

    public class CategoriaLink
    {
        public string Categoria { get; set; }
        public string link { get; set; }
        public string img { get; set; }
    }
}