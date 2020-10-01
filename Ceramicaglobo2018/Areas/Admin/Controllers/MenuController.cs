using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {


        // GET: Admin/Menu
        //[AuthorizeRole("admin")]
        [ChildActionOnly]
        public ActionResult MenuAdmin()
        {
            if (User.Identity.IsAuthenticated )
            {
                //string rol= 
                DbModel db = new DbModel();
                List<Adminmenu> am = db.Adminmenu.OrderBy(x => x.sorting).ToList();
                return PartialView(am);
            }
            else
            {
                return null;
            }
           
        }
    }
}