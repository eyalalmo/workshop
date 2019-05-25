using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


namespace WebApplication18.DAL
{
    public abstract class Connector
    {

            public MySqlConnection con;

            public SqlConnection connection;

            public string Production_DB = "Server=tcp:ws192.database.windows.net,1433;Initial Catalog=ws192;Persist Security Info=False;User ID=etay;Password=ws192!!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
          

            public Connector()
            {
                try
                {   
                        connection = new SqlConnection(Production_DB);               
                }
                catch (Exception e)
                {
                    throw new Exception("error");
                }
            }

           //public abstract LinkedList<DAO> Get();

            //public abstract Boolean Add(DAO obj);

            //public abstract Boolean Remove(DAO obj);
        }
    }
