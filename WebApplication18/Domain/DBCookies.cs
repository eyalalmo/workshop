using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using workshop192.Domain;
using workshop192.ServiceLayer;

namespace workshop192.Domain
{
    public class DBCookies
    {


        private static DBCookies instance;
        private Dictionary<string, Session> cookies;

        private DBCookies()
        {
            cookies = new Dictionary<string, Session>();
        }
        public static DBCookies getInstance()
        {
            if (instance == null)
                instance = new DBCookies();
            return instance;
        }

        public string addSession(string hash, Session session)
        {
            if (hash == null )
                return "fail";
            if (session == null)
                return "fail";

            
                if (!cookies.ContainsKey(hash))
                {
                cookies.Add(hash, session);
                    return "ok";
                }
                else
                {
                cookies[hash] = session;
                    return "ok";
                }
            
        }

        public string generate()
        {
            return Guid.NewGuid().ToString();
        }

        public Session getUserByHash(string hash)
        {
            if (hash == null)
                return null;

            if (cookies.ContainsKey(hash))
            {
                return cookies[hash];
            }

            return null;
        }

        public Session getUserByName(string name)
        {
   
            return null;
        }
        public static void initDB()
        {
            instance = new DBCookies();
        }
    }
}