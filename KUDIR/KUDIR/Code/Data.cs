﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace KUDIR.Code
{
    public class Data
    {
        public DataTable Table
        {
            get
            {
                return _dataSet.Tables.Count == 0 ? null : _dataSet.Tables[0];
            }
        }
        public List<int> HiddenColumns { get; private set; }

        DataSet _dataSet;
        SqlConnection connect;
        SqlDataAdapter _adapter;

        public Data(DataTypes type, string cnStr)
        {
            HiddenColumns = new List<int>();
            _dataSet = new DataSet(type.ToString());
            connect = new SqlConnection(cnStr);
            switch (type)
            {
                case DataTypes.Выручка:
                    Create_Выручка();
                    break;
                default:
                    throw new Exception("Некорректный тип данных!");
            }
        }

        void Create_Выручка()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Выручка", connect);
            _adapter.Fill(_dataSet);

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Документ_выручка"));

            SqlCommand comDel = new SqlCommand(@"Update Выручка SET DEL = 1 WHERE ID = @ID1; Update Документ SET DEL = 1 WHERE DocumentID = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "Документ_выручка");
            _adapter.DeleteCommand = comDel;

            SqlCommand comUpd = new SqlCommand("upd_Выручка", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            comUpd.Parameters.Add("@Реализация", SqlDbType.Money, sizeof(Decimal), "Выручка от реализации товаров (работ, услуг), имущественных прав");
            comUpd.Parameters.Add("@Внереализ", SqlDbType.Money, sizeof(Decimal), "Внереализационные доходы");
            comUpd.Parameters.Add("@Операция", SqlDbType.VarChar, -1, "Содержание операции");
            comUpd.Parameters.Add("@Дата_записи", SqlDbType.DateTime, 8, "Дата записи");
            comUpd.Parameters.Add("@Номер_док", SqlDbType.VarChar, -1, "Номер документа");
            comUpd.Parameters.Add("@Наим_док", SqlDbType.VarChar, -1, "Название документа");
            comUpd.Parameters.Add("@Дата_док", SqlDbType.DateTime, 8, "Дата документа");
            comUpd.Parameters.Add("@Док_выручка", SqlDbType.Int, 4, "Документ_выручка");
            _adapter.UpdateCommand = comUpd;

            SqlCommand comIns = new SqlCommand("add_Выручка", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Реализация", SqlDbType.Money, sizeof(Decimal), "Выручка от реализации товаров (работ, услуг), имущественных прав");
            comIns.Parameters.Add("@Внереализ", SqlDbType.Money, sizeof(Decimal), "Внереализационные доходы");
            comIns.Parameters.Add("@Операция", SqlDbType.VarChar, -1, "Содержание операции");
            comIns.Parameters.Add("@Номер_док", SqlDbType.VarChar, -1, "Номер документа");
            comIns.Parameters.Add("@Наим_док", SqlDbType.VarChar, -1, "Название документа");
            comIns.Parameters.Add("@Дата_док", SqlDbType.DateTime, 8, "Дата документа");

            _adapter.InsertCommand = comIns;
        }


        public enum DataTypes
        {
            Выручка
        }

        public void Update()
        {
            _adapter.Update(_dataSet);
        }
    }
}
