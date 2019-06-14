using System;
using System.Collections.Generic;
using WebApplication18.DAL;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace WebApplication18.Domain
{
    public class DBNotifications :Connector
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
            //waitingNotifications = remains;
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        foreach (Tuple<string, string> message in remains)
                        {
                            string sql = "INSERT INTO [dbo].[Notification] (username, message)" +
                                         " VALUES (@username, @message)";
                            connection.Execute(sql, new { message.Item1, message.Item2 });
                        }
                    }
                }
                //connection.Close();
            }

            catch (Exception)
            {
                //connection.Close();
            }
        }

        public void initTests()
        {
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        // SqlConnection connection = Connector.getInstance().getSQLConnection();
                        connection.Execute("DELETE FROM Notification");
                        //connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //connection.Close();
            }
        }


        internal void init()
        {
            //waitingNotifications.AddFirst(new Tuple<string, string>("ey", "hi there"));
            try
            {
                string username = "ey";
                string message = "an old message to ey";
              //  SqlConnection connection = Connector.getInstance().getSQLConnection();
                string sql = "INSERT INTO [dbo].[Notification] (username, message)" +
                             " VALUES (@username, @message)";
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        connection.Execute(sql, new { username, message });
                    }
                }
                //connection.Close();
            }

            catch (Exception)
            {
                //connection.Close();
            }
        }

        internal void clearMessagesFor(string username)
        {
            try
            {
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        //SqlConnection connection = Connector.getInstance().getSQLConnection();
                        connection.Execute("DELETE FROM Notification WHERE username=@username ", new { username });
                        //connection.Close();
                    }
                }
            }

            catch (Exception)
            {
                //connection.Close();
            }
            
            /*
            LinkedList<Tuple<string, string>> toDel = new LinkedList<Tuple<string, string>>();
            foreach (Tuple<string, string> mess in waitingNotifications)
                if(mess.Item1 == username)
                    toDel.AddFirst(mess);
            foreach (Tuple<string, string> mess in toDel)
                waitingNotifications.Remove(mess);
            */
        }

        internal LinkedList<string> getMessagesFor(string username)
        {
            LinkedList<string> result = new LinkedList<string>();
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        var c = connection.Query<Notification>("SELECT username, message FROM [dbo].[Notification] WHERE username=@username ", new { username });
                        if (c.Count() == 0)
                        {
                            //connection.Close();
                            return result;
                        }

                        foreach (Notification message in c)
                        {
                            result.AddFirst(message.message);
                        }

                        //connection.Close();
                        return result;
                    }
                }
            }

            catch (Exception)
            {
                //connection.Close();
                return result;
            }

            /*            LinkedList<string> result = new LinkedList<string>();
                        foreach (Tuple<string, string> mess in waitingNotifications)
                            if(mess.Item1 == username)
                                result.AddFirst(mess.Item2);
                        return result;
                        */
        }

        internal void addMessage(Tuple<string, string> tuple)
        {
            //waitingNotifications.AddFirst(tuple);
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                string sql = "INSERT INTO [dbo].[Notification] (username, message)" +
                                 " VALUES (@username, @message)";
                lock (connection)
                {
                    using (connection)
                    {
                        connection.Open();
                        connection.Execute(sql, new { username = tuple.Item1, message = tuple.Item2 });
                        //connection.Close();
                    }
                }
            }

            catch (Exception)
            {
                //connection.Close();
            }
        }
    }
}