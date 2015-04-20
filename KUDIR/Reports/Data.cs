using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    internal class Data
    {
        static string connStr = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR.mdf;Integrated Security=True;Connect Timeout=30";
        public Data(){}

        public ВыручкаForPrint Get_Выручка(DateTime start, DateTime end)
        {
            DataClassesDataContext context = new DataClassesDataContext(connStr);
            ВыручкаForPrint records = new ВыручкаForPrint();
            records.SumOthersYear = (from v in context.Выручкаs where v.Дата_записи.Value.Year == start.Year select v.Внереализационные_доходы.GetValueOrDefault()).Sum();
            records.SumRealizYear = (from v in context.Выручкаs where v.Дата_записи.Value.Year == start.Year select v.Выручка_от_реализации.GetValueOrDefault()).Sum();
            records.list = (from v in context.Выручкаs where v.Дата_записи.Value >= start && v.Дата_записи.Value <= end select v).ToList();
            records.SumOthers = (from v in records.list select v.Внереализационные_доходы.GetValueOrDefault()).Sum();
            records.SumRealiz = (from v in records.list select v.Выручка_от_реализации.GetValueOrDefault()).Sum();

            return records;
        }
        public Dictionary<Отгрузка_Key, Отгрузка_info> Get_Отгрузка(DateTime start, DateTime end)
        {
            DataClassesDataContext context = new DataClassesDataContext(connStr);
            view_Отгрузка records = new view_Отгрузка();
            Dictionary<Отгрузка_Key, Отгрузка_info> dict = new Dictionary<Отгрузка_Key, Отгрузка_info>();

            var query = from v in context.view_Отгрузкаs where v.Дата_отгрузки >= start && v.Дата_отгрузки <= end select v;

            foreach(view_Отгрузка record in query)
            {
                if(record.Дата_отгрузки.HasValue && record.Номер_док_отгрузки != null)
                {
                    Отгрузка_Key key = new Отгрузка_Key() { date = record.Дата_отгрузки.Value.Date, docNumber = record.Номер_док_отгрузки };
                    if(!dict.ContainsKey(key))
                    {
                        dict.Add(key, new Отгрузка_info() { commonInfo = record, платежи = new List<view_Отгрузка>() });
                    }
                }
            }

            foreach(view_Отгрузка record in query)
            {
                if(!record.Дата_отгрузки.HasValue || record.Номер_док_отгрузки == null)
                    continue;
                Отгрузка_Key key = new Отгрузка_Key() { date = record.Дата_отгрузки.Value.Date, docNumber = record.Номер_док_отгрузки };
                if(dict.ContainsKey(key))
                {
                    dict[key].платежи.Add(record);
                }
            }


            return dict;
        }


        #region Дополнительные типы

        public struct ВыручкаForPrint
        {
            public List<Reports.Выручка> list;
            public Decimal SumRealiz;
            public Decimal SumOthers;
            public Decimal SumRealizYear;
            public Decimal SumOthersYear;
        }

        public struct Отгрузка_Key
        {
            public DateTime date;
            public string docNumber;
        }
        public struct Отгрузка_info
        {
            public view_Отгрузка commonInfo;
            public List<view_Отгрузка> платежи;
        }

        #endregion
    }
}
