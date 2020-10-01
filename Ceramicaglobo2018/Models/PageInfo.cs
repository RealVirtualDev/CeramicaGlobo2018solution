using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Data;

namespace WebSite.Models
{
    public partial class PageInfo
    {
        public int id { get; set; }
        [Key, Column(Order =0)] // indico che è una chiave per relazioni esterne
        public string pname { get; set; }
        public string sectionpath { get; set; }
        public string urlname { get; set; }
        [Required(ErrorMessage ="Il Campo Titolo è Obbligatorio")]
        public string titolo { get; set; }
        [AllowHtml]
        public string content { get; set; }
        [Required(ErrorMessage = "Il Campo Meta Title è Obbligatorio")]
        public string metatitle { get; set; } = "";
        public string metakeywords { get; set; } = "";
        [Required(ErrorMessage = "Il Campo Meta Description è Obbligatorio")]
        public string metadescription { get; set; } = "";
        [Key, Column(Order = 1)]
        public string lang { get; set; }
        public bool modificabile { get; set; }
        public bool hasgallery { get; set; }
        public bool hasfiles { get; set; }
        

        // trasformo la collezione di ModelDatas in datatable
        public DataTable ModelDataTable()
        {
            
                System.Data.DataTable result = new System.Data.DataTable();

                // aggiungo le colonne alla tabella
                foreach (PageModel tmr in Model)
                {
                    result.Columns.Add((string)tmr.propertyname);
                }

                //var t = Webutil.db.getDataTable("select * from pagine_content where pname=\"" + pname + "\" and (lang='" + lang + "' or lang='" + lang + "-all' or lang='all')");

                // compilo con i dati della collezione page.modelDatas
                if (ModelDatas.Count > 0)
                {
                    var r = result.NewRow();
                    foreach (PageModelDatas md in ModelDatas)
                    {
                        r[md.name] = md.val;
                    }
                    result.Rows.Add(r);
                }
                return result;
             
        }

        // navigation
        public virtual ICollection<PageGallery> Gallery { get; set; }
        public virtual ICollection<PageGallerySetting> GallerySetting { get; set; }
        public virtual ICollection<PageFiles> Files { get; set; }
        public virtual ICollection<PageModel> Model { get; set; }
        public virtual ICollection<PageModelDatas> ModelDatas { get; set; }

        
      

    }

    

  

   

}