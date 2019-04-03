using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class Complaint
    {
        private String username;
        private String description;

        public Complaint(String username, String description)
        {
            this.username = username;
            this.description = description;
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getDescription()
        {
            return this.description;
        }

        public String toString()
        {
            return username + ": " + description + "\n";
        }
    }
}
