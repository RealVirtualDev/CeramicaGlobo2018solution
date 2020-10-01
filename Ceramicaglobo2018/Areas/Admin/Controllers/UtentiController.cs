using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    public class UtentiController : Controller
    {

        private DbModel db = new DbModel();

        [AuthorizeRole("admin")]
        public ActionResult List()
        {
            return View("UsersList");
        }

        // DATASOUECE PER JLGRID
        [HttpPost]
        [AuthorizeRole("admin")]
        public JsonResult getData(int pagesize, int currentpage, string tablename, string where)
        {
            // senza la serializzazione non funziona
            db.Configuration.ProxyCreationEnabled = false;

            List<Utenti> pai = db.Utenti.OrderBy(x=>x.email).Skip((currentpage-1)*pagesize).Take(pagesize).ToList();
            return Json(pai);

        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public ContentResult getDetail(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            // ritorna la pagina singola per ottenere i dati da inserire nel form di modifica
            Utenti pai = db.Utenti.Where(d => d.id == id).First();
            //return Json(pai);
            // return JsonConvert.SerializeObject(pai)

            return Content(JsonConvert.SerializeObject(pai, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }), "application/json");
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public int Count()
        {
            int result = db.Utenti.Count();
            return result;
        }


        #region SAVE 

        [HttpPost]
        [AuthorizeRole("admin")]
        public string savePage(Utenti pmodel)
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
                int conta = db.Utenti.Where(x => x.email == pmodel.email).Count();
                if (conta > 0)
                {
                    errors = "L\'email inserita è già presente.";
                    return errors;
                }
                // nuovo
                db.Utenti.Add(pmodel);
                db.SaveChanges();
            }
            else
            {

                int conta = db.Utenti.Where(x => x.email == pmodel.email & x.id != pmodel.id).Count();
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
        public PartialViewResult getEditor(int id)
        {
            Utenti ad;
            if (id == 0)
                ad = new Utenti();
            else
                ad = db.Utenti.Where(x => x.id == id).First();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };


            return PartialView("_UsersEditor", ad);
        }

        [AuthorizeRole("admin")]
        public  ActionResult getCsv()
        {
            List<Utenti> lu = db.Utenti.OrderBy(x => x.email).ToList();
            string intestazione = "\"ragionesociale\",\"nome\",\"cognome\",\"email\",\"nazione\",\"citta\",\"provincia\",\"lang\",\"data\",\"professione\",\"indirizzo\";";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(intestazione);
            string template = "\"{0}\"{1}";

            foreach (Utenti u in lu)
            {
                sb
                    .Append(string.Format(template, u.ragionesociale, ","))
                    .Append(string.Format(template, u.nome, ","))
                    .Append(string.Format(template, u.cognome, ","))
                    .Append(string.Format(template, u.email, ","))
                    .Append(string.Format(template, u.nazione, ","))
                    .Append(string.Format(template, u.citta, ","))
                    .Append(string.Format(template, u.provincia, ","))
                    .Append(string.Format(template, u.lang, ","))
                    .Append(string.Format(template, u.data.ToShortDateString(), ","))
                    .Append(string.Format(template, u.professione, ","))
                    .AppendLine(string.Format(template, u.indirizzo, ";"));
            }

            //byte[] fileBytes = System.IO.File.ReadAllBytes(sb.ToString());
            byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
            string fileName = System.IO.Path.GetFileName("utenti.csv");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

           // System.IO.File.WriteAllText(Server.MapPath("/public/temp/" + fileName), sb.ToString());

        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public string delete(int itemgroup)
        {
            Utenti ad = db.Utenti.Where(x => x.id == itemgroup).First();
            db.Utenti.Attach(ad);
            db.Utenti.Remove(ad);
            db.SaveChanges();

            return "OK";
        }

    }
}