using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;
using WebSite.Infrastructure;
using System.Data.Entity;
using System.Web.Security;
using System.Net.Mail;
using System.Text;

namespace WebSite.Controllers
{
    public class UtentiRegController : Controller
    {
        private DbModel db = new DbModel();

        public ActionResult getPrivacy(string lang = "it")
        {
            PageInfo p = db.PageInfo.Where(x => x.pname == "privacypolicy" && x.lang == lang).FirstOrDefault();
            return PartialView("_privacyText", p);
        }

        public ActionResult RegistrazioneUtente(string lang="it")
        {
            ViewBag.returl = Request.QueryString["r"];
            PageInfo pi = db.PageInfo.Where(x => x.pname == "registrazioneutente" && x.lang == lang).FirstOrDefault();
            return View(pi);
        }

        public ActionResult RegistrazioneNewsletter( string e="", string lang = "it")
        {
            ViewBag.email = e;
            PageInfo pi = db.PageInfo.Where(x => x.pname == "registrazionenewsletter" && x.lang == lang).FirstOrDefault();
            return View(pi);
        }

       

        public ActionResult PasswordRecovery(string email)
        {
            AjaxMessagge resultMessage = new AjaxMessagge();

            if (db.Utenti.Where(x => x.email == email).Count() == 0)
            {
                resultMessage.Success = false;
                resultMessage.Message = "emailnontrovata".Translate();
                resultMessage.RedirectUrl = "/";
            }
            else
            {
                Utenti u = db.Utenti.Where(x => x.email == email ).FirstOrDefault();

                // invio password

                string mailbody = db.Emailtemplate.Where(x=>x.lang==(u.lang!="it"?"en":"it") && x.name== "recuperopassword").Select(x=>x.template).FirstOrDefault().Replace("%pass%",u.password);
                
                
                MailMessage m = new MailMessage();
                m.From = new MailAddress("tuci@ceramicaglobo.com");
                m.To.Add(email);
                m.IsBodyHtml = true;
                m.Body = mailbody;
                m.BodyEncoding = Encoding.UTF8;
                m.Subject = "msg_recuperopassword".Translate();

                SmtpClient s =new SmtpClient("smtp.office365.com");
                s.Port = 587;
                s.EnableSsl = true;
                System.Net.NetworkCredential c = new System.Net.NetworkCredential();

                c.UserName = "daniele.tuci@ceramicaglobo.com";
                c.Password = "Arancia01";
                s.Credentials = c;

                try
                {
                    s.Send(m);
                    resultMessage.Success = true;
                    resultMessage.Message = string.Format("emailinviata".Translate());
                    resultMessage.RedirectUrl = "/";
                }
                catch
                {
                    resultMessage.Success = false;
                    resultMessage.Message = string.Format("err_inviomail".Translate());
                    resultMessage.RedirectUrl = "/";
                }

            }

            return Json(resultMessage);
            
        }

        [HttpPost]
        public ActionResult Login(string email,string password,string r="")
        {
            AjaxMessagge resultMessage = new AjaxMessagge();
            
            if (db.Utenti.Where(x=>x.email==email && x.password == password).Count() == 0)
            {
                resultMessage.Success = false;
                resultMessage.Message = "err_login".Translate();
                resultMessage.RedirectUrl = "/"; 
            }
            else
            {
                Utenti u = db.Utenti.Where(x => x.email == email && x.password == password).FirstOrDefault();
                FormsAuthentication.SetAuthCookie(email, false);
                
                // loggato
                resultMessage.Success = true;
                resultMessage.Message =string.Format("txt_welcome".Translate(),u.nome + " " + u.cognome);
                resultMessage.RedirectUrl = "/";
            }

            return Json(resultMessage);
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistraUtente(Utenti u)
        {
            u.lang = LanguageSetting.Lang;
            u.data = DateTime.Now;
            AjaxMessagge resultMessage = new AjaxMessagge();

            if (ModelState.IsValid)
            {
                // registro
                // controllo univocità email
                bool isduplicate = db.Utenti.Where(x => x.email == u.email).Count() > 0;
                if (isduplicate)
                {
                    resultMessage.Success = false;
                    resultMessage.Message = "registrazionedoppia".Translate();
                    resultMessage.RedirectUrl = "/"; // poi mettere dashboard
                }
                else
                {
                    db.Utenti.Add(u);
                    db.SaveChanges();
                    resultMessage.Success = true;
                    resultMessage.Message = "grazieregistrato".Translate();
                    //resultMessage.RedirectUrl = LanguageSetting.GetLangNavigation() + "/login" + (!string.IsNullOrEmpty(Request.QueryString["r"]) ? "?r=" + Request.QueryString["r"] : ""); // poi mettere dashboard
                    resultMessage.RedirectUrl = (!string.IsNullOrEmpty(Request.QueryString["r"]) ? Request.QueryString["r"] : LanguageSetting.GetLangNavigation() + "/"); // poi mettere dashboard

                }

            }
            else
            {

                resultMessage.Success = false;
                resultMessage.Message = "err_controllacampi".Translate();
                resultMessage.RedirectUrl = "/"; // poi mettere dashboard
            }

            return Json(resultMessage);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistraNewsletter(UtentiNewsletter u)
        {
            u.lang = LanguageSetting.Lang;
            u.data = DateTime.Now;
            AjaxMessagge resultMessage = new AjaxMessagge();

            if (ModelState.IsValid)
            {
                // registro
                // devo vedere se l'indirizzo email è già presente
                if(db.UtentiNewsletter.Where(x=>x.email==u.email).Count()>0)
                 {
                    UtentiNewsletter uvecchio = db.UtentiNewsletter.Where(x => x.email == u.email).FirstOrDefault();
                    uvecchio.ragionesociale = u.ragionesociale;
                    uvecchio.attivo = true;
                    uvecchio.professione = u.professione;
                    db.UtentiNewsletter.Attach(uvecchio);
                    db.Entry(uvecchio).State = EntityState.Modified;
                }
                else
                {
                    db.UtentiNewsletter.Add(u);
                }

                
                db.SaveChanges();

                resultMessage.Success = true;
                resultMessage.Message = "grazieregistratonewsletter".Translate();
                resultMessage.RedirectUrl = LanguageSetting.GetLangNavigation() + "/"; 

            }
            else
            {

                resultMessage.Success = false;
                resultMessage.Message = "err_controllacampi".Translate();
                resultMessage.RedirectUrl = LanguageSetting.GetLangNavigation() + "/"; 
            }

            return Json(resultMessage);
        }
    }
}