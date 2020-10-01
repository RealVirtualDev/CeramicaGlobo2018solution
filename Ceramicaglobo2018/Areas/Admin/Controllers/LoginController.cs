using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebSite.Infrastructure.Security;
using WebSite.Models;


namespace WebSite.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private DbModel db = new DbModel();

        public ActionResult Index()
        {

            AccessControl AccessControl = new AccessControl();
            //ViewBag.test = AccessControl.encryptPassword("adminaaagfdgdfg-dfgdf");
            //ViewBag.check = AccessControl.encryptPassword("admin","");
            return View();
        }

        [HttpPost] // Login
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string password)
        {
            // vede se ci sono errori nel modello o roba non ammessa
            // ModelState.IsValid()
            AccessControl AccessControl = new AccessControl();
            AjaxMessagge resultMessage = new AjaxMessagge();

            if (ModelState.IsValid)
            {
                if (AccessControl.CheckLogin(email, password, AccessControl.LoginType.admin))
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    

                    resultMessage.Success = true;
                    resultMessage.Message = "Benvenuto";
                    resultMessage.RedirectUrl = "/admin/pages"; // poi mettere dashboard
                    //return Redirect("/admin/administratorsList");
                }
                else
                {
                    ModelState.AddModelError("errLogin", "Credenziali Errate");
                    resultMessage.Message = "Credenziali Errate";
                    resultMessage.Success = false;

                    //return View();
                }
            }
            else
            {
                ModelState.AddModelError("errLogin", "Credenziali Errate");
                resultMessage.Message = "Credenziali Errate";
                resultMessage.Success = false;

            }

            //ViewBag.test = HttpContext.User.Identity.Name;
            //return View();
            return Json(resultMessage);
        }

        public ActionResult adminLogout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("index", "admin");
        }
    }
}