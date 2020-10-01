using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class PageDatas
    {
        public PageInfo PageInfo { get; set; }
        public PagingInfo Paginator { get; set; }
    }
}