using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DBSession
    {
        private static DBSession instance;
        private LinkedList<Session> sessions;
    
        public static DBSession getInstance()
        {
            if (instance == null)
                instance = new DBSession();
            return instance;
        }

        private DBSession()
        {
            sessions = new LinkedList<Session>();
        }

        public String addSession(Session s)
        {
            if (sessions.Contains(s))
                return "ERROR: Session already exists";
            sessions.AddFirst(s);
            return "";
        }
        public String removeSession(Session s)
        {
            if (!sessions.Contains(s))
                return "ERROR: Session does not exist, cannot remove it";
            sessions.Remove(s);
            return "";
        }
        public Session getSessionOfSubscribedUser(SubscribedUser sub)
        {
            foreach(Session s in sessions)
            {
                if (s.getSubscribedUser().Equals(sub))
                {
                    return s;
                }
            }
            return null;
        }
    }
}
