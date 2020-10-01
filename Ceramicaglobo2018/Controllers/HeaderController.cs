using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;

namespace CeramicaGlobo2018.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Header
        [ChildActionOnly]
        public PartialViewResult GetHeader()
        {
            DbModel db = new DbModel();
            List<MenuTop> m = db.MenuTop.Where(x => x.lang == LanguageSetting.Lang && x.attivo==true).ToList();
            return PartialView("_Header",m);
        }
    }
}