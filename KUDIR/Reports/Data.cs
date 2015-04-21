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
        DataClassesDataContext context;
        const int ПодоходныйНалогПроцент = 13; //нужно дергать из конфига
        public Data()
        {
            context = new DataClassesDataContext(connStr);
        }

        public ВыручкаForPrint Get_Выручка(DateTime start, DateTime end)
        {
            ВыручкаForPrint records = new ВыручкаForPrint();
            records.SumOthersYear = (from v in context.Выручкаs where v.Дата_записи.Value.Year == start.Year select v.Внереализационные_доходы.GetValueOrDefault()).Sum();
            records.SumRealizYear = (from v in context.Выручкаs where v.Дата_записи.Value.Year == start.Year select v.Выручка_от_реализации.GetValueOrDefault()).Sum();
            records.list = (from v in context.Выручкаs where v.Дата_записи.Value >= start && v.Дата_записи.Value <= end orderby v.Дата_записи select v).ToList();
            records.SumOthers = (from v in records.list select v.Внереализационные_доходы.GetValueOrDefault()).Sum();
            records.SumRealiz = (from v in records.list select v.Выручка_от_реализации.GetValueOrDefault()).Sum();

            return records;
        }
        public Dictionary<Отгрузка_Key, Отгрузка_info> Get_Отгрузка(DateTime start, DateTime end)
        {
            view_Отгрузка records = new view_Отгрузка();
            Dictionary<Отгрузка_Key, Отгрузка_info> dict = new Dictionary<Отгрузка_Key, Отгрузка_info>();

            var query = from v in context.view_Отгрузкаs where v.Дата_отгрузки >= start && v.Дата_отгрузки <= end orderby v.Дата_отгрузки select v;

            foreach (view_Отгрузка record in query)
            {
                if (record.Дата_отгрузки.HasValue && record.Номер_док_отгрузки != null)
                {
                    Отгрузка_Key key = new Отгрузка_Key() { date = record.Дата_отгрузки.Value.Date, docNumber = record.Номер_док_отгрузки };
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, new Отгрузка_info() { commonInfo = record, платежи = new List<view_Отгрузка>() });
                    }
                }
            }

            foreach (view_Отгрузка record in query)
            {
                if (!record.Дата_отгрузки.HasValue || record.Номер_док_отгрузки == null)
                    continue;
                Отгрузка_Key key = new Отгрузка_Key() { date = record.Дата_отгрузки.Value.Date, docNumber = record.Номер_док_отгрузки };
                if (dict.ContainsKey(key))
                {
                    dict[key].платежи.Add(record);
                }
            }
            return dict;
        }
        public Dictionary<Предоплата_Key, Предоплата_info> Get_Предоплата(DateTime start, DateTime end)
        {
            view_Предоплата records = new view_Предоплата();
            Dictionary<Предоплата_Key, Предоплата_info> dict = new Dictionary<Предоплата_Key, Предоплата_info>();

            var query = from v in context.view_Предоплатаs where v.Дата_предоплаты >= start && v.Дата_предоплаты <= end orderby v.Дата_предоплаты select v;

            foreach (view_Предоплата record in query)
            {
                if (record.Дата_предоплаты.HasValue && record.Номер_док_оплаты != null)
                {
                    Предоплата_Key key = new Предоплата_Key() { date = record.Дата_предоплаты.Value.Date, docNumber = record.Номер_док_оплаты };
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, new Предоплата_info() { commonInfo = record, платежи = new List<view_Предоплата>() });
                    }
                }
            }

            foreach (view_Предоплата record in query)
            {
                if (!record.Дата_предоплаты.HasValue || record.Номер_док_оплаты == null)
                    continue;
                Предоплата_Key key = new Предоплата_Key() { date = record.Дата_предоплаты.Value.Date, docNumber = record.Номер_док_оплаты };
                if (dict.ContainsKey(key))
                {
                    dict[key].платежи.Add(record);
                }
            }
            return dict;
        }
        public List<Кредитор> Get_Кредитор(DateTime start, DateTime end)
        {
            return (from k in context.Кредиторs where k.Док_задолж_Дата.Value >= start && k.Док_задолж_Дата.Value <= end orderby k.Док_задолж_Дата select k).ToList<Кредитор>();
        }
        public РаботникForPrint Get_ПодоходныйНалог(DateTime start, DateTime end, int emplID)
        {
            РаботникForPrint emplInfo = new РаботникForPrint();
            emplInfo.employee = (from e in context.Работникs where e.работникID == emplID select e).First();
            emplInfo.payments = new Dictionary<DateTime, ПодоходныйНалог_Info>();

            for (DateTime date = start; date < end; date = date.AddMonths(1))
            {
                ПодоходныйНалог_Info info = new ПодоходныйНалог_Info();
                
                info.Зарплата = (from v in context.view_Зарплатыs
                                 where v.работникID == emplID &&
                                     v.Начало_периода <= date && v.Конец_периода >= date &&
                                     v.Зарплата_сумма.HasValue
                                 select v.Зарплата_сумма).Sum();

                info.иныеВыплаты = new ИныеВыплаты[2];
                var query = from v in context.view_Выплатыs
                            where v.работникID == emplID &&
                                v.Начало_периода <= date && v.Конец_периода >= date &&
                                v.Сумма.HasValue && v.Причина != null
                            select v;
                int i = 0;
                foreach (var v in query)
                {
                    if (i == 2)
                        break;
                    if(v.Причина.IndexOf("зарплат", StringComparison.InvariantCultureIgnoreCase) < 0)
                    {
                        info.иныеВыплаты[i].Name = v.Причина;
                        info.иныеВыплаты[i].summ = v.Сумма.Value;
                        i++;
                    }
                }

                info.итогоМесяц = (info.Зарплата.HasValue ? info.Зарплата.Value : 0) + 
                    (info.иныеВыплаты[0].summ.HasValue ? info.иныеВыплаты[0].summ.Value : 0) + 
                    (info.иныеВыплаты[1].summ.HasValue ? info.иныеВыплаты[1].summ.Value : 0);
                 
                Decimal? v1 = 0;
                Decimal? v2 = 0;
                Decimal? v3 = 0;
                Decimal? v4 = 0;
                Decimal? v5 = 0;
                context.СуммыВычетов(emplID, date, ref v1, ref v2, ref v3, ref v4, ref v5);
                info.стандВычеты = v1;
                info.соцВычеты = v2;
                info.имущВычеты = v3;
                info.профВычеты = v4;
                info.освобождаемыеДоходы = v5;

                info.налоговаяБаза = info.итогоМесяц - 
                    (info.освобождаемыеДоходы.HasValue ? info.освобождаемыеДоходы.Value : 0) -
                    (info.профВычеты.HasValue ? info.профВычеты.Value : 0) - 
                    (info.соцВычеты.HasValue ? info.соцВычеты.Value : 0) - 
                    (info.стандВычеты.HasValue ? info.стандВычеты.Value : 0) - 
                    (info.имущВычеты.HasValue ? info.имущВычеты.Value : 0);

                info.подоходНалог = info.налоговаяБаза * (decimal)((float)ПодоходныйНалогПроцент / 100);


                info.иныеУдержания = (from v in context.Удержанияs
                                      where v.работникID == emplID &&
                                          v.Дата.Year == date.Year && v.Дата.Month == date.Month &&
                                          v.Сумма.HasValue
                                      select v.Сумма).Sum();
                info.итогоУдержано = info.иныеУдержания.HasValue ? info.иныеУдержания.Value : 0 + info.подоходНалог;
                info.кВыплате = info.итогоМесяц - info.итогоУдержано;
                emplInfo.payments.Add(date, info);
            }

            return emplInfo;
        }
        public List<ПодоходныйНалогПеречислForPrint> Get_ПодоходныйНалогПеречисл(DateTime start, DateTime end)
        {
            List<ПодоходныйНалогПеречислForPrint> list = new List<ПодоходныйНалогПеречислForPrint>();
            var query = from v in context.view_ПодоходныйНалогПеречислs 
                   where v.Месяц >= start && v.Месяц <= end && v.Начислено.HasValue
                    orderby v.Месяц
                    select v;

            foreach(var record in query)
            {
                ПодоходныйНалогПеречислForPrint p = new ПодоходныйНалогПеречислForPrint();
                p.date = record.Месяц;
                p.Начислено = record.Начислено.Value;
                p.платежки = new List<ПлатежнаяИнструкция>();
                if(record.Номер_платежной_инструкции != null && record.Перечислено.HasValue)
                {
                    ПлатежнаяИнструкция pInstr = new ПлатежнаяИнструкция();
                    pInstr.Перечислено = record.Перечислено.Value;
                    pInstr.дата = record.Дата;
                    pInstr.номер = record.Номер_платежной_инструкции;
                    p.платежки.Add(pInstr);
                }
                list.Add(p);
            }
            return list;
        }
        public List<view_Дивиденты> Get_Дивиденты(DateTime start, DateTime end)
        {
            return (from v in context.view_Дивидентыs
                   where v.Дата_начисления >= start && v.Дата_начисления <= end &&
                       v.Сумма.HasValue && v.Налоговая_база.HasValue && v.Ставка_налога.HasValue
                   orderby v.Дата_начисления
                   select v).ToList<view_Дивиденты>();
        }
        public List<view_НалоговыйАгент> Get_НалоговыйАгент(DateTime start, DateTime end)
        {
            return (from v in context.view_НалоговыйАгентs
                    where v.Дата_начисления_платежа >= start && v.Дата_начисления_платежа <= end &&
                    v.Наименование_организации != null && v.Сумма_платежа != null
                    orderby v.Наименование_организации
                    select v).ToList<view_НалоговыйАгент>();
        }
        public List<Производственный_кооператив> Get_Кооператив()
        {
            return (from v in context.Производственный_кооперативs
                        where v.ФИО != null
                        select v).ToList<Производственный_кооператив>();
        }
        public List<СтраховойВзносForPrint> Get_СтраховойВзнос(int year, int emplID)
        {
            List<СтраховойВзносForPrint> list = new List<СтраховойВзносForPrint>();
            var query = from v in context.view_СтрахВзносs where v.работникID == emplID && v.Дата.Year == year select v;
            foreach(var record in query)
            {
                СтраховойВзносForPrint inclusion = new СтраховойВзносForPrint();
                inclusion.info = record;
                Decimal? temp = 0;
                inclusion.пособия = new decimal?[6];
                inclusion.выплаты = new decimal?[4];
                context.СтандартныеПособия(emplID, record.Дата, ref inclusion.пособия[0], ref inclusion.пособия[1], ref inclusion.пособия[2], ref inclusion.пособия[3], ref inclusion.пособия[5], ref inclusion.пособия[4], ref temp, ref temp, ref temp);
                inclusion.количествоПособий = 0;
                foreach(var p in inclusion.пособия)
                {
                    if (p.HasValue)
                        inclusion.количествоПособий++;
                }

                inclusion.выплаты[0] = (from v in context.view_Зарплатыs where v.работникID == emplID && v.Начало_периода <= record.Дата && v.Конец_периода >= record.Дата && v.Зарплата_сумма.HasValue select v.Зарплата_сумма).Sum();

                var выплаты = from v in context.view_Выплатыs
                            where v.работникID == emplID &&
                                v.Начало_периода <= record.Дата && v.Конец_периода >= record.Дата &&
                                v.Сумма.HasValue && v.Причина != null
                            select v;
                int i = 1;
                foreach (var v in выплаты)
                {
                    if (i == 3)
                        break;
                    if (v.Причина.IndexOf("зарплат", StringComparison.InvariantCultureIgnoreCase) < 0)
                    {
                        inclusion.выплаты[i] = v.Сумма.Value;
                        i++;
                    }
                }
                list.Add(inclusion);
            }
            return list;
        }
        public Работник Get_Работник(int emplID)
        {
            return (from v in context.Работникs where v.работникID == emplID select v).First();
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

        public struct Предоплата_Key
        {
            public DateTime date;
            public string docNumber;
        }
        public struct Предоплата_info
        {
            public view_Предоплата commonInfo;
            public List<view_Предоплата> платежи;
        }

        public struct РаботникForPrint
        {
            public Работник employee;
            public Dictionary<DateTime, ПодоходныйНалог_Info> payments;
        }
        public struct ПодоходныйНалог_Info
        {
            public Decimal? Зарплата;
            public ИныеВыплаты[] иныеВыплаты;
            public Decimal итогоМесяц;
            public Decimal? освобождаемыеДоходы;
            public Decimal? стандВычеты;
            public Decimal? соцВычеты;
            public Decimal? имущВычеты;
            public Decimal? профВычеты;
            public Decimal налоговаяБаза;
            public Decimal подоходНалог;
            public Decimal? иныеУдержания;
            public Decimal итогоУдержано;
            public Decimal кВыплате;
        }
        public struct ИныеВыплаты
        {
            public Decimal? summ;
            public string Name;
        }

        public struct СтраховойВзносForPrint
        {
            public view_СтрахВзнос info;
            public Decimal?[] пособия;
            public Decimal?[] выплаты;
            public int количествоПособий;
            public Работник employee;
        }

        public struct ПодоходныйНалогПеречислForPrint
        {
            public DateTime date;
            public Decimal Начислено;
            public List<ПлатежнаяИнструкция> платежки;
        }
        public struct ПлатежнаяИнструкция
        {
            public Decimal? Перечислено;
            public DateTime? дата;
            public string номер;
        }

        #endregion
    }
}
