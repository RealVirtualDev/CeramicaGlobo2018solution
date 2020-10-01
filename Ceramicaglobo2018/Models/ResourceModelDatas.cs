using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebSite.Models
{
    public class ResourceModelDatas
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Key, Column(Order = 1)]
        public string rname { get; set; }
        public int idrisorsa { get; set; }
        public int itemgroup { get; set; }
        public string tipo { get; set; }
        public string name { get; set; }
        [AllowHtml]
        public string val { get; set; }
        [Key, Column(Order = 2)]
        public string lang { get; set; }
        public bool isinvariant { get; set; }
        public int ordinamento { get; set; }
       
        [ForeignKey("rname")]
        [NotMapped]
        public virtual Resource Resource { get; set; }

        // Navigation
        //public virtual ICollection<ResourceGallery> Gallery { get; set; }
        //public virtual ICollection<ResourceFiles> Files { get; set; }



    }
}