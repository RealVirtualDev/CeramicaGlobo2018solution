using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace CeramicaGlobo2018.Controllers
{
    public class PolicyController : Controller
    {

        private DbModel db = new DbModel();

        // GET: Policy
        public ActionResult CondizioniUso(string lang="it")
        {
            PageInfo pi = db.PageInfo.Where(x => x.pname == "condizioniuso" && x.lang == lang).FirstOrDefault();
            return View(pi);
        }

        public ActionResult PrivacyPolicy(string lang = "it")
        {
            PageInfo pi = db.PageInfo.Where(x => x.pname == "privacypolicy" && x.lang == lang).FirstOrDefault();
            return View(pi);
        }

        public ActionResult CookiesPolicy(string lang = "it")
        {
            PageInfo pi = db.PageInfo.Where(x => x.pname == "cookiespolicy" && x.lang == lang).FirstOrDefault();
            return View(pi);
        }
    }
}