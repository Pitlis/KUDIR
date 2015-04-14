using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUDIR.Code
{
    public class Data_Работник
    {
        Data _dataEmpl;
        DataTable _table;
        int _number; // -1 - новая запись

        public Data_Работник(Data data, int number)
        {
            _dataEmpl = data;
            _table = data.Table;
            _number = number;
            if (_number > _table.Rows.Count)
                throw new IndexOutOfRangeException();
            LoadFromBase();
        }
        public Data_Работник(Data data)
        {
            _dataEmpl = data;
            _table = data.Table;
            _number = -1;
            работникID = -1;
        }


        void LoadFromBase()
        {
            работникID = (int)_table.Rows[_number]["работникID"];
            Условия_договора = _table.Rows[_number]["Условия_договора"].ToString();
            Номер_договора = _table.Rows[_number]["Номер_договора"].ToString();
            ФИО = _table.Rows[_number]["ФИО"].ToString();
            Дата_договора = _table.Rows[_number]["Дата_договора"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_договора"];
            Инвалидность = _table.Rows[_number]["Инвалидность"].ToString();
            Дата_выплаты_вознаграждения = _table.Rows[_number]["Дата_выплаты_вознаграждения"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_выплаты_вознаграждения"];
            Тариф_пенс_взносов = _table.Rows[_number]["Тариф_пенс_взносов"] == DBNull.Value ? null : (Nullable<short>)_table.Rows[_number]["Тариф_пенс_взносов"];
            Документы_вычеты = _table.Rows[_number]["Документы_вычеты"].ToString();
            Тариф_страх_взносов = _table.Rows[_number]["Тариф_страх_взносов"] == DBNull.Value ? null : (Nullable<short>)_table.Rows[_number]["Тариф_страх_взносов"];
        }
        void LoadToBase(DataRow row)
        {
            row["Условия_договора"] = Условия_договора == null ? (object)DBNull.Value : Условия_договора;
            row["Номер_договора"] = Номер_договора == null ? (object)DBNull.Value : Номер_договора;
            row["ФИО"] = ФИО == null ? (object)DBNull.Value : ФИО;
            row["Дата_договора"] = Дата_договора == null ? (object)DBNull.Value : Дата_договора;
            row["Инвалидность"] = Инвалидность == null ? (object)DBNull.Value : Инвалидность;
            row["Дата_выплаты_вознаграждения"] = Дата_выплаты_вознаграждения == null ? (object)DBNull.Value : Дата_выплаты_вознаграждения;
            row["Тариф_пенс_взносов"] = Тариф_пенс_взносов == null ? (object)DBNull.Value : Тариф_пенс_взносов;
            row["Документы_вычеты"] = Документы_вычеты == null ? (object)DBNull.Value : Документы_вычеты;
            row["Тариф_страх_взносов"] = Тариф_страх_взносов == null ? (object)DBNull.Value : Тариф_страх_взносов;
        }

        public void Update()
        {
            if (_number > -1)
            {
                LoadToBase(_table.Rows[_number]);
            }
            else
            {
                DataRow row = _table.NewRow();
                LoadToBase(row);
                _table.Rows.Add(row);
                _number = _table.Rows.IndexOf(row);
            }
            _dataEmpl.Update();
        }
        public void Delete()
        {
            if (_number != -1)
            {
                _table.Rows[_number].Delete();
                _dataEmpl.Update();
            }
        }

        #region Свойства

        public int работникID { get; set; }


        public string Условия_договора { get; set; }
        public string Номер_договора { get; set; }
        public string ФИО { get; set; }
        public Nullable<System.DateTime> Дата_договора { get; set; }
        public string Инвалидность { get; set; }
        public string Документы_вычеты { get; set; }
        public Nullable<System.DateTime> Дата_выплаты_вознаграждения { get; set; }
        public Nullable<short> Тариф_пенс_взносов { get; set; }
        public Nullable<short> Тариф_страх_взносов { get; set; }

        #endregion
    }
}
