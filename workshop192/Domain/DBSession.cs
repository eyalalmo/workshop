using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DBSession
    {
        public static DBSession instance;
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
            if()
        }
        public String removeSession(Session s)
        {

        }
        public Session getSessionOfSubscribedUser(SubscribedUser sub)
        {

        }
    }
}
