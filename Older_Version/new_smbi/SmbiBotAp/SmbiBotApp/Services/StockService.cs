using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using SmbiBotApp.Model;

namespace SmbiBotApp.Services
{
    public class StockService 
    {
        public static async Task<StockItem>GetStockPrice(string symbol)
        {
            string uri = $"http://dev.markitondemand.com/api/v2/Quote/json?symbol={symbol}";
            var stockitem = new StockItem();
            using (var client = new WebClient())
            {
                
                var rawdata = await client.DownloadStringTaskAsync(new Uri(uri));
                stockitem = JsonConvert.DeserializeObject<StockItem>(rawdata);
                stockitem.Status = stockitem.Status ?? "FAIL"; 
            }
                return stockitem;
        }

        internal static Task GetStockPrice()
        {
            throw new NotImplementedException();
        }
    }
}