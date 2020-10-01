using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using WebSite.Helpers;

namespace ecommerce.paypal
{
 
    public class PaypalNVPCall
    {
        //Flag that determines the PayPal environment (live or sandbox)
        private bool bSandbox  = false;
        private bool _useipn=false;

        private const string CVV2  = "CVV2";

        // Live strings.
        private  string pEndPointURL  = "https://api-3t.paypal.com/nvp";
        private  string host = "www.paypal.com";

        // Sandbox strings.
        private  string  pEndPointURL_SB= "https://api-3t.sandbox.paypal.com/nvp";
        private string host_SB = "www.sandbox.paypal.com";

        private const string  SIGNATURE = "SIGNATURE";
        private const string  PWD = "PWD";
        private const string   ACCT = "ACCT";

        //Replace <Your API Username> with your API Username
        //Replace <Your API Password> with your API Password
        //Replace <Your Signature> with your Signature
        public string APIUsername = WebSetting.val.paypalAPIUsername;// Webutil.impostazioni.gettextvalue("paypalAPIUsername"); // "elvio-facilitator_api1.realvirtual.it"
        public string APIPassword = WebSetting.val.paypalAPIPassword;//Webutil.impostazioni.gettextvalue("paypalAPIPassword"); // "1364219506"
        public string APISignature = WebSetting.val.paypalAPISignature;//Webutil.impostazioni.gettextvalue("paypalAPISignature"); // "ApbyTyvnLn2urirXjwBC-WJFgU7TAkdlj8IWfh2b0N6UM5vBaz0RvQ6j"
        public string Subject= "";
        public string BNCode  = "PP-ECWizard";

        private string _currency = "EUR";
        private string _landing = "Login";
        private string _brandname = "";
        private paypalCart _cart ;
        private string _ipnpage  = "/services/paypalIPNhandler.ashx";
        private string _baseurl= "";
        private string _cancelurl= "";
        private string _reviewurl = "";
        private string _localeui  = "";

        public string paymentdescription{get;set;}

        private const Int32 Timeout = 15000;


        public PaypalNVPCall(bool useipn = false, bool istest = false)
        {
            bSandbox = istest;
            _useipn = useipn;
        }

        public void SetCredentials(string Userid ,string Pwd ,string Signature ){

            APIUsername = Userid;
            APIPassword = Pwd;
            APISignature = Signature;

        }

        public void SetConfig(string baseurl, string cancelpage , string reviewpage ,string  brandname,paypalCartItem item  , paypalLandingpageType landing  = paypalLandingpageType.Login, string currency = "EUR", string localeui  = "IT"){

            _currency = currency;
            _landing = (landing == paypalLandingpageType.Billing ? "Billing" : "Login");
            _brandname = brandname;
            _baseurl = baseurl;
            _cart = new paypalCart();
            _cart.Add(item);
            _localeui = localeui;
            _cancelurl = cancelpage;
            _reviewurl = reviewpage;
           
        }

        public void SetConfig(string baseurl, string cancelpage, string reviewpage, string brandname,paypalCart cart , paypalLandingpageType landing = paypalLandingpageType.Login, string currency = "EUR", string localeui = "IT")
        {

            _currency = currency;
            _landing = (landing == paypalLandingpageType.Billing ? "Billing" : "Login");
            _brandname = brandname;
            _baseurl = baseurl;
            _cart = cart;
            _localeui = localeui;
            _cancelurl = cancelpage;
            _reviewurl = reviewpage;

        }

         public enum paypalLandingpageType{
             Login,Billing
         }
            
