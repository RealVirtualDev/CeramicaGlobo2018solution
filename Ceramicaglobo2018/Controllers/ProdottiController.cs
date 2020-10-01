using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Helpers;
using WebSite.Models;
using WebSite.Models.ViewModels;

namespace CeramicaGlobo2018.Controllers
{
    public class ProdottiController : Controller
    {
        private DbModel db = new DbModel();

        public ActionResult ProdottoTest( string collezione = "", string categoria = "", string prodotto = "")
        {
            string lang = "it";
            if (!string.IsNullOrEmpty(prodotto))
            {
                //PAGINA PRODOTTO
                Collezioni coll = db.Collezioni.Where(x => x.lang == lang && x.urlname == collezione).FirstOrDefault();
                Categorie cat = db.Categorie.Where(x => x.lang == lang && x.urlname == categoria).FirstOrDefault();

                string listacollezioniurl = (lang == "it" ? "/collezioni" : "/collections");
                string listacollezionitext = (lang == "it" ? "Collezioni" : "Collections");

                string collezioneig = coll.itemgroup.ToString();
                string categoriaig = cat.itemgroup.ToString();

                Prodotti p = db.Prodotti.Where(x => x.collezione.StartsWith(collezioneig + "|") && x.categoria.StartsWith(categoriaig + "|") && x.lang == lang && x.visibile == true && x.codice == prodotto).FirstOrDefault();

                if (p == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni"));
                }

                string link = "<a href=\"" + LanguageSetting.GetLangNavigation() + "{0}\">{1}</a>";



                ProdottoPage pp = new ProdottoPage();
                pp.Prodotto = p;

                //TODO - RIMUOVERE modifica temporanea in attesa del listino it a marzo 2020
                if (lang == "it")
                {
                    pp.Finiture = p.ListaFiniture == null ? new List<Finiture>() : p.ListaFiniture;
                }
                else
                {
                    pp.Finiture = p.ListaFinitureEstero == null ? new List<Finiture>() : p.ListaFinitureEstero;
                }

                string[] igfiniture = pp.Finiture.Select(f => f.gruppoitemgroup).ToArray();
                pp.GruppiFiniture = db.FinitureGruppi.Where(x => x.lang == lang && igfiniture.Contains(x.itemgroup.ToString())).OrderBy(x => x.ordinamento).ToList();


                pp.Accessori = p.ListaAccessori;

                pp.breadcrumb = string.Format(link, "/", "Ceramica Globo") + " | " +
                    string.Format(link, listacollezioniurl, listacollezionitext) + " | " +
                    string.Format(link, listacollezioniurl + "/" + coll.urlname, coll.titolo) + " | " +
                    string.Format(link, listacollezioniurl + "/" + coll.urlname + "/" + cat.urlname, cat.titolo) + " | " +
                    "<span>" + p.titolo + " " + p.codice + "</span>";

                pp.backurl = listacollezioniurl + "/" + coll.urlname + "/" + cat.urlname + "?p=" + prodotto;

                pp.isLogged = HttpContext.User.Identity.IsAuthenticated;
                return View("ProdottoTest", pp);

            }

            return View("ProdottoTest", null);

        }

