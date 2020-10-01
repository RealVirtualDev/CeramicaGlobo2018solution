using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebSite.Models;


namespace CeramicaGlobo2018.Controllers
{

    public class PagamentiController : Controller
    {
        DbModel db = new DbModel();

        // GET: Pagamenti
        public ActionResult PagaOra()
        {
            return View();
        }

        public ActionResult nonpagato()
        {
            try
            {
                string status = Request.Params["status"].ToString();
                string addInfo3 = Request.Params["addInfo3"].ToString().Replace("'", "\'");
                string addInfo4 = Request.Params["addInfo4"].ToString().Replace("'", "\'");

                //string cardnumber = Request.Params["cardnumber"].ToString();
                //string refnumber = Request.Params["refnumber"].ToString();
                //string oid = Request.Params["oid"].ToString();
                //string paymentMethod = Request.Params["paymentMethod"].ToString();

                if (string.IsNullOrEmpty(addInfo3) || !(HttpContext.Session["addInfo3"] == null))
                {
                    addInfo3 = HttpContext.Session["addInfo3"].ToString();
                }
                string q = "update transazioni set refglobo='" + addInfo4 + "', esito='" + status + "',status='" + status + "' where transid='" + addInfo3 + "'";
                db.Database.ExecuteSqlCommand(q);

            }
            catch
            {

            }

          

            return View("fail");
        }

