using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Infrastructure.Security
{
    public class AuthorizeRoleAttribute: AuthorizeAttribute
    {
        private readonly string[] userAssignedRoles;
        private DbModel db = new DbModel();

        public AuthorizeRoleAttribute(params string[] roles)
        {
            userAssignedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            string userMail = httpContext.User.Identity.Name;

            foreach (string role in userAssignedRoles)
            {
                // nel sistema sono previsti solo 2 tipi di ruolo: admin e user
                if (role == "admin")
                {
                    // cerco il login negli amministratori
                   if(db.Administrators.Where(d => d.email == userMail ).Count()>0)
                        authorize=true;
                }
                if (role == "user")
                {
                    //  cerco il login negli utenti / clienti
                    if (db.Utenti.Where(d => d.email == userMail).Count() > 0)
                        authorize = true;
                }
            }

              
                return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/admin");
        }
    }
}