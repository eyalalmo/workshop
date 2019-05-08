﻿using System;
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
        private static int sessionNum = 0;

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
            sessions.Add(sessionNum, new Session());
            return sessionNum++;
        }

        internal Session getSession(int sessionid)
        {
            if (!sessions.ContainsKey(sessionid))
                throw new DoesntExistException("session doesnt exist");
            return sessions[sessionid];
        }

        public void removeSession(int s)
        {
            if (!sessions.ContainsKey(s))
                throw new DoesntExistException("session doesnt exist");
            sessions.Remove(s);
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

        internal int getSessionOfUserName(string username)
        {
            foreach (KeyValuePair<int, Session> s in sessions) {
                if (s.Value.getSubscribedUser().getUsername() == username)
                    return s.Key;
            }
            return -1;
        }
    }
}

