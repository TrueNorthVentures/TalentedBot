using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using SmbiBotApp.Model;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.Serialization;

namespace SmbiBotApp.Services
{
    public class LuisService
    {
        public static async Task<LuisResponse> ParseUserInput(string phrase)
        {
            string returnvalue = string.Empty;
            string escapedString = Uri.EscapeDataString(phrase);
            using (var client = new HttpClient())
            {
                // string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/45ea4b09-2acf-46fc-a23e-62c59ce38901?subscription-key=9898c808f2b74e0d8267ae2c39d818d3&verbose=true&q={escapedString}";
                //usama   string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/45ea4b09-2acf-46fc-a23e-62c59ce38901?subscription-key=9898c808f2b74e0d8267ae2c39d818d3&timezoneOffset=0.0&verbose=true&q={escapedString}"; // usama
                //    string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/e1497dd1-64dc-4fac-8e78-ec01ab990ca0?subscription-key=9d70f83f88cf4293ab0656178c22c692&timezoneOffset=0&verbose=true&q={escapedString}";   // waseem
                //string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/3d52c5ee-e4a3-4f05-a77e-52f87a347c06?subscription-key=ebf2a6c25c8543b28d4e0ad853beda42&verbose=true&timezoneOffset=0&q={escapedString}";//rizwan
                // string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/ca353423-ce52-4f24-b7e0-f95be58bb897?subscription-key=cc1c1be1e6a84766b63e9a580b65577f&verbose=true&timezoneOffset=0&q={escapedString}";///anwar
                // string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/02aa0a1c-9e20-44a8-a573-531cf5293257?subscription-key=f206159d1acb402f8fff6d2884eeee0d&verbose=true&timezoneOffset=0&q={escapedString}";//iqra
                // string uri = $"https://southeastasia.api.cognitive.microsoft.com/luis/v2.0/apps/a3b237a6-fd65-4c8d-a9ea-4d0b972412c7?subscription-key=b44678ff702244668e8f410e7d1d9dd6&timezoneOffset=0&verbose=true&q={escapedString}";
                string uri = $"https://southeastasia.api.cognitive.microsoft.com/luis/v2.0/apps/fe54595a-ec04-4d59-906b-5d516927a8f5?subscription-key=b44678ff702244668e8f410e7d1d9dd6&verbose=true&timezoneOffset=0&q={escapedString}";//modifiedversion	// asia region
                                                                                                                                                                                                                                       //string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/fe54595a-ec04-4d59-906b-5d516927a8f5?subscription-key=ebf2a6c25c8543b28d4e0ad853beda42&verbose=true&timezoneOffset=0&q={escapedString}";// mdifiedversion // north america region

                var msg = await client.GetAsync(uri);
                if (msg.IsSuccessStatusCode)
                {
                    var jsonresponse = await msg.Content.ReadAsStringAsync();
                    var jsonresp = msg.Content.ToBsonDocument();

                    var data = JsonConvert.DeserializeObject<LuisResponse>(jsonresponse);
                    var daa = JsonConvert.SerializeObject(data);
                    BsonDocument bs = new BsonDocument();

                    //  bs = (BsonDocument)daa;
                    //DoSomethingAsync(jsonresp);

                    string creditApplicationJson = JsonConvert.SerializeObject(
                    new
                    {
                        jsonCreditApplication = data
                    });

                    return data;
                }

            }
            return null;

        }

        public static void DoSomethingAsync(string user)
        {


            // var collection = db.GetCollection<BsonDocument>("locations");
            var Client = new MongoClient();
            var MongoDB = Client.GetDatabase("bot");
            var Collec = MongoDB.GetCollection<BsonDocument>("tests");

            var locations = new List<BsonDocument>();
            var json = JObject.Parse(user);
            BsonDocument document = BsonDocument.Parse(user);
            var bs = BsonSerializer.Deserialize<BsonDocument>(user); //Deserialize JSON String to BSon Document
            Collec.InsertOneAsync(bs);
            //            var mcollection = Program._database.GetCollection<BsonDocument>("test_collection_05");
            //           await mcollection.InsertOneAsync(bsdocument); //Insert into mongoDB

            foreach (var j in bs)
            {
                //  locations.Add(j);     

            }


            //foreach (var d in json["locations"])
            //{

            //    //  var context = BsonDeserializationContext.CreateRoot(jsonReader);
            //    //  var document = Collec.DocumentSerializer.Deserialize<BsonDocument>(d);
            //  //  var document = BsonSerializer.Deserialize<BsonDocument>(user);

            //    //  locations.Add(document);

            //}
            //  Collec.InsertManyAsync(locations).wait();


            //var Client = new MongoClient();
            //var MongoDB = Client.GetDatabase("bot");
            //var Collec = MongoDB.GetCollection<BsonDocument>("tests");
            var documnt = new BsonDocument
{
    {"Brand","Dll"},
    {"Price","40"},
    {"Ram","89GB"},
    {"HardDisk","78TB"},
    {"Screen","16inch"}
};
            //   Collec.InsertOneAsync(bs);
            //Console.ReadLine();
        }
    }
}