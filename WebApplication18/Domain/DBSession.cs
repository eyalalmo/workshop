using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.DAL;
using Dapper;

namespace workshop192.Domain
{
    public class DBSession :Connector
    {
        private static DBSession instance;
        private Dictionary<int, Session> sessions;
       // private static int sessionNum = 1;

        public static DBSession getInstance()
        {
            if (instance == null)
                instance = new DBSession();
            return instance;
        }

        private DBSession()
        {
            sessions = new Dictionary<int, Session>();
        }

        public void init()
        {
            sessions = new Dictionary<int, Session>();
        }

        public int generate()
        {
            try
            {
                connection.Open();
                int sessionNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "session" }).First();
                int next = sessionNum + 1;
                connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "session" });
                string sql = "INSERT INTO [dbo].[Session] (id)" +
                                 " VALUES (@id)";
                connection.Execute(sql, new {id=sessionNum});
                sessions.Add(sessionNum, new Session());

                connection.Close();
                return sessionNum;
            }
            catch (Exception)
            {
                connection.Close();
                return -1;
            }
        }

        internal Session getSession(int sessionid)
        {
            try
            {
                connection.Open();
                var v = connection.Query<Session>("SELECT id FROM [dbo].[Session] WHERE id=@id ", new { id = sessionid });
                if (!sessions.ContainsKey(sessionid))
                {
                    Session s = new Session();
                    sessions.Add(sessionid,s);
                    connection.Close();
                    return s;
                }
                else
                {
                    sessions.TryGetValue(sessionid, out Session s);
                    connection.Close();
                    return s;
                }
                
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }

        public void removeSession(int s)
        {
            if (!sessions.ContainsKey(s))
                throw new DoesntExistException("session doesnt exist");
            try
            {
                sessions.Remove(s);
                connection.Open();
                connection.Execute("DELETE FROM Session WHERE  id=@id ", new { id = s });
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();

            }
        }

        public Session getSessionOfSubscribedUser(SubscribedUser sub)
        {
            foreach (KeyValuePair<int, Session> s in sessions)
            {
                if (s.Value.getSubscribedUser().Equals(sub))
                {
                    return s.Value;
                }
            }
            throw new DoesntExistException("session doesnt exist");
        }
        
        public void initSession()
        {
            sessions = new Dictionary<int, Session>();
        }

        internal LinkedList<int> getSessionOfUserName(string username)
        {
            LinkedList<int> result = new LinkedList<int>();

            foreach (KeyValuePair<int, Session> s in sessions) {
                SubscribedUser su = s.Value.getSubscribedUser();
                if (su != null && su.getUsername() == username)
                    result.AddFirst(s.Key);
            }
            return result;
        }
    }
}

