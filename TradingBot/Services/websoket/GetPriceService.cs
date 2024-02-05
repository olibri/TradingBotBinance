using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using TradingBot.Models;
using TradingBot.Services.RestApi;
using TradingBot.Websockets.interfaces;

namespace TradingBot.Services.websoket
{

    public class GetPriceService : IStrategy<PriceModel>
    {
        public async Task<PriceModel> ExecuteAsync()
        {
            using (ClientWebSocket client = new ClientWebSocket())
            {
                try
                {
                    //Uri binanceUrl = new Uri("wss://fstream.binance.com/ws/btcusdt@account");
                    Uri binanceUrl = new Uri("wss://fstream.binance.com/ws/arbusdt@aggTrade");

                    //Uri binanceUrl = new Uri("wss://fstream.binance.com/ws/mckPQ6ifvKudd1zC6kNE9uEHHGNVpvi4QEp1nwWakOrKN0srQvu6dZ0zaj5VSux3");

                    await client.ConnectAsync(binanceUrl, CancellationToken.None);

                    while (client.State == WebSocketState.Open)
                    {
                        byte[] buffer = new byte[1024];
                        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            //Console.WriteLine(json);
                            var priceInfo = GetPrice(json);

                            return priceInfo;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return null;
            }
        }
        private PriceModel GetPrice(string json)
        {
            try
            {
                var doge = JsonConvert.DeserializeObject<PriceModel>(json);
                return doge;
            }
            catch (Exception e)
            {
                Console.WriteLine("JSON parsing error: " + e.Message);
                return null;
            }
        }
        //public async Task checkPrice(NewOrder newOrder)
        //{
        //    SetValue setService = new SetValue();

        //    decimal stopLoss = newOrder.price - (newOrder.price * (decimal)0.001);
        //    decimal takeProfit = newOrder.price + (newOrder.price * (decimal)0.003);

        //    newOrder.stopPrice = stopLoss;

        //    string tpQuery = $"side=SELL&type=TAKE_PROFIT_MARKET&quantity={newOrder.quantity}&stopPrice={takeProfit}&reduceOnly=true";
        //    string stQuery = $"side=BUY&type=STOP_MARKET&quantity={newOrder.quantity}&stopPrice={stopLoss}&reduceOnly=true";


        //    await setService.SetInstance("/fapi/v1/order", tpQuery);
        //    await setService.SetInstance("/fapi/v1/order", stQuery);
        //}
    }
}
