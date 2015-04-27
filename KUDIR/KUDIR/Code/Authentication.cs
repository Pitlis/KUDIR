using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using KUDIR.Properties;

namespace KUDIR.Code
{
    public class Authentication
    {
        public static bool Security = false;
        public Authentication(string login, string password)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["KUDIR"].ConnectionString);
            connection.UserID = login;
            connection.Password = password;
        }

        public static string GetSqlConnectionString()
        {
            return Settings.Default["KUDIRConnectionString"].ToString();
        }
    }
}
