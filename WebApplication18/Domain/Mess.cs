using System;
using System.Collections.Generic;
using WebApplication18.Controllers;

namespace workshop192.Domain
{
    public class Mess
    {
        public static LinkedList<Tuple<int, String>> PendingMessages = new LinkedList<Tuple<int, string>>();

        public static Boolean notifyUser(int sessionid, String message)
        {
            if (sessionid < 0)
            {
                WebSocketController.sendMessageToClient(sessionid, message);
                return true;
            }
            LinkedList<String> CurrentPendingMessages;
            WebSocketController.PendingMessages.TryGetValue(sessionid, out CurrentPendingMessages);
            if (CurrentPendingMessages != null)
            {
                PendingMessages.AddFirst(new Tuple<int, string>(sessionid, message));
                CurrentPendingMessages.AddLast(message);
            }
            else
            {
                PendingMessages.AddFirst(new Tuple<int, string>(sessionid, message));
                CurrentPendingMessages = new LinkedList<String>();
                CurrentPendingMessages.AddLast(message);
                WebSocketController.PendingMessages.Add(sessionid, CurrentPendingMessages);
            }
            return false;
        }
    }
}