using System;
using System.Collections.Generic;

namespace workshop192.Domain
{
    public class StoreManager : StoreRole

    {
        public SubscribedUser appointedBy;
        private Store store;
        public SubscribedUser user;
        public Permissions permissions;
        public bool isOwner = false;

        public StoreManager(SubscribedUser appointedBy, Store store,
            SubscribedUser user, Permissions permissions)
        {
            this.appointedBy = appointedBy;
            this.store = store;
            this.user = user;
            this.permissions = permissions;
        }

        public void addManager(SubscribedUser manager, Permissions permissions)
        {
            throw new RoleException("Error: A manager cannot appoint a manager");
        }

        public void addOwner(SubscribedUser owner)
        {
            throw new RoleException("Error: A manager cannot appoint an owner");
        }

        public void remove(SubscribedUser owner)
        {
            throw new RoleException("Error: A manager can't remove a role from the store");
        }

        public void addProduct(Product product)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() + 
                    " has no permissions to edit products in store " +
                    store.getStoreName());
            store.addProduct(product);
            DBProduct.getInstance().addProduct(product);
        }

        public void removeProduct(Product product)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() +
                     " has no permissions to edit products in store " +
                     store.getStoreName());
            if (product.getStore() != store || !store.getProductList().Contains(product))
                throw new StoreException("product " + product.getProductName() +
                    " doesn't belong to store " + store.getStoreName());
            store.removeProduct(product);
            DBProduct.getInstance().removeProduct(product);
        }

        public void setProductPrice(Product product, int price)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() +
                    " has no permission to set product's price in store "
                    + store.getStoreName());
            product.setPrice(price);
        }

        public void setProductName(Product product, string name)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() +
                    " has no permission to set product's name in store " +
                    store.getStoreName());
            product.setProductName(name);
        }

        public void addToProductQuantity(Product product, int amount)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() +
                    " has no permission to add to product's quantity in store " +
                    store.getStoreName());
            product.addQuantityLeft(amount);
        }

        public void decFromProductQuantity(Product product, int amount)
        {
            if (!permissions.editProduct())
                throw new PermissionsException("Error:" + user.getUsername() +
                    " has no permission to decrease from product's quantity in store "
                    + store.getStoreName());
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                throw new AlreadyExistException("current quantity is " +
                    curQuan + " and it can't be decreased by " + amount);
            product.decQuantityLeft(amount);
        }

        public void setProductDiscount(Product product, DiscountComponent discount)
        {
            if (!permissions.editDiscount())
                throw new PermissionsException("Error:" + user.getUsername() +
                    " has no permission to set product's discount in store " +
                    store.getStoreName());
            //product.setDiscount(discount);
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
            return;
        }
        public Permissions getPermissions()
        {
            return permissions;

        }

        public void removeRoleAppointedByMe(StoreRole role)
        {
            return;
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
        
        public void addProductVisibleDiscount(Product product, double percentage, string duration)
        {
            VisibleDiscount discount = new VisibleDiscount(percentage, duration);
            product.setDiscount(discount);
        }

        public void removeProductDiscount(Product product)
        {
            product.removeDiscount();
        }

        //////

        

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

       

        public void setMaxAmountPolicy( int newMaxAmount)
        {
            store.setMaxPurchasePolicy(newMaxAmount);

        }

        public void removeCouponFromStore(string couponCode)
        {
            store.removeCoupon(couponCode);
        }
        
       /* public void addCouponToStore(string couponCode, int percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }
        */
        public void addCouponToStore(string couponCode, double percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }

    }
}
