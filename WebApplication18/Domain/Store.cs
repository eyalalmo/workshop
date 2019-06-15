using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18;
using Dapper.Contrib;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication18.Domain;

namespace workshop192.Domain
{
    public class Store
    {
        private int storeId;
        private string name;
        private string description;
        private LinkedList<Product> productList;
        private List<StoreRole> roles;
        private int numOfOwners;
        private bool active;
        /* private MinAmountPurchase minPurchasePolicy;
         private MaxAmountPurchase maxPurchasePolicy;
         private TotalPricePolicy minTotalprice;
        private ComplexPurchasePolicy complexPurchase;
             * */
        private LinkedList<PurchasePolicy> policies;
        private LinkedList<InvisibleDiscount> invisibleDiscountList;
        [JsonIgnore]
        public LinkedList<DiscountComponent> discountList;
       // public LinkedList<InvisibleDiscount> invisibleDiscountList;
        public LinkedList<Contract> contracts;
        public LinkedList<string> pendingOwners;

        public Store(string storeName, string description)
        {
            this.storeId = DBStore.getInstance().getNextStoreID();
            this.name = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
            discountList = new LinkedList<DiscountComponent>();
            // invisibleDiscountList = new LinkedList<InvisibleDiscount>();
            policies = new LinkedList<PurchasePolicy>();
            contracts = new LinkedList<Contract>();
            pendingOwners = new LinkedList<string>();
        }
        public Store(int storeId,string name, string description)
        {
            this.storeId = storeId;
            this.name = name;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
            contracts = new LinkedList<Contract>();
            pendingOwners = new LinkedList<string>();
            discountList = new LinkedList<DiscountComponent>();
            invisibleDiscountList = new LinkedList<InvisibleDiscount>();
            policies = new LinkedList<PurchasePolicy>();
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }
        public void setDiscountList(LinkedList<DiscountComponent> discounts)
        {
            this.discountList = discounts;
        }
        public LinkedList<DiscountComponent> getDiscounts()
        {
            return this.discountList;
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

        public void addStoreRoleFromInitOwner(StoreOwner so)
        {
            roles.Add(so);
            numOfOwners++;
        }

        public string getProductsString()
        {
            string s = JsonConvert.SerializeObject(this.productList);
            return s;
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
            return this.storeId;
        }
        public void setStoreID(int id)
        {
            this.storeId = id;
        }

        public String getStoreName()
        {
            return this.name;
        }

        public void setStoreName(String storeName)
        {
            this.name = storeName;
        }

        public void addStoreRoleFromInitManager(StoreRole so)
        {
            roles.Add(so);
        }

        public String getDescription()
        {
            return this.description;
        }

        public void setDescription(String description)
        {
            this.description = description;
        }

        public void addStoreRoleFromInitOwner(StoreRole toAdd)
        {
            if (toAdd is StoreOwner)
            {
                numOfOwners++;
                DBStore.getInstance().addownerNumerByOne(storeId, numOfOwners);
            }
            roles.Add(toAdd);
        }

        //public void addCoupon(string coupon, double percentage, string duration)
        //{
        //    foreach (InvisibleDiscount d1 in invisibleDiscountList)
        //    {
        //        string c = d1.getCoupon();
        //        if (c == coupon)
        //            throw new AlreadyExistException("Error: A Store cannot have two identical coupons");

        //    }
        //    InvisibleDiscount d = new InvisibleDiscount(percentage, coupon, duration);
        //    invisibleDiscountList.AddLast(d);
        //}

        //public void removeCoupon(string coupon)
        //{
        //    bool found = false;
        //    foreach (InvisibleDiscount d1 in invisibleDiscountList)
        //    {
        //        string c = d1.getCoupon();
        //        if (c == coupon)
        //        {
        //            found = true;
        //            invisibleDiscountList.Remove(d1);
        //            break;
        //        }
        //    }

        //    if (!found)
        //        throw new DoesntExistException("Error:Coupon does not exist");
        //}

        //public void checkCouponCode(string coupon)
        //{
        //    bool found = false;
        //    foreach (InvisibleDiscount d in invisibleDiscountList)
        //    {
        //        string c = d.getCoupon();
        //        if (c == coupon)
        //        {
        //            found = true;
        //            break;
        //        }
        //    }
        //    if (!found)
        //        throw new ArgumentException("Error:Coupon does not exist");
        //}


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
                DBStore.getInstance().removeOwnerNumerByOne(storeId, numOfOwners);

            }
            roles.Remove(toRemove);
            DBStore.getInstance().removeStoreRole(toRemove);
        }


