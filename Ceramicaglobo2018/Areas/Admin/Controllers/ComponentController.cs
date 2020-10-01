using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{

    public class ComponentController : Controller
    {

        private DbModel db = new DbModel();
        [ChildActionOnly]
        [AuthorizeRole("admin")]
        public ActionResult FinitureSelector(string jsonParams, string jsonDataSource, string currentValue, string propertyName , string lang)
        {
            List<Finiture> allfiniture = db.Finiture.Where(x => x.lang == "it").ToList();
            //currentValue = "171|bianco ceramico lucido 001|7|Laccati;183|cipria lucido 016 cipria opaco 015 | 7 | Laccati; 213 | metalux zaffiro 414 | 8 | Laccati Metallizzati; 249 | verde salvia legno 6007 | 10 | Laminati Rovere Laccati; 292 | nero opaco | 3 | Laminato Delizia Laccato; 293 | agata | 3 | Laminato Delizia Laccato; 294 | bianco opaco | 3 | Laminato Delizia Laccato; 295 | camoscio | 3 | Laminato Delizia Laccato; 296 | cachemire | 3 | Laminato Delizia Laccato; 297 | castagno | 3 | Laminato Delizia Laccato; 298 | felce | 3 | Laminato Delizia Laccato; 299 | ghiaccio | 3 | Laminato Delizia Laccato; 300 | malva | 3 | Laminato Delizia Laccato; 301 | perla | 3 | Laminato Delizia Laccato; 302 | petrolio | 3 | Laminato Delizia Laccato; 303 | rugiada | 3 | Laminato Delizia Laccato; 304 | visone | 3 | Laminato Delizia Laccato";
            if (currentValue != "")
            {
                Func<string,string> getIg = v => v.Split("|".ToCharArray(),StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                // segno come selected le finiture passate come value
                string[] righe = currentValue.Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries)
                    .Select(x=> getIg(x)).ToArray();

                allfiniture.Where(x => righe.Contains(x.itemgroup.ToString())).All(x => x.selected = true);

               // int c = righe.Length;
            }
            return PartialView("_FinitureSelector", allfiniture);
        }
        [ChildActionOnly]
        [AuthorizeRole("admin")]
        public ActionResult AccessoriSelector(string jsonParams, string jsonDataSource, string currentValue, string propertyName, string lang)
        {
            ViewBag.currentValue = currentValue;
            return PartialView("_AccessoriSelector");
        }
        
        [HttpPost]
        [AuthorizeRole("admin")]
        public ActionResult SearchAccessorio(string stext)
        {
            string currentValue = ViewBag.currentValue;

            string txtsearch = stext.Split('.')[0];

            List<Prodotti> pl = db.Prodotti.Where(x =>x.lang=="it" &&( x.codice == txtsearch | x.codice.StartsWith(txtsearch))).ToList();
            return PartialView("_AccessoriResult",pl);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public ActionResult FillAccessorio(string currentValue)
        {
           
            Func<string,string> getIg = v => v.Split("|".ToCharArray(),StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            string[] righe = currentValue.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => getIg(x)).ToArray();

            List<Prodotti> pl = db.Prodotti.Where(x => x.lang == "it" && righe.Contains( x.itemgroup.ToString())).ToList();
            return PartialView("_AccessoriResult", pl);
        }


    }
}