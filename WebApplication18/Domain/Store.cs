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
        public MinAmountPurchase minPurchasePolicy;
        public MaxAmountPurchase maxPurchasePolicy;
        public LinkedList<InvisibleDiscount> invisibleDiscountList;

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
            invisibleDiscountList = new LinkedList<InvisibleDiscount>();
            maxPurchasePolicy = null;
            minPurchasePolicy = null;
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

        public void addCoupon(string coupon, double percentage, string duration)
        {
            foreach (InvisibleDiscount d1 in invisibleDiscountList)
            {
                string c = d1.getCoupon();
                if (c == coupon)
                    throw new AlreadyExistException("Error: A Store cannot have two identical coupons");

            }
            InvisibleDiscount d = new InvisibleDiscount(percentage, coupon, duration);
            invisibleDiscountList.AddLast(d);
        }

        public void removeCoupon(string coupon)
        {
            bool found = false;
            foreach (InvisibleDiscount d1 in invisibleDiscountList)
            {
                string c = d1.getCoupon();
                if (c == coupon)
                {
                    found = true;
                    invisibleDiscountList.Remove(d1);
                    break;
                }
            }

            if (!found)
                throw new DoesntExistException("Error:Coupon does not exist");
        }

        public void checkCouponCode(string coupon)
        {
            bool found = false;
            foreach (InvisibleDiscount d in invisibleDiscountList)
            {
                string c = d.getCoupon();
                if (c == coupon)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                throw new ArgumentException("Error:Coupon does not exist");
        }


        public Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach (DiscountComponent d in discountList)
            {
                if (d.checkCondition(productList, productsActualPrice))
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                }
            }
            return productsActualPrice;

        }


        public Dictionary<Product, double> updatePriceAfterCoupon(string coupon, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach (InvisibleDiscount d in invisibleDiscountList)
            {
                if (coupon == d.getCoupon())
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                    break;
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
            if (discountList.Count == 0)
            {
                throw new DoesntExistException("Error: Discount does not exist so it cannot be removed");
            }
            discountList = new LinkedList<DiscountComponent>();
        }
        /*
         private void checkValidityofPurchases(PurchasePolicy p1, PurchasePolicy p2)
         {
             if (p1!= null && p1.GetType() == typeof(MaxAmountPurchase))
             {
                 if (p1.getAmount() < p2.getAmount())
                     throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
             }
             else if (p1.GetType() == typeof(MinAmountPurchase))
             {
                 if (p1.getAmount() > p2.getAmount())
                     throw new ArgumentException("contradiction! minimum amount can not be larger than maximum amount Purchase Policy");
             }

        }
       */ 
        public void setMinPurchasePolicy(int MinAmount)
        {
            if (maxPurchasePolicy == null)
                minPurchasePolicy = new MinAmountPurchase(MinAmount);
            else
            {
                if (maxPurchasePolicy.getAmount() < MinAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                    minPurchasePolicy = new MinAmountPurchase(MinAmount);

            }

        }

        public void setMaxPurchasePolicy(int maxAmount)
        {
            if (minPurchasePolicy == null)
                maxPurchasePolicy = new MaxAmountPurchase(maxAmount);
            else
            {
                if (minPurchasePolicy.getAmount() > maxAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                    maxPurchasePolicy = new MaxAmountPurchase(maxAmount);

            }
        }

        public void checkPolicy(Product p, int amount)
        {
            if (maxPurchasePolicy != null)
                maxPurchasePolicy.checkPolicy(p, amount);
            if (minPurchasePolicy != null)
                minPurchasePolicy.checkPolicy(p, amount);
        }

        public void removeMaxAMountPolicy()
        {
            maxPurchasePolicy = null;

        }

        public void removeMinAmountPolicy()
        {
            minPurchasePolicy = null;
        }

        public bool hasMinPurchasePolicy()
        {
            return (minPurchasePolicy != null);
        }
        public bool hasMaxPurchasePolicy()
        {
            return (maxPurchasePolicy != null);
        }

        public MinAmountPurchase getMinAmountPolicy()
        {
            if (minPurchasePolicy != null)
                return minPurchasePolicy;
            else
                throw new DoesntExistException();
        }
        public MaxAmountPurchase getMaxAmountPolicy()
        {
            if (maxPurchasePolicy != null)
                return maxPurchasePolicy;
            else
                throw new DoesntExistException();
        }

        public VisibleDiscount getVisibleDiscount()
        {
            foreach(Discount d in discountList)
            {
                if (d is VisibleDiscount)
                    return (VisibleDiscount)d;
            }
            return null;
        }

    }

}
