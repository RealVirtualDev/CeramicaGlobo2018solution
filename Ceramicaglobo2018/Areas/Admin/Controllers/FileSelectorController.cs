using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSite.Infrastructure.Security;
using WebSite.Models;
using Admin.Models;
using Newtonsoft.Json;
using System.IO;
using WebSite.Infrastructure;

namespace WebSite.Areas.Admin.Controllers
{
    public class FileSelectorController : Controller
    {
        private DbModel db = new DbModel();

        //[HttpPost]
        //[AuthorizeRole("admin")]
        //public JsonResult getPageSetting(int modelId)
        //{
        //    PageModel pm = db.PageModel.Where(x => x.id == modelId).FirstOrDefault();
        //    return Json(pm.jsonadminparams);
        //}

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult render(string mode,int modelId){

           
            FileSelectorState fs = new FileSelectorState();
            dynamic adminparam;
            string propertyname,admintype = "";

            if (mode == "page")
            {
                PageModel pm = db.PageModel.Where(x => x.id == modelId).FirstOrDefault();
                adminparam = pm.AdminParams();
                propertyname = pm.propertyname;
                admintype = pm.admintype;
                fs.source = "page";
            }
            else // resource
            {
                ResourceModel rm = db.ResourceModel.Where(x => x.id == modelId).FirstOrDefault();
                adminparam = rm.AdminParams();
                propertyname = rm.propertyname;
                admintype = rm.admintype;
                fs.source = "resource";
            }
            
            fs.modelId = modelId;
            fs.allowNewFolder = adminparam.allownewfolder;
            fs.startFolder = adminparam.folder;
            fs.currentFolder = adminparam.folder;
            fs.imgh = adminparam.imgh;
            fs.imgw = adminparam.imgw;
            fs.cropmode = adminparam.cropmode;
            fs.fillcolor = adminparam.fillcolor;
            fs.parentControlId = "modelcontrol" + propertyname;
            fs.parentImgId = admintype == "img" ? "modelcontrolimg" + propertyname : "";
            fs.uploadMode= admintype == "img" ? "image"  : "file";
            fs.jsonData = JsonConvert.SerializeObject(fs, new JsonSerializerSettings());


            return PartialView("_FileSelector", fs);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult ListContent(string path,string mode,int modelId)
        {
            string startPath;
            // controllo di sicurezza, posso vedere solo sottocartele della cartella iniziale del modello
            if (mode == "page")
            {
                startPath = db.PageModel
                .Where(x => x.id == modelId)
                .First()
                .AdminParams().folder;
            }
            else
            {
                startPath = db.ResourceModel
                .Where(x => x.id == modelId)
                .First()
                .AdminParams().folder;
            }
           // string safepath = "/public/temp/";
            
            if (!path.Contains(startPath))
                path = startPath;


            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath(path));

            List<FileSelectorItem> fsi = new List<FileSelectorItem>();

            fsi.Add(new FileSelectorItem
            {
                type = "back",
                path = path,
                filename = "...",
                img = "fold.png"
            });

            // DIR
            di.GetDirectories()
                .Where(x => x.Name != "original")
                .ToList().ForEach(d =>
                    fsi.Add(new FileSelectorItem
                    {
                        type = "folder",
                        path = path,
                        filename = d.Name,
                        img = "fold.png"
                    })
                );
            // FILES
            di.GetFiles()
                .Where(x => x.Extension != ".db")
                .ToList().ForEach(d =>
                    fsi.Add(new FileSelectorItem
                    {
                        type = ".jpg.png.gif.bmp.tiff".Contains(d.Extension.ToLower()) ? "img" : "file",
                        path = path,
                        filename = d.Name,
                        img = d.Extension.Trim('.') + ".png"
                    })
                );

            return PartialView("_FileSelectorExplore", fsi);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public string changeFolder(string path, string mode, int modelId)
        {
            string startPath;
            // controllo di sicurezza, posso vedere solo sottocartele della cartella iniziale del modello
            if (mode == "page")
            {
                startPath = db.PageModel
                .Where(x => x.id == modelId)
                .First()
                .AdminParams().folder;
            }
            else
            {
                startPath = db.ResourceModel
                .Where(x => x.id == modelId)
                .First()
                .AdminParams().folder;
            }
            // string safepath = "/public/temp/";

            if (!path.Contains(startPath))
                path = startPath;


            return path;
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult createFolder(string path, string fname,string mode, int modelId)
        {
            string startPath;
            if (mode == "page")
            {
                startPath = db.PageModel
                .Where(x => x.id == modelId)
                .First()
                .AdminParams().folder;
            }
            else
            {
                startPath = db.ResourceModel
              .Where(x => x.id == modelId)
              .First()
              .AdminParams().folder;
            }

            if (!path.Contains(startPath))
                return ListContent(startPath,mode, modelId);

            path = path + (path.EndsWith("/") ? "" : "/");
            System.IO.Directory.CreateDirectory(Server.MapPath(path + fname.ToSafeFilename()));
            return ListContent(path,mode, modelId);
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult delete(string path,string fname, string mode, int modelId)
        {

          
            string safepath = "/public/temp/";
            string startPath;
            if (mode == "page")
            {
              startPath = db.PageModel
              .Where(x => x.id == modelId)
              .First()
              .AdminParams().folder;
            }
            else
            {
                startPath = db.ResourceModel
              .Where(x => x.id == modelId)
              .First()
              .AdminParams().folder;
            }

          
            if (!path.Contains(startPath))
                path = safepath;

            path = path + (path.EndsWith("/") ? "" : "/");
            startPath = startPath + (startPath.EndsWith("/") ? "" : "/");

            FileAttributes attr = System.IO.File.GetAttributes(Server.MapPath(path  + fname));
            if (attr.HasFlag(FileAttributes.Directory))
            {
                // directory
                System.IO.Directory.Delete(Server.MapPath(path + fname), true);
            }
            else
            {
                // file
                System.IO.File.Delete(Server.MapPath(path  + fname));
                try
                {
                    //provo ad eliminare anche il file originale
                    
                    System.IO.File.Delete(Server.MapPath(startPath + fname));
                }
                catch
                {

                }
            }

            return ListContent(path,mode, modelId);
           
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult resizeImage(string mode, string fname, int modelId)
        {

            int imgw, imgh = 0;
            string cropmode = "";
            dynamic adminparam;


            if (mode == "page")
            {
                PageModel pm = db.PageModel.Where(x => x.id == modelId).FirstOrDefault();
                 adminparam = pm.AdminParams();
               
            }
            else
            {
                ResourceModel rm = db.ResourceModel.Where(x => x.id == modelId).FirstOrDefault();
                 adminparam = rm.AdminParams();
                
            }

            imgw = adminparam.imgw;
            imgh = adminparam.imgh;
            cropmode = adminparam.cropmode;

            return render(mode, modelId);

        }


    }
}