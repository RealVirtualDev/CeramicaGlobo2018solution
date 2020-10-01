using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class ProdottoPage
    {
        public Prodotti Prodotto { get; set; }
        public List<Finiture> Finiture { get; set; }
        public List<FinitureGruppi> GruppiFiniture { get; set; }
        public List<Prodotti> Accessori { get; set; }
        public string breadcrumb { get; set; }
        public string backurl { get; set; }
        public bool isLogged { get; set; }
    }
}