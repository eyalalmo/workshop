using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBSession
    {
        private static DBSession instance;
        private Dictionary<int, Session> sessions;
        private static int nextID;

        public static DBSession getInstance()
        {
            if (instance == null)
            {
                instance = new DBSession();
            }
            return instance;
        }

        private DBSession()
        {
            sessions = new Dictionary<int, Session>();
            nextID = 1;
        }


        public void init()
        {
            sessions = new Dictionary<int, Session>();
            nextID = 1;
        }
        public void addSession(Session s)
        {
            if (sessions.Contains(s))
                throw new AlreadyExistException("session already exists");
            sessions.AddFirst(s);
        }
        public void removeSession(Session s)
        {
            if (!sessions.Contains(s))
                throw new DoesntExistException("session does not exist, can't remove it");
            sessions.Remove(s);
        }
        public Session getSessionOfSubscribedUser(SubscribedUser sub)
        {
            foreach (Session s in sessions)
            {
                if (s.getSubscribedUser().Equals(sub))
                {
                    return s;
                }
            }
            return null;
        }
        public bool sessionExists(Session s)
        {
            return sessions.Contains(s);
        }
        public void initSession()
        {
            sessions = new LinkedList<Session>();
        }

        public Session getSessionByID(int sessionID)
        {

        }
    }
}

