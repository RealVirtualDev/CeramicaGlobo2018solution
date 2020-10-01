using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CeramicaGlobo2018
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.RouteExistingFiles = false;

            routes.MapRoute(
              name: "home",
              url: "{lang}",
              defaults: new { controller = "Home", action = "Index" , lang=UrlParameter.Optional},
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
            );

            //routes.MapRoute(
            //  name: "home_lang",
            //  url: "{lang}",
            //  defaults: new { controller = "Home", action = "Index" },
            //  constraints: new { lang = "it|en|fr|es|de|ru|ch" }
            //);

            routes.MapRoute(
              name: "home_detail",
              url: "home/{urlname}",
              defaults: new { controller = "Home", action = "BlockDetail"}
             
          );
            routes.MapRoute(
              name: "home_detail_lang",
              url: "{lang}/home/{urlname}",
              defaults: new { controller = "Home", action = "BlockDetail" },
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
          );


            //    routes.MapRoute(
            //    name: "home_detail_lang",
            //    url: "{lang}/home/{urlname}",
            //    defaults: new { controller = "Home", action = "BlockDetail" },
            //    constraints: new { lang = "it|en|fr|es|de|ru|ch" }

            //);
            // routes.MapRoute(
            //  name: "azienda",
            //  url: "{lang}/{type:AziendaRouteContraint}/{pname}",
            //  defaults: new { controller = "Page", action = "Azienda", pname = UrlParameter.Optional,lang=UrlParameter.Optional },
            //  constraints:new {lang=new LangRouteContraint()}
            //);

            //  routes.MapRoute(
            //  name: "azienda",
            //  url: "{type:AziendaRouteContraint}/{pname}",
            //  defaults: new { controller = "Azienda", action = "Index" }

            //);

            // AZIENDA

            routes.MapRoute(
           name: "azienda",
           url: "{az}/{pname}",
           defaults: new { controller = "Azienda", action = "Index" },
           constraints: new {az="azienda|company"}

         );

          //  routes.MapRoute(
          //  name: "azienda_lang",
          //  url: "{lang}/{type:AziendaRouteContraint}/{pname}",
          //  defaults: new { controller = "Azienda", action = "Index" }
            
          //);
            routes.MapRoute(
          name: "azienda_lang",
          url: "{lang}/{az}/{pname}",
          defaults: new { controller = "Azienda", action = "Index" },
          constraints: new {az= "azienda|company", lang = "it|en|fr|es|de|ru|ch" }

        );

            // MONDO GLOBO

            routes.MapRoute(
               name: "mondoglobo",
               url: "{mg}/{pname}",
               defaults: new { controller = "MondoGlobo", action = "Index" },
               constraints: new { mg = "mondo-globo|globo-world" }
            );

            routes.MapRoute(
              name: "mondoglobo_lang",
              url: "{lang}/{mg}/{pname}",
              defaults: new { controller = "MondoGlobo", action = "Index" },
              constraints: new { mg = "mondo-globo|globo-world", lang = "it|en|fr|es|de|ru|ch" }
            );

            // DOWNLOAD

            routes.MapRoute(
            name: "download",
            url: "download/{pname}",
            defaults: new { controller = "Download", action = "Index" }
            );

            routes.MapRoute(
              name: "download_lang",
              url: "{lang}/download/{pname}",
              defaults: new { controller = "Download", action = "Index" },
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
            );

            // RETE VENDITA
           routes.MapRoute(
             name: "retedivendita_rivenditeitalia",
             url: "getResellers",
             defaults: new { controller = "ReteDiVendita", action = "getResellers" }
           );

            routes.MapRoute(
               name: "retedivendita",
               url: "{rv}/{pname}",
               defaults: new { controller = "ReteDiVendita", action = "Index" },
               constraints: new { rv = "rete-di-vendita|dealer-locator" }
            );

            routes.MapRoute(
              name: "retedivendita_lang",
              url: "{lang}/{rv}/{pname}",
              defaults: new { controller = "ReteDiVendita", action = "Index" },
              constraints: new {rv = "rete-di-vendita|dealer-locator", lang = "it|en|fr|es|de|ru|ch" }
            );


           
            // NEWS

            routes.MapRoute(
               name: "news",
               url: "news/{pname}",
               defaults: new { controller = "News", action = "Index" }
           );

            routes.MapRoute(
              name: "news_lang",
              url: "{lang}/news/{pname}",
              defaults: new { controller = "News", action = "Index" },
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
            );



            // CONTATTI

            routes.MapRoute(
               name: "contatti",
               url: "{ct}/{pname}",
               defaults: new { controller = "Contatti", action = "Index" },
               constraints: new { ct = "contatti|contacts" }
            );

            routes.MapRoute(
              name: "contatti_lang",
              url: "{lang}/{ct}/{pname}",
              defaults: new { controller = "Contatti", action = "Index" },
              constraints: new { ct = "contatti|contacts", lang = "it|en|fr|es|de|ru|ch" }
            );

            // COLLEZIONI

            routes.MapRoute(
               name: "collezioni",
               url: "collezioni/{collezione}/{categoria}/{prodotto}",
               defaults: new { controller = "Prodotti", action = "Collezioni", collezione = UrlParameter.Optional, categoria = UrlParameter.Optional,prodotto=UrlParameter.Optional }
              
            );

            routes.MapRoute(
              name: "collezioni_lang",
              url: "{lang}/{c}/{collezione}/{categoria}/{prodotto}",
              defaults: new { controller = "Prodotti", action = "Collezioni", collezione = UrlParameter.Optional, categoria = UrlParameter.Optional, prodotto = UrlParameter.Optional },
              constraints: new { c="collezioni|collections", lang = "it|en|fr|es|de|ru|ch" }
            );


            // PRODOTTO TEST DA COLLEZIONI
            //routes.MapRoute(
            //  name: "collezionitest",
            //  url: "collezionitest/{collezione}/{categoria}/{prodotto}",
            //  defaults: new { controller = "Prodotti", action = "ProdottoTest", tipologia = UrlParameter.Optional, collezione = UrlParameter.Optional, prodotto = UrlParameter.Optional }
            //);

            // TIPOLOGIE

            routes.MapRoute(
               name: "tipologie",
               url: "tipologie/{tipologia}/{collezione}/{prodotto}",
               defaults: new { controller = "Prodotti", action = "Tipologie", tipologia = UrlParameter.Optional, collezione= UrlParameter.Optional, prodotto = UrlParameter.Optional }

            );

            routes.MapRoute(
              name: "tipologie_lang",
              url: "{lang}/{c}/{tipologia}/{collezione}/{prodotto}",
              defaults: new { controller = "Prodotti", action = "Tipologie", tipologia = UrlParameter.Optional, collezione = UrlParameter.Optional, prodotto = UrlParameter.Optional },
              constraints: new { c = "tipologie|typologies", lang = "it|en|fr|es|de|ru|ch" }
            );

            // PRODOTTO TEST DA TIPOLOGIA
            //routes.MapRoute(
            //  name: "tipologietest",
            //  url: "tipologietest/{tipologia}/{collezione}/{prodotto}",
            //  defaults: new { controller = "Prodotti", action = "ProdottoTest", tipologia = UrlParameter.Optional, collezione = UrlParameter.Optional, prodotto = UrlParameter.Optional }
            //);

            // LOGIN

            routes.MapRoute(
               name: "login",
               url: "login",
               defaults: new { controller = "UtentiReg", action = "LoginUtente" },
                namespaces: new string[] { "WebSite.Controllers" }

            );

            routes.MapRoute(
             name: "login_lang",
             url: "{lang}/login",
             defaults: new { controller = "UtentiReg", action = "LoginUtente" },
             constraints: new { lang = "it|en|fr|es|de|ru|ch" },
              namespaces: new string[] { "WebSite.Controllers" }
            );

            // RECUPERO PASSWORD

            routes.MapRoute(
               name: "passrecovery",
               url: "passrecovery",
               defaults: new { controller = "UtentiReg", action = "PasswordRecovery" },
                namespaces: new string[] { "WebSite.Controllers" }

            );

            routes.MapRoute(
             name: "passrecovery_lang",
             url: "{lang}/passrecovery",
             defaults: new { controller = "UtentiReg", action = "PasswordRecovery" },
             constraints: new { lang = "it|en|fr|es|de|ru|ch" },
              namespaces: new string[] { "WebSite.Controllers" }
            );

            // REGISTRAZIONE

            routes.MapRoute(
               name: "registrazione",
               url: "registrazione",
               defaults: new { controller = "UtentiReg", action = "RegistrazioneUtente" },
               namespaces:new string[] { "WebSite.Controllers" }
            );

            routes.MapRoute(
             name: "registrazione_lang",
             url: "{lang}/register",
             defaults: new { controller = "UtentiReg", action = "RegistrazioneUtente" },
             constraints: new {lang = "it|en|fr|es|de|ru|ch" },
             namespaces: new string[] { "WebSite.Controllers" }
            );


            // REGISTRAZIONE NEWSLETTER

            routes.MapRoute(
               name: "registrazionenewsletter",
               url: "registrazionenewsletter",
               defaults: new { controller = "UtentiReg", action = "RegistrazioneNewsletter" },
                namespaces: new string[] { "WebSite.Controllers" }
            );

            routes.MapRoute(
             name: "registrazionenewsletter_lang",
             url: "{lang}/newsletterjoin",
             defaults: new { controller = "UtentiReg", action = "RegistrazioneNewsletter" },
             constraints: new { lang = "it|en|fr|es|de|ru|ch" },
              namespaces: new string[] { "WebSite.Controllers" }
            );

            //getPrivacy
           // routes.MapRoute(
           //   name: "getprivacy",
           //   url: "getPrivacy",
           //   defaults: new { controller = "UtentiReg", action = "getPrivacy" },
           //    namespaces: new string[] { "WebSite.Controllers" }
           //);



            routes.MapRoute(
            name: "condizioni",
            url: "condizioni",
            defaults: new { controller = "Policy", action = "CondizioniUso"}
           
           );
            routes.MapRoute(
           name: "condizionilang",
           url: "{lang}/conditions",
           defaults: new { controller = "Policy", action = "CondizioniUso" },
           constraints: new { lang = "it|en|fr|es|de|ru|ch" }
          );


            routes.MapRoute(
           name: "privacypolicy",
           url: "privacypolicy",
           defaults: new { controller = "Policy", action = "PrivacyPolicy" }
          );

            routes.MapRoute(
           name: "privacypolicylang",
           url: "{lang}/privacypolicy",
           defaults: new { controller = "Policy", action = "PrivacyPolicy"},
           constraints: new { lang = "it|en|fr|es|de|ru|ch" }
          );
          


            routes.MapRoute(
           name: "cookiespolicy",
           url: "cookiespolicy",
           defaults: new {controller = "Policy", action = "CookiesPolicy" }
           
            );

            routes.MapRoute(
          name: "cookiespolicylang",
          url: "{lang}/cookiespolicy",
          defaults: new { controller = "Policy", action = "CookiesPolicy" },
          constraints: new { lang = "it|en|fr|es|de|ru|ch" }
           );


            // CERCA
            routes.MapRoute(
          name: "search",
          url: "search",
          defaults: new { controller = "Prodotti", action = "Search" }

           );

            routes.MapRoute(
          name: "searchlang",
          url: "{lang}/search",
          defaults: new { controller = "Prodotti", action = "Search"},
          constraints: new { lang = "it|en|fr|es|de|ru|ch" }
           );

            // RICERCA AVANZATA
            routes.MapRoute(
         name: "advsearch",
         url: "AdvancedSearch",
         defaults: new { controller = "Prodotti", action = "AdvancedSearch" }

          );

            routes.MapRoute(
          name: "advsearchlang",
          url: "{lang}/AdvancedSearch",
          defaults: new { controller = "Prodotti", action = "AdvancedSearch" },
          constraints: new { lang = "it|en|fr|es|de|ru|ch" }
           );

            // COMPARA

            routes.MapRoute(
              name: "compara",
              url: "Compare",
              defaults: new { controller = "Prodotti", action = "Compara" }

               );

            routes.MapRoute(
              name: "comparalang",
              url: "{lang}/Compare",
              defaults: new { controller = "Prodotti", action = "Compara" },
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
               );


            // SAVE WATER
            routes.MapRoute(
              name: "savewater",
              url: "save-water",
              defaults: new { controller = "commonpages", action = "savewater" }

               );

            routes.MapRoute(
              name: "savewaterlang",
              url: "{lang}/save-water",
              defaults: new { controller = "commonpages", action = "savewater" },
              constraints: new { lang = "it|en|fr|es|de|ru|ch" }
               );


            // FILES
            routes.MapRoute(
             name: "files",
             url: "files/{lang}/{igprodotto}/{ftype}",
             defaults: new { controller = "Files", action = "Download" },
             constraints: new { lang = "it|en|fr|es|de|ru|ch" }
              );


             // sitemap
             //routes.MapRoute(
             //name: "sitemaps",
             //url: "sitemap-{lang}",
             //defaults: new { controller = "Sitemap", action = "Index",lang = UrlParameter.Optional }
             //);
            // sitemap
            routes.MapRoute(
            name: "sitemapsext",
            url: "sitemap-{lang}.xml",
            defaults: new { controller = "Files", action = "Sitemap", lang = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "pagamenti",
            url: "paynow",
            defaults: new { controller = "pagamenti", action = "PagaOra"}
            );

            routes.MapRoute(
            name: "pagamentilang",
            url: "{lang}/paynow",
            defaults: new { controller = "pagamenti", action = "PagaOra" }
            );

            routes.MapRoute(
            name: "pagamentifail",
            url: "paymentfail",
            defaults: new { controller = "pagamenti", action = "nonpagato" }
            );

            routes.MapRoute(
            name: "pagamentifaillang",
            url: "{lang}/paymentfail",
            defaults: new { controller = "pagamenti", action = "nonpagato" }
            );


            routes.MapRoute(
            name: "pagamentiok",
            url: "paymentdone",
            defaults: new { controller = "pagamenti", action = "pagato" }
            );

            routes.MapRoute(
            name: "pagamentioklang",
            url: "{lang}/paymentdone",
            defaults: new { controller = "pagamenti", action = "pagato" }
            );

            routes.MapRoute(
               name: "default",
               url: "{controller}/{action}",
               defaults: new { controller = "Home", action = "index" });


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }


    // ROUTE CONSTRAINT
    //public class AziendaRouteContraint : IRouteConstraint
    //{


    //    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    //    {
    //        // You can also try Enum.IsDefined, but docs say nothing as to
    //        // is it case sensitive or not.
    //        var response = Enum.GetNames(typeof(azienda)).Any(s => s.ToLowerInvariant() == values[parameterName].ToString().ToLowerInvariant());
    //        return response;
    //    }

    //    public enum azienda
    //    {
    //        azienda,
    //        company

    //    }
    //}
    // ROUTE CONSTRAINT
    //public class MondoGloboRouteContraint : IRouteConstraint
    //{


    //    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    //    {
    //        // You can also try Enum.IsDefined, but docs say nothing as to
    //        // is it case sensitive or not.
    //        var response = Enum.GetNames(typeof(azienda)).Any(s => s.ToLowerInvariant() == values[parameterName].ToString().ToLowerInvariant());
    //        return response;
    //    }

    //    public enum azienda
    //    {
    //        azienda,
    //        company

    //    }
    //}
    //public class LangRouteContraint : IRouteConstraint
    //{


    //    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    //    {
    //        // You can also try Enum.IsDefined, but docs say nothing as to
    //        // is it case sensitive or not.
    //        var response = Enum.GetNames(typeof(lang)).Any(s => s.ToLowerInvariant() == values[parameterName].ToString().ToLowerInvariant() || s.ToString()=="");
    //        return response;
    //    }

    //    public enum lang
    //    {
            
    //        it,
    //        en,
    //        fr,
    //        es,
    //        de,
    //        ru,
    //        ch

    //    }
    //}
}
