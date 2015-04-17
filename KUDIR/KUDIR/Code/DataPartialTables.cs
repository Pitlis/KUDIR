﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUDIR.Code
{
    //Для частичных таблицы
    //Редактирование и вывод - как обычно, в datagrid
    //Для добавления записей используется отдельная форма

    public class DataPartialTables: Data
    {

        public DataPartialTables(DataTypes type, string cnStr)
            :base(type, cnStr)
        {

        }

        public void AddNewRecord(int работникID, DateTime Дата, int ПодоходныйНалогПроцент, string Номер_ПлатежныйДок, DateTime? Дата_ПлатежныйДок, Decimal? Сумма_ПлатежныйДок)
        {
            SqlConnection connection = new SqlConnection(connect.ConnectionString);
            SqlCommand comIns = new SqlCommand("Create_ПодоходНалогПеречислено", connection);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add(new SqlParameter("@работникID", работникID));
            comIns.Parameters.Add(new SqlParameter("@Дата", Дата));
            comIns.Parameters.Add(new SqlParameter("@ПодоходныйНалогПроцент", ПодоходныйНалогПроцент));
            comIns.Parameters.Add(new SqlParameter("@Номер_ПлатежныйДок", Номер_ПлатежныйДок == null ? DBNull.Value : (object)Номер_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Дата_ПлатежныйДок", Дата_ПлатежныйДок == null ? DBNull.Value : (object)Дата_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Сумма_ПлатежныйДок", Сумма_ПлатежныйДок == null ? DBNull.Value : (object)Сумма_ПлатежныйДок));

            connection.Open();
            using(connection)
            {
                try
                {
                    comIns.ExecuteNonQuery();
                }
                catch { }
            }
        }
    }
}