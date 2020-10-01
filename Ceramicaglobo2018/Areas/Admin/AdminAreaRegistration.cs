using System.Web.Mvc;

namespace WebSite.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "admin_resource",
               "Admin/res-{rname}",
               new { action = "LoadResourcePage", Controller = "Resources" }
           );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index",Controller="Login", id = UrlParameter.Optional }
            );
           

        }
    }
}