using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebSite.Infrastructure.Security;
using WebSite.Models;
using WebSite.Areas.Admin.Models.ViewModels;
using Newtonsoft.Json;
using System.Data.Entity;
using WebSite.Infrastructure;
using WebSite.Helpers;
using System.Data;
using System.Reflection;
using System.Text;


namespace WebSite.Areas.Admin.Controllers
{
    public class ResourcesController : Controller
    {
        private DbModel db = new DbModel();


        #region RISORSE_SPECIFICHE

        [AuthorizeRole("admin")]
        public ActionResult LoadResourcePage(string rname)
        {
            string title;

            switch (rname)
            {

                case "comunicazione":
                    title = "LISTA COMUNICAZIONE";
                    break;
                case "certificati":
                    title = "LISTA CERTIFICATI";
                    break;
                case "conformita":
                    title = "LISTA DIC. CONFORMITÀ";
                    break;
                case "prestazione":
                    title = "LISTA DIC. PRESTAZIONE";
                    break;
                case "sectionslider":
                    title = "LISTA IMMAGINI DISSOLVENZA";
                    break;
                case "rispettoambiente":
                    title = "LISTA PARAGRAFI RISPETTO DELL' AMBIENTE";
                    break;
                case "designers":
                    title = "LISTA DESIGNERS";
                    break;
                case "referenze":
                    title = "LISTA REFERENZE";
                    break;
                case "cataloghi":
                    title = "LISTA CATALOGHI";
                    break;
                case "video":
                    title = "LISTA VIDEO AZIENDALI";
                    break;
                case "newsprodotto":
                    title = "LISTA NEWS PRODOTTO";
                    break;
                case "newsrassegnastampa":
                    title = "LISTA NEWS RASSEGNA STAMPA";
                    break;
                case "newseventi":
                    title = "LISTA NEWS EVENTI";
                    break;
                case "newscomunicatistampa":
                    title = "LISTA NEWS COMUNICATI STAMPA";
                    break;
                case "newspress":
                    title = "LISTA NEWS PRESS";
                    break;
                case "rivenditeitalia":
                    title = "LISTA RIVENDITE ITALIA";
                    break;
                case "blocchihome":
                    title = "BLOCCHI IN HOMEPAGE";
                    break;
                case "collezioni":
                    title = "COLLEZIONI";
                    break;
                case "tipologiemenu":
                    title = "MENÙ TIPOLOGIE";
                    break;
                case "sottocategorie":
                    title = "SOTTOCATEGORIE PRODOTTI";
                    break;
                case "categorie":
                    title = "CATEGORIE PRODOTTI";
                    break;
                case "finituregruppi":
                    title = "GRUPPI FINITURE";
                    break;
                case "finiture":
                    title = "FINITURE";
                    break;
                case "prodotti":
                    title = "PRODOTTI";
                    break;
                case "transazioni":
                    title = "TRANSAZIONI";
                    break;

                default:
                    title = "";
                    break;
            }

            ViewBag.Title = title;
            ViewBag.rname = rname;

           // Response.Headers["Content-Type"] = "charset=utf-8";
            return View("Resources", new AdminCommon
            {
                Language = db.Language.Where(x => x.abilitata).OrderByDescending(x => x.isdefault)
            });
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public JsonResult getResource(int pagesize, int currentpage, string rname, string refig="",string refrname="")
        {
            // senza la seguente riga, la serializzazione non funziona
            db.Configuration.ProxyCreationEnabled = false;
           

            JsonResult result = new JsonResult();
            object objlist;

            switch (rname)
            {

                case "comunicazione":
                    objlist = db.Comunicazione.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "certificati":
                    objlist = db.Certificati.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "conformita":
                    objlist = db.Conformita.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "prestazione":
                    objlist = db.Prestazione.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "sectionslider":
                    objlist = db.SectionSlider.Where(x => x.lang == "it").OrderBy(x => x.sezione).ThenBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "rispettoambiente":
                    objlist = db.RispettoAmbiente.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "designers":
                    objlist = db.Designers.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "referenze":
                    objlist = db.Referenze.Where(x => x.lang == "it").OrderBy(x => x.gruppo).ThenBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "cataloghi":
                    objlist = db.Cataloghi.Where(x => x.lang == "it").OrderByDescending(x => x.data)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "video":
                    objlist = db.Video.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "newsprodotto":
                    objlist = db.NewsProdotto.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "newsrassegnastampa":
                    objlist = db.NewsRassegnaStampa.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "newseventi":
                    objlist = db.NewsEventi.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "newscomunicatistampa":
                    objlist = db.NewsComunicatiStampa.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "newspress":
                    objlist = db.NewsPress.Where(x => x.lang == "it").OrderByDescending(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "rivenditeitalia":
                    objlist = db.RivenditeItalia.Where(x => x.lang == "it" ).OrderBy(x => x.regione).ThenBy(x=>x.titolo)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "blocchihome":
                    objlist = db.BlocchiHome.Where(x => x.lang == "it" ).OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "collezioni":
                    objlist = db.Collezioni.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "tipologiemenu":
                    objlist = db.TipologieMenu.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "sottocategorie":
                    objlist = db.Sottocategorie.Where(x => x.lang == "it")
                        //.ToList()
                        //.OrderBy(x => x.categorianame).ThenBy(x=>x.titolo)
                        .OrderBy(x=>x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "categorie":
                    objlist = db.Categorie.Where(x => x.lang == "it").OrderBy(x => x.ordinamento)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "finituregruppi":
                    objlist = db.FinitureGruppi.Where(x => x.lang == "it").OrderBy(x => x.titolo)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "finiture":
                    if (refig != "")
                    {
                        objlist = db.Finiture.Where(x => x.lang == "it" &&  x.gruppo.StartsWith(refig + "|")).OrderBy(x => x.titolo)
                            .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                        //objlist = db.Finiture.Where(x => x.lang == "it" &&  "gruppo LIKE @st",new objectpa).OrderBy(x => x.titolo).ToList();
                    }
                    else
                        objlist = db.Finiture.Where(x => x.lang == "it").OrderBy(x => x.gruppo).ThenBy(x=>x.titolo)
                            .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                case "prodotti":
                   // Func<string, string> getCollection = x => x.Split('|')[1];
                   // Func<string, string> getCategory = x => x.Split('|')[1];

                    objlist = db.Prodotti.Where(x => x.lang == "it").ToList()
                        .Select(x => new { x.icona, x.titolo, x.tipologiamenu, x.codice, x.itemgroup, x.collezione, x.categoria,x.collezionename,x.categorianame,x.tipologiaprodotto,x.sottocategorianame,x.visibile })
                        .OrderBy(x =>x.collezionename)
                        .ThenBy(x=> x.categorianame)
                        .ThenBy(x=>x.tipologiamenu)
                        .ThenBy(x=>x.titolo)
                        .ToList()
                        .Skip(pagesize*(currentpage-1))
                        .Take(pagesize);

                    break;
                case "transazioni":
                    objlist = db.Transazioni.OrderByDescending(x => x.id)
                        .Skip(pagesize * (currentpage - 1))
                            .Take(pagesize)
                            .ToList();
                    break;
                default:
                    objlist = null;
                    break;
            }

            return Json(objlist);


        }
        
        #endregion


        #region SAVE 

        [HttpPost]
        [AuthorizeRole("admin")]
        public string savePage(resourceDetail resource)
        {
            bool modelvalid = ModelState.IsValid;
            if (!ModelState.IsValid)
            {
                string errors = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage));
                return errors;
            }

            if (resource.itemgroup == 0)
            {
                // nuovo

                int newItemGroup = db.Database.SqlQuery<int>("select ifnull(max(itemgroup+1),1) from " + resource.rname + " where lang='" + resource.lang + "'").First();
                int newOrdinamento = db.Database.SqlQuery<int>("select ifnull(max(ordinamento+1),1) from " + resource.rname + " where lang='" + resource.lang + "'").First();
                int newStartId = db.Database.SqlQuery<int>("select ifnull(max(id+1),1) from " + resource.rname + " where lang='" + resource.lang + "'").First();
                int idrisorsa = db.Resource.Where(x => x.rname == resource.rname).Select(x => x.id).First();


                resource.itemgroup = newItemGroup;

                
                // se esistono inserisco gli urlname automatici
                List<ResourceModel> rmurlname = resource.Model.Where(x => x.admintype == "urlname" && x.adminshow == false).ToList();
                foreach (ResourceModel rm in rmurlname)
                {

                    string refer = rm.AdminParams().urlnamerefer;
                    string urlname = resource.Model.Where(x => x.propertyname == refer).Select(x => x.value).FirstOrDefault().ToSafeUrlname();
                   

                    //resource.Model.Where(x => x.propertyname == rm.propertyname).FirstOrDefault().val = urlname;
                    rm.value = urlname;

                };

                StringBuilder sb = new StringBuilder();
                sb.Append("insert into " + resource.rname + " (");
                string values = "";
                string names = "";

                foreach(ResourceModel r in resource.Model)
                {
                    names+=r.propertyname + ",";
                    
                    if (r.admintype == "date")
                    {
                        values += "'" + Convert.ToDateTime(r.value).ToString("yyyy-MM-dd") + "',";
                    }
                    else if (r.admintype == "number")
                    {
                        values +=  r.value.Replace(",",".") + ",";
                    }
                    else
                    {
                        values += "'" + r.value.escapeForSql() + "',";
                    }
                }
                names += "itemgroup,ordinamento,lang";
                values += newItemGroup + "," + newOrdinamento + ",'{0}'";

                // ITALIANO
                sb.Append(names.Trim(',') + ") values(" + string.Format( values ,resource.lang) + ");");


                // ALTRE LINGUE
                string[] langs = db.Language.Where(x => x.lang != "it" && x.abilitata == true).Select(x => x.lang).ToArray();
                 
                // STO CLONANDO??
                if (resource.igclone > 0)
                {
                    // *********************************************************************************************************
                    //
                    //      STO CLONANDO UNA RISORSA, PER LE ALTRE LINGUE DOVREI SALVARE I VALORI NON INVARIANTI IN LINGUA
                    //
                    // *********************************************************************************************************

                    foreach (string lang in langs)
                    {

                        string nameslang = "", valueslang = "";

                        foreach (ResourceModel r in resource.Model)
                        {

                            nameslang += r.propertyname + ",";

                            if (r.isinvariant)
                            {
                                // prendo il valore it del modello passato
                                if (r.admintype == "date")
                                {
                                    valueslang += "'" + Convert.ToDateTime(r.value).ToString("yyyy-MM-dd") + "',";
                                }
                                else if (r.admintype == "number")
                                {
                                    valueslang += r.value.Replace(",", ".") + ",";
                                }
                                else
                                {
                                    valueslang += "'" + r.value.escapeForSql() + "',";
                                }
                            }
                            else
                            {
                                // devo prendere il valore dal db nella lingua specifica
                                string dbval = db.Database.SqlQuery<String>("SELECT CAST(" + r.propertyname + " AS CHAR) FROM " + resource.rname + " where itemgroup=" + resource.igclone + " and lang='" + lang + "'").DefaultIfEmpty("").First();

                                if (r.admintype == "date")
                                {
                                    valueslang += "'" + Convert.ToDateTime(dbval).ToString("yyyy-MM-dd") + "',";
                                }
                                else if (r.admintype == "number")
                                {
                                    valueslang += dbval.Replace(",", ".") + ",";
                                }
                                else
                                {
                                    valueslang += "'" + dbval.escapeForSql() + "',";
                                }
                            }

                        }

                        sb.Append("insert into " + resource.rname + " (");
                        nameslang += "itemgroup,ordinamento,lang";
                        valueslang += newItemGroup + "," + newOrdinamento + ",'{0}'";
                        sb.AppendLine(nameslang.Trim(',') + ") values(" + string.Format(valueslang, lang) + ");");

                    }

                }
                else
                {
                    foreach (string lang in langs)
                    {
                        sb.Append("insert into " + resource.rname + " (");

                        sb.AppendLine(names.Trim(',') + ") values(" + string.Format(values, lang) + ");");

                    }
                }
               
                


                db.Database.ExecuteSqlCommand(sb.ToString());
                
                // gallery
                saveGallery(ref resource);
                if (resource.hasgallery && !(resource.Gallery == null) && resource.Gallery.Count > 0)
                    resource.Gallery.ForEach(x => db.ResourceGallery.Add(x));
                //Files
                saveFiles(ref resource);
                if (resource.hasfiles && !(resource.Files == null) && resource.Files.Count > 0)
                    resource.Files.ForEach(x => db.ResourceFiles.Add(x));
                
            }
            else
            {

                // save gallery
                if (resource.hasgallery)
                {
                    saveGallery(ref resource);
                }
                // save Files
                if (resource.hasfiles)
                {
                    saveFiles(ref resource);
                }

                string values = "";
                StringBuilder sb = new StringBuilder();
                string valuesinvariant = "";
                StringBuilder sbinvariant = new StringBuilder(); // usato per campi invarianti, cioè da salvare in tutte le lingue

                sb.Append("update " + resource.rname + " set ");
                
               


                List<ResourceModel> rmurlname = resource.Model.Where(x => x.admintype == "urlname" && x.adminshow == false).ToList();
                foreach (ResourceModel rm in rmurlname)
                {

                    string refer = rm.AdminParams().urlnamerefer;
                    string urlname = resource.Model.Where(x => x.propertyname == refer).Select(x => x.value).FirstOrDefault().ToSafeUrlname();

                    // NON UTILIZZO LE LINGUE CINESE E RUSSO PER LE PAGINE
                    if (resource.lang == "ch" || resource.lang == "ru")
                    {
                        // uso il riferimento inglese o italiano
                        string langrefer = "en";
                        if (db.Language.Where(x => x.lang == "en").Count() == 0)
                            langrefer = "it";

                        urlname = db.Database.SqlQuery<string>("select " +rm.propertyname + " from " + rm.rname + " where itemgroup=" + rm.itemgroup + " and lang='" + langrefer  + "'" ).FirstOrDefault();
                    }

                    //resource.Model.Where(x => x.propertyname == rm.propertyname).FirstOrDefault().val = urlname;
                    rm.value = urlname;

                };

                foreach (ResourceModel r in resource.Model)
                {
                    string tempval = "";
                    
                    if (!string.IsNullOrEmpty(r.value))
                    {
                        if (r.admintype == "date")
                        {
                            tempval += "='" + Convert.ToDateTime(r.value).ToString("yyyy-MM-dd") + "',";
                        }
                        else if (r.admintype == "number")
                        {
                            tempval += "=" + r.value.Replace(",", ".") + ",";
                        }
                        else
                        {
                            tempval += "='" + r.value.escapeForSql() + "',";
                        }
                    }
                    else
                    {
                        tempval += "='',";
                    }

                    if (r.isinvariant)
                    {
                        // devo aggiornare tutte le lingue
                        valuesinvariant +=r.propertyname + tempval;
                    }
                    else
                    {
                        values += r.propertyname + tempval;
                    }
                    

                    
                }

                sb.Append(values.Trim(','));
                sb.Append(" where itemgroup=" + resource.itemgroup + " and lang='" + resource.lang + "';");

                sbinvariant.Append(valuesinvariant.Trim(','));

                if (!string.IsNullOrEmpty(sbinvariant.ToString()))
                {
                    sbinvariant.Insert(0,"update " + resource.rname + " set ");
                    sbinvariant.Append(" where itemgroup=" + resource.itemgroup + ";");
                }
                
                
                db.Database.ExecuteSqlCommand(sb.AppendLine(sbinvariant.ToString()).ToString());

            }

            // chiamo operazioni aggiuntive del sito specifico
            if (resource.itemgroup != 0)
            {
                saveAdd(resource);
            }
            

            return "OK";
        }

        private void saveAdd(resourceDetail resource)
        {
            //db.Database.ExecuteSqlCommand(sb.ToString());'
            StringBuilder sb = new StringBuilder();
           
            switch (resource.rname.ToLower())
            {
                case "collezioni":
                    string collezioneit = db.Collezioni.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                    // aggiorno i prodotti
                    sb.AppendLine("update prodotti set collezione='" + resource.itemgroup + "|" + collezioneit + "' where collezione like '" + resource.itemgroup + "|%';");
                    break;
                case "categorie":
                    string categoriait = db.Categorie.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                    // aggiorno i prodotti
                    sb.AppendLine("update prodotti set categoria='" + resource.itemgroup + "|" + categoriait + "' where categoria like '" + resource.itemgroup + "|%';");
                    // aggiorno le sottocategorie
                    sb.AppendLine("update sottocategorie set categoria='" + resource.itemgroup + "|" + categoriait + "' where categoria like '" + resource.itemgroup + "|%';");
                    break;
                case "tipologiemenu":
                    string tipologiait = db.TipologieMenu.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                    // aggiorno i prodotti
                    sb.AppendLine("update prodotti set tipologiamenu='" + resource.itemgroup + "|" + tipologiait + "' where tipologiamenu like '" + resource.itemgroup + "|%';");
                    break;
                case "sottocategorie":
                    string sottocategoriait = db.Sottocategorie.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                    // aggiorno i prodotti
                    sb.AppendLine("update prodotti set sottocategoria='" + resource.itemgroup + "|" + sottocategoriait + "' where sottocategoria like '" + resource.itemgroup + "|%';");
                    break;
                case "finituregruppi":
                    //152|Salice|11|Finiture Disponibili;8|Betulla|11|Finiture Disponibili;153|Smoke|11|Finiture Disponibili;68|Desert|11|Finiture Disponibili
                    //igfinitura|finiturait|igfinituragruppo|finituragruppoit
                    string finituragruppoit= db.FinitureGruppi.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                   
                    List<Prodotti> lp1 = db.Prodotti.Where(x =>x.lang=="it" && x.finiture.Contains("|" + resource.itemgroup.ToString() + "|")).ToList();
                    foreach(Prodotti p in lp1)
                    {
                        string newfiniture = "";
                        string[] finiture = p.finiture.Split(';');
                        foreach(string f in finiture)
                        {
                            if(f.Split('|')[2]== resource.itemgroup.ToString())
                            {
                                // aggiorno finitura
                                newfiniture += f[0] + "|" + f[1] + "|" + f[2] + "|" + finituragruppoit + ";";
                            }
                            else
                            {
                                newfiniture += f + ";";
                            }
                        }

                        if (!string.IsNullOrEmpty(newfiniture))
                        {
                            sb.AppendLine("update prodotti set finiture=\"" + newfiniture.TrimEnd(';') + "\" where itemgroup=" + p.itemgroup + ";");
                        }
                    }
                    
                    break;
                case "finiture":
                    string finiturait = db.Finiture.Where(x => x.itemgroup == resource.itemgroup && x.lang == "it").Select(x => x.titolo).FirstOrDefault();
                    List<Prodotti> lp2 = db.Prodotti.Where(x => x.finiture.Contains(";" + resource.itemgroup.ToString() + "|") | x.finiture.StartsWith(resource.itemgroup.ToString() + "|")).ToList();
                    foreach (Prodotti p in lp2)
                    {
                        string newfiniture = "";
                        string[] finiture = p.finiture.Split(';');
                        foreach (string f in finiture)
                        {
                            if (f.Split('|')[0] == resource.itemgroup.ToString())
                            {
                                // aggiorno finitura
                                newfiniture += f[0] + "|" + finiturait + "|" + f[2] + "|" + f[3] + ";";
                            }
                            else
                            {
                                newfiniture += f + ";";
                            }
                        }

                        if (!string.IsNullOrEmpty(newfiniture))
                        {
                            sb.AppendLine("update prodotti set finiture=\"" + newfiniture.TrimEnd(';') + "\" where itemgroup=" + p.itemgroup + ";");
                        }
                    }
                    break;
                case "prodotti":
                    Prodotti accessorio = db.Prodotti.Where(x => x.itemgroup == resource.itemgroup && x.tipologiaprodotto != "prodotto" && x.lang == "it").DefaultIfEmpty(null).FirstOrDefault();
                    if (!(accessorio == null))
                    {
                        // è un accessorio
                        //igprodotto|codice|titolo

                        List<Prodotti> lp = db.Prodotti.Where(x => x.accessori.Contains(";" + resource.itemgroup.ToString() + "|") | x.accessori.StartsWith(resource.itemgroup.ToString() + "|")).ToList();

                        foreach (Prodotti p in lp)
                        {
                            string newaccessori = "";
                            string[] oldaccessori = p.accessori.Split(';');
                            foreach (string a in oldaccessori)
                            {
                                if (a.Split('|')[0] == resource.itemgroup.ToString())
                                {
                                    // aggiorno finitura
                                    newaccessori += a[0] + "|" + accessorio.codice + "|" + accessorio.titolo  + ";";
                                }
                                else
                                {
                                    newaccessori += a + ";";
                                }
                            }

                            if (!string.IsNullOrEmpty(newaccessori))
                            {
                                sb.AppendLine("update prodotti set accessori=\"" + newaccessori.TrimEnd(';') + "\" where itemgroup=" + p.itemgroup + ";");
                            }
                        }

                       
                    }
                    break;
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                db.Database.ExecuteSqlCommand(sb.ToString());
            }
        }

        private void saveGallery(ref resourceDetail resource)
        {
            // modifica
            int iggallery = 1;
            string basepathgallery = "/public/resource/gallery/" + resource.rname + "/";
            // creo la cartella se non esiste
            if (!System.IO.Directory.Exists(Server.MapPath(basepathgallery)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery));
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery+"min"));
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathgallery + "big"));
            }
           

            if (!(resource.Gallery == null) && !(resource.Gallery[0]==null))
            {
                StringBuilder sb = new StringBuilder();
                iggallery = resource.Gallery.Where(x => x.status == "saved").Select(x => x.itemgroup).DefaultIfEmpty(0).FirstOrDefault() + 1;

                foreach (ResourceGallery rg in resource.Gallery)
                {

                    //db.PageGallery.Attach(pg);
                    rg.itemgroupcontent = resource.itemgroup;
                    rg.rname = resource.rname;
                    rg.lang = resource.lang;
                   
                    switch (rg.status)
                    {
                        case "deleted":

                            // cancello i files
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(rg.folder + "min/" + rg.img));
                            }
                            catch (Exception e)
                            {

                            }
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(rg.folder + "big/" + rg.img));

                            }
                            catch (Exception e)
                            {

                            }

                            //db.Entry(rg).State = EntityState.Deleted;
                            sb.AppendLine("delete from " + resource.rname + "_gallery where itemgroup=" + rg.itemgroup + " and itemgroupcontent=" + rg.itemgroupcontent + " and lang='" + rg.lang + "';");
                            break;
                        case "new":
                            // elaboro e copio l'immagine


                            int icow = resource.GallerySetting.Count > 0 ? resource.GallerySetting.First().icowidth : 350;
                            int icoh = resource.GallerySetting.Count > 0 ? resource.GallerySetting.First().icoheight : 350;



                            //ir.fill(300, 300, "#ffffff").Save(Server.MapPath("/public/temp/fill01.jpg"));

                            string originalfilename = rg.img;
                            string originalfolder = rg.folder;

                            string destfilename = resource.rname + "_ig" + iggallery + "_"+ resource.itemgroup +"_" + rg.img.ToSafeFilename();
                            string safeprefix = Guid.NewGuid().ToString().Replace("-","_");

                            rg.img = destfilename;
                            rg.folder = basepathgallery;
                            rg.itemgroup = iggallery;

                            // resize icona

                            ImageResizer ir = new ImageResizer(Server.MapPath(originalfolder + originalfilename));

                            // controllo immagine con sesso nome già presente
                            if (System.IO.File.Exists(Server.MapPath(rg.folder + "min/" + rg.img)))
                            {
                                rg.img = safeprefix + rg.img;
                                
                            }
                            // MIN
                            ir.crop(icow, icoh).Save(Server.MapPath(rg.folder + "min/" + rg.img));
                            // BIG
                            System.IO.File.Copy(Server.MapPath(originalfolder + originalfilename), Server.MapPath(rg.folder + "big/" + rg.img), true);


                            // ELIMINO IL FILE TEMP
                            System.Threading.Thread.Sleep(500); // attendo 0.5 secondi
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(originalfolder + originalfilename));
                            }
                            catch (Exception e)
                            {

                            }
                            sb.AppendLine("insert into " + resource.rname + "_gallery (idrisorsa,itemgroupcontent,itemgroup,rname,folder,img,titolo,descrizione,urlvideo,lang)"+
                                "values("+rg.idrisorsa+","+rg.itemgroupcontent+ "," + rg.itemgroup + ",'" + rg.rname + "','" + rg.folder + "','" + rg.img + "','" + rg.titolo.escapeForSql() + "','" + rg.descrizione.escapeForSql() + "','" + rg.urlvideo + "','" + rg.lang +"');");

                            //db.Entry(rg).State = EntityState.Added;
                            iggallery++;

                            break;
                        default: // update
                           // rg.itemgroup = iggallery;
                            sb.AppendLine("update " + resource.rname + "_gallery " +
                               "set folder='" + rg.folder + "',img='" + rg.img + "',titolo='" + rg.titolo.escapeForSql() + "',descrizione='" + rg.descrizione.escapeForSql() + "',urlvideo='" + rg.urlvideo + "' where lang='" + rg.lang + "' and itemgroup=" + rg.itemgroup + " and itemgroupcontent=" + resource.itemgroup + ";");

                            iggallery++;
                            break;
                      
                    }
                   



                }

                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    db.Database.ExecuteSqlCommand(sb.ToString());
                }
            }
        }

        private void saveFiles(ref resourceDetail resource)
        {
            // FILES
            int igfile = 1;
            string basepathfile = "/public/resources/files/" + resource.rname + "/";

            if (!System.IO.Directory.Exists(Server.MapPath(basepathfile)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(basepathfile));
            }

            if (!(resource.Files == null) && !(resource.Files[0] == null))
            {
                StringBuilder sb = new StringBuilder();

                foreach (ResourceFiles rf in resource.Files)
                {
                    rf.itemgroupcontent = resource.itemgroup;
                    rf.rname = resource.rname;
                    rf.lang = resource.lang;

                    switch (rf.status)
                    {
                        case "deleted":


                            // cancello i files
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(rf.folder + rf.file));
                            }
                            catch (Exception e)
                            {

                            }

                            db.Entry(rf).State = EntityState.Deleted;
                            sb.AppendLine("delete from " + resource.rname + "_files where itemgroup=" + rf.itemgroup + " and itemgroupcontent=" + rf.itemgroupcontent + " and lang='" + rf.lang + "';");

                            break;
                        case "new":
                            // elaboro e copio l'immagine
                            string originalfilename = rf.file;
                            string originalfolder = rf.folder;
                            string destfilename = resource.rname + "_ig" + igfile + "_" + rf.file.ToSafeFilename();
                            rf.file = destfilename;
                            rf.folder = basepathfile ;
                            rf.itemgroup = igfile;

                            
                            System.IO.File.Copy(Server.MapPath(originalfolder + originalfilename), Server.MapPath(rf.folder + rf.file), true);
                            // ELIMINO IL FILE TEMP
                            System.Threading.Thread.Sleep(500); // attendo 0.5 secondi
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(originalfolder + originalfilename));
                            }
                            catch (Exception e)
                            {

                            }

                            //db.Entry(rf).State = EntityState.Added;

                            sb.AppendLine("insert into " + resource.rname + "_files (idrisorsa,itemgroupcontent,rname,folder,file,displayname,ico,lang)" +
                                "values(" + rf.idrisorsa + "," + rf.itemgroupcontent + ",'" + rf.rname + "','" + rf.folder + "','" + rf.file + "','" + rf.displayname.escapeForSql() + "','" + rf.ico + "','" + rf.lang + "');");


                            igfile++;

                            break;
                        default:
                            rf.itemgroup = igfile;
                            //db.Entry(rf).State = EntityState.Modified;

                            sb.AppendLine("update " + resource.rname + "_files " +
                             "set folder='" + rf.folder + "',ico='" + rf.ico + "',displayname='" + rf.displayname.escapeForSql() + "',file='" + rf.file + "' where lang='" + rf.lang + "' and itemgroup=" + rf.itemgroup + ");");
                            igfile++;
                            break;

                    }
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        db.Database.ExecuteSqlCommand(sb.ToString());
                    }

                }
            }
        }

        #endregion

        #region COMMON_METHODS
        /// <summary>
        /// Legge la tabella specifica e compila i valori nel resourceModel
        /// </summary>
        /// <param name="rname"></param>
        /// <param name="itemgroup"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private List<ResourceModel> getDatas(string rname, int itemgroup = 0, string lang = "it",bool clone=false)
        {

            List<ResourceModel> rm= db.ResourceModel.Where(x => x.rname == rname).OrderBy(x => x.ordinamento).ToList();
            //Primopiano p = db.Primopiano.Where(x => x.itemgroup == itemgroup && x.lang==lang).First();

           
                foreach (ResourceModel rmdetail in rm)
                {
                    string val = db.Database.SqlQuery<String>("SELECT CAST(" + rmdetail.propertyname + " AS CHAR) FROM " + rname + " where itemgroup=" + itemgroup + " and lang='" + lang + "'").DefaultIfEmpty("").First();

                if(itemgroup==0) // nuovo elemento setto eventuali valori di default
                {
                    if (  rmdetail.admintype == "date")
                    {
                        val = System.DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(rmdetail.jsonadminparams) && rmdetail.jsonadminparams.Contains("defaultvalue"))
                        {
                            val = rmdetail.AdminParams().defaultvalue;
                        }
                        else
                        {
                            val = "";
                        }
                    }
                    
                }

                rmdetail.value = val == null ? "" : val.ToString();
                    rmdetail.itemgroup =clone==false ? itemgroup : 0;
                    rmdetail.lang = lang;
                }
           
            return rm;
            
            
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public PartialViewResult getEditor(string rname, int itemgroup, string lang,bool clone=false)
        {

            AdminCommon viewresult = new AdminCommon();
            resourceDetail detail = new resourceDetail();

            try
            {
                detail.GallerySetting = db.ResourceGallerySetting.Where(x => x.rname == rname && x.lang == lang).ToList();
                detail.Model = getDatas(rname, itemgroup, lang, clone);

                detail.itemgroup = itemgroup;
                detail.rname = rname;
                detail.lang = lang;
                detail.hasgallery = db.Resource.Where(x => x.rname == rname).Select(x => x.hasgallery).First();
                detail.hasfiles = db.Resource.Where(x => x.rname == rname).Select(x => x.hasfiles).First();
            }
            catch
            {
                throw new Exception("Errore blocco 1");
            }



            try
            {
                if (itemgroup == 0)
                {


                    detail.Gallery = new List<ResourceGallery>();
                    detail.Files = new List<ResourceFiles>();

                }
                else
                {

                    if (detail.hasgallery)
                    {
                        try
                        {
                            detail.Gallery = db.ResourceGallery.SqlQuery("select * from " + rname + "_gallery where rname=\"" + rname + "\" and lang='" + lang + "' and itemgroupcontent=" + itemgroup).ToList();
                        }
                        catch
                        {
                            detail.Gallery=new List<ResourceGallery>();
                        }
                    }



                    if (detail.hasfiles)
                    {
                        try
                        {
                            detail.Files = db.ResourceFiles.SqlQuery("select * from " + rname + "_files where rname=\"" + rname + "\" and lang='" + lang + "' and itemgroupcontent=" + itemgroup).ToList();
                        }
                        catch
                        {
                            detail.Files = new List<ResourceFiles>();
                        }
                    }


                }

                if (clone)
                {
                    detail.igclone = detail.itemgroup;
                    detail.itemgroup = 0;

                }

            }
            catch
            {
                throw new Exception("Errore blocco 2");
            }

            viewresult.Resource = detail;



            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                
            };

            viewresult.jsonData = JsonConvert.SerializeObject(detail, settings)
            .Replace("\\", "\\\\");

           // Response.Headers["Content-Type"] = "charset=utf-8";
            return PartialView("_ResourceEditor", viewresult);


        }


        [HttpPost]
        [AuthorizeRole("admin")]
        public string delete(string rname, int itemgroup)
        {

            db.Database.ExecuteSqlCommand("delete from " + rname + " where itemgroup=" + itemgroup + ";");
            try
            {
                db.Database.ExecuteSqlCommand("delete from " + rname + "_gallery where itemgroupcontent=" + itemgroup + ";");

            }
            catch
            {

            }
            try
            {
                db.Database.ExecuteSqlCommand("delete from " + rname + "_files where itemgroupcontent=" + itemgroup + ";");

            }
            catch
            {

            }
            
            // eliminare fisicamente files e gallery

            db.SaveChanges();
            return "OK";
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public string reorder(string rname, string movedirection,string igtomove,string igtarget,string ordtomove,string ordtarget)
        {
            // direzione up
            //      ord= ord target e sposto +1 agli ordinamenti successivi
            // direzione down
            //      ordinamento=ord target +1 e gli ord <= target e > moved diventano -1
            string q = "";
           // int _ordtarget = Convert.ToInt32(db.Database.SqlQuery<int>("select ordinamento from " + rname + " where itemgroup=" + igtarget + " lang='it'").DefaultIfEmpty(0).SingleOrDefault());
           // int _ordtomove = Convert.ToInt32(db.Database.SqlQuery<int>("select ordinamento from " + rname + " where itemgroup=" + igtarget + " lang='it'").DefaultIfEmpty(0).SingleOrDefault());
            int _neword =Convert.ToInt32(ordtarget);

            if (movedirection == "up")
            {
                q+= "update " + rname + " set ordinamento=ordinamento+1 where ordinamento<" + ordtomove + " and ordinamento>=" + ordtarget + " and itemgroup<>" + igtomove + ";";
                q += "update " + rname + " set ordinamento=" + _neword + " where itemgroup=" + igtomove + ";";

            }
            else
            {
                q += "update " + rname + " set ordinamento=ordinamento-1 where ordinamento<=" + ordtarget + " and ordinamento>" + ordtomove + " and itemgroup<>" + igtomove + ";";
                q += "update " + rname + " set ordinamento=" + _neword + " where itemgroup=" + igtomove +";";
            }

            db.Database.ExecuteSqlCommand(q);
           

            return "";
        }

        [HttpPost]
        [AuthorizeRole("admin")]
        public int Count(string rname)
        {
            int result = Convert.ToInt32(db.Database.SqlQuery<int>("select count(id) from " + rname + " where lang='it'").DefaultIfEmpty(0).SingleOrDefault());
            return result;
        }

        #endregion

    }
}