        public ActionResult Collezioni(string lang="it",string collezione="",string categoria="",string prodotto="")
        {

            if (!string.IsNullOrEmpty(prodotto))
            {
                //PAGINA PRODOTTO
                Collezioni coll = db.Collezioni.Where(x => x.lang == lang && x.urlname == collezione).FirstOrDefault();
                Categorie cat = db.Categorie.Where(x => x.lang == lang && x.urlname == categoria).FirstOrDefault();

                string listacollezioniurl = (lang == "it" ? "/collezioni" : "/collections");
                string listacollezionitext = (lang == "it" ? "Collezioni" : "Collections");

                string collezioneig = coll.itemgroup.ToString();
                string categoriaig = cat.itemgroup.ToString();

                Prodotti p = db.Prodotti.Where(x => x.collezione.StartsWith(collezioneig + "|") && x.categoria.StartsWith(categoriaig + "|") && x.lang==lang && x.visibile==true && x.codice==prodotto).FirstOrDefault();
                
                if (p == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni"));
                }

                string link = "<a href=\"" + LanguageSetting.GetLangNavigation() + "{0}\">{1}</a>";
                
                               
                ProdottoPage pp = new ProdottoPage();
                pp.Prodotto = p;

                //TODO - RIMUOVERE modifica temporanea in attesa del listino it a marzo 2020
                //if (lang == "it" || lang=="de")
                //{
                //    pp.Finiture = p.ListaFiniture == null ? new List<Finiture>() : p.ListaFiniture;
                //}
                //else
                //{
                //    pp.Finiture = p.ListaFinitureEstero == null ? new List<Finiture>() : p.ListaFinitureEstero;
                //}

                pp.Finiture = p.ListaFiniture == null ? new List<Finiture>() : p.ListaFiniture;


                string[] igfiniture = pp.Finiture.Select(f => f.gruppoitemgroup).ToArray();
                pp.GruppiFiniture = db.FinitureGruppi.Where(x => x.lang == lang && igfiniture.Contains(x.itemgroup.ToString())).OrderBy(x => x.ordinamento).ToList();

                pp.Accessori = p.ListaAccessori;

                pp.breadcrumb = string.Format(link,  "/", "Ceramica Globo") + " | " +
                    string.Format(link, listacollezioniurl, listacollezionitext) + " | " +
                    string.Format(link, listacollezioniurl + "/" + coll.urlname, coll.titolo) + " | " +
                    string.Format(link, listacollezioniurl + "/" + coll.urlname + "/" + cat.urlname, cat.titolo) + " | " +
                    "<span>" + p.titolo + " " + p.codice + "</span>";

                pp.backurl = listacollezioniurl + "/" + coll.urlname + "/" + cat.urlname + "?p=" + prodotto;

                pp.isLogged = HttpContext.User.Identity.IsAuthenticated;
                
                return View("Prodotto", pp);
            }
            else if (!string.IsNullOrEmpty(categoria))
            {
                // COLLEZIONE CATEGORIA (INVIO I PRODOTTI DI UNA SPECIFICA CATEGORIA)

                Collezioni c = db.Collezioni.Where(x => x.urlname == collezione && x.lang == lang).DefaultIfEmpty(null).FirstOrDefault();
                if (c == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni"));
                }

                CollezioneProdotti cp = new CollezioneProdotti();
                cp.Collezione = c;
                cp.Prodotti = new List<Prodotti>();
                cp.Sottocategorie = new Dictionary<int, string>();

                string igcat = db.Categorie.Where(x => x.urlname == categoria && x.lang == lang).Select(x => x.itemgroup).FirstOrDefault().ToString();

                // ECCEZIONE VASI E BIDET 4ALL
                cp.Prodotti = db.Prodotti
                    .Where(x => x.collezione.StartsWith(c.itemgroup.ToString() + "|") && x.lang == lang && x.visibile == true && x.tipologiaprodotto == "prodotto" && x.categoria.StartsWith(igcat + "|"))
                    //.OrderBy(x => x.codice).ToList();
                    .OrderByDescending(x => x.basecm).ThenByDescending(x => x.altezzacm).ThenByDescending(x => x.profonditacm).ThenBy(x => x.codice)
                    .ToList();



                string[] igsubcat = cp.Prodotti.GroupBy(x => x.sottocategoria).Select(x => x.FirstOrDefault()).ToList().Select(x => x.sottocategoriaitemgroup).ToArray();

                List<Sottocategorie> sc = db.Sottocategorie.Where(x => x.lang == lang && igsubcat.Contains(x.itemgroup.ToString())).OrderBy(x => x.ordinamento).ToList();
                sc.ForEach(x =>
                {
                    cp.Sottocategorie.Add(x.itemgroup, x.titolo);
                });

                cp.CategoriaName = db.Categorie.Where(x=>x.itemgroup.ToString()==igcat && x.lang==lang).Select(x=>x.titolo).FirstOrDefault();
                cp.CollezioneLink = (lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni") + "/" + c.urlname;
                cp.CategoriaUrlName = categoria;

                return View("CollezioneCategoria", cp);
            }
            else if (!string.IsNullOrEmpty(collezione))
            {
                // COLLEZIONE SPECIFICA
                Collezioni c = db.Collezioni.Where(x => x.urlname == collezione && x.lang == lang).DefaultIfEmpty(null).FirstOrDefault();
                if (c == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni"));
                }

                CollezioneCategorie cat = new CollezioneCategorie();
                cat.Collezione = c;
                cat.Categorie = new List<CategoriaLink>();

                // categorie
                List<Prodotti> prodotti = db.Prodotti.Where(x => x.collezione.StartsWith(c.itemgroup.ToString() + "|") && x.lang == lang && x.visibile==true && x.tipologiaprodotto=="prodotto").GroupBy(x => x.categoria).Select(x=>x.FirstOrDefault()).ToList();
                string collezioneurlnameita = db.Collezioni.Where(x => x.lang == "it" && x.itemgroup == c.itemgroup).Select(x => x.urlname).FirstOrDefault();
                string[] igcat;

              
                igcat = prodotti.Select(x => x.categoriaitemgroup).ToArray();

                List<Categorie> cc = db.Categorie.Where(x => igcat.Contains( x.itemgroup.ToString()) && x.lang == lang).OrderBy(x => x.ordinamento).ToList();
                cc.ForEach(x =>
                {
                    string urlinvariant = db.Categorie.Where(coll => coll.itemgroup == x.itemgroup && coll.lang == "it").Select(coll => coll.urlname).FirstOrDefault();

                    string imgcat = "/public/resource/prodotti/imgcategorie/" + urlinvariant + "/" + collezioneurlnameita + ".jpg";

                    if (!System.IO.File.Exists(Server.MapPath(imgcat)))
                    {
                        imgcat = prodotti.Where(p => p.categoria.StartsWith(x.itemgroup.ToString() + "|")).Select(p => p.imgmain).FirstOrDefault();
                    }
                    // se non ci sono prodotti non visualizzo la categoria|
                    if(db.Prodotti.Where(j=>j.collezione.StartsWith(c.itemgroup+"|") && j.categoria.StartsWith(x.itemgroup + "|") && j.visibile==true && j.lang==lang && j.tipologiaprodotto!="accessorio").Count() > 0)
                    {
                        cat.Categorie.Add(new CategoriaLink
                        {
                            Categoria = x.titolo,
                            img = imgcat,
                            link = x.urlname
                        });
                    }
                    

                });

                return View("CollezioniDettaglio", cat);
            }
            else
            {
                // LISTA COLLEZIONI
                PageInfo p = db.PageInfo.Where(x => x.pname == "collezioni" && x.lang == lang)?.FirstOrDefault();
                return View(p);
            }

            
 
        }

        public ActionResult Tipologie(string lang = "it", string tipologia = "",string prodotto="",string collezione="")
        {

            if (!string.IsNullOrEmpty(prodotto))
            {
                // PRODOTTO
                Collezioni coll = db.Collezioni.Where(x => x.lang == lang && x.urlname == collezione).FirstOrDefault();
                TipologieMenu tip = db.TipologieMenu.Where(x => x.lang == lang && x.urlname == tipologia).FirstOrDefault();

                string listatipologieurl = (lang == "it" ? "/tipologie" : "/typologies");
                string listatipologietext = (lang == "it" ? "Tipologie" : "Typologies");
                string listacollezioniurl = (lang == "it" ? "/collezioni" : "/collections");
                
                string collezioneig = coll.itemgroup.ToString();
                string tipologiaig = tip.itemgroup.ToString();

                Prodotti p = db.Prodotti.Where(x => x.collezione.StartsWith(collezioneig + "|") && x.tipologiamenu.StartsWith(tipologiaig + "|") && x.lang == lang && x.visibile == true && x.codice == prodotto).FirstOrDefault();

                if (p == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "collections" : "collezioni"));
                }

                string link = "<a href=\"" + LanguageSetting.GetLangNavigation() + "{0}\">{1}</a>";


                ProdottoPage pp = new ProdottoPage();
                pp.Prodotto = p;
                pp.Finiture = p.ListaFiniture == null ? new List<Finiture>() : p.ListaFiniture;
                string[] igfiniture = pp.Finiture.Select(f => f.gruppoitemgroup).ToArray();
                pp.GruppiFiniture = db.FinitureGruppi.Where(x => x.lang == lang && igfiniture.Contains(x.itemgroup.ToString())).OrderBy(x => x.ordinamento).ToList();
                pp.Accessori = p.ListaAccessori;

                pp.breadcrumb = string.Format(link, "/", "Ceramica Globo") + " | " +
                    string.Format(link, listatipologieurl, listatipologietext) + " | " +
                    string.Format(link, listatipologieurl + "/" + tip.urlname, tip.titolo) + " | " +
                    string.Format(link, listacollezioniurl + "/" + coll.urlname , coll.titolo) + " | " +
                    "<span>" + p.titolo + " " + p.codice + "</span>";

               // pp.backurl = listacollezioniurl + "/" + coll.urlname;
                pp.backurl = listatipologieurl + "/" + tip.urlname + "?p=" + prodotto;


                pp.isLogged = HttpContext.User.Identity.IsAuthenticated;

                return View("Prodotto", pp);
                

            }
            else if (!string.IsNullOrEmpty(collezione))
            {
                // REDIRECT TO COLLEZIONE
                return Redirect(lang == "it" ? "/collezioni/" + collezione : "/collections/" + collezione);
            }
            else if (!string.IsNullOrEmpty(tipologia))
            {
                // DETTAGLIO TIPOLOGIA
                // lista prodotti per tipologia
                TipologieProdotti tp = new TipologieProdotti();

                TipologieMenu c = db.TipologieMenu.Where(x => x.urlname == tipologia && x.lang == lang).DefaultIfEmpty(null).FirstOrDefault();
                if (c == null)
                {
                    Response.Redirect((lang != "it" ? "/" + lang : "") + "/" + (lang != "it" ? "typologies" : "tipologie"));
                }

                tp.Metatitle = c.metatitle;
                tp.Metadescription = c.metadescription;
                tp.TipologiaName = c.titolo;
                tp.TipologieLink = (lang == "it" ? "/tipologie" : ("/" + lang + "/typologies"));
                tp.TipologiaUrlname = tipologia;

                List<Prodotti> lp = db.Prodotti
                    .Where(x => x.tipologiamenu.StartsWith(c.itemgroup + "|") && x.lang == lang && x.visibile==true)
                    .OrderByDescending(x => x.basecm).ThenByDescending(x => x.altezzacm).ThenByDescending(x => x.profonditacm).ThenBy(x => x.codice)
                    .ToList();

                tp.Prodotti = lp;

                return View("TipologieDettaglio", tp);
            }
            else
            {
                // LISTA TIPOLOGIE
                PageInfo p = db.PageInfo.Where(x => x.pname == "tipologie" && x.lang == lang)?.FirstOrDefault();
                return View(p);
            }

           

        }


