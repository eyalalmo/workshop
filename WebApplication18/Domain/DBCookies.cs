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
        private Dictionary<string, int> cookies;

        private DBCookies()
        {
            cookies = new Dictionary<string, int>();
        }

        public static DBCookies getInstance()
        {
            if (instance == null)
                instance = new DBCookies();
            return instance;
        }

        public string addSession(string hash, int session)
        {
            if (hash == null )
                return "fail";
            if (session < 0)
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

        public int getUserByHash(string hash)
        {
            if (hash == null)
                return -1;

            if (cookies.ContainsKey(hash))
            {
                return cookies[hash];
            }

            return -1;
        }
        
        public static void initDB()
        {
            instance = new DBCookies();
        }
    }
}