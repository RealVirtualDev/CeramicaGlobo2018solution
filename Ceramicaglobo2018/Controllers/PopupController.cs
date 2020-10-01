using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;

namespace CeramicaGlobo2018.Controllers
{
    public class PopupController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Popup
        public ActionResult LanguageSelector()
        {
            List<Language> l = db.Language.Where(x => x.abilitata == true).ToList();
            return PartialView("_LanguageSelector",l);
        }

        public ActionResult CookiesInfo()
        {
            PageInfo p = db.PageInfo.Where(x => x.lang == LanguageSetting.Lang && x.pname == "cookiespopup").FirstOrDefault();
            return PartialView("_cookies", p);
        }

        public ActionResult acceptCookies(string urlback)
        {
            HttpCookie ac = new HttpCookie("acceptcookies");
            ac.Value = "true";
            ac.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(ac);
            return Redirect(urlback);
        }

        public ActionResult LoginPopup(string retUrl = "")
        {
            //PageInfo p = db.PageInfo.Where(x => x.lang == LanguageSetting.Lang && x.pname == "cookiespopup").FirstOrDefault();
            ViewBag.returl = retUrl;
            return PartialView("_Login");
        }
    }
}