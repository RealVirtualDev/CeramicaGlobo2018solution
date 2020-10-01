using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Helpers;
using WebSite.Infrastructure.Security;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    
    public class UploaderController : Controller
    {
        // Admin/Upload
        [HttpPost]
        [AuthorizeRole("admin")]
        public JsonResult upload()
        {
            // lavoro con la request
            string folder = Request.Params["folder"];

            // read param fileprefix from http post request
            string fileprefix = Request.Params["fileprefix"];


            // FILE ABORTED
            if (Request.Files.Count == 0)
                return Json("[status:aborted]");

            // get 1st file in request
            HttpPostedFileBase f = Request.Files[0];
            // compone filename
            string fileName = fileprefix + f.FileName;


            string mode = Request.Params["mode"];

            if (!string.IsNullOrEmpty(mode))
            {
                // file o immagine caricata da una risorsa dinamica
                DbModel db = new DbModel();
                int modelId = Convert.ToInt32(Request.Params["modelId"]);
                string admintype;
                dynamic adminparam;

                if (mode == "page")
                {
                    PageModel pm = db.PageModel.Where(x => x.id == modelId).FirstOrDefault();
                    admintype = pm.admintype;
                    adminparam = pm.AdminParams();
                }
                else
                {
                    ResourceModel rm = db.ResourceModel.Where(x => x.id == modelId).FirstOrDefault();
                    admintype = rm.admintype;
                    adminparam = rm.AdminParams();
                }


                string cropmode = !string.IsNullOrEmpty(adminparam.cropmode) ? adminparam.cropmode : "none";
                string fillcolor = !string.IsNullOrEmpty( adminparam.fillcolor)? adminparam.fillcolor : "#ffffff";
                // cartella default
                string startfolder = adminparam.folder;
                startfolder = (startfolder.StartsWith("/") ? "" : "/") + startfolder;
                startfolder = startfolder + (startfolder.EndsWith("/") ? "" : "/");
                // cartella di destinazione
                string targetfolder = !string.IsNullOrEmpty(Request.Params["targetfolder"]) ? Request.Params["targetfolder"] : folder;
                targetfolder= (targetfolder.StartsWith("/") ? "" : "/") + targetfolder;
                targetfolder = targetfolder + (targetfolder.EndsWith("/") ? "" : "/");
                // cartella working
                string uploadfolder =(folder.StartsWith("/") ? "" : "/" ) + folder;
                uploadfolder = uploadfolder + (uploadfolder.EndsWith("/") ? "" : "/");

                // dimensioni di resize
                int imgh = adminparam.imgh!=null ? adminparam.imgh : 0;
                int imgw = adminparam.imgw!= null ? adminparam.imgw : 0;


                string originalfolder = startfolder + "original/";

                if (admintype == "img" )
                {

                    // controllo che esista la cartella original, in caso contrario la creo
                    if (!System.IO.Directory.Exists(Server.MapPath(originalfolder)))
                        System.IO.Directory.CreateDirectory(Server.MapPath(originalfolder));


                    // salvo il file originale
                    f.SaveAs(Server.MapPath(originalfolder + fileName));
                    // attendo 2 secondi per il salvataggio sul disco
                    System.Threading.Thread.Sleep(2000);

                    // devo fare il crop dell'immagine
                    // in tal caso la cartella target è differente dalla cartella startFolder del modello
                    ImageResizer ir = new ImageResizer(Server.MapPath(originalfolder + fileName));
                    switch (cropmode)
                    {
                        case "fit": //adatta e imposta il colore di fondo
                            ir.fit(imgw, imgh,fillcolor).Save(Server.MapPath(targetfolder + fileName));
                            break;
                        case "fill": //riempi tagliando lo scarto
                            if (imgw == 0)
                            {
                                // altezza fissa
                                ir.resize(imgh, ImageResizer.fixedSize.Height).Save(Server.MapPath(targetfolder + fileName));
                            }
                            else if(imgh==0)
                            {
                                // larghezza fissa
                                ir.resize(imgw, ImageResizer.fixedSize.Width).Save(Server.MapPath(targetfolder + fileName));
                            }
                            else
                            {
                                ir.crop(imgw, imgh).Save(Server.MapPath(targetfolder + fileName));
                            }
                            break;
                        default:
                            // copio il file nella cartella selezionata
                            f.SaveAs(Server.MapPath(uploadfolder + fileName));
                            break;


                    }
                    

                }
                else
                {
                    f.SaveAs(Server.MapPath(uploadfolder + fileName));
                }

            }
            else
            {
                // save the file
                f.SaveAs(Server.MapPath(String.Format((folder.StartsWith("/") | folder.StartsWith("\\") ? "" : "../ ") + "{0}" + (folder.EndsWith("/") | folder.EndsWith("\\") ? "" : "/") + "{1}", folder, fileName)));

            }



            return Json("[status:saved]");
        }
    }
}