﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Video
    {
        public int id { get; set; }
        public int itemgroup { get; set; }
        public int ordinamento { get; set; }
        public string lang { get; set; }
        public bool visibile { get; set; }
        public string titolo { get; set; }
        public string descrizione { get; set; }
        public string img { get; set; }
        public string file { get; set; }
        public string link { get; set; }
        public string host { get; set; }
        public string urlname { get; set; }
    }
}