using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using TradrORM;

namespace TradrDataAccess
{
    public class PolygonClient
    {
        private static string _apiKey = "hf5J_WjryZGKHEAYxbDY6kdvmJI7BVpn";
        private static string _baseUrl = "https://api.polygon.io";

        public enum TimeSpan
        {
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year
        }

        private static string GetTimeSpanString(TimeSpan t)
        {
            switch(t)
            {
                case TimeSpan.Minute:
                    return "minute";
                case TimeSpan.Hour:
                    return "hour";
                case TimeSpan.Day:
                    return "day";
                case TimeSpan.Week:
                    return "week";
                case TimeSpan.Month:
                    return "month";
                case TimeSpan.Quarter:
                    return "quarter";
                case TimeSpan.Year:
                    return "year";
                default:
                    return "day";
            }
        }

        public static List<Price> GetHistoricalPrices(string symbol, DateTime start, DateTime end, TimeSpan timeSpan, int multiplier)
        {
            List<Price> result = new List<Price>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"/v2/aggs/ticker/{symbol}/range/{multiplier}/{GetTimeSpanString(timeSpan)}/{start.ToString("yyyy-MM-dd")}/{end.ToString("yyyy-MM-dd")}?unadjusted=true&sort=asc&limit=50000&apiKey={_apiKey}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    Stock stock = new Stock(obj["ticker"].ToString());
                    foreach (JToken item in (JArray)obj["results"])
                    {
                        result.Add(new Price(stock.Id, stock.Symbol, Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["t"])), Convert.ToDecimal(item["o"]), Convert.ToDecimal(item["h"]), Convert.ToDecimal(item["l"]), Convert.ToDecimal(item["c"]), Convert.ToInt32(item["v"])));
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            return result;
        }
    }
}
