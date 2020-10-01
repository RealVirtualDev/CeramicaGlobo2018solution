using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Sottocategorie
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public string titolo { get; set; }
        public string urlname { get; set; }
        public string categoria { get; set; } = ""; // il valore è composto da itemgroup|categoria|urlname

        [NotMapped]
        private DbModel db = new DbModel();
        [NotMapped]
        public string categorianame
        {
            get
            {
                if (categoria == null)
                {
                    return "";
                }
                return db.Categorie.Where(x => x.itemgroup.ToString() == categoriaitemgroup && x.lang == lang).Select(x => x.titolo).FirstOrDefault();
            }
        }
        [NotMapped]
        public string categoriaitemgroup
        {
            get
            {
                return categoria != null ? categoria.Split('|')[0] : "";
            }
        }
        //[NotMapped]
        //public string categoriaurlname
        //{
        //    get
        //    {
        //        if (categoria == null)
        //        {
        //            return "";
        //        }
        //        return db.Categorie.Where(x => x.itemgroup.ToString() == categoriaitemgroup && x.lang == lang).Select(x => x.urlname).FirstOrDefault();

        //        //return categoria != null ? categoria.Split('|')[2] : "";
        //    }
        //}
    }
}