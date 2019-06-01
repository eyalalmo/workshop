using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class PaymentService
    {
        
        private static PaymentService instance = null;
        private readonly HttpClient client = new HttpClient();
        private PaymentService()
        {
           

        }

        public static PaymentService getInstance()
        {
            if (instance == null)
            {
                instance = new PaymentService();
            }
            return instance;
        }

        public  async Task<int> checkOut(string card, string month, string year, string holder, string ccv, string id, double price)
        {
           
                var massage = new Dictionary<string, string>
            {
             { "action_type", "pay" },
             { "card_number", card },
             { "month", month },
             { "year", year },
             { "holder", holder },
             { "ccv", ccv },
             { "id", id }
            };

                var massageToSend = new FormUrlEncodedContent(massage);
                var responseFromServer = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageToSend);
                var responseToString = await responseFromServer.Content.ReadAsStringAsync();

                return int.Parse(responseToString);

            
        }
        public bool cancelPayment(String account, double money)
        {
            return true;
        }

        public bool connectToSystem()
        {
          
            return true;
        }
        public async Task<bool> handShake()
        {
            var massage = new Dictionary<string, string>
            {
                { "action_type", "handshake" },
            };

            var massageToSent = new FormUrlEncodedContent(massage);
            var response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageToSent);
            var responseToString = await response.Content.ReadAsStringAsync();

            if (responseToString == "OK")
                return true;
            return false;
        }

    }
}
