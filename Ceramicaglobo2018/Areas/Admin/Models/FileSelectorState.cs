using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class FileSelectorState
    {
        public int modelId { get; set; } = 0;
        public string source { get; set; }
        public string mode { get; set; }
        public string uploadMode { get; set; }
        public string startFolder { get; set; }
        public string currentFolder { get; set; }
        public bool? allowNewFolder { get; set; } = true;
        public int? imgh { get; set; } = 0;
        public int? imgw { get; set; } = 0;
        public string cropmode { get; set; }="";
        public string fillcolor { get; set; } = "";
        public string parentControlId { get; set; } = "";
        public string parentImgId { get; set; } = "";
        public string jsonData { get; set; }

    }
}