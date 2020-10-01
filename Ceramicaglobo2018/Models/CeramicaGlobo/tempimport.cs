using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqToExcel.Attributes;

namespace WebSite.Models
{
    public class tempimport
    {
        public int id { get; set; } = 0;
        [ExcelColumn("Codice")]
        public string codice { get; set; }
        [ExcelColumn("Variante")]
        public string variante { get; set; }
        [ExcelColumn("Descrizione")]
        public string descrizione { get; set; }
        [ExcelColumn("Serie")]
        public string serie { get; set; } 

        public string langref { get; set; } = "all";
        public int idprodotto { get; set; } = 0;
        public string finiture { get; set; } = "";
        public string finitureit { get; set; }
        public string finiturerebuild { get; set; }
        public string igprodotti { get; set; }

    }
}