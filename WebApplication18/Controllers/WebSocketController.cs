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
        public static Dictionary<string, LinkedList<String>> waitingMessages = takeWaitingMessages();

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
                waitingMessages.TryGetValue(username, out messagesToSend);
                if (messagesToSend != null)
                {
                    foreach (String message in messagesToSend)
                    {
                        messageClient(username, message);
                    }
                    //UserService.getInstance().getWaitingMessages().Remove(new Tuple<int, string>(session, ""));
                    messagesToSend.Clear();
                }
            }
        noSession:
            while (sessionToSocket.ContainsValue(socket) && socket.State == WebSocketState.Open)
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

        public static void messageClient(string username, String message)
        {
            WebSocket socket = null;
            int sessionid = UserService.getInstance().getSessionByUserName(username);
            sessionToSocket.TryGetValue(sessionid, out socket);
            if (sessionid < 0 || socket == null)
            {
                addMessageToDB(username, message);
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
                        addMessageToDB(username, message);
                    }
                }
            }

            catch (System.ObjectDisposedException)
            {
                sessionToSocket.Remove(sessionid);
                addMessageToDB(username, message);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addMessageToDB(string username, String message)
        {
            UserService.getInstance().addWaitingMessage(new Tuple<string, string>(username, message));

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
