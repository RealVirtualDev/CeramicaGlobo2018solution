using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebSite.Infrastructure.Security;
using WebSite.Models;
using WebSite.Areas.Admin.Models.ViewModels;
using Newtonsoft.Json;
using System.Data.Entity;
using WebSite.Infrastructure;
using WebSite.Helpers;

namespace WebSite.Areas.Admin.Controllers
{

    public class PagesController : Controller
    {

        private DbModel db = new DbModel();

        [AuthorizeRole("admin")] // è un filtro custom creato in infrastructure/security
        public ActionResult Index()
        {
            return View("Pages", new AdminCommon
            {
                Language = db.Language.Where(x => x.abilitata).OrderByDescending(x => x.isdefault)
            });
        }


        // DATASOUECE PER JLGRID
        [HttpPost]
        [AuthorizeRole("admin")]
        public JsonResult getData(int pagesize, int currentpage, string tablename, string where)
        {
            // senza la serializzazione non funziona
            db.Configuration.ProxyCreationEnabled = false;

            IEnumerable<PageInfo> pai = db.PageInfo.Where(d => d.lang == "it" && d.modificabile==true).OrderBy(x=>x.sectionpath).ThenBy(x=>x.titolo).ToList();
            return Json(pai);

        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public ContentResult getDetail(string pname, string lang)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            // ritorna la pagina singola per ottenere i dati da inserire nel form di modifica
            PageInfo pai = db.PageInfo.Where(d => d.pname == pname && d.lang == lang).First();
            //return Json(pai);
            // return JsonConvert.SerializeObject(pai)

          return Content(JsonConvert.SerializeObject(pai, new JsonSerializerSettings()
          {
              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
          }), "application/json");
        }

        #region SAVE 

        [HttpPost]
        [AuthorizeRole("admin")]
        public string savePage(PageInfo pmodel)
        {
            if (pmodel.id == 0)
            {
                // nuovo
                db.PageInfo.Add(pmodel);

            }
            else
            {
                if (ModelState.IsValid)
                {

                    saveGallery(ref pmodel);

                    saveFiles(ref pmodel);


                    db.SaveChanges();


                    // devo salvare il modello aggiuntivo della pagina
                    //foreach (PageModelDatas pmd in pmodel.ModelDatas)
                    //{
                    //    db.Entry(pmd).State = EntityState.Modified;
                    //}
                    if (!(pmodel.ModelDatas == null))
                    {
                        pmodel.ModelDatas.ToList().ForEach(x => db.Entry(x).State = EntityState.Modified);
                        db.SaveChanges();
                    }


                    // RELAZIONI FIGLIO
                    // ripristino la relazione a mano perchè l'eliminazione crea dei problemi con le chiavi
                    pmodel.Gallery = db.PageGallery.Where(x => x.pname == pmodel.pname && x.lang == pmodel.lang).ToList();
                    pmodel.Files = db.PageFiles.Where(x => x.pname == pmodel.pname && x.lang == pmodel.lang).ToList();

                    db.Entry(pmodel).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    string errors = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage));
                    return errors;
                }
                // model.hasfiles = false;
                // modifica
                // db.Entry(model.Gallery.Last()).State = EntityState.Added;
            }



