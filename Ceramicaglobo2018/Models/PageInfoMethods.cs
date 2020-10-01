using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public partial class PageInfo
    {

        public System.Data.DataTable ModelDatasource(string propertyName)
        {
            PageModel pmodel = Model.Where(x => x.propertyname == propertyName).First();
            return pmodel.Datasource();

        }

        public System.Data.DataTable ModelAdminParams(string propertyName)
        {
            PageModel pmodel = Model.Where(x => x.propertyname == propertyName).First();
            return pmodel.AdminParams();


        }


    }
}