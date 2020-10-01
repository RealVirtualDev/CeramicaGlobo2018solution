using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Admin.Models;
using WebSite.Models;

namespace CeramicaGlobo2018
{
    public partial class _00test : System.Web.UI.Page
    {

        //private DbModel db = new DbModel();


        protected void Page_Load(object sender, EventArgs e)
        {


            //    string folder = "";
            //    string field = "";
            //    string folderfield = "";
            //    string desinenza = "";

            //    //string collezione = "12|Daily";

            //    string[] fields = {
            //       // "scheda|schede|_scheda.pdf",
            //       // "istruzioni|istruzioni|_istruzioni.pdf",
            //       // "istruzioni|istruzioni|_istruzioni.zip",
            //        "scassi|scassi|_scasso.zip"
            //       // "cad|cad|_cad.zip",
            //       // "f3ds|3ds|_3ds.zip",
            //       // "revit|revit|_revit.rfa",
            //       // "archicad|archicad|_archicad.gsm",
            //       // "sketchup|sketchup|_sketchup.skp"
            //    };

            //    int split = Convert.ToInt32(Request.QueryString["s"]);


            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //     db.Prodotti.ToList().Skip(split).Take(500).ToList().ForEach(x=> {
            //         folder = x.collezione.Split('|')[1].Replace(" ","").ToLower().Replace("+","plus").Replace("-", "");
            //         foreach(string s in fields)
            //         {
            //             field = s.Split('|')[0];
            //             folderfield = s.Split('|')[1];
            //             desinenza= s.Split('|')[2];
            //             string folderdest = "/public/resource/prodotti/files/" + folderfield + "/" + folder + "/";

            //             if (System.IO.File.Exists(Server.MapPath(folderdest + x.codice + desinenza)))
            //             {
            //                 sb.AppendLine("update prodotti set " + field + "='" + folderdest + x.codice + desinenza + "' where id=" + x.id + ";");
            //             }
            //         }


            //    });

            //    Response.Write(sb.ToString());


            MailMessage m = new MailMessage();
            m.From = new MailAddress("tuci@ceramicaglobo.com");
            m.To.Add("development@realvirtual.it");
            m.IsBodyHtml = true;
            m.Body = "Test mail";
            m.BodyEncoding = Encoding.UTF8;
            m.Subject = "Test invio";

            SmtpClient s = new SmtpClient("smtp.office365.com");
            s.Port = 587;
            s.EnableSsl = true;
            System.Net.NetworkCredential c = new System.Net.NetworkCredential();

            c.UserName = "daniele.tuci@ceramicaglobo.com";
            c.Password = "Arancia01";
            s.Credentials = c;
            s.Send(m);

        }

       


    }
}