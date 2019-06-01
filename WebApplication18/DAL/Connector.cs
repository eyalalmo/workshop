using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Dapper;
using WebApplication18.Domain;


namespace WebApplication18.DAL
{
    public abstract class Connector
    {

            public MySqlConnection con;

            public SqlConnection connection;

            public string stringDB = "Server=tcp:ws192.database.windows.net,1433;Initial Catalog=ws192;Persist Security Info=False;User ID=etay;Password=ws192!!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            public string stringDBTest = "Server=tcp:ws192.database.windows.net,1433;Initial Catalog = ws192Tests; Persist Security Info=False;User ID =etay; Password=ws192!!!!; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";


    public Connector()
            {
                try
                {  
                if(IsTestsMode.isTest==true)
                    connection = new SqlConnection(stringDBTest);               
                
                else
                    connection = new SqlConnection(stringDB);
            
                 }
                catch (Exception)
                {
                    throw new Exception("error");
                }
            }

      
        }
    }
