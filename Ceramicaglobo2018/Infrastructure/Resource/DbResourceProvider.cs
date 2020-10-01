using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.Models;

namespace WebSite.Infrastructure.Resource
{
    public class DbResourceProvider : BaseResourceProvider
    {

        private DbModelLanguageResource db = new DbModelLanguageResource();

        protected override LanguageResource ReadResource(string name, string culture)
        {
            return db.LanguageResource.Where(x => x.datakey == name && x.lang == culture).First();
        }

        protected override IList<LanguageResource> ReadResources()
        {
            IList<LanguageResource> result=new List<LanguageResource>();

            result = db.LanguageResource.ToList<LanguageResource>();
            return result;

        }
    }
}