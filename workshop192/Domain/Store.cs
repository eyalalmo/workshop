using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class Store
    {       
        private int storeID;
        private string storeName;
        private string description;
        private LinkedList<Product> productList;
        private List<StoreRole> roles;
        private int numOfOwners;
        private bool active;
       

        public Store(string storeName, string description)
        {
            this.storeID = DBStore.getNextStoreID();
            this.storeName = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }
        public bool isActive()
        {
            return this.active;
        }
        public void closeStore()
        {
            active = false;
        }
        public void openStore()
        {
            active = true;
        }
        public void removeProduct(Product p)
        {
            productList.Remove(p);
        }
        public bool productExists(int productID)
        {
            foreach (Product p in productList)
            {
                if (p.getProductID() == productID)
                    return true;
            }
            return false;
        }

        public bool productExists(Product product)
        {
            foreach (Product p in productList)
            {
                if (product.Equals(p))
                    return true;
            }
            return false;
        }
        public LinkedList<Product> getProductList()
        {
            return this.productList;
        }

        public int getStoreID()
        {
            return this.storeID;
        }
        public void setStoreID(int id)
        {
            this.storeID = id;
        }

        public String getStoreName()
        {
            return this.storeName;
        }

        public void setStoreName(String storeName)
        {
            this.storeName = storeName;
        }

        public String getDescription()
        {
            return this.description;
        }

        public void setDescription(String description)
        {
            this.description = description;
        }

        public void addStoreRole(StoreRole toAdd)
        {
            if (toAdd is StoreOwner)
            {
                numOfOwners++;
            }
            roles.Add(toAdd);
        }

        public int getNumberOfOwners()
        {
            return numOfOwners;
        }

        public List<StoreRole> getRoles()
        {
            return roles;
        }

        public StoreRole getStoreRole(SubscribedUser user)
        {
            foreach (StoreRole sr in roles)
            {
                if (sr.getUser() == user)
                {
                    return sr;
                }

            }
            return null;
        }
        public void removeStoreRole(StoreRole toRemove)
        {
            if (toRemove is StoreOwner)
            {
                numOfOwners--;
            }
            roles.Remove(toRemove);
        }
    }
}
