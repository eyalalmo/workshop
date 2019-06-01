using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class RegisterEntry
    {
        private string username;
        private string password;
        
        public RegisterEntry(string username, string password)
        {
            this.username = username;
            this.password = password; 
        }
        public string getUsername()
        {
            return this.username;
        }
        public string getPassword()
        {
            return this.password;
        }


    }
}