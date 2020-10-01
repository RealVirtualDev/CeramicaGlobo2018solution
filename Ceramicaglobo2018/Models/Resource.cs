using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace WebSite.Models
{
    public partial class Resource
    {
        public int id { get; set; }
        public string tipo { get; set; }
        [Key, Column(Order = 0)]
        public string rname { get; set; }
        // non la uso come chiave in quanto le risorse sono settate a livello globale
        // la lingua è sulla tabella dei contenuti
        //[Key, Column(Order = 1)]
        public string lang { get; set; }
        public bool hasgallery { get; set; }
        public bool hasfiles { get; set; }
        public string adminorder { get; set; }
        public string adminwhere { get; set; }


      

            // navigation
        public virtual ICollection<ResourceModel> Model { get; set; }
       


    }

}