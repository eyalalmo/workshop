using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18;

namespace workshop192.Domain
{
    public class Store
    {       
        public int storeID;
        public string storeName;
        public string description;
        public LinkedList<Product> productList;
        public List<StoreRole> roles;
        public int numOfOwners;
        public bool active;
        public LinkedList<DiscountComponent> discountList;
        private LinkedList<DiscountComponent> discountList;
        private LinkedList<PurchasePolicy> purchasePolicyList;


        public Store(string storeName, string description)
        {
            this.storeID = DBStore.getNextStoreID();
            this.storeName = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
            discountList = new LinkedList<DiscountComponent>();
            purchasePolicyList = new LinkedList<PurchasePolicy>();
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

        public string getProductsString()
        {
            return JsonConvert.SerializeObject(this.productList);
           
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


        public Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach( DiscountComponent d in discountList)
            {
                if (d.checkCondition(productList, productsActualPrice))
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                }
            }
            return productsActualPrice;
            
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
        
     
        public void addDiscount(DiscountComponent d)
        {
            discountList.AddLast(d);
        }

        public void removeDiscount()
        {
            if(discountList.Count== 0)
            {
                throw new DoesntExistException("Discount does not exist so it cannot be removed");
            }
            discountList = new LinkedList<DiscountComponent>();
        }
        public void addPurchasePolicy(PurchasePolicy p)
        {
            int listSize = purchasePolicyList.Count;
            if (listSize == 0)
            {
                purchasePolicyList.AddLast(p);
            }
            else if(listSize == 2)
            {
                throw new IllegalAmountException("store can not have more than 2 purchase policies");
            }
            else
            {
                PurchasePolicy policy = purchasePolicyList.First();
                if((policy.GetType() == typeof(MinAmountPurchase) && p.GetType() == typeof(MinAmountPurchase)) || 
                    (policy.GetType() == typeof(MaxAmountPurchase) && p.GetType() == typeof(MaxAmountPurchase)))
                {
                    throw new AlreadyExistException("Store can not have purchase policies of the same type");
                }
                purchasePolicyList.AddLast(p);
            }
        }
        public void checkPolicy(Product p, int amount)
        {
            foreach(PurchasePolicy policy in purchasePolicyList)
            {
                policy.checkPolicy(p, amount);
            }
        }
    }

}
