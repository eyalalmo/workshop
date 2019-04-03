using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class Store
    {
<<<<<<< HEAD

=======
>>>>>>> origin/ProductsAndPurchases
        private int storeID;
        private string storeName;
        private string description;
        private LinkedList<Product> productList;
        private bool status;
        private List<StoreRole> roles;
        private int numOfOwners;
        




        public Store (int id, string storeName, string description)

        {
            this.storeID = DBStore.getNextStoreID();
            this.storeName = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            status = true;
            numOfOwners = 0;
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }
        public void removeProduct(Product p)
        {
            productList.Remove(p);
        }
        public bool productExists(int productID)
        {
           foreach (Product p  in productList)
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

        public void changeStatus()
        {
            status = !status;
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
<<<<<<< HEAD
=======

        public void addStoreRole(StoreRole toAdd){
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
            foreach(StoreRole sr in roles)
            {
                if (sr.getUser() == user)
                {
                    return sr;
                }

            }
            return null;
        }
        
>>>>>>> origin/ProductsAndPurchases


    }
}
