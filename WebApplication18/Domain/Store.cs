﻿using Newtonsoft.Json;
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
        public int storeId;
        public string name;
        public string description;
        public LinkedList<Product> productList;
        public List<StoreRole> roles;
        public int numOfOwners;
        public bool active;
        /* private MinAmountPurchase minPurchasePolicy;
         private MaxAmountPurchase maxPurchasePolicy;
         private TotalPricePolicy minTotalprice;
        private ComplexPurchasePolicy complexPurchase;
             * */
        public LinkedList<InvisibleDiscount> invisibleDiscountList;
        [JsonIgnore]
        public LinkedList<DiscountComponent> discountList;
        // public LinkedList<InvisibleDiscount> invisibleDiscountList;
        [JsonIgnore]
        public LinkedList<PurchasePolicy> policies;
        [JsonIgnore]
        public LinkedList<Contract> contracts;
        [JsonIgnore]
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
        public void removeDiscoutFromList(int discountID)
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
        public void removeDiscount(int discountID)
        {
            foreach (DiscountComponent d in discountList)
            {
                if (d.getId() == discountID)
                {
                    //
                    discountList.Remove(d);
                    if(d is ReliantDiscount)
                    {
                        ReliantDiscount r = (ReliantDiscount)d;
                        
                        if (r.getProduct() != null) {
                            r.getProduct().removeReliantDiscount();
                        }
                    }
                    if(d is VisibleDiscount)
                    {
                        VisibleDiscount v = (VisibleDiscount)d;

                        if (v.getProduct() != null)
                        {
                            v.getProduct().removeDiscount();
                        }
                    }
                    if (d is DiscountComposite)
                    {
                        removeChildren((DiscountComposite)d);
                    }
                    break;
                }
            }
        }

        private void removeChildren(DiscountComposite d) {
            foreach (DiscountComponent child in d.getChildren())
            {
                if (child is DiscountComposite)
                    removeChildren((DiscountComposite)child);
                else
                {
                    if (child is ReliantDiscount)
                    {
                        ReliantDiscount r = (ReliantDiscount)child;

                        if (r.getProduct() != null)
                        {
                            r.getProduct().removeReliantDiscount();
                        }
                    }
                    if (child is VisibleDiscount)
                    {
                        VisibleDiscount v = (VisibleDiscount)child;

                        if (v.getProduct() != null)
                        {
                            v.getProduct().removeDiscount();
                        }
                    }
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


        public void setMinPurchasePolicy(int minAmount, PurchasePolicy p)
        {
            PurchasePolicy max = getMaxPolicy();
            if (max == null)
            {
                p.setAmount(minAmount);
                DBStore.getInstance().setPolicy(p, storeId, minAmount);
            }
            else
            {
                if (max.getAmount() < minAmount)
                {
                    throw new ILLArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    p.setAmount(minAmount);
                    DBStore.getInstance().setPolicy(p, storeId, minAmount);
                }
            }

        }

        internal void setPolicyByID(int newAmount, int policyID)
        {
            PurchasePolicy p = findPolicyByID(policyID);
            if (p is ComplexPurchasePolicy)
                throw new ILLArgumentException("Can not set complex policy.");
            if (p is MaxAmountPurchase)
                setMaxPurchasePolicy(newAmount, p);
            if (p is MinAmountPurchase)
                setMinPurchasePolicy(newAmount, p);
            if (p is TotalPricePolicy)
                setTotalPolicy(newAmount, p);
        }

        private void setTotalPolicy(int newAmount, PurchasePolicy p)
        {
            if (newAmount < 0)
                throw new ILLArgumentException("Total cart price can not be a negative number.");
            p.setAmount(newAmount);
            DBStore.getInstance().setPolicy(p, storeId, newAmount);

        }

        private MaxAmountPurchase getMaxPolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MaxAmountPurchase)
                    return (MaxAmountPurchase)p;

                if (p is ComplexPurchasePolicy)
                {
                    if ((((ComplexPurchasePolicy)p).getFirstPolicyChild() is MaxAmountPurchase))
                        return (MaxAmountPurchase)((ComplexPurchasePolicy)p).getFirstPolicyChild();
                    if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is MaxAmountPurchase)
                        return (MaxAmountPurchase)((ComplexPurchasePolicy)p).getSecondPolicyChild();

                    if (((ComplexPurchasePolicy)p).getFirstPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MaxAmountPurchase)
                            return ((MaxAmountPurchase)(c.getFirstPolicyChild()));

                        else if (c.getSecondPolicyChild() is MaxAmountPurchase)
                        {
                            return ((MaxAmountPurchase)c.getSecondPolicyChild());
                        }
                    }
                    else if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MaxAmountPurchase)
                            return ((MaxAmountPurchase)(c.getFirstPolicyChild()));

                        else if (c.getSecondPolicyChild() is MaxAmountPurchase)
                        {
                            return ((MaxAmountPurchase)c.getSecondPolicyChild());
                        }
                    }
                }
            }
            return null;
        }
        private MinAmountPurchase getMinPolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MinAmountPurchase)
                    return (MinAmountPurchase)p;

                if (p is ComplexPurchasePolicy)
                {
                    if ((((ComplexPurchasePolicy)p).getFirstPolicyChild() is MinAmountPurchase))
                        return (MinAmountPurchase)((ComplexPurchasePolicy)p).getFirstPolicyChild();
                    if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is MinAmountPurchase)
                        return (MinAmountPurchase)((ComplexPurchasePolicy)p).getSecondPolicyChild();

                    if (((ComplexPurchasePolicy)p).getFirstPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MinAmountPurchase)
                            return ((MinAmountPurchase)(c.getFirstPolicyChild()));

                        else if (c.getSecondPolicyChild() is MinAmountPurchase)
                        {
                            return ((MinAmountPurchase)c.getSecondPolicyChild());
                        }
                    }
                    else if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MinAmountPurchase)
                            return ((MinAmountPurchase)(c.getFirstPolicyChild()));

                        else if (c.getSecondPolicyChild() is MinAmountPurchase)
                        {
                            return ((MinAmountPurchase)c.getSecondPolicyChild());
                        }
                    }
                }
            }
            return null;

    }
    public void setMaxPurchasePolicy(int maxAmount,PurchasePolicy p)
        {
            
            PurchasePolicy min = getMinPolicy();
            if (min == null)
            {
                p.setAmount(maxAmount);
                DBStore.getInstance().setPolicy(p, storeId, maxAmount);
            }
            else
            {
                if (min.getAmount() > maxAmount)
                {
                    throw new ILLArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    p.setAmount(maxAmount);
                    DBStore.getInstance().setPolicy(p, storeId, maxAmount);
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
        public void removePolicyByID(int policyID)
        {

            PurchasePolicy p = findPolicyByID(policyID);
            policies.Remove(p);
            DBStore.getInstance().removePolicy(p, storeId);
        }

        private PurchasePolicy findPolicyByID(int policyID)
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p.getPolicyID() == policyID)
                    return p;
            }
            return null;
        }

        
        public bool hasMinPurchasePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MinAmountPurchase)
                    return true;
                if (p is ComplexPurchasePolicy)
                {
                    if ((((ComplexPurchasePolicy)p).getFirstPolicyChild() is MinAmountPurchase) || ((ComplexPurchasePolicy)p).getSecondPolicyChild() is MinAmountPurchase)
                        return true;
                    if (((ComplexPurchasePolicy)p).getFirstPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MinAmountPurchase || c.getSecondPolicyChild() is MinAmountPurchase)
                        {
                            return true;
                        }
                    }
                    else if(((ComplexPurchasePolicy)p).getSecondPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getSecondPolicyChild());
                        if (c.getFirstPolicyChild() is MinAmountPurchase || c.getSecondPolicyChild() is MinAmountPurchase)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }
        public bool hasMaxPurchasePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is MaxAmountPurchase)
                    return true;
                if (p is ComplexPurchasePolicy)
                {
                    if ((((ComplexPurchasePolicy)p).getFirstPolicyChild() is MaxAmountPurchase) || ((ComplexPurchasePolicy)p).getSecondPolicyChild() is MaxAmountPurchase)
                        return true;
                    if (((ComplexPurchasePolicy)p).getFirstPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is MaxAmountPurchase || c.getSecondPolicyChild() is MaxAmountPurchase)
                        {
                            return true;
                        }
                    }
                    else if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getSecondPolicyChild());
                        if (c.getFirstPolicyChild() is MaxAmountPurchase || c.getSecondPolicyChild() is MaxAmountPurchase)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;

        }

        public bool hasTotalPricePolicy()
        {
            foreach (PurchasePolicy p in policies)
            {
                if (p is TotalPricePolicy)
                    return true;
                if (p is ComplexPurchasePolicy)
                {
                    if ((((ComplexPurchasePolicy)p).getFirstPolicyChild() is TotalPricePolicy) || ((ComplexPurchasePolicy)p).getSecondPolicyChild() is TotalPricePolicy)
                        return true;
                    if (((ComplexPurchasePolicy)p).getFirstPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getFirstPolicyChild());
                        if (c.getFirstPolicyChild() is TotalPricePolicy || c.getSecondPolicyChild() is TotalPricePolicy)
                        {
                            return true;
                        }
                    }
                    else if (((ComplexPurchasePolicy)p).getSecondPolicyChild() is ComplexPurchasePolicy)
                    {
                        ComplexPurchasePolicy c = (ComplexPurchasePolicy)(((ComplexPurchasePolicy)p).getSecondPolicyChild());
                        if (c.getFirstPolicyChild() is TotalPricePolicy || c.getSecondPolicyChild() is TotalPricePolicy)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;

        }

        public void addMinAmountPolicy(int minAmount)
        {
            if (hasMinPurchasePolicy())
            {
                throw new AlreadyExistException("Store can not have 2 minimum amount purchase policy.");
            }
            MaxAmountPurchase maxPolicy = getMaxPolicy();
            if (maxPolicy != null)
            {
                if (maxPolicy.getAmount() < minAmount)
                    throw new AlreadyExistException("Min purchase policy can not be bigger than Max purchase policy.");
            }

            MinAmountPurchase p = new MinAmountPurchase(minAmount);
            policies.AddLast(p);
            DBStore.getInstance().addMinPolicy(p, storeId);
        }
        public void addMaxAmountPolicy(int minAmount)
        {
            if (hasMaxPurchasePolicy())
            {
                throw new AlreadyExistException("Store can not have 2 maximum amount purchase policy.");
            }
            MinAmountPurchase minPolicy = getMinPolicy();
            if (minPolicy != null)
            {
                if (minPolicy.getAmount() < minAmount)
                    throw new AlreadyExistException("Max purchase policy can not be smaller than Min purchase policy.");
            }
            MaxAmountPurchase p = new MaxAmountPurchase(minAmount);
            policies.AddLast(p);
            DBStore.getInstance().addMaxPolicy(p, storeId);
        }

        public void addTotalAmountPolicy(int minPrice)
        {
            if (hasTotalPricePolicy())
            {
                throw new AlreadyExistException("Store can not have 2 total cart price - purchase policy.");
            }

            TotalPricePolicy p = new TotalPricePolicy(minPrice);
            policies.AddLast(p);
            DBStore.getInstance().addTotalPrice(p, storeId);
        }

        public void addComplexPurchasePolicy(int index1, int index2, string type)
        {
            PurchasePolicy p1 = policies.ElementAt(index1);
            PurchasePolicy p2 = policies.ElementAt(index2);

            ComplexPurchasePolicy complexPurchase = new ComplexPurchasePolicy(type, p1, p2);
            policies.Remove(p1);
            policies.Remove(p2);
            policies.AddLast(complexPurchase);
            DBStore.getInstance().addComplexPolicy(complexPurchase, storeId);
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
