using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using TradingBot.Models;
using TradingBot.Services.RestApi;
using TradingBot.Websockets.interfaces;

namespace TradingBot.Services.websoket
{
    public class GetOpenPosition
    {
        private readonly SetValue setService = new SetValue();
        //private readonly OrderModel newOrder = new OrderModel();

        //private readonly IDistributedCache cache;
        //public GetOpenPosition(IDistributedCache _cache)
        //{
        //    cache = _cache;
        //}
        public async Task<string> ExecuteAsync()
        {
            var s = await setService.GetListenKey("/fapi/v1/listenKey");

            Timer timer = new Timer(async _ => await RenewKey(), null, 0, 1800000);

            using (ClientWebSocket client = new ClientWebSocket())
            {
                try
                {
                    Uri binanceUrl = new Uri($"wss://fstream.binance.com/ws/{s}");

                    await client.ConnectAsync(binanceUrl, CancellationToken.None);
                    while (client.State == WebSocketState.Open)
                    {
                        byte[] buffer = new byte[1024];
                        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            //await ProcessMessage(json);

                            //Console.WriteLine(json);

                            //var priceInfo = GetPrice(json);

                            return json;
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


       

        private async Task RenewKey()
        {
            await setService.GetListenKey("/fapi/v1/listenKey");
            //Console.WriteLine(s);
        }
    }
}
