using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;
using WebSite.Infrastructure;

namespace CeramicaGlobo2018.Controllers
{
    public class HomeController : Controller
    {
        private DbModel db =new DbModel();

        // GET: Home
        public ActionResult Index(string lang="it")
        {
            PageInfo p = db.PageInfo.Where(x => x.pname == "home" && x.lang == lang).FirstOrDefault();
            ViewBag.metatitle = p.metatitle;
            ViewBag.metadescription = p.metadescription;

            return View();
        }

        [ChildActionOnly]
        public ActionResult BlocchiHome(string lang = "it")
        {
            return PartialView("_BlocchiHome",db.BlocchiHome
                .Where(x=>x.lang== lang && x.visibile==true)
                .OrderBy(x=>x.ordinamento)
                .ToList());
        }


        public ActionResult BlockDetail(string urlname)
        {
            //return View("DettaglioBlocco", db.BlocchiHome
            //    .Where(x => x.lang == LanguageSetting.Lang && x.visibile == true && x.urlname== urlname)
            //   .First());

            return View("DettaglioBlocco", db.BlocchiHome
               .Where(x => x.lang == LanguageSetting.Lang &&  x.urlname == urlname)
              .First());
        }


        [ChildActionOnly]
        public ActionResult HomeSlider()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath("/public/homeslider/"));
            List<string> fl = di.EnumerateFiles().Where(f => f.Extension == ".jpg").OrderBy(x => Guid.NewGuid()).Select(f =>string.Concat("/public/homeslider/", f.Name)).ToList();

            return PartialView("_HomeSlider",fl);
        }

        [ChildActionOnly]
        public ActionResult BlogStrip()
        {
            DbBlogContext dbblog = new DbBlogContext();
            string bloglang = LanguageSetting.Lang == "it" ? "it" : "en";
            
            List<resource_content> res = dbblog.resource_content.Where(x => x.parentname == "post" && (x.lang == bloglang || x.lang==bloglang + "-all")).OrderByDescending(x=>x.id).Skip(0).Take(50).ToList();
            List<ResourceModelDatas> rmd = new List<ResourceModelDatas>();
            res.ForEach(
                x =>
                {
                    ResourceModelDatas rd = new ResourceModelDatas();
                   
                    rd.id = x.id;
                    rd.idrisorsa = x.idrisorsa;
                    rd.isinvariant = false;
                    rd.itemgroup = x.itemgroup;
                    rd.lang = x.lang;
                    rd.name = x.name;
                    rd.ordinamento = x.ordinamento;
                    rd.rname = x.parentname;
                    rd.tipo = x.tipo;
                    rd.val = x.val;
                    rmd.Add(rd);
                }
                );


            List<BlogPost> p = new List<BlogPost>();
           p.fill(rmd);


            return PartialView("_BlogStrip",p.Skip(0).Take(3).ToList());

        }

    }
}