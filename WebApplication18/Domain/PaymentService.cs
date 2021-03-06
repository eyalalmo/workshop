﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Exceptions;

namespace workshop192.Domain
{
    class PaymentService
    {
        
        private static PaymentService instance = null;
        private readonly HttpClient client = new HttpClient();
        public int toLock = 0;

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

        public  int checkOut(string card, string month, string year, string holder, string ccv, string id)
        {
            lock (client)
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
                        throw new ExternalConnectionException("Payment system connection error");
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

                        // by calling .Result you are synchronously reading the result
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        if (responseString == "OK")
                        {
                            return true;
                        }

                    }
                    else
                        throw new ExternalConnectionException("Payment system connection error");

                }
                return false;
            }
        }

        public  int cancelPayment(string id)
        {
            lock (client)
            {
                var massage = new Dictionary<string, string>
            {
             { "action_type", "cancel_pay" },
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
                        throw new ExternalConnectionException("Payment system connection error");
                }
            }
        }
    }
}