        public void addDiscount(DiscountComponent d)
        {
            discountList.AddLast(d);
        }

        public void removeDiscount(int discountID)
        {
            foreach (DiscountComponent d in discountList)
            {
                if (d.getId() == discountID)
                {
                    discountList.Remove(d);
                    break;
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


        public LinkedList<Contract> getContracts()
        {
            return contracts;
        }

        public LinkedList<string> getPending()
        {
            return pendingOwners;
        }


        public void setMinPurchasePolicy(int minAmount, int index)
        {
            PurchasePolicy p = policies.ElementAt(index);
            PurchasePolicy max = getMaxPolicy();
            if (max == null)
            {
                p.setAmount(minAmount);
            }
            else
            {
                if (max.getAmount() < minAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    p.setAmount(minAmount);
                }
            }

        }

        internal void setPolicyByID(int newAmount, int index)
        {
            PurchasePolicy p = policies.ElementAt(index);
            if (p is ComplexPurchasePolicy)
                throw new ArgumentException("Can not set complex policy.");
            if (p is MaxAmountPurchase)
                setMaxPurchasePolicy(newAmount, index);
            if (p is MinAmountPurchase)
                setMinPurchasePolicy(newAmount, index);
            if (p is TotalPricePolicy)
                setTotalPolicy(newAmount, index);
        }

        private void setTotalPolicy(int newAmount, int index)
        {
            PurchasePolicy p = policies.ElementAt(index);
            if (newAmount < 0)
                throw new ArgumentException("Total cart price can not be a negative number.");
            p.setAmount(newAmount);

        }

        private PurchasePolicy getMaxPolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MaxAmountPurchase)
                    return p;
            }
            return null;

        }
        private PurchasePolicy getMinPolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MinAmountPurchase)
                    return p;
            }
            return null;

        }
        public void setMaxPurchasePolicy(int maxAmount, int index)
        {
            PurchasePolicy p = policies.ElementAt(index);
            PurchasePolicy min = getMinPolicy();
            if (min == null)
            {
                p.setAmount(maxAmount);
            }
            else
            {
                if (min.getAmount() > maxAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    p.setAmount(maxAmount);
                }

            }
        }

        public bool checkStorePolicy(int amount, double cartTotalPrice)
        {
            bool ans = true;
            foreach (PurchasePolicy p in policies)
            {
                ans = ans & p.checkPolicy(cartTotalPrice, amount);
            }
            return ans;
        }
        public void removePolicyByindex(int index)
        {
            PurchasePolicy p = policies.ElementAt(index);
            policies.Remove(p);
        }
        public bool hasMinPurchasePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MinAmountPurchase)
                    return true;
            }
            return false;
        }
        public bool hasMaxPurchasePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MaxAmountPurchase)
                    return true;
            }
            return false;

        }

        public bool hasTotalPricePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is TotalPricePolicy)
                    return true;
            }
            return false;

        }

        public void addMinAmountPolicy(int minAmount)
        {
            if (hasMinPurchasePolicy())
            {
                throw new ArgumentException("Store can not have 2 minimum amount purchase policy.");
            }
            MinAmountPurchase p = new MinAmountPurchase(minAmount);
            policies.AddLast(p);
        }
        public void addMaxAmountPolicy(int minAmount)
        {
            if (hasMaxPurchasePolicy())
            {
                throw new ArgumentException("Store can not have 2 maximum amount purchase policy.");
            }
            MaxAmountPurchase p = new MaxAmountPurchase(minAmount);
            policies.AddLast(p);
        }

        public void addTotalAmountPolicy(int minPrice)
        {
            if (hasTotalPricePolicy())
            {
                throw new ArgumentException("Store can not have 2 total cart price - purchase policy.");
            }
            MaxAmountPurchase p = new MaxAmountPurchase(minPrice);
            policies.AddLast(p);
        }

        public void addComplexPurchasePolicy(int index1, int index2, string type)
        {
            PurchasePolicy p1 = policies.ElementAt(index1);
            PurchasePolicy p2 = policies.ElementAt(index2);

            ComplexPurchasePolicy complexPurchase = new ComplexPurchasePolicy(type, p1, p2);
            policies.Remove(p1);
            policies.Remove(p2);
            policies.AddLast(complexPurchase);
        }

        public LinkedList<PurchasePolicy> getStorePolicyList()
        {
            return policies;
        }

        public void setPolicyList(LinkedList<PurchasePolicy> policyList)
        {
            this.policies = policyList;
        }
    }

}
