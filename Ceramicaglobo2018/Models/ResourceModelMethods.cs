using System.Linq;
using System.Web.Script.Serialization;
using System.Linq.Dynamic;
using System.Collections.Generic;

namespace WebSite.Models
{
    public partial class ResourceModel
    {

        // DATASOURCE
        public dynamic Datasource()
        {
            if (jsondatasource != "")
            {
                dynamic res = System.Web.Helpers.Json.Decode(jsondatasource);
                dynamic result;

               

                // risorsa dinamica
                if (!string.IsNullOrEmpty(res[0].resource))
                {
                    DbModel db = new DbModel();
                    string data = res[0].data;
                    string val = res[0].val;
                    string sortrow = string.IsNullOrEmpty(res[0].resourceorder) ? data : res[0].resourceorder;
                    string sortfield = sortrow.ToLower().Replace("asc", "").Replace("desc", "").Trim();
                    string sortdir = sortrow.ToLower().Contains(" desc") ? "desc" : "asc";

                    // query che ottiene un array data,value presa dai dati della tabella resource
                    //var  jsresult = db.ResourceModelDatas.Where(x => x.rname == data && (x.name == data | x.name == val))
                    //      .GroupBy(x => x.itemgroup, (key, group) => 
                    //      new {
                    //          data = group.Where(z => z.name == data).Select(z => z.val).FirstOrDefault(),
                    //          val = group.Where(z => z.name == val).Select(z => z.val).FirstOrDefault(),
                    //          sorval= group.Where(z => z.name == sortfield).Select(z => z.val).FirstOrDefault()
                    //      })
                    //      .Select(x => new { data=x.data,val= x.val,sortval=x.sorval })
                    //      .OrderBy("sortval " + sortdir) // qua uso linqdynamic
                    //      .ToList();

                   // List<ResourceDataSource> jsresult = db.Database.SqlQuery<ResourceDataSource>("select " + data + " as data,"+  val + " as val, " + sortfield + " as sortval from " + res[0].resource + " order by " + sortfield + " " + sortdir ).ToList();
                    string rname = res[0].resource;
                    var jsresult = db.Database.SqlQuery<ResourceDataSource>("select " + data + " as data," + val + " as val, " + sortfield + " as sortval from " + rname + " order by " + sortfield + " " + sortdir).ToList();


                    var serializer = new JavaScriptSerializer();
                    var serializedResult = serializer.Serialize(jsresult);
                                    
                    
                    result = System.Web.Helpers.Json.Decode(serializedResult);
                }
                else
                {
                    // risorsa statica
                    result= res;

                }

                return result;
            }
            else
            {
                return null;
            }
        }


        // ADMIN PARAMS

        public dynamic AdminParams()
        {
            if (jsonadminparams != "")
            {
                return System.Web.Helpers.Json.Decode(jsonadminparams);
            }
            else
            {
                return null;
            }
        }
    }
}