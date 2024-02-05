using Newtonsoft.Json;

namespace TradingBot.Models
{
    public class OrderModel
    {
        private NewOrder newOrder; 
        public OrderModel() { 
            newOrder = new NewOrder();  
        }
        
        public void SetCurentOrder(NewOrder order)
        {
            newOrder = order;
        }
        public NewOrder GetCurentOrder() { 
        
            return newOrder;
        }
    }

}