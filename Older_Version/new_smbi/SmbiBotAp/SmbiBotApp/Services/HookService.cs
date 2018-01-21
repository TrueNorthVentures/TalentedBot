using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmbiBotApp.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmbiBotApp.Services
{
    public class HookService
    {

        //public static async Task<webhoook> GetInputFromWebhook(string phrase)
        //{
        //    string returnvalue = string.Empty;
        //    string escapedString = Uri.EscapeDataString(phrase);
        //    using (var client = new HttpClient())
        //    {

        //        string uri = $"https://graph.facebook.com/v2.8/me/messenger_profile?fields=get_started&access_token={escapedString}";

        //       // string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/45ea4b09-2acf-46fc-a23e-62c59ce38901?subscription-key=9898c808f2b74e0d8267ae2c39d818d3&verbose=true&q={escapedString}";
        //        var msg = await client.GetAsync(uri);
        //        if (msg.IsSuccessStatusCode)
        //        {
        //            var jsonresponse = await msg.Content.ReadAsStringAsync();
        //            var data = JsonConvert.DeserializeObject<webhoook>(jsonresponse);
        //            return data;
        //        }
        //    }
        //    return null;

        //}


    }
}