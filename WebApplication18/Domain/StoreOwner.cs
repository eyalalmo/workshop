using System;
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
        public SubscribedUser user;
        private List<StoreRole> appointedByMe;
        public bool isOwner = true;

        public StoreOwner(SubscribedUser appointedBy, SubscribedUser user, Store store)
        {
            this.appointedBy = appointedBy;
            this.user = user;
            this.store = store;
            appointedByMe = new List<StoreRole>();
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
            StoreRole newManager = new StoreManager(this.user, store, manager, permissions);
            DBStore.getInstance().addStoreRole(newManager);
            if (store.getStoreRole(manager) != null)
                throw new RoleException("Error: Username "  + manager.getUsername() + 
                    " already has a role in store " + 
                    store.getStoreName());
            store.addStoreRole(newManager);
            manager.addStoreRole(newManager);
            appointedByMe.Add(newManager);
        }
        
        public void addOwner(SubscribedUser owner)
        {
            StoreRole newOwner = new StoreOwner(this.user, owner, store);
            if (store.getStoreRole(owner) != null)
                throw new RoleException("Error: Username " + owner.getUsername() +
                    " already has a role in store " +
                    store.getStoreName());
            store.addStoreRole(newOwner);
            owner.addStoreRole(newOwner);
            appointedByMe.Add(newOwner);
            DBStore.getInstance().addStoreRole(newOwner);
        }

        public void remove(SubscribedUser role)
        {
            StoreRole sr = role.getStoreRole(store);
            DBStore.getInstance().removeStoreRole(sr);
            if (sr == null)
                throw new RoleException("user " + role.getUsername() + 
                    " doesn't have a role in store " 
                    + store.getStoreName());
            if (sr.getAppointedBy() != this.user)
                throw new RoleException("Error: User " + user.getUsername() + 
                    " didn't appoint " + 
                    role.getUsername());
            sr.removeAllAppointedBy();
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
            return user;
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
                remove(sr.getUser());
        }

        

        public void addProductVisibleDiscount(Product product, double percentage, string duration)
        {
            VisibleDiscount discount = new VisibleDiscount(percentage, duration);
            product.setDiscount(discount);
        }
        public void removeProductDiscount(Product product)
        {
            product.removeDiscount();
        }

        public void addStoreVisibleDiscount(double percentage, string duration)
        {
            VisibleDiscount v = new VisibleDiscount(percentage, duration);
            store.addDiscount(v);
        }

        public void addReliantDiscountSameProduct(double percentage, String duration, int numOfProducts, Product product)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, numOfProducts, product);
            store.addDiscount(r);
        }

        public void addReliantDiscountTotalAmount(double percentage, String duration, int amount)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, amount);
            store.addDiscount(r);
        }

        public void removeStoreDiscount(Store store)
        {
            store.removeDiscount();
        }
        public void addComplexDiscount(List<DiscountComponent> list, string type)
        {
            DiscountComposite composite = new DiscountComposite(list, type);
            store.addDiscount(composite);
        }

        public void addMaxAmountPolicy( int maxAmount)
        {
            MaxAmountPurchase p = new MaxAmountPurchase(maxAmount);
            store.addPurchasePolicy(p);
        }

        public void removeMaxAmountPolicy()
        {
            store.removeMaxAMountPolicy();
        }
        public void removeMinAmountPolicy()
        {
            store.removeMinAmountPolicy();
        }

        public void setMinAmountPolicy( int newMinAmount)
        {
            store.setMinPurchasePolicy(newMinAmount);
        }

        public void addMinAmountPolicy( int minAmount)
        {
            MinAmountPurchase p = new MinAmountPurchase(minAmount);
            store.addPurchasePolicy(p);
        }

        public void setMaxAmountPolicy(int newMaxAmount)
        {
            store.setMaxPurchasePolicy(newMaxAmount);

        }

        public void removeCouponFromStore(string couponCode)
        {
            store.removeCoupon(couponCode);
        }

        public void addCouponToStore(string couponCode, int percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }

        public void addCouponToStore(string couponCode, double percentage, string duration)
        {
            throw new NotImplementedException();
        }
    }
}
