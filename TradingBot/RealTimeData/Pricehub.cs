using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using TradingBot.Models;
using TradingBot.Websockets.interfaces;

namespace TradingBot.RealTimeData
{
    public class Pricehub : Hub
    {
        private readonly IStrategy<PriceModel> webSocketStrategy;
        public Pricehub(IStrategy<PriceModel> webSocketStrategy)
        {
            this.webSocketStrategy = webSocketStrategy;
        }

        public async Task PriceGet()
        {
            try
            {
                while (true)
                {
                    var priceModel = await webSocketStrategy.ExecuteAsync();
                    await Clients.Caller.SendAsync("Receive", priceModel);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
