using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class galleryPopup
    {
        public string titolo { get; set; }
        public string content { get; set; }
        public string embeddedcontent { get; set; }
        public string imgmain { get; set; }
        public List<ResourceGallery> gallery { get; set; }
    }
    
}