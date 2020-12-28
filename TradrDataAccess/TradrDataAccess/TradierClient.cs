using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TradrORM;

namespace TradrDataAccess
{
    public class TradierClient
    {
        //private static string _accessToken = "J8MtQFPcAZjjtpQJjAsDxALkuGdR";
        private static string _accessToken = " J8MtQFPcAZjjtpQJjAsDxALkuGdR";
        //private static string _baseUrl = "https://api.polygon.io";
        private static string _baseUrl = "https://sandbox.tradier.com";

        public static List<Quote> GetQuotes(Stock[] stocks)
        {
            List<Quote> result = new List<Quote>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                HttpResponseMessage response = client.GetAsync($"/v1/markets/quotes?symbols={string.Join(",", stocks.Select((s) => s.Symbol))}&greeks=false").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (obj["quotes"]["quote"] is JArray)
                    {
                        foreach (JObject item in (JArray)obj["quotes"]["quote"])
                        {
                            Stock s = stocks.Where((s) => s.Symbol == item["symbol"].ToString()).FirstOrDefault();
                            if (s != null)
                            {
                                result.Add(
                                    new Quote(
                                        s.Id,
                                        s.Symbol,
                                        Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["trade_date"])),
                                        item["last"].ToString() != "" ? Convert.ToDecimal(item["last"]) : 0.0m,
                                        item["last_volume"].ToString() != "" ? Convert.ToInt32(item["last_volume"]) : 0,
                                        item["change"].ToString() != "" ? Convert.ToDecimal(item["change"]) : 0.0m,
                                        item["change_percentage"].ToString() != "" ? Convert.ToDecimal(item["change_percentage"]) : 0.0m,
                                        item["open"].ToString() != "" ? Convert.ToDecimal(item["open"]) : 0.0m,
                                        item["high"].ToString() != "" ? Convert.ToDecimal(item["high"]) : 0.0m,
                                        item["low"].ToString() != "" ? Convert.ToDecimal(item["low"]) : 0.0m,
                                        item["prevclose"].ToString() != "" ? Convert.ToDecimal(item["prevclose"]) : 0.0m,
                                        item["close"].ToString() != "" ? Convert.ToDecimal(item["close"]) : 0.0m,
                                        item["volume"].ToString() != "" ? Convert.ToInt32(item["volume"]) : 0,
                                        item["average_volume"].ToString() != "" ? Convert.ToInt32(item["average_volume"]) : 0,
                                        item["bid"].ToString() != "" ? Convert.ToDecimal(item["bid"]) : 0.0m,
                                        item["bidsize"].ToString() != "" ? Convert.ToInt32(item["bidsize"]) : 0,
                                        item["bid_date"].ToString() != "" ? Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["bid_date"])) : new DateTime(),
                                        item["ask"].ToString() != "" ? Convert.ToDecimal(item["ask"]) : 0.0m,
                                        item["asksize"].ToString() != "" ? Convert.ToInt32(item["asksize"]) : 0,
                                        item["ask_date"].ToString() != "" ? Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["ask_date"])) : new DateTime(),
                                        item["week_52_high"].ToString() != "" ? Convert.ToDecimal(item["week_52_high"]) : 0.0m,
                                        item["week_52_low"].ToString() != "" ? Convert.ToDecimal(item["week_52_low"]) : 0.0m
                                    )
                                );
                            }
                        }
                    }
                    else
                    {
                        JObject item = (JObject)obj["quotes"]["quote"];
                        Stock s = stocks.Where((s) => s.Symbol == item["symbol"].ToString()).FirstOrDefault();
                        if (s != null)
                        {
                            var o = item["close"].ToString();
                            result.Add(
                                new Quote(
                                    s.Id,
                                    s.Symbol,
                                    Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["trade_date"])),
                                    item["last"].ToString() != "" ? Convert.ToDecimal(item["last"]) : 0.0m,
                                    item["last_volume"].ToString() != "" ? Convert.ToInt32(item["last_volume"]) : 0,
                                    item["change"].ToString() != "" ? Convert.ToDecimal(item["change"]) : 0.0m,
                                    item["change_percentage"].ToString() != "" ? Convert.ToDecimal(item["change_percentage"]) : 0.0m,
                                    item["open"].ToString() != "" ? Convert.ToDecimal(item["open"]) : 0.0m,
                                    item["high"].ToString() != "" ? Convert.ToDecimal(item["high"]) : 0.0m,
                                    item["low"].ToString() != "" ? Convert.ToDecimal(item["low"]) : 0.0m,
                                    item["prevclose"].ToString() != "" ? Convert.ToDecimal(item["prevclose"]) : 0.0m,
                                    item["close"].ToString() != "" ? Convert.ToDecimal(item["close"]) : 0.0m,
                                    item["volume"].ToString() != "" ? Convert.ToInt32(item["volume"]) : 0,
                                    item["average_volume"].ToString() != "" ? Convert.ToInt32(item["average_volume"]) : 0,
                                    item["bid"].ToString() != "" ? Convert.ToDecimal(item["bid"]) : 0.0m,
                                    item["bidsize"].ToString() != "" ? Convert.ToInt32(item["bidsize"]) : 0,
                                    item["bid_date"].ToString() != "" ? Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["bid_date"])) : new DateTime(),
                                    item["ask"].ToString() != "" ? Convert.ToDecimal(item["ask"]) : 0.0m,
                                    item["asksize"].ToString() != "" ? Convert.ToInt32(item["asksize"]) : 0,
                                    item["ask_date"].ToString() != "" ? Util.DateTimeFromUnixTimestampMilliseconds(Convert.ToInt64(item["ask_date"])) : new DateTime(),
                                    item["week_52_high"].ToString() != "" ? Convert.ToDecimal(item["week_52_high"]) : 0.0m,
                                    item["week_52_low"].ToString() != "" ? Convert.ToDecimal(item["week_52_low"]) : 0.0m
                                )
                            );
                        }
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            return result;
        }

        public bool PlaceOrder(Order order)
        {
            bool result = false;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                StringContent content = new StringContent($"account_id={order.BrokerAccountId}&class=equity&symbol={order.Symbol}&side={GetOrderSideString(order.Side)}&quantity={order.Quantity}&type={GetOrderTypeString(order.Type)}&duration={GetOrderDurationString(order.Duration)}&price={order.Price}&stop={order.Stop}", Encoding.ASCII, "application/x-www-form-urlencoded"); 

                HttpResponseMessage response = client.PostAsync($"/v1/accounts/{order.BrokerAccountId}/orders", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                    order.BrokerOrderId = obj["order"]["id"].ToString();
                    order.Save();
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            return result;
        }

        public bool CancelOrder(Order order)
        {
            bool result = false;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                HttpResponseMessage response = client.DeleteAsync($"/v1/accounts/{order.BrokerAccountId}/orders/{order.BrokerOrderId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);

                    result = true;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            return result;
        }

        public static string GetOrderSideString(Order.OrderSide side)
        {
            switch (side)
            {
                case Order.OrderSide.Buy:
                    return "buy";
                case Order.OrderSide.BuyToCover:
                    return "buy_to_cover";
                case Order.OrderSide.Sell:
                    return "sell";
                case Order.OrderSide.SellShort:
                    return "sell_short";
                default:
                    return "";
            }
        }

        public static string GetOrderTypeString(Order.OrderType type)
        {
            switch (type)
            {
                case Order.OrderType.Market:
                    return "market";
                case Order.OrderType.Limit:
                    return "limit";
                case Order.OrderType.Stop:
                    return "stop";
                case Order.OrderType.StopLimit:
                    return "stop_limit";
                default:
                    return "";
            }
        }

        public static string GetOrderDurationString(Order.OrderDuration duration)
        {
            switch (duration)
            {
                case Order.OrderDuration.Day:
                    return "day";
                case Order.OrderDuration.Gtc:
                    return "gtc";
                case Order.OrderDuration.Pre:
                    return "pre";
                case Order.OrderDuration.Post:
                    return "post";
                default:
                    return "";
            }
        }
    }
}
