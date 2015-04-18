using System;
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

        //Для таблицы ПодоходныйНалог
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

        //Для таблицы СтраховойВзнос
        public void AddNewRecord(int работникID, DateTime Дата, Int16 Дней, Decimal? ИныеПлатежи, Decimal? ПеречисленоФондом, Decimal? ЗадолжЗаПредПериод, int ЗаМесяц, string Номер_ПлатежныйДок, DateTime? Дата_ПлатежныйДок, Decimal? Сумма_ПлатежныйДок)
        {
            SqlConnection connection = new SqlConnection(connect.ConnectionString);
            SqlCommand comIns = new SqlCommand("add_СтраховойВзнос", connection);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add(new SqlParameter("@работникID", работникID));
            comIns.Parameters.Add(new SqlParameter("@Дата", Дата));
            comIns.Parameters.Add(new SqlParameter("@ИныеПлатежи", ИныеПлатежи == null ? DBNull.Value : (object)ИныеПлатежи));
            comIns.Parameters.Add(new SqlParameter("@Дней", Дней));
            comIns.Parameters.Add(new SqlParameter("@ПеречисленоФондом", ПеречисленоФондом == null ? DBNull.Value : (object)ПеречисленоФондом));
            comIns.Parameters.Add(new SqlParameter("@ЗадолжЗаПредПериод", ЗадолжЗаПредПериод == null ? DBNull.Value : (object)ЗадолжЗаПредПериод));
            comIns.Parameters.Add(new SqlParameter("@ЗаМесяц", ЗаМесяц == 0 ? DBNull.Value : (object)ЗаМесяц));
            comIns.Parameters.Add(new SqlParameter("@Номер_ПлатежныйДок", Номер_ПлатежныйДок == null ? DBNull.Value : (object)Номер_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Дата_ПлатежныйДок", Дата_ПлатежныйДок == null ? DBNull.Value : (object)Дата_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Сумма_ПлатежныйДок", Сумма_ПлатежныйДок == null ? DBNull.Value : (object)Сумма_ПлатежныйДок));

            connection.Open();
            using (connection)
            {
                try
                {
                    comIns.ExecuteNonQuery();
                }
                catch(Exception ex)
                {

                }
            }
        }

        //Для таблицы ПенсионныйВзнос
        public void AddNewRecord(int работникID, DateTime Дата, Decimal? ИныеПлатежи, Decimal? ЗадолжЗаПредПериод, string Номер_ПлатежныйДок, DateTime? Дата_ПлатежныйДок, Decimal? Сумма_ПлатежныйДок)
        {
            SqlConnection connection = new SqlConnection(connect.ConnectionString);
            SqlCommand comIns = new SqlCommand("add_ПенсионныйВзнос", connection);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add(new SqlParameter("@работникID", работникID));
            comIns.Parameters.Add(new SqlParameter("@Дата", Дата));
            comIns.Parameters.Add(new SqlParameter("@ИныеПлатежи", ИныеПлатежи == null ? DBNull.Value : (object)ИныеПлатежи));
            comIns.Parameters.Add(new SqlParameter("@ЗадолжЗаПредПериод", ЗадолжЗаПредПериод == null ? DBNull.Value : (object)ЗадолжЗаПредПериод));
            comIns.Parameters.Add(new SqlParameter("@Номер_ПлатежныйДок", Номер_ПлатежныйДок == null ? DBNull.Value : (object)Номер_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Дата_ПлатежныйДок", Дата_ПлатежныйДок == null ? DBNull.Value : (object)Дата_ПлатежныйДок));
            comIns.Parameters.Add(new SqlParameter("@Сумма_ПлатежныйДок", Сумма_ПлатежныйДок == null ? DBNull.Value : (object)Сумма_ПлатежныйДок));

            connection.Open();
            using (connection)
            {
                try
                {
                    comIns.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
