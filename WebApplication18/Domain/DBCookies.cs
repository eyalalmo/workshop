using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication18.Domain;
using workshop192.Domain;
using workshop192.ServiceLayer;
using WebApplication18.DAL;
using Dapper;



namespace workshop192.Domain
{
    public class DBCookies : Connector
    {
        private static DBCookies instance;
        // private LinkedList<Cookie> cookies;

        private DBCookies()
        {
            //cookies = new LinkedList<Cookie>();
        }

        public static DBCookies getInstance()
        {
            if (instance == null)
                 instance = new DBCookies();
            return instance;
        }
      /*  public Cookie getCookieByHash(string hash)
        {
            foreach (Cookie c in cookies)
            {
                if (c.getHash().Equals(hash))
                    return c;
            }
            return null;
        }*/
        public string addSession(string hash, int session)
        {
            if (hash == null)
                return "fail";
            if (session < 0)
                return "fail";

            try {
                connection.Open();

                Cookie c = connection.Query<Cookie>("SELECT hash, session FROM [dbo].[Cookie] WHERE hash=@hash ", new { hash=hash }).First();
               // Cookie cooki = (Cookie)v[0];
               // Cookie cookie = getCookieByHash(hash);
                if (c==null) //doesnt exist in DB
                {
                    // Cookie c = new Cookie(hash, session);
                    //cookies.AddFirst(c);

                    string sql = "INSERT INTO [dbo].[Cookie] (hash, session)" +
                                 " VALUES (@hash, @session)";
                    connection.Execute(sql, new { hash, session });
                    connection.Close();
                    return "ok";

                }           
                else
                {
                connection.Execute("UPDATE session = @session FROM Cookie WHERE hash=@hash ", new { session=session, hash=hash });
                    // Cookie cooki = (Cookie)v;

                    // cookie.setSession(session);
                    connection.Close();
                    return "ok";
                }
             }

            catch (Exception)
            {
                connection.Close();
                return "fail";
            }
        }

        public string generate()
        {
            return Guid.NewGuid().ToString();
        }

        public int getUserByHash(string hash)
        {
            /*if (hash == null)
                return -1;
            Cookie c = getCookieByHash(hash);
            if (c!=null)
            {
                return c.getSession();
            }

            return -1;*/
            try
            {
                connection.Open();

                Cookie c = connection.Query<Cookie>("SELECT hash, session FROM [dbo].[Cookie] WHERE hash=@hash ", new { hash=hash }).First();
                if (c==null)//doesnt exist in DB
                {
                    connection.Close();
                    return -1;
                }
               // Cookie c = (Cookie)v[0];

                connection.Close();
                return c.getSession();
            }
            catch (Exception)
            {
                connection.Close();
                return -1;
            }
        }
        
        public static void initDB()
        {
            instance = new DBCookies();
        }

        internal void deleteUserBySessionId(string hash)
        {
            //Cookie c = getCookieByHash(hash);
            //cookies.Remove(c);
            try
            {
                connection.Open();
                var affectedRows = connection.Execute("DELETE FROM Cookie WHERE  hash=@hash ", new {hash = hash });
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
            }
        }
    }
}