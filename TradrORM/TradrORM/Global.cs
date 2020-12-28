using System;
using System.Collections.Generic;
using System.Text;

namespace TradrORM
{
    public class Global
    {
        private static string _defaultConnectionString = "Server=localhost\\SQLEXPRESS;Database=TRADR;Trusted_Connection=True;";
        private static string _connectionString = "Server=localhost\\SQLEXPRESS;Database=TRADR;Trusted_Connection=True;";

        public static string DefaultConnectionString
        {
            get
            {
                return _defaultConnectionString;
            }
        }

        public static string ConnectionString 
        { 
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
            }
        }
    }
}
