using System;
using System.Collections.Generic;

namespace workshop192.Domain
{
    public class StoreManager : StoreRole

    {
        private SubscribedUser appointedBy;
        private Store store;
        private SubscribedUser user;
        private Permissions permissions;

        public StoreManager(SubscribedUser appointedBy, Store store,
            SubscribedUser user, Permissions permissions)
        {
            this.appointedBy = appointedBy;
            this.store = store;
            this.user = user;
            this.permissions = permissions;
        }

        public string addManager(SubscribedUser manager, Permissions permissions)
        {
            return user.getUsername() + " is not allowed to add manager to " + store.getStoreName();
        }

        public string addOwner(SubscribedUser owner)
        {
            return user.getUsername() + " is not allowed to add owner to " + store.getStoreName();
        }

        public string remove(SubscribedUser owner)
        {
            return user.getUsername() + " is not allowed to remove roles from " + store.getStoreName();
        }

        public string addProduct(Product product)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to add products in store " + store.getStoreName();
            store.addProduct(product);
            DBProduct.getInstance().addProduct(product);
            return "";
        }

        public string removeProduct(Product product)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to remove products in store " + store.getStoreName();
            if (product.getStore() != store || !store.getProductList().Contains(product))
                return "product doesn't belong to this store";
            store.removeProduct(product);
            DBProduct.getInstance().removeProduct(product);
            return "";
        }

        public string setProductPrice(Product product, int price)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to set product's price in store " + store.getStoreName();
            product.setPrice(price);
            return "";
        }

        public string setProductName(Product product, string name)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to set product's name in store " + store.getStoreName();
            product.setProductName(name);
            return "";
        }

        public string addToProductQuantity(Product product, int amount)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to add to product's quantity in store " + store.getStoreName();
            product.addQuantityLeft(amount);
            return "";
        }

        public string decFromProductQuantity(Product product, int amount)
        {
            if (!permissions.editProduct())
                return user.getUsername() + " has no permission to decrease from product's quantity in store " + store.getStoreName();
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                return "current quantity is " + curQuan + " and it can't be decreased by " + amount;
            product.decQuantityLeft(amount);
            return "";
        }

        public string setProductDiscount(Product product, Discount discount)
        {
            if (!permissions.editDiscount())
                return user.getUsername() + " has no permission to set product's discount in store " + store.getStoreName();
            //product.setDiscount(discount);
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

        public void removeAllAppointedBy()
        {
            return;
        }

    }
}
