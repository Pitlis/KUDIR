using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KUDIR.Properties;

namespace KUDIR.Code
{
    public static class DataBaseConfig
    {
        public static void CreateCopyOfCurrentDB(string path)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(GetSqlConnectionString());
            string pathToFile = connection.AttachDBFilename;
            try
            {
                File.Copy(pathToFile, path);

                SqlConnectionStringBuilder backupConnect = new SqlConnectionStringBuilder(connection.ConnectionString);
                backupConnect.AttachDBFilename = Path.Combine(path);
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
        public static void ChangeDB(string path)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["KUDIR"].ConnectionString);
            connection.AttachDBFilename = Path.Combine(path);
            SqlConnection connect = new SqlConnection(connection.ConnectionString);
            try
            {
                connect.Open();
                connect.Close();
                Settings.Default["KUDIRcs"] = connect.ConnectionString;
                Settings.Default.Save();
                SqlConnection.ClearPool(connect);
                connect.Dispose();
            }
            catch(Exception ex)
            {
                connect.Close();
                SqlConnection.ClearPool(connect);
                connect.Dispose();
                throw;
            }
        }

        public static bool TestConnect(string strConnect)
        {
            SqlConnection connect = new SqlConnection(strConnect);
            try
            {
                connect.Open();
                connect.Close();
                SqlConnection.ClearPool(connect);
                connect.Dispose();
            }
            catch(Exception ex)
            {
                connect.Close();
                SqlConnection.ClearPool(connect);
                connect.Dispose();
                return false;
            }
            return true;
        }


        public static string GetSqlConnectionString()
        {
            return Settings.Default["KUDIRcs"].ToString();
        }

        public static void CreateNewDB(string path)
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(GetSqlConnectionString());
            string pathToFile = connection.AttachDBFilename;
            try
            {
                File.WriteAllBytes(path + "//KUDIR.mdf", Properties.Resources.KUDIRempty);
                File.WriteAllBytes(path + "//KUDIR_log.ldf", Properties.Resources.KUDIRempty_log);

                SqlConnectionStringBuilder backupConnect = new SqlConnectionStringBuilder(connection.ConnectionString);
                string p1, p2;
                p1 = Path.Combine(path, "KUDIR.mdf");
                p2 = Path.Combine(path, "KUDIR_log.ldf");
                backupConnect.AttachDBFilename = p1;
                if (!TestConnect(backupConnect.ConnectionString))
                {
                    File.Delete(p1);
                    File.Delete(p2);
                    throw new Exception();
                }
                else
                {
                    ChangeDB(p1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удается создать новую базу:\n" + ex.Message);
            }
        }
    }
}
