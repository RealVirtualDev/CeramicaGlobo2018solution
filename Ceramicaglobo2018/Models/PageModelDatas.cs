using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class PageModelDatas
    {
        public int id { get; set; }
        public string pname { get; set; }
        public string name { get; set; }
        public string val { get; set; }
        public string lang { get; set; }
        public bool isinvariant { get; set; }
        [ForeignKey("pname,lang")]
        public virtual PageInfo PageInfo { get; set; }
    }


}