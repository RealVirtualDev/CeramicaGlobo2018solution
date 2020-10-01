using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Newtonsoft.Json;

namespace WebSite.Models
{
    public partial class PageModel
    {
        public int id { get; set; }
        public string pname { get; set; }
        public string propertyname { get; set; }
        public string tipo { get; set; }
        public string lang { get; set; }
        public string adminlabel { get; set; }
        public string admindescription { get; set; }
        public string admintype { get; set; }
        public string jsonadminparams { get; set; }
        public string jsondatasource { get; set; }
        public string jsvalidator { get; set; }
        public string jsvalidatorlangkey { get; set; }
        public bool isinvariant { get; set; }
        public bool adminshow { get; set; }
        [ForeignKey("pname,lang")]
        public virtual PageInfo PageInfo { get; set; }


     

       

    }
}