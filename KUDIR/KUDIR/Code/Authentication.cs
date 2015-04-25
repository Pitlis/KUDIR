using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace KUDIR.Code
{
    public class Authentication
    {
        public Authentication(string login, string password)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["KUDIR"].ConnectionString);
            connection.UserID = login;
            connection.Password = password;
        }

        public static string GetSqlConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["KUDIR"].ConnectionString;
        }
    }
}
