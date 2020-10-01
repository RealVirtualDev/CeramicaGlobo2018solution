using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Infrastructure;

namespace WebSite.Areas.Admin.Models.ViewModels
{
    public class AdminCommon
    {
        //public IEnumerable<PageInfo> Pages { get; set; }
        public IEnumerable<Language> Language { get; set; }
        public PageInfo PageInfo { get; set; }
        public resourceDetail Resource { get; set; }
        public string jsonData { get; set; }
        
    }

    public class resourceDetail
    {
        public int itemgroup { get; set; }
        public int igclone { get; set; } = 0;
        public string rname { get; set; }
        public string lang { get; set; }
        public bool hasgallery { get; set; }
        public bool hasfiles { get; set; }
        public List<ResourceModel> Model { get; set; }
        public List<ResourceGallery> Gallery { get; set; }
        public List<ResourceFiles> Files { get; set; }
        public List<ResourceGallerySetting> GallerySetting { get; set; }

    }
}