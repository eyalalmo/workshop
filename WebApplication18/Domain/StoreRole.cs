﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public interface StoreRole
    {
        SubscribedUser getUser();
        Store getStore();
        void addProduct(Product product);
        void removeProduct(Product product);
        void setProductPrice(Product product, int price);
        void setProductName(Product product, String name);
        void addToProductQuantity(Product product, int amount);
        void decFromProductQuantity(Product product, int amount);
        void setProductDiscount(Product product, DiscountComponent discount);
        void addManager(SubscribedUser manager, Permissions permissions);
        void addOwner(SubscribedUser owner);
        void remove(SubscribedUser user);
        void closeStore();
        void removeRoleAppointedByMe(StoreRole role);
        SubscribedUser getAppointedBy();
        void removeAllAppointedBy();
        void addProductVisibleDiscount(Product product, double percentage, string duration);
        void removeProductDiscount(Product product);
        void addStoreVisibleDiscount(double percentage, string duration);
        void addReliantDiscountSameProduct(double percentage, String duration, int numOfProducts, Product product);
        void addReliantDiscountTotalAmount(double percentage, String duration, int amount);
        void removeStoreDiscount(int discountID, Store store);
        void addComplexDiscount(List<DiscountComponent> list, string type, double percentage, string duration);
        void removeMaxAmountPolicy();
        void removeMinAmountPolicy();
        void setMinAmountPolicy(int newMinAmount);
        void setMaxAmountPolicy(int newMinAmount);
        void removeCouponFromStore(string couponCode);
        void addCouponToStore(string couponCode, double percentage, string duration);
        void addPendingOwner(SubscribedUser pending);
        void signContract(string owner, SubscribedUser pending);
        void declineContract(string owner, SubscribedUser pending);
         int getIsOwner();
        Permissions GetPermissions();
        
    }
}