        [HttpPost]
        public ActionResult getCollezioneCategorie(string igcollezione)
        {
           

            if (igcollezione == "0" || igcollezione == "")
            {
                Dictionary<string, string> result = db.Categorie.Where(x => x.lang == LanguageSetting.Lang ).OrderBy(x => x.titolo).ToDictionary(x => x.itemgroup.ToString(), x => x.titolo);
                //return Json(result);

                Dictionary<string, string> fin = new Dictionary<string, string>();
                //db.Finiture.Where(x => x.lang == LanguageSetting.Lang)
                //    .OrderBy(x => x.titolo)
                //    .GroupBy(x => x.titolo)
                //    .Select(x => x.FirstOrDefault())
                //    .ToList()
                //    .ForEach(x =>
                //    {
                //        fin.Add(x.itemgroup, x.titolo);
                //    });

                db.FinitureGruppi.Where(x => x.lang == LanguageSetting.Lang && x.itemgroup!=11).OrderBy(x => x.titolo).ToList().ForEach(x =>
                {
                    fin.Add(x.itemgroup.ToString(), x.titolo);
                });

                return Json(new { categorie = result, finiture = fin });
            }
            else
            {
                List<Prodotti> prodotti = db.Prodotti.Where(x => x.collezione.StartsWith(igcollezione + "|") && x.lang == LanguageSetting.Lang).ToList();
                List<string> pp = prodotti.Select(x => x.categoria).Distinct().ToList();
                string[] ig = pp.Select(x => x.Split('|')[0]).ToArray();
                Dictionary<string, string> result = db.Categorie.Where(x => x.lang == LanguageSetting.Lang && ig.Contains(x.itemgroup.ToString())).OrderBy(x => x.titolo).ToDictionary(x => x.itemgroup.ToString(), x => x.titolo);
                //return Json(result);

                // le finiture devono essere filtrate
                List<Finiture> allfin = db.Finiture.Where(x => x.visibile == true && x.lang == LanguageSetting.Lang).ToList();
                List<Finiture> filteredfin = new List<Finiture>();

                List<FinitureGruppi> fingruppi = db.FinitureGruppi.Where(x => x.lang == LanguageSetting.Lang && x.itemgroup != 11).OrderBy(x => x.titolo).ToList();
                List<FinitureGruppi> fingruppifiltered = new List<FinitureGruppi>();

                foreach (FinitureGruppi fg in fingruppi)
                {
                    if (prodotti.Where(x => x.finiture != null && (x.finiture.Contains("|" + fg.itemgroup.ToString() + "|"))).Count() > 0)
                    {
                        fingruppifiltered.Add(fg);
                    }
                }

                //foreach (Finiture f in allfin)
                //{
                //    if(prodotti.Where(x=>x.finiture!=null && (x.finiture.StartsWith(f.itemgroup.ToString()+"|") || x.finiture.Contains(";" + f.itemgroup.ToString() + "|"))).Count() > 0)
                //    {
                //        filteredfin.Add(f);
                //    }
                //}

                Dictionary<string, string> fin = new Dictionary<string, string>();

                fingruppifiltered
                    .OrderBy(x => x.titolo)
                    .GroupBy(x => x.titolo)
                    .Select(x => x.FirstOrDefault())
                    .ToList()
                    .ForEach(x =>
                    {
                        fin.Add(x.itemgroup.ToString(), x.titolo);
                    });

                return Json(new { categorie = result, finiture = fin });

            }

        }

