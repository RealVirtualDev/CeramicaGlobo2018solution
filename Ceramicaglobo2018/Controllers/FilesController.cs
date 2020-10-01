using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebSite.Models;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Helpers;
using System.Net.Http;
using System.Text;

namespace WebSite.Controllers
{

    

    public class FilesController : Controller
    {

        private DbModel db = new DbModel();

        public ActionResult Download(string lang, string igprodotto,string ftype)
        {

            // return Redirect(LanguageSetting.GetLangNavigation() + "/login");

            bool mustlogged = true;
            
            Prodotti p= db.Prodotti.Where(x => x.lang == lang && x.itemgroup.ToString() == igprodotto).FirstOrDefault();
            if (p == null)
            {
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            }
            string fname = "";

            switch (ftype)
            {
                case "scheda":
                    mustlogged = false;
                    fname = p.scheda;
                    break;
                case "istruzioni":
                    mustlogged = false;
                    fname = p.istruzioni;
                    break;
                case "scassi":
                    mustlogged = false;
                    fname = p.scassi;
                    break;
                case "capitolato":
                    mustlogged = false;
                    fname = p.capitolato;
                    break;
                case "dop":
                    mustlogged = false;
                    fname = p.prestazione;
                    break;
                case "cad":
                    mustlogged = true;
                    fname = p.cad;
                    break;
                case "3ds":
                    mustlogged = true;
                    fname = p.f3ds;
                    break;
                case "revit":
                    mustlogged = true;
                    fname = p.revit;
                    break;
                case "archicad":
                    mustlogged = true;
                    fname = p.archicad;
                    break;
                case "sketchup":
                    mustlogged = true;
                    fname = p.sketchup;
                    break;
            }
            
            if (string.IsNullOrEmpty(fname))
            {
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            }


            if(mustlogged==true && !HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            }
           
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(fname));
            string fileName = System.IO.Path.GetFileName(fname);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            
        }

        public ActionResult Sitemap(string lang = "it")
        {
            StringBuilder oBuilder = new StringBuilder();
            oBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            oBuilder.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml =\"http://www.w3.org/1999/xhtml\">");

            string[] langs = db.Language.Where(x => x.abilitata == true).Select(x => x.lang).ToArray();
            string langsuffix = lang == "it" ? "" : lang + "/";

            //return this.Content(xmlString, "text/xml");
            string datastr = DateTime.Now.AddDays(-3).Year + "-" + string.Format("{0:00}", DateTime.Now.AddDays(-3).Month) + "-" + string.Format("{0:00}", DateTime.Now.AddDays(-3).Day) + "T09:00:00+00:00";

