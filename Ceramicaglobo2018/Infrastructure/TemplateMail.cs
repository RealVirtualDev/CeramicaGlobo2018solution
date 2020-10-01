using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using WebSite.Models;

namespace WebSite.Infrastructure
{
    public class TemplateMail:MailMessage
    {
        public IDictionary<string, object> vars;
        public string TemplateName;
        public string currentLang="it";

        public TemplateMail(string templateName,dynamic emailVars , string lang="it")
        {
            TemplateName = templateName;
            currentLang = lang;
            vars = emailVars;

            compose();
        }

        private void compose()
        {
            DbModel db = new DbModel();
            Emailtemplate emailTemplate = db.Emailtemplate.Where(x => x.name == TemplateName && x.lang == currentLang).First();
            string bodyTemplate = emailTemplate.template;
            string subjectTemplate = emailTemplate.subject;

            foreach(string k in vars.Keys)
            {
                bodyTemplate = bodyTemplate.Replace("%" + k + "%", (string)vars[k]);
                subjectTemplate= subjectTemplate.Replace("%" + k + "%", (string)vars[k]);
            }

            this.IsBodyHtml = true;
            this.BodyEncoding = System.Text.Encoding.UTF8;
            this.Subject = subjectTemplate;
            this.Body = bodyTemplate;
        }

        public void reset(string templateName, dynamic emailVars, string lang = "it")
        {
            TemplateName = templateName;
            currentLang = lang;
            vars = emailVars;

            compose();
        }

    }
}