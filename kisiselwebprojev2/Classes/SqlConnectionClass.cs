using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace kisiselwebprojev2.Classes
{
    public class SqlConnectionClass
    {

        public static SqlConnection db = new SqlConnection(@" Data Source = HALIL; Initial Catalog = proje; Integrated Security = True");

        public static void CheckConnection()
        {
            if (db.State == System.Data.ConnectionState.Closed)
            {
                db.Open();
            }

            else
            {

            }
        }

    }
}