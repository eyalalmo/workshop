using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class SubscribedUser
    {
<<<<<<< HEAD
        private String username;
        private String password;
        private String purchaseHistory;

        public SubscribedUser(String username, String password)
        {
            this.username = username;
            this.password = password;
            purchaseHistory = "";
        }

        public String getPassword()
        {
            return this.password;
        }

        public String getUsername()
        {
            return this.username;
        }

        public String getPurchaseHistory()
        {
            return this.purchaseHistory;
        }

        public void addToPurchaseHistory(String purchaseDetails)
        {
            purchaseHistory = purchaseHistory + purchaseDetails;
        }
        
=======
>>>>>>> origin/bar's_branch
    }
}
