using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public partial class Resource
    {
        public System.Data.DataTable ModelDatasource(string propertyName)
        {
            ResourceModel rmodel = Model.Where(x => x.propertyname == propertyName).First();
            return rmodel.Datasource();

        }

        public System.Data.DataTable ModelAdminParams(string propertyName)
        {
            ResourceModel rmodel = Model.Where(x => x.propertyname == propertyName).First();
            return rmodel.AdminParams();


        }
    }
}