            return "OK";
        }

        private void saveGallery(ref PageInfo pinfo)
        {

            int iggallery = 1;
            string basepathgallery = "/public/pages/gallery/" + pinfo.pname + "/";

            if (!System.IO.Directory.Exists(Server.MapPath(basepathgallery)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery));
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery + "min"));
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery + "big"));

            }

            if (!(pinfo.Gallery == null))
            {
                foreach (PageGallery pg in pinfo.Gallery)
                {

                    //db.PageGallery.Attach(pg);
                    pg.itemgroupcontent = 1;
                    pg.pname = pinfo.pname;
                    pg.lang = pinfo.lang;

                    switch (pg.status)
                    {
                        case "deleted":


                            // cancello i files
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(pg.folder + "min/" + pg.img));

                            }
                            catch (Exception e)
                            {

                            }
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(pg.folder + "big/" + pg.img));

                            }
                            catch (Exception e)
                            {

                            }

                            db.Entry(pg).State = EntityState.Deleted;

                            break;
                        case "new":
                            // elaboro e copio l'immagine


                            int icow = pinfo.GallerySetting.Count > 0 ? pinfo.GallerySetting.First().icowidth : 350;
                            int icoh = pinfo.GallerySetting.Count > 0 ? pinfo.GallerySetting.First().icoheight : 350;



                            //ir.fill(300, 300, "#ffffff").Save(Server.MapPath("/public/temp/fill01.jpg"));

                            string originalfilename = pg.img;
                            string originalfolder = pg.folder;

                            string destfilename = "pag_" + pinfo.pname + "_ig" + iggallery + "_" + pg.img.ToSafeFilename();
                            string safeprefix = Guid.NewGuid().ToString().Replace("-", "_");

                            pg.img = destfilename;
                            pg.folder = basepathgallery;
                            pg.itemgroup = iggallery;

                            // resize icona

                            ImageResizer ir = new ImageResizer(Server.MapPath(originalfolder + originalfilename));

                            // controllo immagine con sesso nome già presente
                            if (System.IO.File.Exists(Server.MapPath(pg.folder + "min/" + pg.img)))
                            {
                                pg.img = safeprefix + pg.img;

                            }
                            // MIN
                            ir.crop(icow, icoh).Save(Server.MapPath(pg.folder + "min/" + pg.img));
                            // BIG
                            System.IO.File.Copy(Server.MapPath(originalfolder + originalfilename), Server.MapPath(pg.folder + "big/" + pg.img), true);


                            // ELIMINO IL FILE TEMP
                            System.Threading.Thread.Sleep(500); // attendo 0.5 secondi
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(originalfolder + originalfilename));
                            }
                            catch (Exception e)
                            {

                            }

                            db.Entry(pg).State = EntityState.Added;
                            iggallery++;

                            break;
                        default:
                            pg.itemgroup = iggallery;
                            db.Entry(pg).State = EntityState.Modified;
                            iggallery++;
                            break;
                    }



                }
            }
        }

        private void saveFiles(ref PageInfo pinfo)
        {
            int igfile = 1;
            string basepathfile = "/public/pages/files/" + pinfo.pname + "/";

            if (!System.IO.Directory.Exists(Server.MapPath(basepathfile)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathfile));
            }

            if (!(pinfo.Files == null))
            {
                foreach (PageFiles pf in pinfo.Files)
                {
                    pf.itemgroupcontent = 1;
                    pf.pname = pinfo.pname;
                    pf.lang = pinfo.lang;

                    switch (pf.status)
                    {
                        case "deleted":


                            // cancello i files
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(pf.folder + pf.file));
                            }
                            catch (Exception e)
                            {

                            }

                            db.Entry(pf).State = EntityState.Deleted;

                            break;
                        case "new":
                            // elaboro e copio l'immagine
                            string originalfilename = pf.file;
                            string originalfolder = pf.folder;
                            string destfilename = "pag_" + pinfo.pname + "_ig" + igfile + "_" + pf.file.ToSafeFilename();
                            pf.file = destfilename;
                            pf.folder = basepathfile;
                            pf.itemgroup = igfile;


                            System.IO.File.Copy(Server.MapPath(originalfolder + originalfilename), Server.MapPath(pf.folder + pf.file), true);
                            // ELIMINO IL FILE TEMP
                            System.Threading.Thread.Sleep(500); // attendo 0.5 secondi
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(originalfolder + originalfilename));
                            }
                            catch (Exception e)
                            {

                            }

                            db.Entry(pf).State = EntityState.Added;
                            igfile++;

                            break;
                        default:
                            pf.itemgroup = igfile;
                            db.Entry(pf).State = EntityState.Modified;
                            igfile++;
                            break;

                    }
                }
            }

        }

        #endregion

        [HttpPost]
        [AuthorizeRole("admin")]
        public int Count(string lang)
        {
            int result = db.PageInfo
                .Where(x=>x.lang==lang)
                .Count();

            return result;
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getEditor(string pname, string lang)
        {
            PageInfo pai = db.PageInfo.Where(d => d.pname == pname && d.lang == lang).First();
            AdminCommon viewresult = new AdminCommon();
            
            viewresult.PageInfo = pai;
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            viewresult.jsonData = JsonConvert.SerializeObject(pai, settings)
            .Replace("\\", "\\\\");
            
            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_PageEditor", viewresult);
        }

    }
}