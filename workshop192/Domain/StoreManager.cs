using System;
using System.Collections.Generic;

namespace workshop192.Domain
{


    public class StoreManager : StoreRole

    {
        private SubscribedUser appointedBy;
        private Store store;
        private SubscribedUser user;
        private Dictionary<string, bool> permissions;

        public StoreManager(SubscribedUser appointedBy, Store store,
            SubscribedUser user, Dictionary<string, bool> permissions)
        {
            this.appointedBy = appointedBy;
            this.store = store;
            this.user = user;
            this.permissions = permissions;
        }

        public string addManager(SubscribedUser manager, Dictionary<string, bool> permissions)
        {
            return user.getUsername() + " is not allowed to add manager to " + store.getStoreName();
        }

        public string addOwner(SubscribedUser owner)
        {
            return user.getUsername() + " is not allowed to add owner to " + store.getStoreName();
        }

        public String removeManager(SubscribedUser manager)
        {
            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, manager);
            if (sr == null)
                return "user " + manager.getUsername() + " doesn't have a role in store " + store.getStoreName();
            if (sr is StoreOwner)
                return "user " + manager.getUsername() + " is an owner of " + store.getStoreName();
            storeDB.removeStoreRole(store, manager);
            return "";
        }

        public string removeOwner(SubscribedUser owner)
        {
            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, owner);
            if (sr == null)
                return "user " + owner.getUsername() + " doesn't have a role in store " + store.getStoreName();
            if (sr is StoreManager)
                return "user " + owner.getUsername() + " is a manager of " + store.getStoreName();
            storeDB.removeStoreRole(store, owner);
            return "";
        }

        public string addProduct(string name, string category, int price, int quantity)
        {
            if (!permissions["addProduct"])
                return user.getUsername() + " has no permission to add products in store " + store.getStoreName();
            store.addProduct(new Product(name, category, price, 0, quantity, store));
            return "";
        }

        public string removeProduct(Product product)
        {
            if (!permissions["removeProduct"])
                return user.getUsername() + " has no permission to remove products in store " + store.getStoreName();
            if (!store.productExists(product))
                return "product doesn't exsits";
            store.removeProduct(product);
            return "";
        }

        public string setProductPrice(Product product, int price)
        {
            if (!permissions["setProductPrice"])
                return user.getUsername() + " has no permission to set product's price in store " + store.getStoreName();
            product.setPrice(price);
            return "";
        }

        public string setProductName(Product product, string name)
        {
            if (!permissions["setProductName"])
                return user.getUsername() + " has no permission to set product's name in store " + store.getStoreName();
            product.setProductName(name);
            return "";
        }

        public string addToProductQuantity(Product product, int amount)
        {
            if (!permissions["addToProductQuantity"])
                return user.getUsername() + " has no permission to add to product's quantity in store " + store.getStoreName();
            product.addQuantityLeft(amount);
            return "";
        }

        public string decFromProductQuantity(Product product, int amount)
        {
            if (!permissions["decFromProductQuantity"])
                return user.getUsername() + " has no permission to decrease from product's quantity in store " + store.getStoreName();
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                return "current quantity is " + curQuan + " and it can't be decreased by " + amount;
            product.decQuantityLeft(amount);
            return "";
        }

        public string setProductDiscount(Product product, Discount discount)
        {
            if (!permissions["setProductDiscount"])
                return user.getUsername() + " has no permission to set product's discount in store " + store.getStoreName();
            product.setDiscount(discount);
            return "";
        }

        public string closeStore()
        {
            DBStore storeDB = DBStore.getInstance();
            storeDB.closeStore(store);
            return "";
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
    }
}
