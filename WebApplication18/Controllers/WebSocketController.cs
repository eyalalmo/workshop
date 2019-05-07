using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace WebApplication18.Controllers
{
    public class WebSocketController : ApiController
    {
        static readonly Dictionary<int, WebSocket> _users = new Dictionary<int, WebSocket>();
        public static Dictionary<int, LinkedList<String>> PendingMessages = initPendingMessages();

        public static Dictionary<int, LinkedList<String>> initPendingMessages()
        {
            Dictionary<int, LinkedList<String>> ans = new Dictionary<int, LinkedList<String>>();
            foreach (Tuple<int, string> msg in Mess.PendingMessages)
            {
                if (!ans.ContainsKey(msg.Item1))
                {
                    ans.Add(msg.Item1, new LinkedList<string>());
                }
                ans[msg.Item1].AddFirst(msg.Item2);
            }
            return ans;
        }

        public HttpResponseMessage Get()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest(Process);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task Process(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4096]);
            string hash = "";
            if (context.CookieCollection[0].Name == "HashCode")
            {
                hash = context.CookieCollection[0].Value;
            }
            else if (context.CookieCollection.Count > 1 && context.CookieCollection[1].Name == "HashCode")
            {
                hash = context.CookieCollection[1].Value;
            }
            else
            {
                return;
            }

            int session = UserService.getInstance().getUserByHash(hash);
            updateSocket(session, socket);

            if (socket.State == WebSocketState.Open)
            {
                LinkedList<String> CurrentPendingMessages;
                PendingMessages.TryGetValue(session, out CurrentPendingMessages);
                if (CurrentPendingMessages != null)
                {
                    foreach (String message in CurrentPendingMessages)
                    {
                        sendMessageToClient(session, message);
                    }
                    Mess.PendingMessages.Remove(new Tuple<int, string>(session, ""));
                    CurrentPendingMessages.Clear();
                }
            }


            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None)
                                                            .ConfigureAwait(false);
                String userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                userMessage = "You sent: " + userMessage + " at " +
                    DateTime.Now.ToLongTimeString() + " from ip " + context.UserHostAddress.ToString();
                var sendbuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));


                await socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None)
                            .ConfigureAwait(false);
            }
        }

        public static void sendMessageToClient(int sessionid, String message)
        {
            WebSocket socket = null;
            _users.TryGetValue(sessionid, out socket);
            if (socket == null)
            {
                addMessage(sessionid, message);
                return; //no such socket exists
            }
            try
            {
                if (socket.State == WebSocketState.Open)
                {

                    var sendbuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));

                    socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None)
                                .ConfigureAwait(false);
                }
                else
                {
                    lock (_users) //make sure the socket wasn't reconnected so we won't lose the socket
                    {
                        _users.Remove(sessionid);
                        addMessage(sessionid, message);
                    }

                }
            }
            catch (System.ObjectDisposedException /*e*/)
            {
                _users.Remove(sessionid);
                addMessage(sessionid, message);
            }
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void addMessage(int sessionid, String message)
        {
            LinkedList<String> CurrentPendingMessages;
            PendingMessages.TryGetValue(sessionid, out CurrentPendingMessages);
            if (CurrentPendingMessages == null)
            {
                Mess.PendingMessages.AddFirst(new Tuple<int, string>(sessionid, message));
                CurrentPendingMessages = new LinkedList<String>();
                CurrentPendingMessages.AddLast(message);
                PendingMessages.Add(sessionid, CurrentPendingMessages);
            }
            else
            {
                Mess.PendingMessages.AddFirst(new Tuple<int, string>(sessionid, message));
                CurrentPendingMessages.AddLast(message);
            }
        }

        private static void updateSocket(int sessionid, WebSocket socket)
        {
            WebSocket soc;
            _users.TryGetValue(sessionid, out soc);
            if (soc != null)
                _users.Remove(sessionid);
            _users.Add(sessionid, socket);
        }
    }
}
