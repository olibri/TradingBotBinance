using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using TradingBot.Models;
using TradingBot.RealTimeData;
using TradingBot.Services;
using TradingBot.Services.RestApi;
using TradingBot.Services.websoket;
using static System.Net.Mime.MediaTypeNames;

namespace TradingBot.Controllers
{
    public class SetOrderController : Controller
    {
        private SetValue setService;
        OrderModel orderModel;
        //private readonly IDistributedCache _cache;
        public SetOrderController()
        {
            setService = new SetValue();
            orderModel = new OrderModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewOrder newOrder)
        {
            //var key = "OrderModelKey";
            //var value = JsonConvert.SerializeObject(newOrder);

            //await _cache.SetStringAsync(key, "value");

            orderModel.SetCurentOrder(newOrder);
            string query = NewOrder.ToQueryString(newOrder);

            //Console.WriteLine(query);
            await setService.SetInstance("/fapi/v1/order", query);


            //var order = new GetOpenPosition();
            //await order.ExecuteAsync();


            //string tpQuery = $"type=TAKE_PROFIT_MARKET&stopPrice={stopLoss}";


            //await setService.SetInstance("/fapi/v1/leverage", $"&leverage=20");               // i have to add chance to choose leverage from ui 
            //await setService.SetInstance("/fapi/v1/marginType", "marginType", "ISOLATED") ;   //ISOLATED, CROSSED

            return Json(new { success = true, message = "Order set 👌" });
            //return Ok();
        }
      

    }
}
