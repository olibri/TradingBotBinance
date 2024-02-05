namespace TradingBot.Services
{
    public class Configuration
    {
        private static Configuration instance;

        public string apiKey { get; private set; }
        public string apiSecret { get; private set; }
        //public int leverage { get; private set; }
        public string symbol { get; private set; }
        public string endpoint { get; private set; }
        private Configuration()
        {
            apiKey = "vEa7eKxqBvjze5VpVYZvwWXjAsOntWSgAl1p7rm8LsCr2aik2sNq7d0c0g365lSU";
            apiSecret = "fzVm4uesJMmDvaCM1qt9UXB69pe6oq628JoiqeI726cI25kA4TZBX0DA4mFIvBWW";
            //leverage = 22;
            symbol = "ARBUSDT";
            endpoint = "https://fapi.binance.com";
        }

        public static Configuration GetInstance()
        {
            if (instance == null)
            {
                instance = new Configuration();
            }
            return instance;
        }
    }
}
