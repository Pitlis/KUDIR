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
        public static bool Security = true;
        public static Access UserAccess;

        static SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(Settings.Default["KUDIR_SERVER"].ToString());
        public Authentication(string login, string password)
        {
            SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder(Settings.Default["KUDIR_SERVER"].ToString());
            connect.UserID = login;
            connect.Password = password;
            if(DataBaseConfig.TestConnect(connect.ConnectionString))
            {
                connection = connect;
                SetAccess(login);
            }
            else
            {
                throw new Exception("Неверный логин или пароль!");
            }
        }
        
        void SetAccess(string login)
        {
            switch (login)
            {
                case "administrator":
                    UserAccess = Access.Администратор;
                    break;
                case "book":
                    UserAccess = Access.Бухгалтер;
                    break;
                case "seller":
                    UserAccess = Access.Продавец;
                    break;
                case "printer":
                    UserAccess = Access.Принтер;
                    break;
                default:
                    break;
            }
        }
        public static string GetSqlConnectionString()
        {
            if (!Security)
            {
                return Settings.Default["KUDIRcs"].ToString();
            }
            else
            {
                return connection.ConnectionString;
            }
        }

        public enum Access
        {
            Администратор, Продавец, Бухгалтер, Принтер
        }
    }
}
