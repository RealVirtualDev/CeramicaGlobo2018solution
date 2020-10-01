using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebSite.Helpers
{
    public class LanguageSetting:IRequiresSessionState
    {

        public static string DefaultLang = "it";
        public static string DefaultLangLabel = "Italiano";

        public static string Lang
        {
            get { return GetLang(); }
            set { SetLang(value); }
        }

        public static string Langlabel
        {
            get { return GetLangLabel(); }
            set { SetLangLabel(value); }
        }


        private static string GetLang()
        {

            if (HttpContext.Current.Session["lang"] == null)
            {
                HttpContext.Current.Session["lang"] = "it";
            }

            return HttpContext.Current.Session["lang"].ToString();
        }

        private static void SetLang(String l)
        {
            if (l.Length > 2)
            {
                l = DefaultLang;
            }
            HttpContext.Current.Session["lang"] = l;
        }

        private static string GetLangLabel()
        {

            if (HttpContext.Current.Session["langlabel"] == null)
            {
                HttpContext.Current.Session["langlabel"] = DefaultLangLabel;
            }

            return HttpContext.Current.Session["langlabel"].ToString();
        }

        private static void SetLangLabel(String l)
        {
           HttpContext.Current.Session["langlabel"] = l;
        }

        public static string GetLangNavigation()
        {

            //string dom=HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower().Replace("www.", "");
            if (Lang == DefaultLang)
            {
                return "";
            }
            else
            {
                return "/" + Lang;
            }
        }
    }
}