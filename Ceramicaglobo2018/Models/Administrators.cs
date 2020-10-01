using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Administrators
    {
        public int id { get; set; }
        public string name { get; set; }
        [Required(ErrorMessage = "*")]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string password { get; set; }

    }
}