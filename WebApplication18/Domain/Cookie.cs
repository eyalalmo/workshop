using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

 namespace WebApplication18.Domain
{
    public class Cookie
    {
        public string hash;
        public int session;

        public Cookie(string hash, int session)
        {
            this.hash = hash;
            this.session = session;
        }
        public int getSession()
        {
            return this.session;
        }
        public string getHash()
        {
            return this.hash;
        }
        public void setSession(int session)
        {
            this.session = session;
        }
    }
}