        public ActionResult pagato()
        {
            string status = Request.Params["status"].ToString();
            string addInfo3 = Request.Params["addInfo3"].ToString().Replace("'", "\'");
            string addInfo4 = Request.Params["addInfo4"].ToString().Replace("'", "\'");
            
            string cardnumber = Request.Params["cardnumber"].ToString();
            string refnumber = Request.Params["refnumber"].ToString();
            string oid = Request.Params["oid"].ToString();
            string paymentMethod = Request.Params["paymentMethod"].ToString();

            if(string.IsNullOrEmpty(addInfo3) || !(HttpContext.Session["addInfo3"]==null))
            {
                addInfo3 = HttpContext.Session["addInfo3"].ToString();
            }

            // salvo transazione
            if (!string.IsNullOrEmpty(addInfo3))
            {
                string q = "update transazioni set refglobo='" + addInfo4 + "', esito='" + status + "',status='" + status + "',cardnumber='" + cardnumber + "',refnumber='" + refnumber + "',oid='" + oid + "',paymentMethod='" + paymentMethod + "' where transid='" + addInfo3 + "'";
                db.Database.ExecuteSqlCommand(q);
            }
            //If addInfo3 <> "" Then
            //    Dim q As String = "update transazioni set refglobo=""" & addInfo4 & """, esito=""" & status & """,status=""" & status & """,cardnumber=""" & cardnumber & """,refnumber=""" & refnumber & """,oid=""" & oid & """,paymentMethod=""" & paymentMethod & """ where transid=""" & addInfo3 & """"
            //    Webutil.db.SQLrun(q)
            //End If


            return View("pagato");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendPayment(string lingua,string importo, string ragionesociale,string riferimento)
        {
            dynamic result = new ExpandoObject();
            float _import;
            bool haserror = false;

            // controllo errori lato server
            if (string.IsNullOrEmpty(lingua) || string.IsNullOrEmpty(importo) || string.IsNullOrEmpty(ragionesociale) || string.IsNullOrEmpty(riferimento))
            {
                result.success = false;
                result.message = "Please check all required fields." + "<br/>";
                haserror = true;
            }
            else 
            {
                if(!float.TryParse(importo.Replace(".",","), out _import))
                {
                    result.success = false;
                    result.message = "Please check import field." + "<br/>";
                    haserror = true;
                }
            }
            if (haserror)
            {
                return Json(result);
            }

            string storename = "06830398_S";
            string shared_secret = "ZPretAe80fs3FExu1Pi6Uy31P7=";
            string url = "https://pf.bnlpositivity.it/service/";

            int newxtprogressivo = db.Transazioni.Max(x => x.progressivo)+1;
            string transid = DateTime.Now.Year.ToString() + String.Format("{0:0000}", newxtprogressivo);

            string addInfo3 = transid;
            string addInfo4 = riferimento;
            HttpContext.Session["addInfo3"] = transid;

            string txntype = "PURCHASE";
            string timezone = "CET";
            string txndatetime = DateTime.Now.ToString("yyyy:MM:dd-HH:mm:ss").Replace(".", ":");
            string hash = "";
            string mode = "payonly";
            string currency = "EUR";
            string language  = lingua;
            //string chargetotal = "";
           
            // i link devono essere della giusta lingua
            //string responseSuccessURL = "https://www.ceramicaglobo.com/" + (lingua=="it" ? "" : lingua + "/") + "paymentdone";
            //string responseFailURL = "http://www.ceramicaglobo.com/"+ (lingua=="it" ? "" : lingua + " / ") + "paymentfail";

            string responseSuccessURL = "https://www.ceramicaglobo.com/paymentdone";
            string responseFailURL = "http://www.ceramicaglobo.com/paymentfail";

            //string transactionNotificationURL = "http://www.ceramicaglobo.com/pagatoasync.ashx";



            string hash_hex  = "";
            string importostr = importo.Replace(",", ".");
            
            if(!importostr.Contains("."))
            {
                importostr += ".00";
            }
            txndatetime = "2018:06:14-11:09:05";
            hash_hex = strToHex(String.Concat(storename, txndatetime, importostr, currency, shared_secret));
            hash = FormsAuthentication.HashPasswordForStoringInConfigFile(hash_hex, "sha1").ToLower();
           
            //hash = HashString(hash_hex, "sha1").ToLower(); // ritorna un risultato diverso

            Hashtable dati= new Hashtable();
            dati.Add("txntype", txntype);
            dati.Add("timezone", timezone);
            dati.Add("txndatetime", txndatetime);
            dati.Add("hash", hash);
            dati.Add("storename", storename);
            dati.Add("mode", mode);
            dati.Add("currency", currency);
            dati.Add("language", language);
            dati.Add("chargetotal", importostr);
            dati.Add("responseSuccessURL", responseSuccessURL);
            dati.Add("responseFailURL", responseFailURL);
            dati.Add("addInfo3", addInfo3);
            dati.Add("addInfo4", addInfo4);

            StringBuilder sb = new StringBuilder();
            Response.Clear();
           // sb.AppendLine("<html>");
            //sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form id=\"formpay\" action='{0}' method='post'>", url);
            foreach (string k in dati.Keys){
                sb.AppendFormat("<input type='hidden' name='" + k + "' value='{0}'>", dati[k]);
            }

            sb.Append("</form>");
            //sb.Append("</body>");
            //sb.Append("</html>");


            // SALVO LA TRANSAZIONE
            int ig = db.Transazioni.Max(x => x.itemgroup) + 1;
            string q = "insert into transazioni(itemgroup,ordinamento,lang,data,intestatario,importo,anno,progressivo,transid) values(" + ig + "," + ig + ",\"IT\",\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"" + ragionesociale + "\"," + importo.ToString().Replace(",", ".") + "," + DateTime.Now.Year + "," + newxtprogressivo + ",\"" + transid + "\");";
            db.Database.ExecuteSqlCommand(q);

            // Response.Write(sb.ToString());
            // Response.End();
            result.success = true;
            result.message = sb.ToString();
            return Json(new { success = result.success, message = result.message });
        }


        //public static string HashString(string inputString, string hashName)
        //{
        //    HashAlgorithm algorithm = HashAlgorithm.Create(hashName);
        //    if (algorithm == null)
        //    {
        //        throw new ArgumentException("Unrecognized hash name", "hashName");
        //    }
        //    byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        //    return Convert.ToBase64String(hash);
        //}



        string strToHex(string myStr)
        {
            string hexStr = "";
            string hexTmp = "";

            foreach (char c in myStr)
            {
                hexTmp = string.Format("{0:X}", (int)(c));
                hexStr += hexTmp;
            }

            //for(int i = 1; i < myStr.Length;i++)
            //{
            //    hexTmp =string.Format("{0:X}", (int)(Mid(myStr, i)));
            //    hexStr += hexTmp;
            //}

            return hexStr.ToLower();
        }

        //public  char Mid(string s, int a)

        //{
        //    return s.Substring(a - 1, 1)[0];
        //}

    }
}