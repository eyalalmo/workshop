﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Exceptions;

namespace workshop192.Domain
{
    class DeliveryService
    {

        private static DeliveryService instance = null;
        private readonly HttpClient client = new HttpClient();
        int toLock = 0;

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

        public int sendToUser(string name, string address, string city, string country, string zip, string cvv)
        {
            lock (client)
            {
                var massage = new Dictionary<string, string>
            {
             { "action_type", "supply" },
             { "name", name },
             { "address", address },
             { "city",city },
             { "country", country },
             { "zip", zip }
            };
                using (var client1 = new HttpClient())
                {
                    var massageToSent = new FormUrlEncodedContent(massage);
                    var response = client1.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageToSent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        string responseString = responseContent.ReadAsStringAsync().Result;
                        return int.Parse(responseString);

                    }
                    else
                        throw new ExternalConnectionException("Delivery system connection error");
                }
            }
        }

        public bool connectToSystem()
        {
            handShake();
            return true;
        }
        public bool handShake()
        {
            lock (client)
            {
                var massage = new Dictionary<string, string>
            {
                { "action_type", "handshake" },
            };

                using (var client1 = new HttpClient())
                {
                    var massageToSent = new FormUrlEncodedContent(massage);
                    var response = client1.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageToSent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        string responseString = responseContent.ReadAsStringAsync().Result;
                        if (responseString == "OK")
                        {
                            return true;
                        }

                    }
                    else
                        throw new ExternalConnectionException("Delivery system connection error");
                }
                return false;
            }
        }

        public int cancelDelivery(string id)
        {
            lock (client)
            {
                var massage = new Dictionary<string, string>
            {
             { "action_type", "cancel_supply" },
             { "transaction_id", id}

            };

                using (var client1 = new HttpClient())
                {
                    var massageToSent = new FormUrlEncodedContent(massage);
                    var response = client1.PostAsync("https://cs-bgu-wsep.herokuapp.com/", massageToSent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        string responseString = responseContent.ReadAsStringAsync().Result;
                        return int.Parse(responseString);
                    }
                    else
                        throw new ExternalConnectionException("Delivery system connection error");
                }
            }
        }
    }
}
