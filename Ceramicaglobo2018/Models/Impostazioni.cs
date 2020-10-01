using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Impostazioni
    {
        [Key]
        public int id { get; set; }
        public string keystr { get; set; }
        public string tipo { get; set; }
        public string valore { get; set; }
        public decimal? valorenumerico { get; set; }

    }
}