using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class ResourceFiles
    {
        [Key]
        public int id { get; set; }
        public string rname { get; set; }
        public int idrisorsa { get; set; }

        public int itemgroup { get; set; }
        public int itemgroupcontent { get; set; }
        public string folder { get; set; }
        public string file { get; set; }
        public string displayname { get; set; }
        public string ico { get; set; }
        public string lang { get; set; }


        //[ForeignKey("rname,lang")] // indico che la chiave esterna per la relazione è pname
        //public virtual ResourceModelDatas ResourceModelDatas { get; set; }

        [NotMapped]
        public string status { get; set; } = "saved";

    }
}