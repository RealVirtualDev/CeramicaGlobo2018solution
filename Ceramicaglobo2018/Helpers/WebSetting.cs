using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Models;
using System.Web.SessionState;
using System.Dynamic;
using System.Xml.Linq;

namespace WebSite.Helpers
{
    public class WebSetting : IRequiresSessionState
    {
        //private DbModelStaticResource db = new DbModelStaticResource();

        private static List<Impostazioni> _settings;



        private static void init()
        {
            DbModel db = new DbModel();
            _settings = db.Impostazioni.ToList();
        }


        private static List<Impostazioni> settings
        {
            get
            {
                if (HttpContext.Current.Session["websettings"] == null)
                {
                    init();
                }
                return _settings;
            }
        }

        public static dynamic val
        {
            get
            {
                var result = new ExpandoObject() as IDictionary<string, Object>;
                settings.ForEach(x => {

                    switch (x.tipo)
                    {
                        case "string":
                            result.Add(x.keystr, x.valore);
                            break;
                        case "bool":
                            result.Add(x.keystr, Convert.ToBoolean(x.valore));
                            break;
                        case "int":
                            result.Add(x.keystr, Convert.ToInt32(x.valore));
                            break;
                        case "decimal":
                            result.Add(x.keystr, Convert.ToDecimal(x.valore));
                            break;
                    }
                });
                return result;
            }

        }
    }
}