using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class Prodotti
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public string titolo { get; set; }
        public string collezione { get; set; }
        public string categoria { get; set; }
        public string sottocategoria { get; set; }
        public string tipologiamenu { get; set; }
        public string codice { get; set; }
        public string codicesuffisso { get; set; }
        public string tipologiaprodotto { get; set; } = "prodotto";
        public decimal basecm { get; set; }
        public decimal altezzacm { get; set; }
        public decimal profonditacm { get; set; }
        public decimal waterlabel { get; set; } = 0;
        public string descrizione { get; set; }
        public string content { get; set; }
        public string metatitle { get; set; }
        public string metadescription { get; set; }
        public bool visibile { get; set; }

        public string imgheader { get; set; }
        public string imgmain { get; set; }
        public string imgmain2 { get; set; }
        public string icona { get; set; }
        public string pianta { get; set; }

        public string scheda { get; set; }
        public string istruzioni { get; set; }
        public string scassi { get; set; }
        public string capitolato { get; set; }
        public string prestazione { get; set; }
        public string cad { get; set; }
        public string f3ds { get; set; }
        public string revit { get; set; }
        public string archicad { get; set; }
        public string sketchup { get; set; }


        public string accessori { get; set; }
        public string finiture { get; set; }
        public string finitureexp { get; set; }



        [NotMapped]
        private DbModel db = new DbModel();
        [NotMapped]
        public string categorianame
        {
            get
            {
                if (string.IsNullOrEmpty(categoria))
                {
                    return "";
                }
                if(lang=="it")
                {
                    return categoria.Split('|')[1]; // velocizzo evitando chiamate al db per la lingua it
                }
                return db.Categorie.Where(x => x.itemgroup.ToString() == categoriaitemgroup && x.lang == lang).Select(x => x.titolo).FirstOrDefault();
            }
        }
        [NotMapped]
        public string categoriaitemgroup
        {
            get
            {
                return categoria != null ? categoria.Split('|')[0] : "";
            }
        }

        [NotMapped]
        public string sottocategorianame
        {
            get
            {
                if (string.IsNullOrEmpty( sottocategoria))
                {
                    return "";
                }
                if (lang == "it")
                {
                    return sottocategoria.Split('|')[1]; // velocizzo evitando chiamate al db per la lingua it
                }
                return db.Sottocategorie.Where(x => x.itemgroup.ToString() == sottocategoriaitemgroup && x.lang == lang).Select(x => x.titolo).FirstOrDefault();
            }
        }


        [NotMapped]
        public string sottocategoriaitemgroup
        {
            get
            {
                return sottocategoria != null ? sottocategoria.Split('|')[0] : "";
            }
        }


        [NotMapped]
        public string collezionename
        {
            get
            {
                if (string.IsNullOrEmpty(collezione))
                {
                    return "";
                }
                if (lang == "it")
                {
                    return collezione.Split('|')[1]; // velocizzo evitando chiamate al db per la lingua it
                }
                return db.Collezioni.Where(x => x.itemgroup.ToString() == collezioneitemgroup && x.lang == lang).Select(x => x.titolo).FirstOrDefault();
            }
        }

        [NotMapped]
        public string collezioneitemgroup
        {
            get
            {
                return collezione != null ? collezione.Split('|')[0] : "";
            }
        }

        [NotMapped]
        public string collezioneurlname
        {
            get
            {
                return collezione != null ? db.Collezioni.Where(x=>x.itemgroup.ToString()==collezioneitemgroup && x.lang==lang).Select(x=>x.urlname).FirstOrDefault().ToString() : "";
            }
        }
        [NotMapped]
        public string collezioneurlnameit
        {
            get
            {
                return collezione != null ? db.Collezioni.Where(x => x.itemgroup.ToString() == collezioneitemgroup && x.lang == "it").Select(x => x.urlname).FirstOrDefault().ToString() : "";
            }
        }
        [NotMapped]
        public string codiceCompleto
        {
            get
            {
                return codice + codicesuffisso;
            }
        }

        [NotMapped]
        public List<Prodotti> ListaAccessori
        {
            get
            {
                if (accessori == null)
                {
                    return new List<Prodotti>();
                }
                else
                {
                    List<String> el = accessori.Split(';').ToList().Select(x=>x.Split('|')[0]).ToList();
                    return db.Prodotti.Where(x=>el.Contains(x.itemgroup.ToString()) && x.lang==lang).ToList();
                }
            }
        }

        [NotMapped]
        public List<Finiture> ListaFiniture
        {
            get
            {
                if (finiture == null)
                {
                    return new List<Finiture>();
                }
                else
                {
                    Func<string, string> getIg = v => v.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                    // segno come selected le finiture passate come value
                    string[] righe = finiture
                        .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => getIg(x)).ToArray();

                    return db.Finiture.Where(x => righe.Contains(x.itemgroup.ToString()) && x.lang == lang).ToList();
                }
            }
        }

        //TODO - Modifica temp in attesa marzo 2020 listino ita
        [NotMapped]
        public List<Finiture> ListaFinitureEstero
        {
            get
            {
                if (finitureexp == null)
                {
                    return new List<Finiture>();
                }
                else
                {
                    Func<string, string> getIg = v => v.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    
                    // segno come selected le finiture passate come value
                    string[] righe = finitureexp
                        .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => getIg(x)).ToArray();

                    return db.Finiture.Where(x => righe.Contains(x.itemgroup.ToString()) && x.lang == lang).ToList();
                }
            }
        }

    }
}