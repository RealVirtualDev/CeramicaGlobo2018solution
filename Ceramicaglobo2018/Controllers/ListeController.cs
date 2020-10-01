using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Models.ViewModels;
using WebSite.Helpers;


namespace WebSite.Controllers
{
    public class ListeController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Admin/Liste
        public PartialViewResult Comunicazioni()
        {
            return PartialView("_Comunicazioni",db.Comunicazione.Where(x=>x.lang==LanguageSetting.Lang && x.visibile == true).OrderByDescending(x=>x.ordinamento).ToList());
        }

        public PartialViewResult Certificati()
        {
            return PartialView("_Certificati", db.Certificati.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderBy(x => x.ordinamento).ToList());
        }

        public PartialViewResult Conformita()
        {
            return PartialView("_Conformita", db.Conformita.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderBy(x => x.ordinamento).ToList());
        }

        public PartialViewResult Prestazione()
        {
            return PartialView("_Prestazione", db.Prestazione.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderBy(x => x.ordinamento).ToList());
        }

        [ChildActionOnly]
        public PartialViewResult ParagrafiRispettoAmbiente()
        {
            List<RispettoAmbiente> ra = db.RispettoAmbiente.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_RispettoAmbiente", ra);
        }

        [ChildActionOnly]
        public PartialViewResult Designers()
        {
            List<Designers> des = db.Designers.Where(x => x.lang == LanguageSetting.Lang ).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_Designers", des);
        }

        [ChildActionOnly]
        public PartialViewResult Referenze()
        {
            List<Referenze> rf = db.Referenze.Where(x => x.lang == LanguageSetting.Lang ).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_Referenze", rf);
        }

        [ChildActionOnly]
        public PartialViewResult Cataloghi()
        {
            List<Cataloghi> rf = db.Cataloghi.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.data).ToList();
            return PartialView("_Cataloghi", rf);
        }

        [ChildActionOnly]
        public PartialViewResult Video()
        {
            List<Video> vd = db.Video.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_Video", vd);
        }


        [ChildActionOnly]
        public PartialViewResult RassegnaStampa()
        {
            List<NewsRassegnaStampa> rs = db.NewsRassegnaStampa.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_NewsRassegnaStampa", rs);
        }

        [ChildActionOnly]
        public PartialViewResult NewsProdotto()
        {
            List<NewsProdotto> np = db.NewsProdotto.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_NewsProdotto", np);
        }

        [ChildActionOnly]
        public PartialViewResult NewsEventi()
        {
            List<NewsEventi> ne = db.NewsEventi.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_NewsEventi", ne);
        }

        [ChildActionOnly]
        public PartialViewResult NewsComunicatiStampa()
        {
            List<NewsComunicatiStampa> ne = db.NewsComunicatiStampa.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_NewsComunicatiStampa", ne);
        }

        [ChildActionOnly]
        public PartialViewResult NewsPress()
        {
            List<NewsPress> ne = db.NewsPress.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true).OrderByDescending(x => x.ordinamento).ToList();
            return PartialView("_NewsPress", ne);
        }

        [ChildActionOnly]
        public PartialViewResult Collezioni()
        {
            ViewBag.sectionurl = LanguageSetting.GetLangNavigation() + "/" + (LanguageSetting.Lang == "it" ? "collezioni" : "collections") + "/";
            List<Collezioni> ne = db.Collezioni.Where(x => x.lang == LanguageSetting.Lang && x.visibile==true).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_Collezioni", ne);
        }

        [ChildActionOnly]
        public PartialViewResult TipologieMenu()
        {
            ViewBag.sectionurl = LanguageSetting.GetLangNavigation() + "/" + (LanguageSetting.Lang == "it" ? "tipologie" : "typologies") + "/";
            List<TipologieMenu> ne = db.TipologieMenu.Where(x => x.lang == LanguageSetting.Lang && x.visibile == true && x.ismenu==true).OrderBy(x => x.ordinamento).ToList();
            return PartialView("_Tipologie", ne);
        }

        [HttpPost]
        public PartialViewResult getGalleryProdotto(string ig)
        {
            galleryPopup gp = new galleryPopup();
            NewsProdotto np = db.NewsProdotto.Where(x => x.lang == LanguageSetting.Lang && x.itemgroup.ToString() == ig).FirstOrDefault();
            List<ResourceGallery> rg = db.ResourceGallery.SqlQuery("select * from newsprodotto_gallery where rname='NewsProdotto' and lang='" + LanguageSetting.Lang + "' and itemgroupcontent=" + ig).ToList();

            gp.titolo = np.titolo;
            gp.imgmain = np.img;
            gp.content = np.content;
            gp.embeddedcontent = np.embedded;
            gp.gallery = rg;

            return PartialView("_galleryPopup",gp);
        }
        [HttpPost]
        public PartialViewResult getGalleryEvento(string ig)
        {
            galleryPopup gp = new galleryPopup();
            NewsEventi ne= db.NewsEventi.Where(x => x.lang == LanguageSetting.Lang && x.itemgroup.ToString() == ig).FirstOrDefault();
            List<ResourceGallery> rg = db.ResourceGallery.SqlQuery("select * from newseventi_gallery where rname='NewsEventi' and lang='" + LanguageSetting.Lang + "' and itemgroupcontent=" + ig).ToList();

            gp.titolo = ne.titolo;
            gp.imgmain = ne.img;
            gp.content = ne.content;
            gp.embeddedcontent = ne.embedded;
            gp.gallery = rg;

            return PartialView("_galleryPopup", gp);
        }
    }
}