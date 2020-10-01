using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class PageFiles
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int itemgroupcontent { get; set; }
        public string folder { get; set; }
        public string file { get; set; }
        public string displayname { get; set; }
        public string ico { get; set; }
        public string lang { get; set; }

        public string pname { get; set; }
        [ForeignKey("pname,lang")] // indico che la chiave esterna per la relazione è pname
        public virtual PageInfo PageInfo { get; set; }

        [NotMapped]
        public string status { get; set; } = "saved";

    }
}