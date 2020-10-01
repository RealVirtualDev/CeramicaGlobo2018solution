using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CeramicaGlobo2018.Models.OEmbed;
using Newtonsoft.Json;

namespace CeramicaGlobo2018.Helpers
{
    public class OEmbed
    {
        public static OembedYoutube GetYoutubeResult(string url)
        {

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync("https://noembed.com/embed?url=" + url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    OembedYoutube resultobj = JsonConvert.DeserializeObject<OembedYoutube>(responseString);
                    return resultobj;
                 }
                else
                {
                    return null;
                }
            }


            //var client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("https://noembed.com/embed?url=" + url);
            //response.EnsureSuccessStatusCode();
            //var result= await response.Content.ReadAsStringAsync();
            //OembedResult resultobj = JsonConvert.DeserializeObject<OembedResult>(result);
            //return resultobj;
        }
    }
}