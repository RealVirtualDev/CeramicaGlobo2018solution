using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.Models;

namespace CeramicaGlobo2018
{
    public partial class _00templav : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int newiggruppo = 13;
            //string titolo = "Bagno di Colore 2020";
            //string urlname = "bagno-di-colore-2020";
            //string newgruppo = "10|Bagno di Colore 2020";

            StringBuilder sb = new StringBuilder();
            DbModel db = new DbModel();
            //List<Finiture> fin = db.Finiture.Where(x => x.gruppo == "1|Bagno di Colore").ToList();



            //fin.ForEach(x =>
            //{
            //    sb.AppendLine("insert into finiture(lang,titolo,img,codice,urlname,gruppo,desinenzafile)");
            //    sb.Append(" values ('" + x.lang + "','" + x.titolo + "','" + x.img + "','" + x.codice + "','" + x.urlname + "','" + newgruppo + "','" + x.desinenzafile + "');");

            //});

            List<Prodotti> p = db.Prodotti.Where(x=>x.finitureexp!=null).ToList();
            p.ForEach(x =>
            {
                string[] finiture = x.finiture.Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                string ricomposta = "";
                if (x.finiture.Contains("260|") || x.finiture.Contains("262|") || x.finiture.Contains("264|") || x.finiture.Contains("268|") || x.finiture.Contains("266|") || x.finiture.Contains("256|"))
                {
                    ricomposta = finiture
                    .Where(s => !(s.StartsWith("264|") || s.StartsWith("262|") || s.StartsWith("264|") || s.StartsWith("268|") || s.StartsWith("266|") || s.StartsWith("256|")))
                    .Aggregate((cur, next) => cur + ";" + next);
                   
                    if(ricomposta.Contains("|1|Bagno di Colore"))
                    {
                        ricomposta += "327|Bluette|1|Bagno di Colore;328|Fard|1|Bagno di Colore;329|Lime|1|Bagno di Colore;330|Mattone|1|Bagno di Colore;331|Senape|1|Bagno di Colore;332|Smoke|1|Bagno di Colore";
                    }

                    sb.AppendLine("update prodotti set finitureexp='" + ricomposta + "' where id=" + x.id + ";");
                }
            });


            Response.Write(sb.ToString());
        }
    }
}