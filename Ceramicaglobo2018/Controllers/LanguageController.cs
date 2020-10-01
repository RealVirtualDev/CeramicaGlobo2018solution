using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CeramicaGlobo2018.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult ChangeLanguage(string lang)
        {
            HttpCookie clang = new HttpCookie("languser");
            clang.Value = lang;
            clang.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(clang);

            return RedirectToAction("Index", "Home", new { lang =lang=="it" ? "" : lang });
        }
    }
}