using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebSite.Models
{
    public partial class ResourceModel
    {
        public int id { get; set; }
        public string rname { get; set; }
        public string propertyname { get; set; }
        public string propertytype { get; set; }
        public string adminlabel { get; set; }
        public string admindescription { get; set; }
        public string admintype { get; set; }
        public bool adminshow { get; set; }
        public string jsonadminparams { get; set; }
        public string jsondatasource { get; set; }
        public string jsvalidator { get; set; }
        public string jsvalidatorlangkey { get; set; }
        public int ordinamento { get; set; }
        public bool isinvariant { get; set; }
        public string classlayout { get; set; }

        [NotMapped]
        [AllowHtml]
        public string value { get; set; }
        [NotMapped]
        public int itemgroup { get; set; }
        [NotMapped]
        public string lang { get; set; }


        [ForeignKey("rname")]
        [NotMapped]
        public virtual Resource Resource { get; set; }
    }
}