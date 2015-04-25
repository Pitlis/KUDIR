using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUDIR.Code
{
    public static class DataBaseConfig
    {
        public static void CreateCopyOfCurrentDB(string path)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["KUDIR"].ConnectionString);
            string pathToFile = connection.AttachDBFilename;
            try
            {
                File.Copy(pathToFile, path);

                SqlConnectionStringBuilder backupConnect = new SqlConnectionStringBuilder(connection.ConnectionString);
                backupConnect.AttachDBFilename = path;
                if (!TestConnect(backupConnect.ConnectionString))
                {
                    File.Delete(path);
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удается скопировать файл из-за исключения: " + ex.Message);
            }
        }

        public static bool TestConnect(string strConnect)
        {
            SqlConnection connect = new SqlConnection(strConnect);
            try
            {
                connect.Open();
                connect.Close();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
