using System.Collections.Generic;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    public class FilesManagerController:Controller
    {
        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getFiles(ICollection<PageFiles> pfiles)
        {
            // TO DO mandare alla gallery solo il modello gallery
            if (pfiles == null)
            {
                pfiles = new List<PageFiles>();
            }
            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_PageFiles", pfiles);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getFiles(ICollection<ResourceFiles> pfiles)
        {
            // TO DO mandare alla gallery solo il modello gallery
            if (pfiles == null)
            {
                pfiles = new List<ResourceFiles>();
            }
            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_ResourceFiles", pfiles);
        }

    }
}