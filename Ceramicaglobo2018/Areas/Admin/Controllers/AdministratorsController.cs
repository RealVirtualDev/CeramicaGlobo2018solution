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
    public class AdministratorsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Admin/Administrators
        [AuthorizeRole("admin")]
        [Route("")]
        public ActionResult Index()
        {
            return View("AdminList");
        }

        // DATASOUECE PER JLGRID
        [HttpPost]
        [AuthorizeRole("admin")]
        public JsonResult getData(int pagesize, int currentpage, string tablename, string where)
        {
            // senza la serializzazione non funziona
            db.Configuration.ProxyCreationEnabled = false;

            IEnumerable<Administrators> pai = db.Administrators;
            return Json(pai);

        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public ContentResult getDetail(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            // ritorna la pagina singola per ottenere i dati da inserire nel form di modifica
            Administrators pai = db.Administrators.Where(d => d.id == id).First();
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
        public string savePage(Administrators pmodel)
        {
            string errors = "";

            if (!ModelState.IsValid)
            {
                 errors = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage));
                return errors;
            }

            if (pmodel.id == 0)
            {
                // controllo unicità 
                int conta = db.Administrators.Where(x => x.email == pmodel.email).Count();
                if (conta > 0)
                {
                    errors = "L\'email inserita è già presente.";
                    return errors;
                }
                // nuovo
                db.Administrators.Add(pmodel);
                db.SaveChanges();
            }
            else
            {

                int conta = db.Administrators.Where(x => x.email == pmodel.email & x.id!=pmodel.id).Count();
                if (conta > 0)
                {
                    errors = "L\'email inserita è già presente.";
                    return errors;
                }

                db.Entry(pmodel).State = EntityState.Modified;
                    db.SaveChanges();
                
            }



            return "OK";
        }

        #endregion


        [HttpPost]
        [AuthorizeRole("admin")]
        public int Count()
        {
            int result = db.Administrators.Count();
            return result;
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getEditor(int id)
        {
            Administrators ad;
            if (id == 0)
                ad = new Administrators();
            else
                ad = db.Administrators.Where(x => x.id == id).First();

            //AdminCommon viewresult = new AdminCommon();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            //viewresult.jsonData = JsonConvert.SerializeObject(ad, settings)
            //.Replace("\\", "\\\\");

            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_AdministratorsEditor", ad);
        }


        [HttpPost]
        [AuthorizeRole("admin")]
        public string delete(int itemgroup)
        {
            Administrators ad = db.Administrators.Where(x => x.id == itemgroup).First();
            db.Administrators.Attach(ad);
            db.Administrators.Remove(ad);
            db.SaveChanges();
            
            return "OK";
        }

    }
}