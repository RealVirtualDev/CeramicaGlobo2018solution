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
    public class PageGallery
    {
        //[Key]
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int itemgroupcontent { get; set; }
        public string folder { get; set; }
        public string img { get; set; }
        public string titolo { get; set; }
        public string descrizione { get; set; }
        public string urlvideo { get; set; }

        
        public string lang { get; set; }
        // indico che la chiave esterna per la relazione è pname
        public string pname { get; set; }
        [ForeignKey("pname,lang")]
        public virtual PageInfo PageInfo { get; set; }

        [NotMapped]
        public int idx { get; set; }
        [NotMapped]
        public string status { get; set; } = "saved";
        [NotMapped]
        public string imgMin { get
            {
                return folder.Contains("/temp/") ? folder + img : folder + "min/" + img;
            }
        }
        [NotMapped]
        public string imgFull
        {
            get
            {
                return folder + "big/" + img;
            }
        }

    }


}