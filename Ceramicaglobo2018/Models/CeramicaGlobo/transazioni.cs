using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Transazioni
    {
        public int id { get; set; }
        public int ordinamento { get; set; }
        public int itemgroup { get; set; }
        public string lang { get; set; }

        public DateTime data { get; set; }
        public string intestatario { get; set; }
        public decimal importo { get; set; }
        public string esito { get; set; }
        public int anno { get; set; }
        public int progressivo { get; set; }
        public string transid { get; set; }
        public string status { get; set; }
        public string cardnumber { get; set; }
        public string refnumber { get; set; }
        public string oid { get; set; }
        public string paymentmethod { get; set; }
        public string refglobo { get; set; }
       public string datastr
        {
            get
            {
                return data.ToShortDateString();
            }
        }
    }
}