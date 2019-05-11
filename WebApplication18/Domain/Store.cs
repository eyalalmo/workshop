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
        public LinkedList<PurchasePolicy> purchasePolicyList;
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
            purchasePolicyList = new LinkedList<PurchasePolicy>();
            invisibleDiscountList = new LinkedList<InvisibleDiscount>();
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

        public void addCoupon(string coupon, int percentage, string duration)
        {
            foreach (InvisibleDiscount d1 in invisibleDiscountList)
            {
                string c = d1.getCoupon();
                if (c == coupon)
                    throw new AlreadyExistException("Store can not have to identical coupons");

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
                throw new DoesntExistException("no such coupon in store");
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
                throw new ArgumentException("no such coupon in the store");
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


        public Dictionary<Product, double> updatePriceAfterCoupon(string coupon, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach(InvisibleDiscount d in invisibleDiscountList)
            {
                if(coupon == d.getCoupon())
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
                throw new AlreadyExistException("store can not have more than 2 purchase policies");
            }
            else
            {
                PurchasePolicy policy = purchasePolicyList.First();
                if((policy.GetType() == typeof(MinAmountPurchase) && p.GetType() == typeof(MinAmountPurchase)) || 
                    (policy.GetType() == typeof(MaxAmountPurchase) && p.GetType() == typeof(MaxAmountPurchase)))
                {
                    throw new AlreadyExistException("Store can not have purchase policies of the same type");
                }
                checkValidityofPurchases(p, policy);
                purchasePolicyList.AddLast(p);
            }
        }

        private void checkValidityofPurchases(PurchasePolicy p1, PurchasePolicy p2)
        {
            if (p1.GetType() == typeof(MaxAmountPurchase))
                if (p1.getAmount() < p2.getAmount())
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
            else if (p1.GetType() == typeof(MinAmountPurchase))
                    if (p1.getAmount() > p2.getAmount())
                        throw new ArgumentException("contradiction! minimum amount can not be larger than maximum amount Purchase Policy");


        }

        public void setMinPurchasePolicy(int newMinAmount)
        {

            if (purchasePolicyList.Count == 0)
            {
                throw new MissingMemberException("store does not have a Minimum Policy to set");
            }
            bool found = false;
            PurchasePolicy p1 = purchasePolicyList.ElementAt(0);
            if (purchasePolicyList.Count == 1)
            {
                if (p1 is MinAmountPurchase)
                {
                    p1.setAmount(newMinAmount);
                    found = true;
                }

            }
            else if (purchasePolicyList.Count == 2)
            {
                PurchasePolicy p2 = purchasePolicyList.ElementAt(1);
                if (p1 is MinAmountPurchase)
                {
                    checkValidityofPurchases(p1, p2);
                    p1.setAmount(newMinAmount);
                    found = true;
                }
                else if (p2 is MinAmountPurchase)
                {
                    checkValidityofPurchases(p2, p1);
                    p2.setAmount(newMinAmount);
                    found = true;
                }
            }
            if (!found)
                throw new MissingMemberException("store does not have a Maximum Policy to set");

        }

        public void setMaxPurchasePolicy(int newMaxAmount)
        {
            if (purchasePolicyList.Count == 0)
            {
                throw new MissingMemberException("store does not have a Max Policy to set");
            }
            bool found = false;
            PurchasePolicy p1 = purchasePolicyList.ElementAt(0);
            if (purchasePolicyList.Count == 1) {
                if (p1 is MaxAmountPurchase)
                {
                    p1.setAmount(newMaxAmount);
                    found = true;
                }
                
            }
            else if(purchasePolicyList.Count == 2)
            {
                PurchasePolicy p2 = purchasePolicyList.ElementAt(1);
                if (p1 is MaxAmountPurchase)
                {
                    checkValidityofPurchases(p1, p2);
                    p1.setAmount(newMaxAmount);
                    found = true;
                }
                else if(p2 is MaxAmountPurchase)
                {
                    checkValidityofPurchases(p2, p1);
                    p2.setAmount(newMaxAmount);
                    found = true;
                }
            }
            if (!found)
                    throw new MissingMemberException("store does not have a Max Policy to set");

        }

        public void checkPolicy(Product p, int amount)
        {
            foreach(PurchasePolicy policy in purchasePolicyList)
            {
                policy.checkPolicy(p, amount);
            }
        }

        public void removeMaxAMountPolicy()
        {
            if (purchasePolicyList.Count == 0)
            {
                throw new MissingMemberException("store does not have a Max Policy to remove");
            }
            PurchasePolicy p1 = purchasePolicyList.ElementAt(0);
            if (purchasePolicyList.Count == 1)
            {
                if (p1 is MaxAmountPurchase)
                {
                    purchasePolicyList.Remove(p1);
                }
                else
                    throw new MissingMemberException("store does not have a Max Policy to remove");


            }
            else if (purchasePolicyList.Count == 2)
            {
                PurchasePolicy p2 = purchasePolicyList.ElementAt(1);
                if (p1 is MaxAmountPurchase)
                {
                    purchasePolicyList.Remove(p1);
                }
                else if (p2 is MaxAmountPurchase)
                {
                    purchasePolicyList.Remove(p2);
                }
            }

        }

        public void removeMinAmountPolicy()
        {
            if (purchasePolicyList.Count == 0)
            {
                throw new MissingMemberException("store does not have a Min Policy to remove");
            }
            PurchasePolicy p1 = purchasePolicyList.ElementAt(0);
            if (purchasePolicyList.Count == 1)
            {
                if (p1 is MinAmountPurchase)
                {
                    purchasePolicyList.Remove(p1);
                }
                else
                    throw new MissingMemberException("store does not have a min Policy to remove");


            }
            else if (purchasePolicyList.Count == 2)
            {
                PurchasePolicy p2 = purchasePolicyList.ElementAt(1);
                if (p1 is MinAmountPurchase)
                {
                    purchasePolicyList.Remove(p1);
                }
                else if (p2 is MinAmountPurchase)
                {
                    purchasePolicyList.Remove(p2);
                }
            }

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
