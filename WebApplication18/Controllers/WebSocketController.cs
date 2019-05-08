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
        static readonly Dictionary<int, WebSocket> sessionToSocket = new Dictionary<int, WebSocket>();
        public static Dictionary<int, LinkedList<String>> waitingMessages = takeWaitingMessages();

        public static Dictionary<int, LinkedList<String>> takeWaitingMessages()
        {
            Dictionary<int, LinkedList<String>> waitings = new Dictionary<int, LinkedList<String>>();
            LinkedList<Tuple<string, string>> oldWaitingMessages = UserService.getInstance().getWaitingMessages();
            LinkedList<Tuple<string, string>> remains = new LinkedList<Tuple<string, string>>();

            foreach (Tuple<string, string> newMessage in oldWaitingMessages)
            {
                int session = UserService.getInstance().getSessionByUserName(newMessage.Item1);
                if (session >= 0) { 
                    if(!waitings.ContainsKey(session))
                        waitings.Add(session, new LinkedList<string>());
                    else
                        remains.AddLast(newMessage);
                }
                waitings[session].AddFirst(newMessage.Item2);
            }
            UserService.getInstance().setWaitingMessages(remains);
            return waitings;
        }

        public HttpResponseMessage Get()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest(communicate);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task communicate(AspNetWebSocketContext context)
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
                waitingMessages.TryGetValue(session, out messagesToSend);
                if (messagesToSend != null)
                {
                    foreach (String message in messagesToSend)
                    {
                        messageClient(session, message);
                    }
                    //UserService.getInstance().getWaitingMessages().Remove(new Tuple<int, string>(session, ""));
                    messagesToSend.Clear();
                }
            }
            
             while (socket.State == WebSocketState.Open)
            {
                /*WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None)
                                                            .ConfigureAwait(false);
                String userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                userMessage = "You sent: " + userMessage + " at " +
                    DateTime.Now.ToLongTimeString() + " from ip " + context.UserHostAddress.ToString();
                var sendbuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));


                await socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None)
                            .ConfigureAwait(false);
                  */
            } 
        }

        public static void messageClient(int sessionid, String message)
        {
            WebSocket socket = null;
            sessionToSocket.TryGetValue(sessionid, out socket);
            if (socket == null)
            {
                addMessageToDB(sessionid, message);
                return;
            }
            try
            {
                if (socket.State == WebSocketState.Open)
                    socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), 
                        WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);
                else
                {
                    lock (sessionToSocket)
                    {
                        sessionToSocket.Remove(sessionid);
                        addMessageToDB(sessionid, message);
                    }
                }
            }

            catch (System.ObjectDisposedException)
            {
                sessionToSocket.Remove(sessionid);
                addMessageToDB(sessionid, message);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addMessageToDB(int sessionid, String message)
        {
            string username = UserService.getInstance().getUserNameBySession(sessionid);

            LinkedList<String> messagesToSend;

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
            }
        }

        public void notifyUser(string username, String message)
        {
            int session = UserService.getInstance().getSessionByUserName(username);
            if (session < 0)
            {
                messageClient(session, message);
                return;
            }

            LinkedList<String> CurrentPendingMessages;

            int sessionid = UserService.getInstance().getSessionByUserName(username);

            waitingMessages.TryGetValue(sessionid, out CurrentPendingMessages);

            if (CurrentPendingMessages != null)
            {
                UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));
                CurrentPendingMessages.AddLast(message);
            }
            else
            {
                UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));
                CurrentPendingMessages = new LinkedList<String>();
                CurrentPendingMessages.AddLast(message);
                waitingMessages.Add(sessionid, CurrentPendingMessages);
            }
            return;
        }
    }
}
