using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using SmbiBotApp.Services;

namespace SmbiBotApp.Controllers
{
    public class RedirectController : ApiController
    {
        public static Activity cont = new Activity();

        [HttpGet]
        [Route("api/Callback")]
        public  Task<HttpResponseMessage> Callback([FromUri] string display)
        {
                       
            HttpContext.Current.Response.Redirect("https://www.messenger.com/closeWindow");
            Activity act = new Activity();
            ConnectorClient connector = new ConnectorClient(new Uri("https://facebook.botframework.com"));
            cont.Text = "welcome back to the bot";
            connector.Conversations.ReplyToActivityAsync(cont);
           //// var response = Request.CreateResponse(HttpStatusCode.OK);
          
           // return response;
            return null;
          
            //return Request.CreateResponse("You are now logged in! Continue talking to the bot.");
        }
    };
}