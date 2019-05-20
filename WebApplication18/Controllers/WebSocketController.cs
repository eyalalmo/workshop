using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebSockets;
using workshop192.ServiceLayer;

namespace WebApplication18.Controllers
{
    public class WebSocketController : ApiController
    {
        public static Dictionary<int, WebSocket> sessionToSocket = new Dictionary<int, WebSocket>();

/*      public static Dictionary<string, LinkedList<String>> waitingMessages = takeWaitingMessages();

        public static Dictionary<string, LinkedList<String>> takeWaitingMessages()
        {
            Dictionary<string, LinkedList<String>> waitings = new Dictionary<string, LinkedList<String>>();
            LinkedList<Tuple<string, string>> oldWaitingMessages = UserService.getInstance().getWaitingMessages();
            LinkedList<Tuple<string, string>> remains = new LinkedList<Tuple<string, string>>();

            foreach (Tuple<string, string> newMessage in oldWaitingMessages)
            {
                if (!waitings.ContainsKey(newMessage.Item1))
                    waitings.Add(newMessage.Item1, new LinkedList<string>());
                else
                    remains.AddLast(newMessage);
                waitings[newMessage.Item1].AddFirst(newMessage.Item2);

            }
            UserService.getInstance().setWaitingMessages(remains);
            return waitings;
        }
*/
        public HttpResponseMessage Get()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest(Communicate);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task Communicate(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4096]);
            string hashSession = "";
            if (context.CookieCollection[0].Name == "HashCode")
            {
                hashSession = context.CookieCollection[0].Value;
            }
            else if (context.CookieCollection.Count > 1 && context.CookieCollection[1].Name == "HashCode")
            {
                hashSession = context.CookieCollection[1].Value;
            }
            else
            {
                return;
            }

            int session = UserService.getInstance().getUserByHash(hashSession);

            if (sessionToSocket.ContainsKey(session))
                sessionToSocket.Remove(session);
            sessionToSocket.Add(session, socket);

            if (socket.State == WebSocketState.Open)
            {
                LinkedList<String> messagesToSend;
                string username;
                try
                {
                    username = UserService.getInstance().getUserNameBySession(session);
                }
                catch (Exception)
                {
                    goto noSession;
                }
                LinkedList<string> waitingMessages = UserService.getInstance().getMessagesFor(username);
                if (waitingMessages != null)
                {
                    foreach (String message in waitingMessages)
                    {
                        messageClient(username, message);
                    }
                    //UserService.getInstance().getWaitingMessages().Remove(new Tuple<int, string>(session, ""));
                    UserService.getInstance().clearMessagesFor(username);
                }
            }
        noSession:
            while (sessionToSocket.ContainsValue(socket) && socket.State == WebSocketState.Open) {}
        }

        public static void messageClient(string username, String message)
        {
            WebSocket socket = null;
            LinkedList<int> sessions = UserService.getInstance().getSessionByUserName(username);
            if (sessions.Count == 0)
            {
                addMessageToDB(username, message);
                return;
            }
            bool sentOnce = false;
            foreach (int sessionid in sessions)
            {
                sessionToSocket.TryGetValue(sessionid, out socket);
                try
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                            WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
                        sentOnce = true;
                    }
                    else
                    {
                        lock (sessionToSocket)
                        {
                            sessionToSocket.Remove(sessionid);
                        }
                    }
                }
                catch (System.ObjectDisposedException)
                {
                    sessionToSocket.Remove(sessionid);
                }
            }
            if(!sentOnce)
                addMessageToDB(username, message);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addMessageToDB(string username, String message)
        {
            UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));

            /*LinkedList<string> messagesToSend;
            waitingMessages.TryGetValue(username, out messagesToSend);

            if (messagesToSend != null)
            {
                messagesToSend.AddLast(message);
            }
            else
            {
                messagesToSend = new LinkedList<string>();
                messagesToSend.AddLast(message);
                waitingMessages.Add(username, messagesToSend);
            }
                /*
                LinkedList<String> messagesToSend;
                int sessionid = UserService.getInstance().getSessionByUserName(username);

                waitingMessages.TryGetValue(sessionid, out messagesToSend);
                if (messagesToSend == null)
                {
                    UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));
                    messagesToSend = new LinkedList<String>();
                    messagesToSend.AddLast(message);
                    waitingMessages.Add(sessionid, messagesToSend);
                }
                else
                {
                    UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));
                    messagesToSend.AddLast(message);
                }*/
        }
    }
}
