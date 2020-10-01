using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Finiture
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public string titolo { get; set; }
        public string img { get; set; }
        public string codice { get; set; }
        public bool visibile { get; set; }
        public string gruppo { get; set; }
        public string urlname { get; set; }
        public string desinenzafile { get; set; }
        
        [NotMapped]
        public bool selected { get; set; } = false;

        [NotMapped]
        private DbModel db = new DbModel();
        [NotMapped]
        public string grupponame
        {
            get
            {
                if (gruppo == null)
                {
                    return "";
                }
                return db.FinitureGruppi.Where(x => x.itemgroup.ToString() == gruppoitemgroup && x.lang == lang).Select(x => x.titolo).FirstOrDefault();
            }
        }
        [NotMapped]
        public string gruppoitemgroup
        {
            get
            {
                return gruppo != null ? gruppo.Split('|')[0] : "";
            }
        }
       

    }
}