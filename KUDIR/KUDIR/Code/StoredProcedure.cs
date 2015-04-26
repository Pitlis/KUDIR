using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUDIR.Code
{
    public class StoredProcedure
    {
        SqlConnection connect;
        public StoredProcedure(string strConnect)
        {
            connect = new SqlConnection(strConnect);
        }

        public void Generate_Выручка(DateTime date)
        {
            SqlCommand comIns = new SqlCommand("АвтозаполнениеВыручка", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add(new SqlParameter("@Месяц ", date));

            
            using (connect)
            {
                try
                {
                    connect.Open();
                    comIns.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
