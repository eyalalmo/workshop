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

=======
>>>>>>> origin/Stores_and_Products
        private String username;
        private String password;
        private ShoppingBasket shoppingBasket;
        private String purchaseHistory;

        public SubscribedUser(String username, String password, ShoppingBasket shoppingBasket)
        {
            this.username = username;
            this.password = password;
            this.shoppingBasket = shoppingBasket;
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
        
<<<<<<< HEAD

=======
>>>>>>> origin/Stores_and_Products
    }
}
