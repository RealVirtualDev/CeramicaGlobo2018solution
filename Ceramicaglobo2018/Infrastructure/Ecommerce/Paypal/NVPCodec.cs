using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Text;

namespace ecommerce.paypal
{

    public class NVPCodec : NameValueCollection
    {

        const string AMPERSAND = "&";
        const string EQ = "=";

        static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
        static readonly char[] EQUALS_CHAR_ARRAY = EQ.ToCharArray();


        public NVPCodec()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string encode()
        {
            StringBuilder sb= new StringBuilder();
            bool firstPair  = true;

            foreach(string kv in AllKeys){

                string name = HttpUtility.UrlEncode(kv);
                string value = HttpUtility.UrlEncode(this[kv]);
                if (! firstPair )
                    sb.Append(AMPERSAND);
                

                sb.Append(name).Append(EQ).Append(value);
                firstPair = false;

            }

            return sb.ToString();
        }

        public void decode(string nvpstring)
        {
            foreach(string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY)){
                string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
                if (tokens.Length >= 2) {
                    string name = HttpUtility.UrlDecode(tokens[0]);
                    string value  = HttpUtility.UrlDecode(tokens[1]);
                    this.Add(name, value);
                }
                    
               
            }
         }

        public void Add(string name,string value,Int32 index)
        {
            Add(GetArrayName(index, name), value);
        }
        public void Remove(string arrayName ,Int32 index )
        {
            Remove(GetArrayName(index, arrayName));
        }

        public string this[string name, int index]
        {
            get
            {
                return this[GetArrayName(index, name)];
            }
            set
            {
                this[GetArrayName(index, name)] = value;
            }
        }

        public static string GetArrayName(Int32 index,string  name){
             if (index < 0)
                throw new ArgumentOutOfRangeException("index", "index cannot be negative : " + index);


             return name + index.ToString();
        }

    }

}
