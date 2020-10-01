using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Utenti
    {
        public int id { get; set; }
        public string ragionesociale { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]
        public string cognome { get; set; }
        [Required(ErrorMessage = "Indirizzo Email errato"), EmailAddress(ErrorMessage = "Indirizzo Email errato, controllare che non ci siano spazi iniziali e finali")]
        public string email { get; set; }
        [Required]
        public string nazione { get; set; }
        [Required]
        public string citta { get; set; }
        [Required]
        public string provincia { get; set; }
        public string lang { get; set; }
        public DateTime data { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string professione { get; set; }
        [Required]
        public string indirizzo { get; set; }

    }
}
