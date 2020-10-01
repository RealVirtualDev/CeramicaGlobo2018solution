using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace HtmlHelpers
{
    public static class AdminMenuHelper
    {
        private static List<Adminmenu> _repository;
        private static StringBuilder sb ;

        public static MvcHtmlString AdminMenuRender(this HtmlHelper html, List<Adminmenu> menuitems)
        {
            sb=new StringBuilder();

            if (menuitems == null)
            {
                return MvcHtmlString.Empty;
            }

            _repository = menuitems.OrderBy(x => x.sorting).ToList();
            buildmenu();
            
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void buildmenu(int idparent = 0)
        {
            string currentliclass;

            List<Adminmenu> currentitems = _repository.Where(x => x.idparent == idparent).OrderBy(x => x.sorting).ToList();
            if (idparent != 0)
            {
                sb.AppendLine("<ul class=\"submenu chiuso\">");
                currentliclass = "limaincat";
            }
            else
            {
                sb.AppendLine("<ul id=\"jlslidemenu\" class=\"ulsubcat chiuso\">");
                currentliclass = "lisubcat";
            }

            currentitems.ForEach(x =>
            {
                if (x.ico != "")
                    sb.AppendLine("<li style=\"padding-left:35px;background-image:url('/images/admin/menu/" + x.ico + "');background-repeat:no-repeat;background-position:5px 8px;\" class=\"" + currentliclass + "\"><a href=\"" + x.link + "\">" + x.testo + "</a>");
                else
                    sb.AppendLine("<li class=\"" + currentliclass + "\"><a href=\"" + x.link + "\">" + x.testo + "</a>");

               // sb.AppendLine("<li><a href=\"\">" + x.testo + "</a></li>");

                if (_repository.Where(i => i.idparent == x.id).Count()>0)
                {
                    // ci sono subitems, chiamo ricorsività
                    buildmenu(x.id);
                }
                sb.AppendLine("</li>");
            });
            sb.AppendLine("</ul>");

        }
    }
}