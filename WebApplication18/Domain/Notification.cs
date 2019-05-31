using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class Notification
    {
        public string username;
        public string message;

        public Notification(string username, string message) {
            this.username = username;
            this.message = message;
        }


    }
}