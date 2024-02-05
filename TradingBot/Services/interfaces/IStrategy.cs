using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;

namespace TradingBot.Websockets.interfaces
{
    public interface IStrategy<T>
    {
        Task<T> ExecuteAsync();
    }
}
