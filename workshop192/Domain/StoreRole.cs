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
        String addProduct(Product product);
        String removeProduct(Product product);
        String setProductPrice(Product product, int price);
        String setProductName(Product product, String name);
        String addToProductQuantity(Product product, int amount);
        String decFromProductQuantity(Product product, int amount);
        String setProductDiscount(Product product, Discount discount);
        String addManager(SubscribedUser manager, Dictionary<string, bool> permissions);
        String addOwner(SubscribedUser owner);
        String remove(SubscribedUser user);
        String closeStore();
        SubscribedUser getAppointedBy();
        void removeAllAppointedBy();
    }
}
