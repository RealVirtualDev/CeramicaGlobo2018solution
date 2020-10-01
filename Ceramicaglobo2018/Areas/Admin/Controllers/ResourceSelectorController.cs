using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Areas.Admin.Controllers
{
    public class ResourceSelectorController : Controller
    {
        private DbModel db = new DbModel();

        #region render

        // GET: Admin/ResourceSelector
        public ActionResult DropDownList(string ddlmodelstr,string currentValue,string propertyName, string lang)
        {

            DropDownListModel ddlmodel = JsonConvert.DeserializeObject<DropDownListModel>(ddlmodelstr.TrimStart('[').TrimEnd(']'));
            ddlmodel.lang = lang;
            ddlmodel.selectedValue = currentValue;
           
           


            ddlmodel.propertyName = propertyName;

            string resultquery = "";
            if (ddlmodel.returnFields.Count() > 1)
            {
                //CONVERT(CAST(city as BINARY) USING utf8)
                resultquery = "CONVERT(CAST(CONCAT(" + string.Join(",'|',", ddlmodel.returnFields) + ") as BINARY) USING utf8)";
            }
            else
            {
                resultquery = ddlmodel.returnFields[0];
            }

            // trasformo la tabella del database in modello dropdownlistmodel
            string q = "SELECT itemgroup as itemGroup,'" + ddlmodel.rname + "' as rname,lang," + ddlmodel.displayField + " as displayValue," + resultquery + " as resultValue,0 as selected";
            if (ddlmodel.groupParentField != "")
                q += "," + ddlmodel.groupParentField + " as parentValue";
           
            q += " FROM " + ddlmodel.rname + " where lang='" + (ddlmodel.isinvariant ? "it" : lang) + "' order by " + ddlmodel.sortField + " " + ddlmodel.sortDir;

            ddlmodel.items = db.Database.SqlQuery<DropDownListItem>(q).ToList();
            //ddlmodel.items.Where(x => x.resultValue == currentValue).ToList().ForEach(x => x.selected = true);
            ddlmodel.items.Where(x => x.resultValue.ToLower() == currentValue.ToLower()).All(x => x.selected = true);

            return PartialView("_DropDownList", ddlmodel);
        }


        public ActionResult TreeListSelector()
        {
            return View();
        }

        public ActionResult ListSelector()
        {
            return View();
        }

        public ActionResult PopupSelector()
        {
            return View();
        }

        #endregion




    }
}