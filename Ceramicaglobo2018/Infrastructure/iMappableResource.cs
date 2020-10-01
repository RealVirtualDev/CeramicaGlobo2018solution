using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebSite.Models;

namespace WebSite.Infrastructure
{
    public interface iMappableResource
    {

        int itemgroup { get; set; }
    }
}