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
        public bool CanEdit { get; private set; } // по умолчанию редактирование разрешено

        protected DataSet _dataSet;
        protected SqlConnection connect;
        protected SqlDataAdapter _adapter;

        public Data(DataTypes type, string cnStr)
        {
            HiddenColumns = new List<int>();
            _dataSet = new DataSet(type.ToString());
            connect = new SqlConnection(cnStr);
            CanEdit = true;
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
                case DataTypes.Дивиденты:
                    Create_Дивиденты();
                    break;
                case DataTypes.Кооператив:
                    Create_Кооператив();
                    break;
                case DataTypes.НезавершенныеСтроения:
                    Create_НезавершенныеСтроения();
                    break;
                case DataTypes.ТоварыТС:
                    Create_ТоварыТС();
                    break;
                case DataTypes.НДС_Приобретение:
                    Create_НДС_Приобретение();
                    break;
                case DataTypes.НДС_Реализация:
                    Create_НДС_Реализация();
                    break;
                case DataTypes.СтоимостьСтроения:
                    Create_СтоимостьСтроения();
                    break;
                case DataTypes.Строение:
                    Create_Строение();
                    break;
                case DataTypes.НалоговыйАгент:
                    Create_НалоговыйАгент();
                    break;
                case DataTypes.Работник:
                    Create_Работник();
                    break;
                case DataTypes.Выплаты:
                    Create_Выплаты();
                    break;
                case DataTypes.Пособия:
                    Create_Пособия();
                    break;
                case DataTypes.Вычеты:
                    Create_Вычеты();
                    break;
                case DataTypes.Удержания:
                    Create_Удержания();
                    break;
                case DataTypes.RO_Зарплаты:
                    Create_Зарплаты();
                    CanEdit = false;
                    break;
                case DataTypes.RO_Премии:
                    Create_Премии();
                    CanEdit = false;
                    break;
                case DataTypes.ПодоходныйНалогПеречислено:
                    Create_ПодоходныйНалогПеречислено();
                    break;
                case DataTypes.СтраховойВзнос:
                    Create_СтраховойВзнос();
                    break;
                case DataTypes.ПенсионныйВзнос:
                    Create_ПенсионныйВзнос();
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

            SqlCommand comDel = new SqlCommand("UPDATE Выручка SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));


            ColumnPositions = new string[] { "Дата_записи", "Док_выручка_Наим", "Док_выручка_Номер", "Док_выручка_Дата", "Содержание_операции", "Выручка_от_реализации", "Внереализационные_доходы" };
            ColumnNames = new string[] { "Дата записи", "Наим докумета выручки", "Номер документа выручки", "Дата документа выручки", "Содержание операции", "Выручка от реализации", "Внереализационные доходы" };
            
        }
        void Create_Отгрузка()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Отгрузка", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Отгрузка_товаров SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

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

            SqlCommand comDel = new SqlCommand("UPDATE Отгрузка_товаров SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            _dataSet.Tables[0].Columns["Предоплачено"].DefaultValue = true;


            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Предоплачено"));
        }
        void Create_Кредитор()
        {
            _adapter = new SqlDataAdapter("Select * FROM Кредитор WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Кредитор SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));

            ColumnPositions = new string[] { "Название", "Номер_договора", "Дата_договора", "Предмет_договора", "Док_задолж_Наим", "Док_задолж_Номер", "Док_задолж_Дата", "Сумма_бр", "Наим_валюты", "Сумма_в_валюте" };
            ColumnNames = new string[] { "Кредитор", "Номер договора", "Дата договора", "Предмет договора", "Наим док задолж", "Номер док задолж", "Дата док задолж", "Сумма в бр", "Наим валюты", "Сумма в валюте" };
        }
        void Create_Дивиденты()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Дивиденты", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Налоги_на_дивиденты SET DEL = 1 WHERE ID = @ID1; UPDATE Платежный_документ SET DEL = 1 WHERE ID_платежный_док = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "ID_платежный_док");

            SqlCommand comUpd = new SqlCommand("upd_Дивиденты", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            comUpd.Parameters.Add("@Дата_Начисл", SqlDbType.DateTime, 8, "Дата начисления");
            comUpd.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comUpd.Parameters.Add("@Наим_орг", SqlDbType.VarChar, -1, "Наименование организации");
            comUpd.Parameters.Add("@Налог_база", SqlDbType.Money, sizeof(Decimal), "Налоговая база");
            comUpd.Parameters.Add("@Ставка_налог", SqlDbType.Int, 4, "Ставка налога");
            comUpd.Parameters.Add("@Номер_ПД", SqlDbType.VarChar, -1, "Номер плат инстр");
            comUpd.Parameters.Add("@ID_ПД", SqlDbType.Int, 4, "ID_платежный_док");
            comUpd.Parameters.Add("@Дата_ПД", SqlDbType.DateTime, 8, "Дата плат инстр");
            comUpd.Parameters.Add("@Сумма_ПД", SqlDbType.Money, sizeof(Decimal), "Перечислено налога");

            SqlCommand comIns = new SqlCommand("add_Дивиденты", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Дата_начисления", SqlDbType.DateTime, 8, "Дата начисления");
            comIns.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comIns.Parameters.Add("@Организация", SqlDbType.VarChar, -1, "Наименование организации");
            comIns.Parameters.Add("@Налоговая_база", SqlDbType.Money, sizeof(Decimal), "Налоговая база");
            comIns.Parameters.Add("@Ставка_налога", SqlDbType.Int, 4, "Ставка налога");
            comIns.Parameters.Add("@Номер_ПлатежныйДок", SqlDbType.VarChar, -1, "Номер плат инстр");
            comIns.Parameters.Add("@Дата_ПлатежныйДок", SqlDbType.DateTime, 8, "Дата плат инстр");
            comIns.Parameters.Add("@Сумма_ПлатежныйДок", SqlDbType.Money, sizeof(Decimal), "Перечислено налога");

            _adapter.UpdateCommand = comUpd;
            _adapter.InsertCommand = comIns;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_платежный_док"));

        }
        void Create_Кооператив()
        {
            _adapter = new SqlDataAdapter("Select * FROM Производственный_кооператив WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Производственный_кооператив SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));

            ColumnPositions = new string[] { "ФИО", "Размер_пая", "Размер_паевых_взносов", "Выплачена_стоимость_пая", "Выдано_иное_имущество", "Иные_выплаты_при_выходе_из_кооператива"};
            ColumnNames = new string[] { "ФИО", "Размер пая", "Размер паевых взносов", "Выплачена стоимость пая", "Выдано иное_ имущество", "Иные  выплаты при выходе из кооператива" };
        }
        void Create_НезавершенныеСтроения()
        {
            _adapter = new SqlDataAdapter("Select * FROM Незавершенное_строение WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Незавершенное_строение SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));

            ColumnPositions = new string[] { "Наименование", "Адрес", "Нормативный_срок", "Дата_на_которую_истек_нормат_срок", "Сумма_затрат", "Дата_акта_приемки" };
            ColumnNames = new string[] { "Наименование", "Адрес объекта", "Нормативный срок строительства", "Дата на которую истек нормативный срок", "Сумма затрат", "Дата утверждения акта приемки" };
        }
        void Create_ТоварыТС()
        {
            _adapter = new SqlDataAdapter("Select * FROM Товары_из_ТС WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Товары_из_ТС SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));

            ColumnPositions = new string[] { "Дата", "Серия_транспортного_документа", "Номер_транспортного_документа", "Дата_транспортного_документа", "Счет_факт_Номер", "Счет_факт_Дата", "Стоимость" };
            ColumnNames = new string[] { "Дата записи", "Серия ТТН", "Номер ТТН", "Дата ТТН", "Номер счета фактуры", "Дата счета фактуры", "Стоимость" };
        }
        void Create_НДС_Приобретение()
        {
            _adapter = new SqlDataAdapter("Select * FROM viev_НДС_Приобретение WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE НДС SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            _dataSet.Tables[0].Columns["Уплачено_при_приобретении"].DefaultValue = true;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Уплачено_при_приобретении"));
        }
        void Create_НДС_Реализация()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_НДС_Реализация WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE НДС SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            _dataSet.Tables[0].Columns["Уплачено_при_приобретении"].DefaultValue = false;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Уплачено_при_приобретении"));
        }
        void Create_СтоимостьСтроения()
        {
            _adapter = new SqlDataAdapter("Select * FROM Стоимость_строения WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Стоимость_строения SET DEL = 1 WHERE ID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_строение"));

            _dataSet.Tables[0].Columns.Add("Остаточная стоимость", typeof(Decimal));
            _dataSet.Tables[0].Columns["Остаточная стоимость"].Expression = "Первоначальная_стоимость - Сумма_армотизации";

            ColumnPositions = new string[] { "Период", "Площадь_всего", "Площадь_аренда", "Первоначальная_стоимость", "Сумма_армотизации" };
            ColumnNames = new string[] { "Период", "Общая площадь", "Площадь сданная в аренду", "Первоначальная стоимость", "Сумма накопленной армотизации" };
        }
        void Create_Строение()
        {
            _adapter = new SqlDataAdapter("Select * FROM Строение WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Строение SET DEL = 1 WHERE ID = @ID1; UPDATE Стоимость_строения SET DEL = 1 WHERE ID_строение = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;


        }
        void Create_НалоговыйАгент()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_НалоговыйАгент", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Налоговый_агент SET DEL = 1 WHERE ID = @ID1; UPDATE Платежный_документ SET DEL = 1 WHERE ID_платежный_док = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "ID_платежный_док");

            SqlCommand comUpd = new SqlCommand("upd_НалоговыйАгент", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            comUpd.Parameters.Add("@Наимен_организации", SqlDbType.VarChar, -1, "Наименование организации");
            comUpd.Parameters.Add("@Страна", SqlDbType.VarChar, -1, "Страна");
            comUpd.Parameters.Add("@Вид_дохода", SqlDbType.VarChar, -1, "Вид дохода");
            comUpd.Parameters.Add("@Дата_платежа", SqlDbType.DateTime, 8, "Дата начисления платежа");
            comUpd.Parameters.Add("@Сумма_платежа", SqlDbType.Money, sizeof(Decimal), "Сумма платежа");
            comUpd.Parameters.Add("@Сумма_затрат_для_налога", SqlDbType.Money, sizeof(Decimal), "Сумма затрат для исчисления налога");
            comUpd.Parameters.Add("@Сумма_освоб_от_налога_РБ", SqlDbType.Money, sizeof(Decimal), "Сумма дохода осв от налога по зак РБ");
            comUpd.Parameters.Add("@Сумма_освоб_от_налога_международн", SqlDbType.Money, sizeof(Decimal), "по международному договору");
            comUpd.Parameters.Add("@Ставка_налога_РБ", SqlDbType.SmallInt, 2, "Ставка налога по зак РБ");
            comUpd.Parameters.Add("@Ставка_налога_международн", SqlDbType.Int, 4, "ставка по международному договору");
            comUpd.Parameters.Add("@Сумма_ПлатежныйДок", SqlDbType.Money, sizeof(Decimal), "Перечислено");
            comUpd.Parameters.Add("@Дата_ПлатежныйДок", SqlDbType.DateTime, 8, "Дата плат инстр");
            comUpd.Parameters.Add("@Номер_ПлатежныйДок", SqlDbType.VarChar, -1, "Номер плат инстр");
            comUpd.Parameters.Add("@ID_платежный_док", SqlDbType.Int, 4, "ID_платежный_док");

            SqlCommand comIns = new SqlCommand("add_НалоговыйАгент", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Наимен_организации", SqlDbType.VarChar, -1, "Наименование организации");
            comIns.Parameters.Add("@Страна", SqlDbType.VarChar, -1, "Страна");
            comIns.Parameters.Add("@Вид_дохода", SqlDbType.VarChar, -1, "Вид дохода");
            comIns.Parameters.Add("@Дата_платежа", SqlDbType.DateTime, 8, "Дата начисления платежа");
            comIns.Parameters.Add("@Сумма_платежа", SqlDbType.Money, sizeof(Decimal), "Сумма платежа");
            comIns.Parameters.Add("@Сумма_затрат_для_налога", SqlDbType.Money, sizeof(Decimal), "Сумма затрат для исчисления налога");
            comIns.Parameters.Add("@Сумма_освоб_от_налога_РБ", SqlDbType.Money, sizeof(Decimal), "Сумма дохода осв от налога по зак РБ");
            comIns.Parameters.Add("@Сумма_освоб_от_налога_международн", SqlDbType.Money, sizeof(Decimal), "по международному договору");
            comIns.Parameters.Add("@Ставка_налога_РБ", SqlDbType.SmallInt, 2, "Ставка налога по зак РБ");
            comIns.Parameters.Add("@Ставка_налога_международн", SqlDbType.Int, 4, "ставка по международному договору");
            comIns.Parameters.Add("@Сумма_ПлатежныйДок", SqlDbType.Money, sizeof(Decimal), "Перечислено");
            comIns.Parameters.Add("@Дата_ПлатежныйДок", SqlDbType.DateTime, 8, "Дата плат инстр");
            comIns.Parameters.Add("@Номер_ПлатежныйДок", SqlDbType.VarChar, -1, "Номер плат инстр");

            _adapter.UpdateCommand = comUpd;
            _adapter.InsertCommand = comIns;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_платежный_док"));

        }
        void Create_Работник()
        {
            _adapter = new SqlDataAdapter("Select * FROM Работник WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);
            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Работник SET DEL = 1 WHERE работникID = @ID1; UPDATE Выплата_работнику SET DEL = 1 WHERE работникID = @ID1; UPDATE Налоговый_вычет_работника SET DEL = 1 WHERE работникID = @ID1; UPDATE Пособие_работника SET DEL = 1 WHERE работникID = @ID1; UPDATE Удержания SET DEL = 1 WHERE работникID = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "ID");
            _adapter.DeleteCommand = comDel;

        }
        void Create_Выплаты()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Выплаты", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Выплаты SET DEL = 1 WHERE Код_выплаты = @ID1; UPDATE Выплата_работнику SET DEL = 1 WHERE Код_выплаты = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "Код_выплаты");

            SqlCommand comUpd = new SqlCommand("upd_Выплата", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@Код_выплаты", SqlDbType.Int, 4, "Код_выплаты");
            comUpd.Parameters.Add("@Причина", SqlDbType.VarChar, -1, "Причина");
            comUpd.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comUpd.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comUpd.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");
            comUpd.Parameters.Add("@Начисляются_страх_взносы", SqlDbType.Bit, 1, "Начисляются страх взносы");
            comUpd.Parameters.Add("@Начисляются_пенс_взносы", SqlDbType.Bit, 1, "Начисляются пенс взносы");

            SqlCommand comIns = new SqlCommand("add_ВыплатаРаботнику", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Причина", SqlDbType.VarChar, -1, "Причина");
            comIns.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comIns.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comIns.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");
            comIns.Parameters.Add("@Начисляются_страх_взносы", SqlDbType.Bit, 1, "Начисляются страх взносы");
            comIns.Parameters.Add("@Начисляются_пенс_взносы", SqlDbType.Bit, 1, "Начисляются пенс взносы");
            comIns.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");

            _adapter.UpdateCommand = comUpd;
            _adapter.InsertCommand = comIns;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Код_выплаты"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL_Выплаты"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL_ВыплатыРаботнику"));

        }
        void Create_Пособия()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Пособие", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Пособия SET DEL = 1 WHERE Код_пособия = @ID1; UPDATE Пособие_работника SET DEL = 1 WHERE Код_пособия = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "Код_пособия");

            SqlCommand comUpd = new SqlCommand("upd_Пособие", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@Код_пособия", SqlDbType.Int, 4, "Код_пособия");
            comUpd.Parameters.Add("@Причина", SqlDbType.VarChar, -1, "Причина");
            comUpd.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comUpd.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comUpd.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");

            SqlCommand comIns = new SqlCommand("add_ПособиеРаботника", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Причина", SqlDbType.VarChar, -1, "Причина");
            comIns.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comIns.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comIns.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");
            comIns.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");

            _adapter.UpdateCommand = comUpd;
            _adapter.InsertCommand = comIns;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Код_пособия"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL_пособие"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEl_пособиеРаботника"));

        }
        void Create_Вычеты()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Вычет", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Налоговые_вычеты SET DEL = 1 WHERE Код_вычета = @ID1; UPDATE Налоговый_вычет_работника SET DEL = 1 WHERE Код_вычета = @ID1", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "Код_вычета");

            SqlCommand comUpd = new SqlCommand("upd_Вычет", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@Код_вычета", SqlDbType.Int, 4, "Код_вычета");
            comUpd.Parameters.Add("@Тип_вычета", SqlDbType.VarChar, -1, "Тип вычета");
            comUpd.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comUpd.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comUpd.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");

            SqlCommand comIns = new SqlCommand("add_ВычетРаботника", connect);
            comIns.CommandType = CommandType.StoredProcedure;
            comIns.Parameters.Add("@Тип_вычета", SqlDbType.VarChar, -1, "Тип вычета");
            comIns.Parameters.Add("@Сумма", SqlDbType.Money, sizeof(Decimal), "Сумма");
            comIns.Parameters.Add("@Период_Начало", SqlDbType.DateTime, 8, "Начало периода");
            comIns.Parameters.Add("@Период_Конец", SqlDbType.DateTime, 8, "Конец периода");
            comIns.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");

            _adapter.UpdateCommand = comUpd;
            _adapter.InsertCommand = comIns;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Код_вычета"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL_вычет"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL_вычетРаботника"));

        }
        void Create_Удержания()
        {
            _adapter = new SqlDataAdapter("Select * FROM Удержания WHERE DEL = 0", connect);
            new SqlCommandBuilder(_adapter);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Удержания SET DEL = 1 WHERE (работникID = @ID1) AND (Дата = @ID2)", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "работникID");
            comDel.Parameters.Add("@ID2", SqlDbType.DateTime, 8, "Дата");
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
        }
        void Create_Зарплаты()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_Зарплаты", connect);
            _adapter.Fill(_dataSet);
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
        }
        void Create_Премии()
        {
            _adapter = new SqlDataAdapter("Select * FROM Выручка", connect);
            _adapter.Fill(_dataSet);
        }
        void Create_ПодоходныйНалогПеречислено()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_ПодоходныйНалогПеречисл", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Подоходный_налог SET DEL = 1 WHERE (работникID = @ID1) AND (Дата = @ID3); UPDATE Платежный_документ SET DEL = 1 WHERE ID_платежный_док = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "работникID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "ID_платежный_док");
            comDel.Parameters.Add("@ID3", SqlDbType.DateTime, 8, "Месяц");

            SqlCommand comUpd = new SqlCommand("upd_ПодоходныйНалогПеречислено", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");
            comUpd.Parameters.Add("@Дата", SqlDbType.DateTime, 8, "Месяц");
            comUpd.Parameters.Add("@Начислено", SqlDbType.Money, sizeof(Decimal), "Начислено");
            comUpd.Parameters.Add("@Номер_ПД", SqlDbType.VarChar, -1, "Номер платежной инструкции");
            comUpd.Parameters.Add("@ID_ПД", SqlDbType.Int, 4, "ID_платежный_док");
            comUpd.Parameters.Add("@Дата_ПД", SqlDbType.DateTime, 8, "Дата");
            comUpd.Parameters.Add("@Сумма_ПД", SqlDbType.Money, sizeof(Decimal), "Перечислено");

            _adapter.UpdateCommand = comUpd;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("Expr1"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_платежный_док"));

        }
        void Create_СтраховойВзнос()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_СтрахВзнос", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Страховой_взнос SET DEL = 1 WHERE (работникID = @ID1) AND (Дата = @ID3); UPDATE Платежный_документ SET DEL = 1 WHERE ID_платежный_док = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "работникID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "ID_платежный_док");
            comDel.Parameters.Add("@ID3", SqlDbType.DateTime, 8, "Дата");

            SqlCommand comUpd = new SqlCommand("upd_СтраховойВзнос", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");
            comUpd.Parameters.Add("@Дата", SqlDbType.DateTime, 8, "Дата");
            comUpd.Parameters.Add("@Номер_ПД", SqlDbType.VarChar, -1, "Номер плат инстр");
            comUpd.Parameters.Add("@ID_ПД", SqlDbType.Int, 4, "ID_платежный_док");
            comUpd.Parameters.Add("@Дата_ПД", SqlDbType.DateTime, 8, "Дата плат инстр");
            comUpd.Parameters.Add("@Сумма_ПД", SqlDbType.Money, sizeof(Decimal), "Перечислено в Фонд");
            comUpd.Parameters.Add("@ЗадолжЗаПредПериод", SqlDbType.Money, sizeof(Decimal), "Остаток задолженности за пред период");
            comUpd.Parameters.Add("@Дней", SqlDbType.SmallInt, 2, "Количество рабочих дней");
            comUpd.Parameters.Add("@ЗаМесяц", SqlDbType.TinyInt, 1, "За месяц");
            comUpd.Parameters.Add("@ИныеПлатежи", SqlDbType.Money, sizeof(Decimal), "Иные платежи");
            comUpd.Parameters.Add("@ПеречисленоФондом", SqlDbType.Money, sizeof(Decimal), "Перечислено фондом плательщику");
            

            _adapter.UpdateCommand = comUpd;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_платежный_док"));

        }
        void Create_ПенсионныйВзнос()
        {
            _adapter = new SqlDataAdapter("Select * FROM view_ПенсВзносы", connect);

            _adapter.Fill(_dataSet);

            SqlCommand comDel = new SqlCommand("UPDATE Пенсионный_взнос SET DEL = 1 WHERE (работникID = @ID1) AND (Дата = @ID3); UPDATE Платежный_документ SET DEL = 1 WHERE ID_платежный_док = @ID2", connect);
            comDel.Parameters.Add("@ID1", SqlDbType.Int, 4, "работникID");
            comDel.Parameters.Add("@ID2", SqlDbType.Int, 4, "ID_платежный_док");
            comDel.Parameters.Add("@ID3", SqlDbType.DateTime, 8, "Дата");

            SqlCommand comUpd = new SqlCommand("upd_ПенсионныйВзнос", connect);
            comUpd.CommandType = CommandType.StoredProcedure;
            comUpd.Parameters.Add("@работникID", SqlDbType.Int, 4, "работникID");
            comUpd.Parameters.Add("@Дата", SqlDbType.DateTime, 8, "Дата");
            comUpd.Parameters.Add("@Номер_ПД", SqlDbType.VarChar, -1, "Номер плат инстр");
            comUpd.Parameters.Add("@ID_ПД", SqlDbType.Int, 4, "ID_платежный_док");
            comUpd.Parameters.Add("@Дата_ПД", SqlDbType.DateTime, 8, "Дата плат инстр");
            comUpd.Parameters.Add("@Сумма_ПД", SqlDbType.Money, sizeof(Decimal), "Перечислено в Фонд");
            comUpd.Parameters.Add("@ЗадолжЗаПредПериод", SqlDbType.Money, sizeof(Decimal), "Остаток задолженности за пред период");
            comUpd.Parameters.Add("@ИныеПлатежи", SqlDbType.Money, sizeof(Decimal), "Иные платежи");
            

            _adapter.UpdateCommand = comUpd;
            _adapter.DeleteCommand = comDel;

            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("DEL"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("работникID"));
            HiddenColumns.Add(_dataSet.Tables[0].Columns.IndexOf("ID_платежный_док"));

        }

        public enum DataTypes
        {
            Выручка,
            Отгрузка,
            Предоплата,
            Кредитор,
            Дивиденты,
            Кооператив,
            НезавершенныеСтроения,
            ТоварыТС,
            НДС_Приобретение,
            НДС_Реализация,
            СтоимостьСтроения,
            Строение,
            НалоговыйАгент,
            Работник,
            Выплаты,
            Пособия,
            Вычеты,
            Удержания,
            RO_Зарплаты,
            RO_Премии,
            ПодоходныйНалогПеречислено,
            СтраховойВзнос,
            ПенсионныйВзнос
        }

        public void Update()
        {
            try
            {
                _adapter.Update(_dataSet);
                _dataSet.Clear();
                _adapter.Fill(_dataSet);
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }


    }
}
