using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebSite.Models
{
    public class Adminmenu
    {
        public int id { get; set; }
        public string ico { get; set; }
        public string testo { get; set; }
        public string link { get; set; }
        public int sorting { get; set; }
        public int idparent { get; set; }
    }
}