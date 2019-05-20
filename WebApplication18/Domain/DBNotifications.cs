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

        internal void clearMessagesFor(string username)
        {
            LinkedList<Tuple<string, string>> toDel = new LinkedList<Tuple<string, string>>();
            foreach (Tuple<string, string> mess in waitingNotifications)
                toDel.AddFirst(mess);
            foreach (Tuple<string, string> mess in toDel)
                waitingNotifications.Remove(mess);
        }

        internal LinkedList<string> getMessagesFor(string username)
        {
            LinkedList<string> result = new LinkedList<string>();
            foreach (Tuple<string, string> mess in waitingNotifications)
                result.AddFirst(mess.Item2);
            return result;
        }

        internal void addMessage(Tuple<string, string> tuple)
        {
            waitingNotifications.AddFirst(tuple);
        }
    }
}