            string baseurl = "https://www.ceramicaglobo.com/" + (lang == "it" ? "" : lang) + "/";
            string domain = "https://www.ceramicaglobo.com";
            oBuilder.AppendLine("<url>");
            //current
            oBuilder.AppendLine("<loc>" + domain + (lang == "it" ? "" : "/" + lang) + "</loc>");
            // italiano
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"it\" href=\"" + domain + "\" />");
            // inglese
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"" + domain + "/en\" />");
            // francese
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"fr\" href=\"" + domain + "/fr\" />");
            // spagnolo
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"es\" href=\"" + domain + "/es\" />");
            // tedesco
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"de\" href=\"" + domain + "/de\" />");
            // russo
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"ru\" href=\"" + domain + "/ru\" />");
            // cinese
            oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"ch\" href=\"" + domain + "/ch\" />");

            oBuilder.AppendLine("<lastmod>" + datastr + "</lastmod>");
            oBuilder.AppendLine("<changefreq>monthly</changefreq>");
            oBuilder.AppendLine("</url>");


            string collcontraint = lang == "it" ? "collezioni" : "collections";

            List<Collezioni> allcoll = db.Collezioni.Where(x => x.visibile == true).ToList();
            List<Categorie> allcat = db.Categorie.ToList();

            int[] igcoll = allcoll.Select(x => x.itemgroup).Distinct().ToArray();

            foreach (int ig in igcoll)
            {

                oBuilder.AppendLine("<url>");
                oBuilder.AppendLine("<loc>" + domain  + (lang == "it" ? "" : "/" + lang) + "/" + collcontraint + "/" + allcoll.Where(x => x.itemgroup == ig && x.lang == lang).Select(x => x.urlname).FirstOrDefault() + "</loc>");

                foreach (string l in langs)
                {
                    collcontraint = l == "it" ? "collezioni" : "collections";
                    oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"" + l + "\" href=\"" + domain + (l == "it" ? "" : "/" + l) + "/" + collcontraint + "/" + allcoll.Where(x => x.itemgroup == ig && x.lang == l).Select(x => x.urlname).FirstOrDefault() + "\" />");
                }

                oBuilder.AppendLine("<lastmod>" + datastr + "</lastmod>");
                oBuilder.AppendLine("<changefreq>monthly</changefreq>");
                oBuilder.AppendLine("</url>");

                // categorie annesse
                List<String> allprodcat = db.Prodotti.Where(x => x.collezione.StartsWith(ig.ToString() + "|")).Select(x=>x.categoria).Distinct().ToList();


                collcontraint = lang == "it" ? "collezioni" : "collections";
                foreach (string catstr in allprodcat)
                {
                    string igcat = catstr.Split('|')[0];

                    oBuilder.AppendLine("<url>");
                    oBuilder.AppendLine("<loc>" + domain + (lang == "it" ? "" : "/" + lang) + "/" + collcontraint + "/" + allcoll.Where(x => x.itemgroup == ig && x.lang == lang).Select(x => x.urlname).FirstOrDefault() + "/" + allcat.Where(x=>x.lang==lang && x.itemgroup.ToString()==igcat).Select(x=>x.urlname).FirstOrDefault() + "</loc>");

                    foreach (string l in langs)
                    {
                        collcontraint = l == "it" ? "collezioni" : "collections";
                        oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"" + l + "\" href=\"" + domain + (l == "it" ? "" : "/" + l) + "/" + collcontraint + "/" + allcoll.Where(x => x.itemgroup == ig && x.lang == l).Select(x => x.urlname).FirstOrDefault() + "/" + allcat.Where(x => x.lang == lang && x.itemgroup.ToString() == igcat).Select(x => x.urlname).FirstOrDefault() + "\" />");
                    }

                    oBuilder.AppendLine("<lastmod>" + datastr + "</lastmod>");
                    oBuilder.AppendLine("<changefreq>monthly</changefreq>");
                    oBuilder.AppendLine("</url>");
                }


            }




           

            // prodotti
            int[] igprod = db.Prodotti.Where(x =>  x.visibile == true && x.tipologiaprodotto == "prodotto").Select(x => x.itemgroup).Distinct().ToArray();
            List<Prodotti> allprod = db.Prodotti.Where(x => x.visibile == true && x.tipologiaprodotto == "prodotto" && x.lang=="it").ToList();

            foreach (int ig in igprod)
            {

                collcontraint = lang == "it" ? "collezioni" : "collections";
                Prodotti p = allprod.Where(x => x.itemgroup == ig).FirstOrDefault();

                string igcollezione = p.collezioneitemgroup;
                string igcategoria = p.categoriaitemgroup;

                string urlnamecollezione = allcoll.Where(x => x.lang == lang && x.itemgroup.ToString()==igcollezione).Select(x => x.urlname).FirstOrDefault();
                string urlnamecategoria = allcat.Where(x => x.lang == lang && x.itemgroup.ToString() == igcategoria).Select(x => x.urlname).FirstOrDefault();


                string codiceprodotto = p.codice;


                oBuilder.AppendLine("<url>");
                oBuilder.AppendLine("<loc>" + domain + "/" + collcontraint + "/" + urlnamecollezione + "/" + urlnamecategoria + "/" + codiceprodotto + "</loc>");

                foreach (string l in langs)
                {
                    collcontraint = l == "it" ? "collezioni" : "collections";

                    urlnamecollezione= allcoll.Where(x => x.lang == l && x.itemgroup.ToString() == igcollezione).Select(x => x.urlname).FirstOrDefault();
                    urlnamecategoria = allcat.Where(x => x.lang == l && x.itemgroup.ToString() == igcategoria).Select(x => x.urlname).FirstOrDefault();

                 //  p = db.Prodotti.Where(x => x.lang == l && x.itemgroup == ig).FirstOrDefault();


                 //   urlnamecat = db.Categorie.Where(x => x.lang == l && x.itemgroup.ToString() == p.categoriaitemgroup).Select(x => x.urlname).FirstOrDefault();

                    oBuilder.AppendLine("<xhtml:link rel=\"alternate\" hreflang=\"" + l + "\" href=\"" + domain + (l == "it" ? "" : "/" + l) + "/" + collcontraint + "/" + urlnamecollezione + "/" + urlnamecategoria + "/" + codiceprodotto + "\" />");
                }

                oBuilder.AppendLine("<lastmod>" + datastr + "</lastmod>");
                oBuilder.AppendLine("<changefreq>monthly</changefreq>");
                oBuilder.AppendLine("</url>");
            }


            oBuilder.AppendLine("</urlset>");
            

            //var ms = new System.IO.MemoryStream(Encoding.ASCII.GetBytes(oBuilder.ToString()));
            //Response.AppendHeader("Content-Disposition", "inline;filename=sitemapindex.xml");
            //return new FileStreamResult(ms, "text/xml");

            return Content(oBuilder.ToString(), "text/xml");
        }

    }
}