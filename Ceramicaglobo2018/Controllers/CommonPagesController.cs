using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class CommonPagesController : Controller
    {
        DbModel db = new DbModel();

        // GET: Pages
        public ActionResult savewater(string lang="it")
        {
            PageInfo p = db.PageInfo.Where(x => x.pname == "savewater" && x.lang == lang)?.FirstOrDefault();
            return View(p);
        }
    }
}