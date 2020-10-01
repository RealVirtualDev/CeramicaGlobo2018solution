using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PagingInfo()
        {
            ItemsPerPage = 25;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }

        }
}