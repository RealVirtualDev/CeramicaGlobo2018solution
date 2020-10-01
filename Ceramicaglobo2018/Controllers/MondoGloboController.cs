using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;


namespace CeramicaGlobo2018.Controllers
{
    public class MondoGloboController : Controller
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

        [ChildActionOnly]
        public PartialViewResult getSliderAmbiente()
        {
            List<SectionSlider> sl = db.SectionSlider.Where(x => x.sezione == "ambiente" && x.lang==LanguageSetting.Lang).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_SectionImageSlider", sl);
        }

        [ChildActionOnly]
        public PartialViewResult getSliderQualita()
        {
            List<SectionSlider> sl = db.SectionSlider.Where(x => x.sezione == "qualita" && x.lang == LanguageSetting.Lang).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_SectionImageSlider", sl);
        }

    }
}