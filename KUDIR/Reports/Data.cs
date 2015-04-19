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
        
        public struct ВыручкаForPrint
        {
            public List<Reports.Выручка> list;
            public Decimal SumRealiz;
            public Decimal SumOthers;
            public Decimal SumRealizYear;
            public Decimal SumOthersYear;
        }
    }
}
