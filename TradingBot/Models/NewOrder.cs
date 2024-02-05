using System.Web;

namespace TradingBot.Models
{   //from 0 
    public enum Side
    {
        BUY, SELL
    }
    public enum PositionSide {
        BOTH, LONG, SHORT
    }
    public enum Type {
        LIMIT, MARKET, STOP, STOP_MARKET, TAKE_PROFIT, TAKE_PROFIT_MARKET, TRAILING_STOP_MARKET
    }


    public class NewOrder
    {
        //public string symbol {  get; set; }
        public string side {  get; set; }
        //public string positionSide {get;set; } //Default BOTH for One-way Mode ; LONG or SHORT for Hedge Mode. It must be sent in Hedge Mode.
        public string type {  get; set; }

        public decimal quantity {  get; set; }
        //public Side side {  get; set; }
        public decimal price { get; set; }
        public decimal stopPrice { get; set; }

        //public decimal activationPrice { get; set; } //Used with TRAILING_STOP_MARKET orders, default as the latest price(supporting different workingType)

        public override string ToString()
        {           
            return $"{type}\n{quantity}\n{stopPrice}";
        }
        public static string ToQueryString(NewOrder order)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            //query["symbol"] = order.symbol;
            query["side"] = order.side;
            //query["positionSide"] = order.positionSide;
            query["type"] = order.type;
            query["quantity"] = order.quantity.ToString();
            query["price"] = order.price.ToString();
            query["stopPrice"] = order.stopPrice.ToString();
            //query["activationPrice"] = order.activationPrice.ToString();

            return query.ToString();
        } 
        public static string ToProfitString(NewOrder order)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            //query["symbol"] = order.symbol;
            query["side"] = order.side;
            //query["positionSide"] = order.positionSide;
            query["type"] = order.type;
            query["quantity"] = order.quantity.ToString();
            query["stopPrice"] = order.stopPrice.ToString();
            //query["activationPrice"] = order.activationPrice.ToString();

            return query.ToString();
        }
      
    }
}
