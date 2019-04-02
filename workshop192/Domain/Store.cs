using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class Store
    {

        public int storeID;
        public string storeName;
        public string description;
        public LinkedList<Product> productList;
        public bool status;

        public Store (int id, string storeName, string description)
        {
            this.storeID = id;
            this.storeName = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            status = true;
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
        

        

        // public ? getPurchaseHistory (){} ------------------


    }
}
