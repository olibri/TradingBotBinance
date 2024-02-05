using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using TradingBot.Models;
using TradingBot.Services.RestApi;
using TradingBot.Services.websoket;

namespace TradingBot.RealTimeData
{
    public class OpenPositionService: BackgroundService
    {
        SetValue setService; 
        public OpenPositionService() {

            setService = new SetValue();    
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            var getOpenPosition = new GetOpenPosition();

            while (!stoppingToken.IsCancellationRequested)
            {
                var result =  await getOpenPosition.ExecuteAsync();
                await ProcessMessage(result);
            }
        }
        private async Task  ProcessMessage(string jsonMessage)
        {
            var message = JObject.Parse(jsonMessage);
            var eventType = (string)message["e"];

            if (eventType == "ORDER_TRADE_UPDATE")
            {
                var orderStatus = (string)message["o"]["X"];
                if (orderStatus == "EXPIRED")
                {
                    var price = (decimal)message["o"]["p"];
                    var quantity = (decimal)message["o"]["q"];
                    var side = (string)message["o"]["s"];                  

                    Console.WriteLine("Order EXPIRED: " + jsonMessage);

                    decimal stopLoss = Math.Round(price - (price * (decimal)0.001), 4);
                    decimal takeProfit = Math.Round(price + (price * (decimal)0.003), 4);

                    side = side == "BUY" ? "SELL" : "BUY";

                    string stQuery = $"side={side}&type=STOP_MARKET&quantity={quantity}&stopPrice={stopLoss}&reduceOnly=true";
                    string tpQuery = $"side={side}&type=TAKE_PROFIT_MARKET&quantity={quantity}&stopPrice={takeProfit}&reduceOnly=true";

                    await setService.SetInstance("/fapi/v1/order", stQuery);
                    await setService.SetInstance("/fapi/v1/order", tpQuery);
                }
                else
                {
                    Console.WriteLine("Order Update: " + jsonMessage);
                }
            }
        }

    }
}
//private async Task ProcessMessage(string jsonMessage)
//{
//    var message = JObject.Parse(jsonMessage);
//    var eventType = (string)message["e"];

//    if (eventType == "ORDER_TRADE_UPDATE")
//    {
//        var orderStatus = (string)message["o"]["X"];
//        if (orderStatus == "FILLED")
//        {
//            Console.WriteLine("Order FILLED: " + jsonMessage);

//            decimal stopLoss = orderModel.GetCurentOrder().price - (orderModel.GetCurentOrder().price * (decimal)0.001);
//            decimal takeProfit = orderModel.GetCurentOrder().price + (orderModel.GetCurentOrder().price * (decimal)0.003);

//            orderModel.GetCurentOrder().stopPrice = stopLoss;

//            string tpQuery = $"side=SELL&type=TAKE_PROFIT_MARKET&quantity={orderModel.GetCurentOrder().quantity}&stopPrice={takeProfit}&reduceOnly=true";
//            string stQuery = $"side=BUY&type=STOP_MARKET&quantity={orderModel.GetCurentOrder().quantity}&stopPrice={stopLoss}&reduceOnly=true";

//            await setService.SetInstance("/fapi/v1/order", stQuery);
//            await setService.SetInstance("/fapi/v1/order", tpQuery);
//        }
//        else
//        {
//            Console.WriteLine("Order Update: " + jsonMessage);
//        }
//    }
//}