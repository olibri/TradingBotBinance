using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using TradingBot.Models;
using TradingBot.Websockets.interfaces;
using Microsoft.Extensions.Configuration;
namespace TradingBot.Services.RestApi
{
    public class SetValue
    {
        public HttpClient httpClient { get; private set; } 
        public string endpoint {  get;  set; }
        public long timeStamp {  get; private set; }
        public string query { get; private set; }
        public string signature {  get; private set; }
        public HttpRequestMessage client {  get; private set; }
        public SetValue()
        {
            this.httpClient = new HttpClient();
            this.endpoint = "";
            this.timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.query = "";
            this.signature = "";
            this.client = new HttpRequestMessage();
        }
        /// <summary>
        /// example: /fapi/v1/leverage
        /// </summary>
        /// <param name="_endpoint"></param>
        /// <returns></returns>
        public async Task<string> SetInstance(string _endpoint, string _setValue)
        {
            var configuration = Configuration.GetInstance();
            this.endpoint = $"{configuration.endpoint}{_endpoint}";

            this.query = $"symbol={configuration.symbol}&{_setValue}&recvWindow=9999999&timestamp={timeStamp}";

            this.client = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}?{this.query}&signature={SignatureService.CreateSignature(query, configuration.apiSecret)}");
            
            client.Headers.Add("X-MBX-APIKEY", configuration.apiKey);
            var response = await httpClient.SendAsync(this.client);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
            return response.Content.ReadAsStream().ToString();
        }

        //shit for rewriting
        public async Task<string> GetListenKey(string _endpoint)
        {
            var configuration = Configuration.GetInstance();
            this.endpoint = $"{configuration.endpoint}{_endpoint}";

            this.query = $"recvWindow=9999999&timestamp={timeStamp}";

            this.client = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}?signature={SignatureService.CreateSignature(query, configuration.apiSecret)}");

            client.Headers.Add("X-MBX-APIKEY", configuration.apiKey);
            var response = await httpClient.SendAsync(this.client);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
            var doge = JsonConvert.DeserializeObject<listenKeyModel>(await response.Content.ReadAsStringAsync());

            return doge.listenKey;
        }
    }    
}
