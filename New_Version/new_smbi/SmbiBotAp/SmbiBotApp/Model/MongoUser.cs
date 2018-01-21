using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
namespace SmbiBotApp.Model
{
    public class MongoUser
    {

        public static bool add_basic_info(BsonDocument record)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");

            try
            {
                Collec.InsertOneAsync(record);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void add_edu_infos(List<BsonDocument> multiple, string id)
        {
            
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set("educational", new BsonArray(multiple));
            try
            {
                Collec.UpdateOneAsync(filter, update);
            }
            catch
            {

            }
        }
        public static void add_edu_info(BsonDocument edu)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            try
            {
                Collec.InsertOneAsync(edu);
            }
            catch
            {

            }
        }

        public static void add_prof_info(BsonDocument prof)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            try
            {
                Collec.InsertOneAsync(prof);

            }
            catch
            {

            }
        }

        public static void upd_basic_info(BsonDocument info, BsonDocument pro, int upd, string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            try
            {
                if (upd == 2)
                {
                    var update1 = Builders<BsonDocument>.Update.Set("basic", info);
                    Collec.FindOneAndUpdateAsync(filter, update1);
                }
                else
                {
                    var update1 = Builders<BsonDocument>.Update.Set("basic", info);
                    Collec.UpdateOneAsync(filter, update1);
                    var update2 = Builders<BsonDocument>.Update.Set("professional", pro);
                    Collec.UpdateOneAsync(filter, update2);
                }

            }
            catch
            {

            }

        }


        public static BsonDocument exist_user(string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var result = Collec.Find(filter).SingleOrDefault();
            return result;
        }

        public static BsonDocument test_name(string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds121495.mlab.com:21495/bottest");
            var MongoDB = Client.GetDatabase("bottest");
            var Collec = MongoDB.GetCollection<BsonDocument>("nested");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var result = Collec.Find(filter).SingleOrDefault();
            return result;
        }

        
        public static void upd_test_info(BsonDocument record, int flagi, int sta, string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            if (flagi == 1)
            {

                var update1 = Builders<BsonDocument>.Update.Set("test", new BsonArray().Add(record));
                var update2 = Builders<BsonDocument>.Update.Set("basic.status", sta);
                Collec.UpdateOneAsync(filter, update1);
                Collec.UpdateOneAsync(filter, update2);

            }
            else
            {
                var update = Builders<BsonDocument>.Update.Push("test", record);
                Collec.UpdateOneAsync(filter, update);

            }

        }


        public static void upd_pro_info(string com, string pos, BsonDocument proj, int sta, string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update1 = Builders<BsonDocument>.Update.
                Set("professional.company_type", com).
                Set("professional.position_type", pos);
            var update2 = Builders<BsonDocument>.Update.Set("project", proj);
            var update3 = Builders<BsonDocument>.Update.Set("basic.status", sta);
            try
            {
                Collec.UpdateOneAsync(filter, update1);
                Collec.UpdateOneAsync(filter, update2);
                Collec.UpdateOneAsync(filter, update3);
            }
            catch
            {

            }

        }

        public static void upd_project_info(List<BsonDocument> multiple, int sta, string id)
        {
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome");
            var MongoDB = Client.GetDatabase("talenthome");
            var Collec = MongoDB.GetCollection<BsonDocument>("botusers");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update1 = Builders<BsonDocument>.Update.Set("project.details", multiple);
            var update2 = Builders<BsonDocument>.Update.Set("basic.status", sta);
            try
            {
                Collec.UpdateOneAsync(filter, update1);
                Collec.UpdateOneAsync(filter, update2);
            }
            catch
            {

            }

        }

        public static BsonValue ret_sel_test(string tech)
        {
            
            var Client = new MongoClient("mongodb://rizwan:rizwan@ds121495.mlab.com:21495/bottest");
            var MongoDB = Client.GetDatabase("bottest");
            var Collec = MongoDB.GetCollection<BsonDocument>("questions");
            var filter = Builders<BsonDocument>.Filter.Eq("dataset.record.type", tech);
            var result = Collec.Find(filter).SingleOrDefault().ElementAt(1).Value;

            return result;

        }

    }
}