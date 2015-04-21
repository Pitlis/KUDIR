using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUDIR.Code
{
    public class Data_Строение
    {
        Data _dataBuild;
        DataTable _table;
        int _number; // -1 - новая запись

        public Data_Строение(Data data, int number)
        {
            _dataBuild = data;
            _table = data.Table;
            _number = number;
            if (_number > _table.Rows.Count)
                throw new IndexOutOfRangeException();
            LoadFromBase();
        }
        public Data_Строение(Data data)
        {
            _dataBuild = data;
            _table = data.Table;
            _number = -1;
            ID = -1;
        }

        void LoadFromBase()
        {
            ID = (int)_table.Rows[_number]["ID"];
            Наименование = _table.Rows[_number]["Наименование"].ToString();
            Адрес = _table.Rows[_number]["Адрес"].ToString();
            Дата_приобретения = _table.Rows[_number]["Дата_приобретения"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_приобретения"];
            Дата_гос_регистрации = _table.Rows[_number]["Дата_гос_регистрации"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_гос_регистрации"];
            Дата_ввода_в_эксплуатацию = _table.Rows[_number]["Дата_ввода_в_эксплуатацию"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_ввода_в_эксплуатацию"];
            Дата_выбытия = _table.Rows[_number]["Дата_выбытия"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Дата_выбытия"];
            Док_Номер_Приобр = _table.Rows[_number]["Док_приобр_Номер"].ToString();
            Док_Наименование_Приобр = _table.Rows[_number]["Док_приобр_Наим"].ToString();
            Док_Дата_Приобр = _table.Rows[_number]["Док_приобр_Дата"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Док_приобр_Дата"];
            Док_Номер_Рег = _table.Rows[_number]["Док_рег_Номер"].ToString();
            Док_Наименование_Рег = _table.Rows[_number]["Док_рег_Наим"].ToString();
            Док_Дата_Рег = _table.Rows[_number]["Док_рег_Дата"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Док_рег_Дата"];
            Док_Номер_Выб = _table.Rows[_number]["Док_выбыт_Номер"].ToString();
            Док_Наименование_Выб = _table.Rows[_number]["Док_выбыт_Наим"].ToString();
            Док_Дата_Выб = _table.Rows[_number]["Док_выбыт_Дата"] == DBNull.Value ? null : (Nullable<System.DateTime>)_table.Rows[_number]["Док_выбыт_Дата"];
            _право = _table.Rows[_number]["Право"] == null ? (byte)0 : (byte)_table.Rows[_number]["Право"];
            if (_право < 0 || _право > 3)
                _право = 0;
        }
        void LoadToBase(DataRow row)
        {
            row["Наименование"] = Наименование == null ? (object)DBNull.Value : Наименование;
            row["Адрес"] = Адрес == null ? (object)DBNull.Value : Адрес;
            row["Дата_приобретения"] = Дата_приобретения == null ? (object)DBNull.Value : Дата_приобретения;
            row["Дата_гос_регистрации"] = Дата_гос_регистрации == null ? (object)DBNull.Value : Дата_гос_регистрации;
            row["Дата_ввода_в_эксплуатацию"] = Дата_ввода_в_эксплуатацию == null ? (object)DBNull.Value : Дата_ввода_в_эксплуатацию;
            row["Дата_выбытия"] = Дата_выбытия == null ? (object)DBNull.Value : Дата_выбытия;
            row["Док_приобр_Номер"] = Док_Номер_Приобр == null ? (object)DBNull.Value : Док_Номер_Приобр;
            row["Док_приобр_Наим"] = Док_Наименование_Приобр == null ? (object)DBNull.Value : Док_Наименование_Приобр;
            row["Док_приобр_Дата"] = Док_Дата_Приобр == null ? (object)DBNull.Value : Док_Дата_Приобр;
            row["Док_рег_Номер"] = Док_Номер_Рег == null ? (object)DBNull.Value : Док_Номер_Рег;
            row["Док_рег_Наим"] = Док_Наименование_Рег == null ? (object)DBNull.Value : Док_Наименование_Рег;
            row["Док_рег_Дата"] = Док_Дата_Рег == null ? (object)DBNull.Value : Док_Дата_Рег;
            row["Док_выбыт_Номер"] = Док_Номер_Выб == null ? (object)DBNull.Value : Док_Номер_Выб;
            row["Док_выбыт_Наим"] = Док_Наименование_Выб == null ? (object)DBNull.Value : Док_Наименование_Выб;
            row["Док_выбыт_Дата"] = Док_Дата_Выб == null ? (object)DBNull.Value : Док_Дата_Выб;
            row["Право"] = _право;
        }

        public void Update()
        {
            if(_number > -1)
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
            _dataBuild.Update();
        }
        public void Delete()
        {
            if(_number != -1)
            {
                _table.Rows[_number].Delete();
                _dataBuild.Update();
            }
        }

        #region Свойства

        public int ID { get; set; }
        public string Наименование { get; set; }
        public string Адрес { get; set; }
        public Nullable<System.DateTime> Дата_приобретения { get; set; }
        public Nullable<System.DateTime> Дата_гос_регистрации { get; set; }
        public Nullable<System.DateTime> Дата_ввода_в_эксплуатацию { get; set; }
        public Nullable<System.DateTime> Дата_выбытия { get; set; }

        public string Док_Номер_Приобр { get; set; }
        public string Док_Наименование_Приобр { get; set; }
        public Nullable<bool> Док_DEL_Приобр { get; set; }
        public Nullable<System.DateTime> Док_Дата_Приобр { get; set; }

        public string Док_Номер_Рег { get; set; }
        public string Док_Наименование_Рег { get; set; }
        public Nullable<bool> Док_DEL_Рег { get; set; }
        public Nullable<System.DateTime> Док_Дата_Рег { get; set; }

        public string Док_Номер_Выб { get; set; }
        public string Док_Наименование_Выб { get; set; }
        public Nullable<bool> Док_DEL_Выб { get; set; }
        public Nullable<System.DateTime> Док_Дата_Выб { get; set; }

        byte _право { get; set; }
        public Type_Право Право { get { return (Type_Право)Enum.ToObject(typeof(Type_Право), _право); } set { _право = (byte)value; } }

        public bool DEL { get; set; }

        #endregion


        public enum Type_Право
        {
            собственности, хоз_ведения, оперативного_упр, пользования
        }
    }
}
