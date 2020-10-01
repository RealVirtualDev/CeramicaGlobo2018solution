using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HtmlHelpers
{
    // Formattatore simil BBCODE molto basico
    // da usare nell'area riservata per non usare direttamente l'html
    public static class ContentFormatterHelper
    {
        private static Dictionary<string, string> CodeTable = new Dictionary<string, string>
        {
            { "[row]","<div class=\"row\">"},
            { "[/row]","</div>"},
            { "[/col]","</div>"},
            { "[col-4]","<div class=\"col-md-4\">"},
            { "[col-6]","<div class=\"col-md-6\">"},
            { "[col-12]","<div class=\"col-md-12\">"},
            { "[col-auto]","<div class=\"col\" style=\"padding:15px\">"},
            { "[title]","<h2>"},
            { "[/title]","</h2>"},
            { "[i]","<span class=\"font-italic\">"},
            { "[/i]","</span>"},
             { "[b]","<span class=\"font-weight-bold\">"},
            { "[/b]","</span>"},
              { "[small]","<span class=\"small\">"},
            { "[/small]","</span>"},
        };

        public static MvcHtmlString FormatContent(this HtmlHelper html, string content)
        {
            if (string.IsNullOrEmpty(content))
                return MvcHtmlString.Empty;

            // se non ci sono colonne ritorna il testo senza la row
            bool hascolumn = content.Contains("[col-");

            //StringBuilder result = new StringBuilder();
            foreach(var item in CodeTable)
            {
                content=content.Replace(item.Key, item.Value);
            }

            return hascolumn ? MvcHtmlString.Create("<div class=\"row\">" + content + "</div>") : MvcHtmlString.Create( content );
        }
    }
}