using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;
using WebSite.Helpers;

namespace CeramicaGlobo2018.Controllers
{

    public class AziendaController : Controller
    {
        private DbModel db = new DbModel();

        public ActionResult Index(string pname,string lang="it")
        {


            // travaso
            //Pagine pa = db.Pagine.Where(x => x.pagina == "CERASLIDE").First();
            //string q = "";

            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('it',\"" + pa.pagina + "\",\"" + pa.testo.Replace("\"","\"\"") + "\",\"" + pa.metadescription?.Replace("\"", "\"\"") + "\");" +  System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('en',\"" + pa.pagina + "\",\"" + pa.testo_en.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_en?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('fr',\"" + pa.pagina + "\",\"" + pa.testo_fr.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_fr?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('es',\"" + pa.pagina + "\",\"" + pa.testo_es.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_es?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('de',\"" + pa.pagina + "\",\"" + pa.testo_de.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_de?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('ru',\"" + pa.pagina + "\",\"" + pa.testo_ru?.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_ru?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;
            // q += "insert into pageinfo(lang,titolo,content,metadescription) values('ru',\"" + pa.pagina + "\",\"" + pa.testo_ch?.Replace("\"", "\"\"") + "\",\"" + pa.metadescription_ch?.Replace("\"", "\"\"") + "\");" + System.Environment.NewLine;

            PageInfo p = db.PageInfo.Where(x => x.urlname == pname && x.lang == lang)?.FirstOrDefault();
            if (p == null)
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            string invariantPname = p.pname.Replace("-","");
            // effettuo il rendering della view chiamata
            return View(invariantPname, p);
        }
    }
}