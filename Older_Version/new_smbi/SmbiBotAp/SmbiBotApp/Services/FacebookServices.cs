using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmbiBotApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;
using Microsoft.Bot.Connector;
using System.Reflection;

namespace SmbiBotApp.Services
{
    

    public class FacebookServices
    {
        
        public static async Task<dynamic> GetUser(string token,string id)
        {
            string[] arr = new string[7];
            var client = new FacebookClient();
            dynamic user = await client.GetTaskAsync($"/{id}",
                new
                {
                    fields = "id,first_name,last_name,birthday,gender,location,email",
                    access_token = token
                });
            DateTime dob = Convert.ToDateTime(user["birthday"]);
            var no_of_days = DateTime.Today - dob;
            user["birthday"] = (no_of_days.Days / 365).ToString();

            for (int i = 0; i <= 6; i++)
            {
                var exi = user.ContainsKey(Enum.GetName(typeof(MessagesController.user_profile), i));
                if (!exi)
                {
                    MessagesController.missing[i] = Enum.GetName(typeof(MessagesController.user_profile), i);
                    arr[i] = "Not Specified";
                }
                else
                {
                    arr[i] = Convert.ToString(user[Enum.GetName(typeof(MessagesController.user_profile), i)]);
                }
            }

            return arr;

        }



        public async Task<dynamic> GetToken()
        {
            var client = new FacebookClient();
            client.AppId = "102667733606511";
            client.AppSecret = "1d81ecb0410c847acc39583d3d542643";


            dynamic user = await client.GetTaskAsync("/197349787419043",
                new
                {
                    fields = "access_token"
                });

            object val = user.GetType().GetProperty("Values").GetValue(user, null);
            string[] arr = ((IEnumerable)val).Cast<object>().Select(x => x.ToString()).ToArray();
            return arr;
        }
    }
}
