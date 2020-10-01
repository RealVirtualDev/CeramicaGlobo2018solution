using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CeramicaGlobo2018.Controllers
{
    public class NewsletterController : Controller
    {
        // GET: Newsletter
        public ActionResult GloboFest()
        {
            return View();
        }
    }
}