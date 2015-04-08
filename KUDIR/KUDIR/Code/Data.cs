using System;
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
        public string[] ColumnPositions { get; private set; }
        public string[] ColumnNames { get; private set; } // новые имена упорядочены согласно их позиции в ColumnPosition

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
                case DataTypes.Отгрузка:
                    Create_Отгрузка();
                    break;
                case DataTypes.Предоплата:
                    Create_Предоплата();
                    break;
                case DataTypes.Кредитор:
                    Create_Кредитор();
                    break;
                default:
                    throw new Exception("Некорректный тип данных!");
            }
        }

        void Create_Выручка()
        {
            _adapter = new SqlDataAdapter("Select * FROM Выручка WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));


            ColumnPositions = new string[] { "Дата_записи", "Док_выручка_Наим", "Док_выручка_Номер", "Док_выручка_Дата", "Содержание_операции", "Выручка_от_реализации", "Внереализационные_доходы" };
            ColumnNames = new string[] { "Дата записи", "Наим докумета выручки", "Номер документа выручки", "Дата документа выручки", "Содержание операции", "Выручка от реализации", "Внереализационные доходы" };
            

            //SqlCommand comDel = new SqlCommand(@"Update Выручка SET DEL = 1 WHERE ID = @ID1; Update Документ SET DEL = 1 WHERE DocumentID = @ID2", connect);
            //comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            //comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "Документ_выручка");
            //_adapter.DeleteCommand = comDel;

            //SqlCommand comUpd = new SqlCommand("upd_Выручка", connect);
            //comUpd.CommandType = CommandType.StoredProcedure;
            //comUpd.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            //comUpd.Parameters.Add("@Реализация", SqlDbType.Money, sizeof(Decimal), "Выручка от реализации");
            //comUpd.Parameters.Add("@Внереализ", SqlDbType.Money, sizeof(Decimal), "Внереализационные доходы");
            //comUpd.Parameters.Add("@Операция", SqlDbType.VarChar, -1, "Содержание операции");
            //comUpd.Parameters.Add("@Дата_записи", SqlDbType.DateTime, 8, "Дата записи");
            //comUpd.Parameters.Add("@Номер_док", SqlDbType.VarChar, -1, "Номер документа");
            //comUpd.Parameters.Add("@Наим_док", SqlDbType.VarChar, -1, "Название документа");
            //comUpd.Parameters.Add("@Дата_док", SqlDbType.DateTime, 8, "Дата документа");
            //comUpd.Parameters.Add("@Док_выручка", SqlDbType.Int, 4, "Документ_выручка");
            //_adapter.UpdateCommand = comUpd;

            //SqlCommand comIns = new SqlCommand("add_Выручка", connect);
            //comIns.CommandType = CommandType.StoredProcedure;
            //comIns.Parameters.Add("@Реализация", SqlDbType.Money, sizeof(Decimal), "Выручка от реализации");
            //comIns.Parameters.Add("@Внереализ", SqlDbType.Money, sizeof(Decimal), "Внереализационные доходы");
            //comIns.Parameters.Add("@Операция", SqlDbType.VarChar, -1, "Содержание операции");
            //comIns.Parameters.Add("@Номер_док", SqlDbType.VarChar, -1, "Номер документа");
            //comIns.Parameters.Add("@Наим_док", SqlDbType.VarChar, -1, "Название документа");
            //comIns.Parameters.Add("@Дата_док", SqlDbType.DateTime, 8, "Дата документа");

            //_adapter.InsertCommand = comIns;
        }
        void Create_Отгрузка()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Отгрузка", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);
            _dataSet.Tables[0].Columns["Предоплачено"].DefaultValue = false;


            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Предоплачено"));
        }
        void Create_Предоплата()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Предоплата", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);
            _dataSet.Tables[0].Columns["Предоплачено"].DefaultValue = true;


            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Предоплачено"));
        }
        void Create_Кредитор()
        {
            _adapter = new SqlDataAdapter("Select * FROM Кредитор", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));

            ColumnPositions = new string[] { "Название", "Номер_договора", "Дата_договора", "Предмет_договора", "Док_задолж_Наим", "Док_задолж_Номер", "Док_задолж_Дата", "Сумма_бр", "Наим_валюты", "Сумма_в_валюте" };
            ColumnNames = new string[] { "Кредитор", "Номер договора", "Дата договора", "Предмет договора", "Наим док задолж", "Номер док задолж", "Дата док задолж", "Сумма в бр", "Наим валюты", "Сумма в валюте" };
        }




        public enum DataTypes
        {
            Выручка, Отгрузка, Предоплата, Кредитор
        }

        public void Update()
        {
            _adapter.Update(_dataSet);
        }


    }
}
