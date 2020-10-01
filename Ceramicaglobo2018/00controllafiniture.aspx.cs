using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.Models;
using System.Text;

namespace CeramicaGlobo2018
{
    public partial class _00controllafiniture : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> ig=new List<int>();
            StringBuilder sb = new StringBuilder();

            DbModel db = new DbModel();
            List<Prodotti> pr = db.Prodotti.Where(x => x.lang == "it").ToList();
            foreach(Prodotti p in pr)
            {
                if(db.Prodotti.Where(x=>x.finiture!=p.finiture && x.itemgroup == p.itemgroup).Count() > 0)
                {
                    // ig.Add(p.itemgroup);
                    sb.AppendLine("update prodotti set finiture=\"" + p.finiture + "\" where itemgroup=" + p.itemgroup + " and lang<>'it';");
                }
            }

           
            Response.Write(sb.ToString());
        }
    }
}