       public bool ShortcutExpressCheckout(out string token ,out string retMsg){

            if (bSandbox) {
                pEndPointURL = pEndPointURL_SB;
                host = host_SB;
            }


            string returnURL = (_baseurl.StartsWith("http") ? "" : "http://") + _baseurl + (_baseurl.EndsWith("/") ? "" : "/") + (_reviewurl.StartsWith("/") ? _reviewurl.Substring(1) : _reviewurl);
            string cancelURL = (_baseurl.StartsWith("http") ? "" : "http://") + _baseurl + (_baseurl.EndsWith("/") ? "" : "/") + (_cancelurl.StartsWith("/") ? _cancelurl.Substring(1) : _cancelurl);

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = returnURL;
            encoder["CANCELURL"] = cancelURL;
            encoder["BRANDNAME"] = WebSetting.val.ecommercename;// Webutil.impostazioni.gettextvalue("ecommercename");//"Nome della società, sovrascrive il nome di default dell'account paypal" 
            encoder["PAYMENTREQUEST_0_AMT"] = _cart.getTotalAmt(); // totale
            encoder["PAYMENTREQUEST_0_DESC"] = (paymentdescription == "" ? "Descrizione Pagamento" : paymentdescription);
            encoder["PAYMENTREQUEST_0_ITEMAMT"] = _cart.getTotalAmt();
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
            encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = _currency.ToUpper();
            encoder["LOCALECODE"] = _localeui;
            encoder["NOSHIPPING"] = "1";
            if (_useipn) 
                encoder["PAYMENTREQUEST_0_NOTIFYURL"] = (_baseurl.StartsWith("http") ? "" : "http://") + _baseurl + (_baseurl.EndsWith("/") ? "" : "/") + _ipnpage; // URL IPN
            
           //encoder("HDRIMG") = "IT" url immagine custom da far apparire nella pagina paypal
            encoder["LANDINGPAGE"] = _landing; //mostro o meno il login oppure carta di credito
            //encoder("BRANDNAME") = "Nome della società, sovrascrive il nome di default dell'account paypal" 

           // se c'è il carrello aggiustare

            foreach (paypalCartItem i in _cart){
                encoder["L_PAYMENTREQUEST_0_NAME" + i.index] = i.name;
                encoder["L_PAYMENTREQUEST_0_DESC" + i.index] = i.description;
                encoder["L_PAYMENTREQUEST_0_AMT" + i.index] = i.amt;
                encoder["L_PAYMENTREQUEST_0_QTY" + i.index] = i.qta;

            }

            // stringa formattata con i parametri da inviare a paypal
            string pStrrequestforNvp  = encoder.encode();
            // chiamata http e messaggio di ritorno
            string pStresponsenvp  = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();

            if (strAck != "" & (strAck == "success" | strAck == "successwithwarning")) {
                token = decoder["TOKEN"];
                string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;
                retMsg = ECURL;
                return true;
            }
            else{
                token = "";
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                   "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                   "Desc2=" + decoder["L_LONGMESSAGE0"];
                return false;
            }
                
          

        }

        public string HttpCall(string NvpRequest){

            string url = pEndPointURL;

            string strPost = NvpRequest + "&" + buildCredentialsNVPString();
            strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode(BNCode);

            WebRequest objRequest = WebRequest.Create(url);
            objRequest.Timeout = Timeout;
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;

            try{
                using(StreamWriter myWriter =new StreamWriter(objRequest.GetRequestStream())){
                     myWriter.Write(strPost);
                }
            }
            catch{

               }

            //Retrieve the Response returned from the NVP API call to PayPal.
            WebResponse objResponse = objRequest.GetResponse();
            string result;
            using(StreamReader sr=new StreamReader(objResponse.GetResponseStream())){
                result = sr.ReadToEnd();
            }

            return result;

        }

        string buildCredentialsNVPString()
        {
            NVPCodec codec = new NVPCodec();

            if (APIUsername != "") 
                codec["USER"] = APIUsername;
            
            if (APIPassword != "") 
                codec["PWD"] = APIPassword;
            
            if (APISignature != "") 
                codec["SIGNATURE"] = APISignature;
            
            if (Subject != "") 
                codec["SUBJECT"] = Subject;
            

            codec["VERSION"] = "88.0";

            return codec.encode();
        }

        public bool GetCheckoutDetails(string token , out string PayerID , out NVPCodec decoder, out string retMsg){

             if (bSandbox)
                pEndPointURL = pEndPointURL_SB;
            

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            string pStrrequestforNvp  = encoder.encode();
            string pStresponsenvp  = HttpCall(pStrrequestforNvp);

            decoder = new NVPCodec();
            decoder.decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();

            if (strAck != "" & (strAck == "success" | strAck == "successwithwarning")) {
                PayerID = decoder["PAYERID"];
                retMsg="";
                return true;
            }
            else{
                 PayerID ="";
                 retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" + 
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" + 
                    "Desc2=" + decoder["L_LONGMESSAGE0"];
                return false;
            }
               

        }

        public bool DoCheckoutPayment(string finalPaymentAmount,string token, string PayerID , out NVPCodec decoder ,out string retMsg){
             
             if (bSandbox)
                pEndPointURL = pEndPointURL_SB;

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["TOKEN"] = token;
            encoder["PAYERID"] = PayerID;
            encoder["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
            encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = _currency;
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";

            string pStrrequestforNvp = encoder.encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            decoder = new NVPCodec();
            decoder.decode(pStresponsenvp);

            string strAck  = decoder["ACK"].ToLower();

            if (strAck != "" & (strAck == "success" | strAck == "successwithwarning")) {
                retMsg = "";
                return true;

            }
            else{
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];
                return false;
            }
               
            
        }

    }
}