        [HttpPost]
        public ActionResult getCollezioneDimensioni(string igcollezione,string igcategoria,string tipodimensione)
        {

           List<decimal> dim = db.Prodotti
                .Where(x => igcollezione!="0" ? x.collezione.StartsWith(igcollezione + "|") : x.collezione!="0")
                .Where(x => igcategoria!="0" ? x.categoria.StartsWith(igcategoria + "|") : x.categoria!="")
                .Where(x=> tipodimensione == "0" ? x.basecm!=0 : x.altezzacm!=0)
                .Select(x=>tipodimensione=="0" ?  x.basecm : x.altezzacm)
                .Distinct()
                .OrderBy(x=>x)
                .ToList();

            return Json(dim);

        }


        public ActionResult Search(string q,string coll="0",string cat="0",string d="0",string td="0",string f="0")
        {

            decimal dimensionevalue = Convert.ToDecimal(d.ToLower().Replace("cm","").Trim());

            ViewBag.titolo =LanguageSetting.Lang=="it"? "Cerca: " +  q : "Search: " + q;
            List<Categorie> c = db.Categorie.Where(x => x.lang == LanguageSetting.Lang).ToList();
            int igcollezioni = db.Collezioni.Where(x => x.titolo.ToString().ToLower().Trim().Replace("+", "") == q.ToLower().Trim().Replace("+","") && x.lang== LanguageSetting.Lang).Select(x => x.itemgroup).DefaultIfEmpty(0).FirstOrDefault();

            var query = db.Prodotti.Where(x=>x.lang==LanguageSetting.Lang && x.tipologiaprodotto=="prodotto" && x.visibile==true);
            if (q != "")
            {
                if (igcollezioni > 0)
                {
                    // ho cercato una collezione
                    query = query.Where(x =>  x.collezione.StartsWith(igcollezioni.ToString() + "|"));
                }
                else
                {
                    query = query.Where(x => x.titolo.Contains(q) | q.Contains(x.titolo) | x.descrizione.Contains(q) | x.codice.Contains(q) | q.Contains(x.codice));
                }
                //query = query.Where(x => x.codice.Contains(q) | q.Contains(x.codice));
            }

            // List<Prodotti> p = db.Prodotti
            //    .Where(x => x.lang == LanguageSetting.Lang && (x.codice.Contains(q) | q.Contains(x.codice) | x.titolo.Contains(q) | q.Contains(x.titolo) | x.descrizione.Contains(q)))
                //.OrderBy(x=>x.codice)

               List<Prodotti> p=query.ToList()
                .Where(x => coll != "0" ? x.collezione.StartsWith(coll + "|") : x.collezione != "0")
                .Where(x => cat != "0" ? x.categoria.StartsWith(cat + "|") : x.categoria != "0")
                .Where(x => td == "0" ? (d!="0" ?  x.basecm== dimensionevalue : x.basecm!=-1)  : (d != "0" ? x.altezzacm == dimensionevalue : x.altezzacm != -1))
                .Where(x=>x.visibile==true)
                .OrderByDescending(x => x.basecm).ThenByDescending(x => x.altezzacm).ThenByDescending(x => x.altezzacm).ThenBy(x => x.codice)
                .ToList();

            // se necessario filtro ulteriormente per gruppo finiuta selezionata
            //List<int> alligfinitura=new List<int>();
            //if (f != "0")
            //{
            //    string finituraname = db.Finiture.Where(x => x.itemgroup.ToString() == f && x.lang == LanguageSetting.Lang).Select(x=>x.titolo).FirstOrDefault();
            //    alligfinitura = db.Finiture.Where(x => x.lang == LanguageSetting.Lang && x.titolo == finituraname).Select(x => x.itemgroup).ToList();

            //}

            p.ForEach(pr =>
            {
                pr.categoria = c.Where(x => x.itemgroup.ToString() == pr.categoria.Split('|')[0]).Select(x => x.urlname).FirstOrDefault();
               // bool trovato = true;

                //foreach (int ig in alligfinitura)
                //{
                //    try
                //    {
                //        if (!(pr.finiture != null && pr.finiture.StartsWith(ig.ToString() + "|") || pr.finiture.Contains(";" + ig.ToString() + "|")))
                //        {
                //            trovato = false;
                //        }
                //        else
                //        {
                //            trovato = true;
                //            break;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        trovato = false;
                //    }

                //}

                //if (!trovato)
                //{
                //    pr.visibile = false;
                //}

            });
            if(f!="0")
            {
                return View("SearchResult", p.Where(x => x.finiture != null && x.finiture.Contains("|" + f + "|")).ToList());

            }
            else
            {
                return View("SearchResult", p.ToList());

            }
        }

