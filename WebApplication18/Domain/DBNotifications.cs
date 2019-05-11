using System;
using System.Collections.Generic;

namespace WebApplication18.Domain
{
    public class DBNotifications
    {
        private static DBNotifications instance;
        private LinkedList<Tuple<String, String>> waitingNotifications;

        public static DBNotifications getInstance()
        {
            if (instance == null)
            {
                instance = new DBNotifications();
            }
            return instance;
        }

        private DBNotifications()
        {
            waitingNotifications = new LinkedList<Tuple<string, string>>();
        }

        public LinkedList<Tuple<string, String>> getWaitingNotifications() {
            return waitingNotifications;
        }

        internal void setWaitingNotifications(LinkedList<Tuple<string, string>> remains)
        {
            waitingNotifications = remains;
        }

        internal void init()
        {
            waitingNotifications.AddFirst(new Tuple<string, string>("ey", "hi there"));
        }
    }
}