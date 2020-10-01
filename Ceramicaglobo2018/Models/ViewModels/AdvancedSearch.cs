using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class AdvancedSearch
    {
        public PageInfo PageInfo { get; set; }
        public Dictionary<string,string> Collezioni { get; set; }
        public Dictionary<string,string> Categorie { get; set; }
        public Dictionary<int, string> Finiture { get; set; }
        public decimal[] Larghezze { get; set; }
        public decimal[] Profondita { get; set; }
        
    }
}