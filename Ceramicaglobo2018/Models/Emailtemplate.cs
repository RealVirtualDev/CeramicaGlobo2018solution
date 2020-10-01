using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Emailtemplate
    {
        public int id { get; set; }
        public string name { get; set; }
        public string template { get; set; }
        public string subject { get; set; }
        public string lang { get; set; }
    }
}