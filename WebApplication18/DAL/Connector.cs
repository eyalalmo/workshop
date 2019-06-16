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
                           + " DELETE FROM IDS \n"
                           + " DELETE FROM PurchasePolicy \n"
                           + "INSERT INTO IDS(type, id)\n" +
                             "VALUES ('store', 0)\n" +
                            "INSERT INTO IDS (type, id)\n" +
                            "VALUES ('policy', 0)\n" +
                            "INSERT INTO IDS (type, id)\n" +
                            "VALUES ('discount', 0)"
                           //+ "UPDATE [dbo].[IDS] SET id = 0 WHERE type = 'store'"
                           );
                connection.Close();

            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public void createAllTables() {

            try
            {
                // SqlConnection connection = Connector.getInstance().getSQLConnection();

                connection.Open();
                connection.Execute(
                    "CREATE TABLE PurchasePolicy(\n" +
                        "storeID int,\n" +
                        "policyID int,\n" +
                        "type varchar(255) NOT NULL,\n" +
                        "amount int,\n" +
                        "isPartOfComplex int,\n" +
                        "subtypeID1 int,\n" +
                        "subtypeID2 int,\n" +
                        "compType varchar(255),\n" +
                        "PRIMARY KEY(storeID, policyID)" +
                    ");\n" +

                    "CREATE TABLE Stores(\n" +
                        "storeId int NOT NULL,\n" +
                        "name varchar(255) NOT NULL,\n" +
                        "description varchar(255),\n" +
                        "numOfOwners int,\n" +
                        "active int,\n" +
                        "PRIMARY KEY(storeId)\n" +
                    ");\n" +

                    "CREATE TABLE StoreRoles(\n" +
                        "storeId int NOT NULL,\n" +
                        "appointedBy varchar(255),\n" +
                        "userName varchar(255) NOT NULL,\n" +
                        "isOwner int,\n" +
                        "editProduct int,\n" +
                        "editDiscount int,\n" +
                        "editPolicy int,\n" +
                       "PRIMARY KEY(storeId, userName)\n" +
                    ");\n" +

                    "CREATE TABLE Product(\n" +
                        "productID int PRIMARY KEY,\n" +
                        "productName varchar(255) NOT NULL,\n" +
                        "productCategory varchar(255),\n" +
                        "price int,\n" +
                        "rank int,\n" +
                        "quantityLeft int,\n" +
                        "storeID int\n" +
                    ");\n" +

                    "CREATE TABLE Notification(\n" +
                        "username varchar(255),\n" +
                        "message varchar(255),\n" +
                        "PRIMARY KEY(username, message)" +
                    ");\n" +

                   "CREATE TABLE Register(\n" +
                       "username varchar(255),\n" +
                       "password varchar(255) NOT NULL,\n" +
                       "PRIMARY KEY(username)" +
                   ");\n" +


                    "CREATE TABLE BasketCart(\n" +
                        "username varchar(255),\n" +
                        "storeID int,\n" +
                    "PRIMARY KEY(username, storeID)" +
                    ");\n" +


                    "CREATE TABLE CartProduct(\n" +
                        "username varchar(255),\n" +
                        "productID int,\n" +
                        "storeID int,\n" +
                        "amount int,\n" +
                        "PRIMARY KEY(username, productID, storeID)" +
                    ");\n" +

                    "CREATE TABLE Contracts(\n" +
                        "storeId int NOT NULL,\n" +
                        "userName varchar(255) NOT NULL,\n" +
                        "approvedBy varchar(255) NOT NULL,\n" +
                        "Constraint PK_Cont PRIMARY KEY(storeId, userName, approvedBy)\n" +
                    ");\n" +

                    "CREATE TABLE PendingOwners(\n" +
                        "storeId int NOT NULL,\n" +
                        "userName varchar(255) NOT NULL,\n" +
                        "Constraint PK_Pend PRIMARY KEY(storeId, userName)\n" +
                    ");\n" +

                    "CREATE TABLE Discount(\n" +
                        "id int,\n" +
                        "type varchar(128),\n" +
                        "reliantType varchar(128),\n" +
                        "visibleType varchar(128),\n" +
                        "productId int,\n" +
                        "storeId int,\n" +
                        "numOfProducts int,\n" +
                        "totalAmount int\n" +
                    ");\n" +

                    "CREATE TABLE DiscountComponent(\n" +
                        "id int,\n" +
                        "percentage float,\n" +
                        "duration varchar(128),\n" +
                        "type varchar(128),\n" +
                        "storeId int,\n" +
                        "isPartOfComplex int\n" +
                    ");\n" +

                    "CREATE TABLE DiscountComposite(\n" +
                        "id int,\n" +
                        "childid int,\n" +
                        "type varchar(128)\n" +
                    "); " +
                    
                    "CREATE TABLE IDS(\n"+
                        "type varchar(255),\n" +
                        "id int\n" +
                    ");\n" +

                    "INSERT INTO IDS (type, id)\n" +
                    "VALUES ('store', 0)\n"+

                    "INSERT INTO IDS (type, id)\n" +
                    "VALUES ('policy', 0)\n" +

                    "INSERT INTO IDS (type, id)\n" +
                    "VALUES ('discount', 0)"


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
