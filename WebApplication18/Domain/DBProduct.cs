using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication18.DAL;
using Dapper;

namespace workshop192.Domain
{
    public class DBProduct : Connector
    {
        private static DBProduct instance;

        private LinkedList<Product> productList;
        public static int nextProductID;

        public static DBProduct getInstance()
        {
            if (instance == null)
                instance = new DBProduct();
            return instance;
        }

        private DBProduct()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }

        public void init()
        {
            try
            {
                connection.Open();
                var products = connection.Query<Product>("SELECT * FROM [dbo].[Product]");

                if (products.Count() == 0)
                {
                    connection.Close();
                    return;
                }

                foreach (Product product in products)
                {
                    product.discount = DBDiscount.getInstance().getDiscount(product.discountID);
                    productList.AddFirst(product);
                    if (product.getProductID() > nextProductID)
                        nextProductID = product.getProductID();
                }

                connection.Close();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }

            nextProductID++;
        }

        public int addProduct(Product p)
        {
            try
            {
                connection.Open();

                string sql = "INSERT INTO [dbo].[Product] (productID, productName, " +
                                                          "productCategory, price, rank, " +
                                                          "quantityLeft, storeID, discountID)" +
                                 " VALUES (@productID, @productName, @productCategory, " +
                                 " @price, @rank, @quantityLeft, @storeID, @discountID)";
                connection.Execute(sql, new
                {
                    productID = p.getProductID(),
                    productName = p.getProductName(),
                    productCategory = p.getProductCategory(),
                    price = p.getPrice(),
                    rank = p.getRank(),
                    quantityLeft = p.getQuantityLeft(),
                    storeID = p.getStoreID(),
                    discountID = p.discount.getId()
                });

                DBDiscount.getInstance().addDiscount(p.discount);
                /*sql = "INSERT INTO [dbo].[Discount] (discountID, percentage, duration) " +
                                 " VALUES (@discountID, @percentage, @duration) ";
                connection.Execute(sql, new
                {
                    discountID = p.discount.getId(),
                    percentage = p.discount.getPercentage(),
                    duration = p.discount.getDuration()
                });*/

                connection.Close();
                productList.AddFirst(p);
                return p.getProductID();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public string AllproductsToJson()
        {

            return JsonConvert.SerializeObject(productList);

            /*LinkedList<Product> result = new LinkedList<Product>();
            try
            {
                connection.Open();
                var products = connection.Query<Product>("SELECT * FROM [dbo].[Product]");
                //var discounts = connection.Query<VisibleDiscount>("SELECT * FROM [dbo].[Discount]");
                if (products.Count() == 0)
                {
                    connection.Close();
                    return JsonConvert.SerializeObject(result);
                }

                foreach (Product product in products)
                {
                    foreach (VisibleDiscount discount in discounts)
                    {
                        if(product.productID == discount.)
                        result.AddFirst(product);
                    }
                    product.discount = DBDiscount.getInstance().getDiscount(product.discountID);
                    result.AddFirst(product);
                }

                connection.Close();
                return JsonConvert.SerializeObject(result);
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
            */
        }
        public static int getNextProductID()
        {
            int id = nextProductID;
            nextProductID++;
            return id;
        }

        public void removeProduct(Product p)
        {
            if (!productList.Contains(p))
                throw new DoesntExistException("Product " + p.getProductName() + " Doesn't exist");

            LinkedList<Product> result = new LinkedList<Product>();
            try
            {
                connection.Open();
                connection.Execute("DELETE FROM Product WHERE productID=@productID ", new { productID = p.getProductID() });
                DBDiscount.getInstance().removeDiscount(p.discount);
                productList.Remove(p);
                connection.Close();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public Product getProductByID(int id)
        {
            Product result;
            try
            {
                foreach (Product p in productList)
                    if (p.getProductID() == id)
                        return p;
                connection.Open();
                var c = connection.Query<Product>("SELECT * FROM [dbo].[Product] WHERE productID=" +
                                                  "@product ", new { product = id });
                if (c.Count() == 0)
                {
                    connection.Close();
                    return null;
                }

                connection.Close();

                Product product = c.First();
                product.discount = DBDiscount.getInstance().getDiscount(product.discountID);
                productList.AddFirst(product);
                return product;
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public LinkedList<Product> getAllProducts()
        {
            /*LinkedList<Product> result = new LinkedList<Product>();
            try
            {
                connection.Open();
                var c = connection.Query<Product>("SELECT * FROM [dbo].[Product]");

                connection.Close();
                foreach (Product product in c)
                {
                    Product p = product;
                    p.discount = DBDiscount.getInstance().getDiscount(p.discountID);
                    result.AddFirst(p);
                }

                return result;
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }*/

            return productList;
        }

        public List<Product> searchProducts(string name, string keywords, string category)
        {
            /*List<Product> result = new List<Product>();
            try
            {
                connection.Open();
                var c = connection.Query<Product>("SELECT * FROM [dbo].[Product] WHERE " +
                                                  "productName=@product name OR " +
                                                  "productCategory=@productCategory OR " +
                                                  "CONTAINS (productName, @keywords) OR " +
                                                  "CONTAINS (productCategory, @keywords)");
                connection.Close();
                foreach (Product product in c)
                {
                    Product p = product;
                    p.discount = DBDiscount.getInstance().getDiscount(p.discountID);
                    result.Add(p);
                }

                return result;
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }*/

            List<Product> res = new List<Product>();
            foreach (Product p in productList)
            {
                if (name != null && p.getProductName() != null && p.getProductName() == name)
                {
                    res.Add(p);
                }
                string pName = p.getProductName();
                if (keywords != null && pName != null && pName.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }
                string categ = p.getProductCategory();
                if (keywords != null && categ != null && categ.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }

                if (category != null && categ != null & categ.Contains(category))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }
            }
            return res;

        }

        public List<Product> filterBy(List<Product> list, int[] price_range, int minimumRank)
        {
            List<Product> toRemove = new List<Product>();
            foreach (Product p in list)
            {
                double price = p.getActualPrice();
                if (price_range != null && (price < price_range[0] || price > price_range[1]))
                    toRemove.Add(p);
                else if (minimumRank != 0 && p.getRank() < minimumRank)
                    if (list.Contains(p))
                        toRemove.Add(p);
            }

            return list.Except(toRemove).ToList();
        }

        public void update(Product p)
        {
            try
            {
                connection.Open();

                connection.Execute("UPDATE productID = @productID, " +
                                          "productName = @productName, " +
                                          "productCategory = @productCategory, " +
                                          "price = @price, " +
                                          "rank = @rank, " +
                                          "quantityLeft = @quantityLeft, " +
                                          "storeID = @storeID, " +
                                          "discountID = @discountID " +
                                          "FROM Product WHERE productID=@productID",
                      new
                      {
                          productID = p.getProductID(),
                          productName = p.getProductName(),
                          productCategory = p.getProductCategory(),
                          price = p.getPrice(),
                          rank = p.getRank(),
                          quantityLeft = p.getQuantityLeft(),
                          storeID = p.getStoreID(),
                          discountID = p.discount.getId()
                      });
                
                DBDiscount.getInstance().update(p.discount);
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
    }
}
