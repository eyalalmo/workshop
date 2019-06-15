using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;
using WebApplication18.Domain;
using workshop192.Domain;

namespace WebApplication18.DAL
{
    public class Connector
    {
        //private static Connector instance;

        public MySqlConnection con;
        public SqlConnection connection;

        public string stringDB = "Server=tcp:wsep192.database.windows.net,1433;Initial Catalog=wsep192;Persist Security Info=False;User ID=eilon532;Password=wsep192!!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public string stringDBTest = "Server=tcp:wsep192.database.windows.net,1433;Initial Catalog=wsep192_Tests;Persist Security Info=False;User ID=eilon532;Password=wsep192!!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public Connector()
        {
            try
            {
                if (MarketSystem.testsMode == true)
                    connection = new SqlConnection(stringDBTest);

                else
                    connection = new SqlConnection(stringDB);

            }
            catch (Exception)
            {
                throw new Exception("error");
            }
        }
        public void deleteAllTable()
        {
            try
            {
                // SqlConnection connection = Connector.getInstance().getSQLConnection();

                connection.Open();
                connection.Execute("DELETE FROM Stores \n"
                           + " DELETE FROM PendingOwners \n"
                           + " DELETE FROM StoreRoles \n"
                           + " DELETE FROM Register \n"
                           + " DELETE FROM Product \n"
                           + " DELETE FROM BasketCart \n"
                           + " DELETE FROM CartProduct \n"

                           + " DELETE FROM Notification \n"
                           + " DELETE FROM Contracts \n"
                           + " DELETE FROM Discount \n"
                           + " DELETE FROM DiscountComponent \n"
                           + " DELETE FROM DiscountComposite \n"
                           + "UPDATE [dbo].[IDS] SET id = 0 WHERE type = 'store'"
                           );
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
