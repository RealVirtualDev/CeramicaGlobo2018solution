﻿using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CeramicaGlobo2018.Models.OEmbed
{
    public class OembedYoutube
    {
        public string html { get; set; }
        public string version { get; set; }
        public string type { get; set; }
        public string provider_url { get; set; }
        public string provider_name { get; set; }
        public int thumbnail_height { get; set; }
        public string author_url { get; set; }
        public string author_name { get; set; }
        public int thumbnail_width { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string thumbnail_url { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        public string src
        {
            get
            {
               return Regex.Match(html, "<iframe.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
            }
        }
    }
}