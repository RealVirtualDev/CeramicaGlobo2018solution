using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebSite.Infrastructure.Resource;
using WebSite.Models;

namespace WebSite.Infrastructure
{
    public static class StringExtension
    {
        private static IResourceProvider resourceProvider = new DbResourceProvider();
        
        public static string ToSafeFilename(this string name)
        {
            foreach(char c in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }

        public static string ToSafeUrlname(this string name)
        {

            name = name.ToLowerInvariant();

            // Remove all accents
            Byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(name);
            name = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            name = Regex.Replace(name, "\\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            name = Regex.Replace(name, "[^a-z0-9\\s-_]", "-", RegexOptions.Compiled);

            //Trim dashes from end
            name = name.Trim(new Char[] { '-', '_'});

            //Replace double occurences of - or _
            name = Regex.Replace(name, "([-_]){2,}", "$1", RegexOptions.Compiled);

            return name;
        }

        public static string PreventSqlInjection(this string val)
        {

            string regExText = "('(''|[^'])*')|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1}|OR|AND|IF)\b)";
            
            //Remove extra separators
            RegexOptions regExOptions  = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            regExText = System.Text.RegularExpressions.Regex.Replace(regExText, "\\(\\|", "(", regExOptions);
            regExText = System.Text.RegularExpressions.Regex.Replace(regExText, "\\|{2,}", "|", regExOptions);
            regExText = System.Text.RegularExpressions.Regex.Replace(regExText, "\\|\\)", ")", regExOptions);
            return System.Text.RegularExpressions.Regex.Replace(val, regExText, "-", regExOptions);

        }

        public static bool IsNumeric(this string s)
        {
            Regex reNum = new Regex(@"^\d+$");
            return reNum.Match(s).Success;
        }

        public static string Translate(this string skey,string lang="")
        {
            if (lang == "")
                lang = Helpers.LanguageSetting.Lang;

            // private DbModel db = new DbModel();
            return resourceProvider.GetResource(skey, lang).ToString();
            //return (string)db.LanguageResource.Where(x=>x.lang=="it").Select(x=>x.datavalue).First();
        }

        public static string escapeForSql(this string s)
        {
            if (s == null)
                return "";

            return s.Replace(@"\", @"\\").Replace("'", @"\'");
        }


    }
}