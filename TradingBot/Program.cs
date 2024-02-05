using Microsoft.Extensions.Caching.Distributed;
using TradingBot.Models;
using TradingBot.RealTimeData;
using TradingBot.Services.websoket;
using TradingBot.Websockets.interfaces;

namespace TradingBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR().AddMessagePackProtocol();
            builder.Services.AddSingleton<IStrategy<PriceModel>, GetPriceService>();
            builder.Services.AddSingleton<OrderModel>();
            //builder.Services.AddSingleton<NewOrder>();
            //builder.Services.AddSingleton<GetOpenPosition>();

            builder.Services.AddHostedService<OpenPositionService>();

          
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseWebSockets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
    
            app.MapHub<Pricehub>("/price");
     
            app.Run();
        }
    }
}
