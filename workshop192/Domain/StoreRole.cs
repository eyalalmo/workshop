using System;
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
        void setProductDiscount(Product product, Discount discount);
        void addManager(SubscribedUser manager, Permissions permissions);
        void addOwner(SubscribedUser owner);
        void remove(SubscribedUser user);
        void closeStore();
        void removeRoleAppointedByMe(StoreRole role);
        SubscribedUser getAppointedBy();
        void removeAllAppointedBy();
    }
}
