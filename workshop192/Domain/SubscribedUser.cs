using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class SubscribedUser
    {

        private String username;
        private String password;
        private ShoppingBasket shoppingBasket;
        private String purchaseHistory;
        private List<StoreRole> storeRoles;

        public SubscribedUser(String username, String password, ShoppingBasket shoppingBasket)
        {
            this.username = username;
            this.password = password;
            this.shoppingBasket = shoppingBasket;
            purchaseHistory = "";
            storeRoles = new List<StoreRole>();
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

        public ShoppingBasket getShoppingBasket()
        {
            return this.shoppingBasket;
        }

        public void addToPurchaseHistory(String purchaseDetails)
        {
            purchaseHistory = purchaseHistory + purchaseDetails;
        }

        public void setPassword(String pass)
        {
            this.password = pass;
        }

        public void addStoreRole(StoreRole role)
        {
            storeRoles.Add(role);
        }
        
        public void removeStoreRole(StoreRole role)
        {
            storeRoles.Remove(role);
        }

        public StoreRole getStoreRole(Store store)
        {
            foreach (StoreRole role in storeRoles)
            {
                if(role.getStore().Equals(store))
                {
                    return role;
                }
            }
            return null;
        }

        public List<StoreRole> getStoreRoles()
        {
            return storeRoles;
        }
    }
}
