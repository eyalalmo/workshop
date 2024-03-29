﻿using System;
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
        private DiscountComponent discount;
       

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


        public Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
           return discount.updatePrice(productList, productsActualPrice);
            
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
        
       /* public void addReliantDiscount(double percentage, String condition, String duration)
        {
            discount = new ReliantDiscount(condition, duration, percentage: percentage);
        }

        public void addVisibleDiscount(double percentage, String duration)
        {
            discount = new VisibleDiscount(percentage, duration);
        }
        public DiscountComponent getDiscount()
        {
            return this.discount;
        }
        public void addReliantDiscount(double percentage, String condition, String duration)
        {
            discount = new ReliantDiscount(percentage, condition, duration);
        }

        public void addVisibleDiscount(double percentage, String duration)
        {
            discount = new VisibleDiscount(percentage, duration);
        }*/
        public void addDiscount(DiscountComponent d)
        {
            if(discount == null)
                this.discount = d;
            else
            {
                List<DiscountComponent> list = new List<DiscountComponent>();
                list.Add(this.discount);
                list.Add(d);
                DiscountComposite composite = new DiscountComposite(list, "or");
                this.discount = composite;
            }
        }

        public void removeDiscount()
        {
            if(discount == null)
            {
                throw new DoesntExistException("Discount does not exist so it cannot be removed");
            }
            discount = null;
            /* if(discount is Discount)
             {
                 if(discount.getId()==discountid)
                     discount = null;
             }
             else
             {
                 if (discount.getId() == discountid)
                     discount = null;
                 else
                 {
                     DiscountComposite dis = (DiscountComposite)discount;
                     dis.remove(discountid);
                 }

             }
         }*/
        }
       // public Dictionary
    }

}
