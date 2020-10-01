using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToExcel;
using WebSite.Models;
using System.Text;

namespace CeramicaGlobo2018
{
    public partial class _00importxls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var excel = new ExcelQueryFactory(Server.MapPath("/import/Listino34export.xls"));
            //var excel = new ExcelQueryFactory(Server.MapPath("/public/imports/COLORSIRCA_PNW_ACQUA_INT_EXT.XLS"));
            IEnumerable<string> worksheetNames = excel.GetWorksheetNames();
            var columnNames = excel.GetColumnNames(worksheetNames.First());

            //var indianaCompanies = from c in excel.Worksheet<FormulaImport>()
            //                       select c;
            DbModel db = new DbModel();

            //List<tempimport> lista = excel.Worksheet<tempimport>(worksheetNames.First()).ToList<tempimport>();
            //db.tempimport.AddRange(lista);
            //db.SaveChanges();
            //List<tempimport> lista = new List<tempimport>();

            //1934
            //int contagruppi =
            //      db.tempimport.Where(x => x.langref == "all")
            //    .GroupBy(x => x.codice).Count();
            //  return;

            //db.tempimport.Where(x => x.langref == "all")
            //    .GroupBy(x => x.codice)
            //    //.ToList()
            //    .OrderBy(x=>x.Key)
            //    .Skip(1500)
            //    .Take(500)
            //    .ToList()
            //    .ForEach(x=>
            //    {
            //        lista.Add(new tempimport()
            //        {
            //            id = 0,
            //            codice = x.Key,
            //            variante = string.Join(",", x.Select(j => j.variante).ToArray()),
            //            langref = "all-grouped",
            //            descrizione = x.First().descrizione,
            //            serie = x.First().serie
            //        });
            //    }
            //    );

            //db.tempimport.AddRange(lista);
            //db.SaveChanges();

            //.Select(a => 
            //new {
            //    id =0,
            //    codice =a.Key,variante=string.Join(",",a.Select(j=>j.variante).ToArray()),
            //    langref="de-grouped",
            //    descrizione=a.First().descrizione,
            //    serie=a.First().serie
            //})
            //.ToList();

            //StringBuilder sb = new StringBuilder();
            //List<Prodotti> pr = db.Prodotti.Where(x => x.lang == "it").ToList();
            //foreach(Prodotti p in pr)
            //{
            //    if (!(string.IsNullOrEmpty(p.finiture)))
            //    {
            //        sb.AppendLine("update tempimport set finiturait=\"" + p.finiture + "\" where codice='" + p.codice + "';");
            //    }
            //}
            //Response.Write(sb.ToString());

            StringBuilder sb = new StringBuilder();

            //List<tempimport> lista = db.tempimport.Where(x => !string.IsNullOrEmpty(x.finitureit) && x.langref=="all-grouped").ToList();

            // RICOSTRUISCO LE FINITURE **********************************************************

            //foreach(tempimport t in lista)
            //{

            //    string[] finiturelines = t.finitureit.Split(';');
            //    string[] ig = t.igprodotti.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    StringBuilder nuovafinitura = new StringBuilder();

            //    //VARIANTI -- AR,AT,BI,BO,CA,CC,CS,FE,GH,MA,NE,PA,PT,RU,VI
            //    foreach(string linea in finiturelines)
            //    {
            //        if(linea.ToLower().Contains("bianco opaco") || linea.ToLower().Contains("bianco lucido"))
            //        {
            //            nuovafinitura.Append(";" + linea);
            //        }
            //        else
            //        {
            //            if (!(string.IsNullOrEmpty(t.variante)))
            //            {
            //                string[] varianti = t.variante.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //                string igfin = linea.Split('|')[0];
            //                string codicefin = db.Finiture.Where(i => i.itemgroup.ToString() == igfin).Select(i => i.codice).First();
            //                if (varianti.Contains(codicefin))
            //                {
            //                    nuovafinitura.Append(";" + linea);
            //                }
            //            }


            //        }

            //    }

            //    if (nuovafinitura.ToString() != "")
            //    {
            //        sb.AppendLine("update tempimport set finiturerebuild=\"" + nuovafinitura.ToString().Trim(';') + "\" where id=" + t.id + ";");
            //    }
            //}


            // AGGIORNO PRODOTTI TEDESCHI
            //sb.AppendLine("update prodotti set visibile=0 where lang='de';");
            //List<tempimport> lista = db.tempimport.Where(x => !string.IsNullOrEmpty(x.igprodotti) && x.langref == "de-grouped").ToList();
            //foreach(tempimport l in lista)
            //{
            //    //string[] igs = l.igprodotti.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    sb.AppendLine("update prodotti set visibile=1, finiture=\"" + l.finiturerebuild + "\" where lang='de' and itemgroup in (" + l.igprodotti.Trim(',') + ");");
            //}
            //Response.Write(sb.ToString());


            // AGGIORNO PRODOTTI EN,FR,ES,RU,CH
            sb.AppendLine("update prodotti set visibile=0 where lang in ('en','fr','es','ru','ch');");
            List<tempimport> lista = db.tempimport.Where(x => !string.IsNullOrEmpty(x.igprodotti) && x.langref == "all-grouped").ToList();
            foreach (tempimport l in lista)
            {
                //string[] igs = l.igprodotti.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                sb.AppendLine("update prodotti set visibile=1, finiture=\"" + l.finiturerebuild + "\" where (lang='en' or lang='fr' or lang='es' or lang='ru' or lang='ch') and itemgroup in (" + l.igprodotti.Trim(',') + ");");
            }
            Response.Write(sb.ToString());

        }
    }
}