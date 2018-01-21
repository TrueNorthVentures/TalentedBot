using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmbiBotApp.Model
{
    public class RootObject
    {
        public Status status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Status //This is the jSON status
    {
        public bool error { get; set; }
        public int code { get; set; }
        public string type { get; set; }
        public string message { get; set; }
    }

    public class Datum //This is the actual data that is returned
    {
        public string access_token { get; set; }
        public string created_at { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int account_id { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        // public custom_attributes custom_attributes { get; set; }

    }
}