        public ActionResult AdvancedSearch(string lang="it")
        {
            AdvancedSearch adv= new AdvancedSearch();
            PageInfo p = db.PageInfo.Where(x => x.lang == lang && x.pname == "ricercaavanzata").FirstOrDefault();
            adv.PageInfo = p;
            adv.Larghezze = db.Prodotti.Select(x => x.basecm).Distinct().OrderBy(x => x).ToArray();
            adv.Profondita = db.Prodotti.Select(x => x.profonditacm).Distinct().OrderBy(x => x).ToArray();
            adv.Collezioni = db.Collezioni.Where(x => x.lang == lang && x.visibile==true).OrderBy(x=>x.titolo).ToDictionary(x=>x.itemgroup.ToString(), x=>x.titolo);
            adv.Categorie = db.Categorie.Where(x => x.lang == lang).OrderBy(x=>x.titolo).ToDictionary(x=>x.itemgroup.ToString(), x=>x.titolo);

            //adv.Finiture = db.Finiture.Where(x => x.lang == lang).OrderBy(x => x.titolo).ToList();
            Dictionary<int, string> fin = new Dictionary<int, string>();

            db.FinitureGruppi.Where(x => x.lang == lang && x.itemgroup != 11).OrderBy(x => x.titolo).GroupBy(x => x.titolo)
                .Select(x => x.FirstOrDefault())
                .ToList()
                .ForEach(x =>
                {
                    fin.Add(x.itemgroup, x.titolo);
                });
                adv.Finiture = fin;

            return View("RicercaAvanzata",adv);
        }


        public ActionResult Compara(string ig)
        {
            string[] igs = ig.Split('-');
            if (igs.Length==0 || igs.Length>5)
            {
                return Redirect(LanguageSetting.GetLangNavigation() + "/");
            }
            List<Prodotti> lp = db.Prodotti.Where(x => x.lang == LanguageSetting.Lang && igs.Contains(x.itemgroup.ToString())).ToList();
            return View(lp);

        }

    }
}