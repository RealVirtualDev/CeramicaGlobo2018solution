using System.Collections.Generic;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    public class GalleryManagerController: Controller
    {
        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getGallery(ICollection<PageGallery> pgallery)
        {
            if (pgallery == null)
            {
                pgallery = new List<PageGallery>();
            }
            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_PageGallery", pgallery);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getResourceGallery(ICollection<ResourceGallery> rgallery)
        {

            if (rgallery == null)
            {
                rgallery = new List<ResourceGallery>();
            }
            //PageInfo pi = db.PageInfo.Where(x => x.pname == pname && x.lang == lang).First();
            return PartialView("_ResourceGallery", rgallery);
        }
    }
}