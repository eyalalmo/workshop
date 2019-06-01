using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DeliveryService
    {

        private static DeliveryService instance = null;
        private readonly HttpClient client = new HttpClient();

        private DeliveryService()
        {


        }

        public static DeliveryService getInstance()
        {
            if (instance == null)
            {
                instance = new DeliveryService();
            }
            return instance;
        }

        public async Task<int> sendToUser(string name, string address, string city, string country, string zip, string cvv)
        {
            if (DeliveryService.getInstance().handShake().Result != true)
            {
                throw new StoreException("cant handshake");

            }
            var massage = new Dictionary<string, string>
            {
             { "action_type", "supply" },
             { "name", name },
             { "address", address },
             { "city",city },
             { "country", country },
             { "zip", zip }
            };
            var massageFromServer = new FormUrlEncodedContent(massage);

            var result = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageFromServer);

            var responseToString = await result.Content.ReadAsStringAsync();
            
            return int.Parse(responseToString);
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
