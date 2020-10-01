using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class DropDownListModel
    {
        public string groupName { get; set; } = "";
        public bool groupMaster { get; set; } = false;
        public string groupParent { get; set; } = "";
        public string groupParentField { get; set; } = "";
        public string rname { get; set; }
        public string lang { get; set; }
        public string displayField { get; set; }
        public string sortField { get; set; }
        public string sortDir { get; set; }
        public string emptyText { get; set; } // vuoto per selezione obbligatoria
        public string selectedValue { get; set; }
        public string propertyName { get; set; }
        public bool isinvariant { get; set; }
        public string[] returnFields { get; set; }
        public List<DropDownListItem> items { get; set; }
    }

    public class DropDownListItem
    {
        public int itemGroup { get; set; }
        public string parentValue { get; set; }
        public string rname { get; set; }
        public string lang { get; set; }
        public string displayValue { get; set; }
        public string resultValue { get; set; }
        public bool selected { get; set; }

    }
}

/*
MODELLO JSON
[{
"groupname":"categorie_tipologie",
"groupmaster":true,
"groupparent":"",
"rname":"categorie",
"displayField":"titolo",
"sortField":"titolo",
"sortDir":"ASC",
"emptyText":"",
"selectedValue":"0",
"returnFields":["itemgroup","titolo","urlname"],
"items":[]
}]*/
