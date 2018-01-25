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
            //var uri = GetUri("https://www.facebook.com/messages/closeWindow",
            //  Tuple.Create("image_url", "http://smbibotapp20170804124326.azurewebsites.net/Images/abstract-logo.png"),
            //  Tuple.Create("display_text", "welcome back to the bot"));


            //var uri = GetUri("https://www.messenger.com/closeWindow",
            //      Tuple.Create("image_url", "http://smbibotapp20170804124326.azurewebsites.net/Images/abstract-logo.png"),
            //      Tuple.Create("display_text", "welcome back to the bot"));
          
            // HttpContext.Current.Response.Redirect(uri.ToString());
            ConnectorClient connector = new ConnectorClient(new Uri("https://facebook.botframework.com"));
            cont.Text = MessagesController.col.ElementAt(9); 
            cont.ChannelData = null;
            selectCompany(cont);
            connector.Conversations.ReplyToActivityAsync(cont);
           //// var response = Request.CreateResponse(HttpStatusCode.OK);
          
           // return response;
            return null;
          
            //return Request.CreateResponse("You are now logged in! Continue talking to the bot.");
        }

        private static Uri GetUri(string endPoint, params Tuple<string, string>[] queryParams)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var queryparam in queryParams)
            {
                queryString[queryparam.Item1] = queryparam.Item2;
            }

            var builder = new UriBuilder(endPoint);
            builder.Query = queryString.ToString();
            return builder.Uri;
        }

        private void selectCompany(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you wanted to work for company Design/Development Studio",
                Type = "postBack",
                Title = "Design/Development Studio"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you wanted to work for company Financial Technology",
                Type = "postBack",
                Title = "Financial Technology"
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you wanted to work for company Financial Technology",
                Type = "postBack",
                Title = "Game Studio"
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you wanted to work for company ECommerce",
                Type = "postBack",
                Title = "ECommerce"
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you wanted to work for company Sales",
                Type = "postBack",
                Title = "Sales"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you wanted to work for company Female Focused Technology",
                Type = "postBack",
                Title = "Female Focused Technology"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }







    };
}