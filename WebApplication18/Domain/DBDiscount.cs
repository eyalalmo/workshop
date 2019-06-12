using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebApplication18.DAL;
namespace workshop192.Domain
{
    public class DBDiscount
    {
        private static DBDiscount instance;
        private Dictionary<int, DiscountComponent> discounts;
        private static int nextID;

        public static DBDiscount getInstance()
        {
            if (instance == null)
            {
                instance = new DBDiscount();
            }
            return instance;
        }

        private DBDiscount()
        {
            discounts = new Dictionary<int, DiscountComponent>();
            nextID = 1;
        }

        public void init()
        {
            /*try
            {
                connection.Open();
                var d = connection.Query<Discount>("SELECT * FROM [dbo].[Discount]");

                if (d.Count() == 0)
                {
                    connection.Close();
                    return;
                }

                foreach (Discount discount in d)
                {
                    discounts.Add(discount.getId(), discount);
                    if (discount.getId() > nextID)
                        nextID = discount.getId();
                }

                connection.Close();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }

            nextID++;*/
        }

        public void initTests()
        {
            init();
        }

        public void addDiscount(Discount d)
        {
            try
            {
                /*connection.Open();

                string sql = "INSERT INTO [dbo].[Discount] (id, percentage, duration)" +
                                 " VALUES (@id, @percentage, @duration)";
                connection.Execute(sql, new
                {
                    id = d.getId(),
                    percentage = d.getPercentage(),
                    duration = d.getDuration()
                });
                
                connection.Close();*/
                discounts.Add(d.getId(), d);
            }

            catch (Exception e)
            {
                //connection.Close();
                throw e;
            }
        }
        public void removeDiscount(Discount d)
        {
            //if (!discounts.ContainsKey(d))
              //  throw new DoesntExistException("Error: Discount does not exist");
            
            LinkedList<Product> result = new LinkedList<Product>();
            try
            {
              //  connection.Open();
                //connection.Execute("DELETE FROM Discount WHERE id=@id ", new { id = d.getId() });
                discounts.Remove(d.getId());
                //connection.Close();
            }

            catch (Exception e)
            {
                //connection.Close();
                throw e;
            }
        }

        public static int getNextDiscountID()
        {
            int id = nextID;
            nextID++;
            return id;
        }

        internal VisibleDiscount getDiscount(int discountID)
        {
            return (VisibleDiscount)discounts[discountID];
        }

        internal void update(Discount discount)
        {
            try
            {
               /* connection.Open();

                connection.Execute("UPDATE Discount SET " +
                                          "id = @id, " +
                                          "percentage = @percentage, " +
                                          "duration = @duration " +
                                          "WHERE id=@id",
                      new
                      {
                          id = discount.getId(),
                          percentage = discount.getPercentage(),
                          duration = discount.getDuration()
                      });

                connection.Close();*/
            }
            catch (Exception e)
            {
                //connection.Close();
                throw e;
            }
        }
        public DiscountComponent getDiscountByID(int id)
        {
            discounts.TryGetValue(id, out DiscountComponent value);
            return value;
        }

    }
}

