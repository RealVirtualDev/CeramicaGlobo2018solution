using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;

namespace CeramicaGlobo2018.Controllers
{
    public class ContattiController : Controller
    {
        private DbModel db = new DbModel();

        public ActionResult Index(string pname, string lang = "it")
        {

            PageInfo p = db.PageInfo.Where(x => x.urlname == pname && x.lang == lang)?.FirstOrDefault();
            if (p == null)
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            string invariantPname = p.pname.Replace("-", "");
            // effettuo il rendering della view chiamata
            return View(invariantPname, p);
        }
    }
}