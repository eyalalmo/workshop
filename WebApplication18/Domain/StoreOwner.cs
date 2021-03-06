﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Domain
{ 
    public class StoreOwner : StoreRole

    {
        public SubscribedUser appointedBy;
        private Store store;
        public SubscribedUser userName;
        private List<StoreRole> appointedByMe;
        public bool isOwner = true;

        public StoreOwner(SubscribedUser appointedBy, SubscribedUser user, Store store)
        {
            this.appointedBy = null;
            this.userName = user;
            this.store = store;
            appointedByMe = new List<StoreRole>();
        }
        public int getIsOwner()
        {
            return 1;
        }
        public StoreOwner()
        {
        }

        public void addProduct(Product product)
        {
            store.addProduct(product);
            DBProduct.getInstance().addProduct(product);
        }

        public void removeProduct(Product product)
        {
            if (product.getStore() != store || !store.getProductList().Contains(product))
                throw new StoreException("product " 
                    + product.getProductName() + " doesn't belong to store " 
                    + store.getStoreName());
            store.removeProduct(product);
            DBProduct.getInstance().removeProduct(product);
        }

        public void setProductPrice(Product product, int price)
        {
            product.setPrice(price);
        }

        public void setProductName(Product product, string name)
        {
            product.setProductName(name);
        }

        public void addToProductQuantity(Product product, int amount)
        {
            product.addQuantityLeft(amount);
        }

        public void decFromProductQuantity(Product product, int amount)
        {
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                throw new AlreadyExistException("product "
                    + product.getProductName() + " quantity is " + product.getQuantityLeft() +
                    "so you decrease " + amount + " of its quantity");
            product.decQuantityLeft(amount);
        }

        public void setProductDiscount(Product product, DiscountComponent discount)
        {
            //product.setDiscount(discount);
        }

        public void removeRoleAppointedByMe(StoreRole role)
        {
            appointedByMe.Remove(role);
        }

        public void addManager(SubscribedUser manager, Permissions permissions)
        {
            StoreRole newManager = new StoreManager(this.userName, store, manager, permissions);
            if (store.getStoreRole(manager) != null)
                throw new RoleException("Error: Username "  + manager.getUsername() + 
                    " already has a role in store " + 
                    store.getStoreName());
            DBStore.getInstance().addStoreRole(newManager);
            store.addStoreRoleFromInitOwner(newManager);
            manager.addStoreRole(newManager);
            appointedByMe.Add(newManager);
        }
        
        public void addOwner(SubscribedUser owner)
        {
            StoreRole newOwner = new StoreOwner(this.userName, owner, store);
            if (store.getStoreRole(owner) != null)
                throw new RoleException("Error: Username " + owner.getUsername() +
                    " already has a role in store " +
                    store.getStoreName());
            store.addStoreRoleFromInitOwner(newOwner);
            owner.addStoreRole(newOwner);
            //appointedByMe.Add(newOwner);
            DBStore.getInstance().addStoreRole(newOwner);
        }

        public void remove(SubscribedUser role)
        {
            StoreRole sr = role.getStoreRole(store);
            
            if (sr == null)
                throw new RoleException("user " + role.getUsername() + 
                    " doesn't have a role in store " 
                    + store.getStoreName());
            if (sr.getAppointedBy() != this.userName)
                throw new RoleException("Error: User " + userName.getUsername() + 
                    " didn't appoint " + 
                    role.getUsername());
            DBStore.getInstance().removeStoreRole(sr);
            role.removeStoreRole(sr);
            store.removeStoreRole(sr);
           
        }

        public void closeStore()
        {
            DBStore storeDB = DBStore.getInstance();
            storeDB.closeStore(store);
        }

        public SubscribedUser getUser()
        {
            return userName;
        }

        public Store getStore()
        {
            return store;
        }

        public SubscribedUser getAppointedBy()
        {
            return appointedBy;
        }

        public void removeAllAppointedBy()
        {
            foreach (StoreRole sr in appointedByMe)
            {
                if(sr is StoreManager)
                    remove(sr.getUser());
            }
        }

        

        public void addProductVisibleDiscount(Product product, double percentage, string duration)
        {
            Store store = product.getStore();
            VisibleDiscount discount = new VisibleDiscount(percentage, duration, "ProductVisibleDiscount", store.getStoreID());
            store.addDiscount(discount);
            discount.setProduct(product);
            product.setDiscount(discount);
            DBDiscount.getInstance().addDiscount(discount);
        }
        public void removeProductDiscount(Product product)
        {
            product.removeDiscount();
        }

        public void addStoreVisibleDiscount(double percentage, string duration)
        {
            VisibleDiscount v = new VisibleDiscount(percentage, duration, "StoreVisibleDiscount", store.getStoreID());       
            DBDiscount.getInstance().addDiscount(v);
            store.addDiscount(v);

        }

        public void addReliantDiscountSameProduct(double percentage, String duration, int numOfProducts, Product product)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, numOfProducts, product, store.getStoreID());
            DBDiscount.getInstance().addDiscount(r);
            store.addDiscount(r);
            product.setReliantDiscountSameProduct(r);

        }

        public void addReliantDiscountTotalAmount(double percentage, String duration, int amount)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, amount, store.getStoreID());
            DBDiscount.getInstance().addDiscount(r);
            store.addDiscount(r);
            

        }

        public void removeStoreDiscount(int discountID, Store store)
        {
            DiscountComponent d = DBDiscount.getInstance().getDiscountByID(discountID);
            DBDiscount.getInstance().removeDiscount(d);
            store.removeDiscount(discountID);
            

        }
        public void addComplexDiscount(List<DiscountComponent> list, string type, double percentage, string duration)
        {
            DiscountComposite composite = new DiscountComposite(list, type, percentage, duration, store.getStoreID());
            foreach (DiscountComponent d in list)
            {
                store.removeDiscoutFromList(d.getId());   
                DBDiscount.getInstance().setIsPartOfComplex(d.getId(), true);
                d.setIsPartOfComplex(true);

            }
            DBDiscount.getInstance().addDiscount(composite);
            store.addDiscount(composite);

            //DBDiscount.getInstance().addDiscount(composite);

        }

        
        //public void removeCouponFromStore(string couponCode)
        //{
        //    store.removeCoupon(couponCode);
        //}

        /* public void addCouponToStore(string couponCode, int percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }
        */
        //public void addCouponToStore(string couponCode, double percentage, string duration)
        //{
        //    store.addCoupon(couponCode, percentage, duration);
        //}

        public void addPendingOwner(SubscribedUser pending)
        { 
            DBStore.getInstance().addPendingOwner(store.getStoreID(),userName.getUsername(), pending.getUsername());

        }
        public void signContract(SubscribedUser pending)
        {
            if (DBStore.getInstance().hasContract(store.getStoreID(), pending.getUsername(), userName.getUsername()))
                throw new AlreadyExistException("You have already signed a contract with " + pending.getUsername());
            int approvedOwners = DBStore.getInstance().getContractNum(store.getStoreID(),pending.getUsername());
            if (approvedOwners == store.getNumberOfOwners()-1)
            {
                StoreRole newOwner = new StoreOwner(this.userName, pending, store);
                //DBStore.getInstance().signContract(store.getStoreID(), userName.getUsername(), pending.getUsername(),true);
                //DBStore.getInstance().removePendingOwner(store.getStoreID(),pending.getUsername());
                //DBStore.getInstance().addStoreRole(newOwner);
                store.addStoreRoleFromInitOwner(newOwner);
                pending.addStoreRole(newOwner);
                appointedByMe.Add(newOwner);
                DBStore.getInstance().signAndAddOwner(store.getStoreID(), userName.getUsername(), pending.getUsername(), newOwner);

            }
            else
            {
                DBStore.getInstance().signContract(store.getStoreID(), userName.getUsername(), pending.getUsername(), false);
            }
        }
        public void declineContract(SubscribedUser pending)
        {
            DBStore.getInstance().declineContract(store.getStoreID(), pending.getUsername());
            //DBStore.getInstance().removeAllUserContracts(store.getStoreID(), pending.getUsername());
            //DBStore.getInstance().removePendingOwner(store.getStoreID(), pending.getUsername());
        }
        public Permissions GetPermissions()
        {
            return new Permissions(true, true, true);
        }



        public void removePolicy(int index)
        {
            store.removePolicyByID(index);
        }

        public void setPolicyByID(int newAmount, int policyID)
        {
            store.setPolicyByID(newAmount, policyID);
        }
        public void addMinPurchasePolicy(int amount)
        {
            store.addMinAmountPolicy(amount);
        }

        public void addMaxPurchasePolicy(int amount)
        {
            store.addMaxAmountPolicy(amount);
        }

        public void addTotalPricePurchasePolicy(int amount)
        {
            store.addTotalAmountPolicy(amount);
        }

        public void addComplexPolicy(int index1, int index2, string type)
        {

            store.addComplexPurchasePolicy(index1, index2, type);
        }

    }
}











