using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Infrastructure.Resource
{
    public interface IResourceProvider
    {
        object GetResource(string name, string culture="");
    }
}