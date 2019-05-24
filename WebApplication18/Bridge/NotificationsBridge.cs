using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication18.Controllers;

namespace workshop192.Bridge
{
    public class NotificationsBridge : Messager
    {
        private static NotificationsBridge instance;

        public static NotificationsBridge getInstance()
        {
            if (instance == null)
                instance = new NotificationsBridge();
            return instance;
        }

        private NotificationsBridge()
        { }
        
        public void message(string username, string message)
        {
            WebSocketController.message(username, message);
        }

        public void setObserver(Observer o)
        {
            o.observe(this);
        }
    }
}