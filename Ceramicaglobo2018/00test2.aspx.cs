using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.Models;
using System.IO;
using System.Text;

namespace CeramicaGlobo2018
{
    public partial class _00test2 : System.Web.UI.Page
    {
        DbModel db = new DbModel();


        protected void Page_Load(object sender, EventArgs e)
        {
            List<Prodotti> lp = db.Prodotti.Where(x => x.categoria.StartsWith("12|") && string.IsNullOrEmpty(x.scheda)).ToList();
            StringBuilder sb = new StringBuilder();
            string folder = "daily";
            string pathstr = string.Format( "/public/resource/prodotti/files/schede/{0}/", folder);
            

            foreach (Prodotti p in lp)
            {
                if (File.Exists(Server.MapPath(pathstr + p.codice + ".pdf")))
                {
                    sb.AppendLine("update prodotti set scheda=\"" + pathstr + p.codice + ".pdf" + "\" where id=" + p.id + ";");
                }
                if (File.Exists(Server.MapPath(pathstr + p.codice + ".zip")))
                {
                    sb.AppendLine("update prodotti set scheda=\"" + pathstr + p.codice + ".zip" + "\" where id=" + p.id + ";");
                }
            }

            Response.Write(sb.ToString());
        }
    }
}