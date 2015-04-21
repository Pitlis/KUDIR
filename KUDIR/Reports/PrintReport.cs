using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;

namespace Reports
{
    public class PrintReport
    {
        string _fileName { get; set; }

        public PrintReport(string fileName)
        {
            _fileName = fileName;
        }

        XLWorkbook GetCopyTemplate(string template)
        {
            XLWorkbook temp = new XLWorkbook(@"..\..\ReportTemplates\\" + template);
            temp.SaveAs(_fileName);
            XLWorkbook wb = new XLWorkbook(_fileName);
            return wb;
        }
        IXLRow InsertRow(IXLRow rowBefore, int startColumn, int endColumn, int fontSize)
        {
            IXLRow newRow = rowBefore.InsertRowsBelow(1).Last();
            newRow.Cells(startColumn, endColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            newRow.Cells(startColumn, endColumn).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            newRow.Cells(startColumn, endColumn).Style.Alignment.WrapText = true;
            newRow.Cells(startColumn, endColumn).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            newRow.Cells(startColumn, endColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            newRow.Cells(startColumn, endColumn).Style.Font.FontName = "Calibri";
            newRow.Cells(startColumn, endColumn).Style.Font.FontSize = fontSize;
            return newRow;
        }

        string DateToString(DateTime? date)
        {
            if (date == null)
                return "";
            return date.Value.ToString("dd/MM/yyyy");
        }

        int GetRowIndexOn_СтраховойВзнос(int month, int startIndex)
        {
            int index = startIndex;
            if (month > 3)
                index += 2;
            if (month > 6)
                index += 2;
            if (month > 9)
                index += 2;
            return index + month;
        }

        #region Отчеты

        public void Выручка(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Выручка.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            Data.ВыручкаForPrint records = new Data().Get_Выручка(startPeriod, endPeriod);

            foreach(Reports.Выручка record in records.list)
            {
                newRow = InsertRow(newRow, 1, 6, 10);
                newRow.Cell(1).Value = DateToString(record.Дата_записи);
                newRow.Cell(2).Value = record.Док_выручка_Наим + ", " + record.Док_выручка_Номер + ", " + DateToString(record.Док_выручка_Дата);
                newRow.Cell(3).Value = record.Содержание_операции;
                newRow.Cell(4).Value = record.Выручка_от_реализации;
                newRow.Cell(5).Value = record.Внереализационные_доходы;
                newRow.Cell(6).Value = "X";
            }

            newRow.RowBelow(1).Cell(4).Value = records.SumRealiz.HasValue ? records.SumRealiz : 0;
            newRow.RowBelow(2).Cell(4).Value = records.SumRealizYear.HasValue ? records.SumRealizYear : 0;
            newRow.RowBelow(1).Cell(5).Value = records.SumOthers.HasValue ? records.SumOthers : 0;
            newRow.RowBelow(2).Cell(5).Value = records.SumOthersYear.HasValue ? records.SumOthersYear : 0;
            newRow.RowBelow(1).Cell(6).Value = (records.SumRealiz.HasValue ? records.SumRealiz : 0) + (records.SumOthers.HasValue ? records.SumOthers : 0);
            newRow.RowBelow(2).Cell(6).Value = (records.SumRealizYear.HasValue ? records.SumRealizYear : 0) + (records.SumOthersYear.HasValue ? records.SumOthersYear : 0);

            wb.Save();
        }

        public void Отгрузка(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Отгрузка.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            Dictionary<Reports.Data.Отгрузка_Key, Reports.Data.Отгрузка_info> records = new Data().Get_Отгрузка(startPeriod, endPeriod);

            Decimal[] results = new Decimal[12];
            foreach(var record in records)
            {
                newRow = InsertRow(newRow, 1, 32, 8);
                newRow.Cell(1).Value = DateToString(record.Key.date);
                newRow.Cell(2).Value = record.Value.commonInfo.Лицо_которому_реализ_товар + ", " +
                    record.Value.commonInfo.Наим_док_отгрузки + ", " +
                    record.Key.docNumber + ", " +
                    DateToString(record.Value.commonInfo.Дата_док_отгрузки);
                newRow.Cell(3).Value = record.Value.commonInfo.Стоимость_отгр_товаров_в_бр;
                newRow.Cell(4).Value = record.Value.commonInfo.Наим_валюты;
                newRow.Cell(5).Value = record.Value.commonInfo.Стоимость_в_валюте;

                foreach(var payment in record.Value.платежи)
                {
                    if (!payment.Дата_оплаты.HasValue && !payment.Дата_док_оплаты.HasValue)
                        continue;

                    int month = 0;
                    if (payment.Дата_док_оплаты.HasValue)
                        month = payment.Дата_док_оплаты.Value.Date.Month;
                    if (payment.Дата_оплаты.HasValue)
                        month = payment.Дата_оплаты.Value.Date.Month;

                    newRow.Cell(4 + month * 2).Value = payment.Наим_док_оплаты + ", " +
                        DateToString(payment.Дата_док_оплаты) + ", " +
                        payment.Номер_док_оплаты + ", " +
                        DateToString(payment.Дата_оплаты);
                    newRow.Cell(5 + month * 2).Value = payment.Сумма;
                    results[month - 1] += payment.Сумма.HasValue ? payment.Сумма.Value : 0;
                }
            }
            for (int i = 0; i < 12; ++i)
            {
                newRow.RowBelow(1).Cell(7 + 2 * i).Value = results[i];
            }
            wb.Save();
        }
        public void Предоплата(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Предоплата.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            Dictionary<Reports.Data.Предоплата_Key, Reports.Data.Предоплата_info> records = new Data().Get_Предоплата(startPeriod, endPeriod);

            Decimal[] results = new Decimal[12];
            foreach (var record in records)
            {
                newRow = InsertRow(newRow, 1, 32, 8);
                newRow.Cell(1).Value = DateToString(record.Key.date);
                newRow.Cell(2).Value = record.Value.commonInfo.Лицо_которому_реализ_товар + ", " +
                    record.Value.commonInfo.Наим_док_оплаты + ", " +
                    record.Key.docNumber + ", " +
                    DateToString(record.Value.commonInfo.Дата_док_оплаты);
                newRow.Cell(3).Value = record.Value.commonInfo.Сумма_предоплаты;
                newRow.Cell(4).Value = record.Value.commonInfo.Наим_валюты;
                newRow.Cell(5).Value = record.Value.commonInfo.Сумма_в_валюте;

                foreach (var payment in record.Value.платежи)
                {
                    if (!payment.Дата_отгрузки.HasValue && !payment.Дата_док_отгрузки.HasValue)
                        continue;

                    int month = 0;
                    if (payment.Дата_док_отгрузки.HasValue)
                        month = payment.Дата_док_отгрузки.Value.Date.Month;
                    if (payment.Дата_отгрузки.HasValue)
                        month = payment.Дата_отгрузки.Value.Date.Month;

                    newRow.Cell(4 + month * 2).Value = payment.Наим_док_отгрузки + ", " +
                        DateToString(payment.Дата_док_отгрузки) + ", " +
                        payment.Номер_док_отгрузки + ", " +
                        DateToString(payment.Дата_отгрузки);
                    newRow.Cell(5 + month * 2).Value = payment.Сумма;
                    results[month - 1] += payment.Сумма.HasValue ? payment.Сумма.Value : 0;
                }
            }
            for (int i = 0; i < 12; ++i)
            {
                newRow.RowBelow(1).Cell(7 + 2 * i).Value = results[i];
            }
            wb.Save();
        }

        public void Кредитор(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Кредитор.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            List<Кредитор> records = new Data().Get_Кредитор(startPeriod, endPeriod);
            Decimal result = 0;
            foreach (Reports.Кредитор record in records)
            {
                newRow = InsertRow(newRow, 1, 6, 10);
                newRow.Cell(1).Value = record.Название;
                newRow.Cell(2).Value = record.Номер_договора + ", " + 
                    DateToString(record.Дата_договора) +  ", " + 
                    record.Предмет_договора;
                newRow.Cell(3).Value = record.Док_задолж_Наим + ", " +
                    record.Док_задолж_Номер + ", " +
                    DateToString(record.Док_задолж_Дата);
                newRow.Cell(4).Value = record.Сумма_бр;
                newRow.Cell(5).Value = record.Наим_валюты;
                newRow.Cell(6).Value = record.Сумма_в_валюте;

                result += record.Сумма_бр.HasValue ? record.Сумма_бр.Value : 0;
            }
            newRow.RowBelow(1).Cell(4).Value = result;

            wb.Save();
        }
        public void ПодоходныйНалог(DateTime startPeriod, DateTime endPeriod, int emplID)
        {
            XLWorkbook wb = GetCopyTemplate("ПодоходныйНалог.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(12);
            Reports.Data.РаботникForPrint empl = new Data().Get_ПодоходныйНалог(startPeriod, endPeriod, emplID);

            foreach(var record in empl.payments)
            {
                newRow = InsertRow(newRow, 1, 15, 8);
                newRow.Cell(1).Value = record.Key.ToString("MMMM");
                newRow.Cell(2).Value = record.Value.Зарплата;
                newRow.Cell(3).Value = record.Value.иныеВыплаты[0].summ;
                newRow.Cell(4).Value = record.Value.иныеВыплаты[1].summ;
                newRow.Cell(5).Value = record.Value.итогоМесяц;
                newRow.Cell(6).Value = record.Value.освобождаемыеДоходы;
                newRow.Cell(7).Value = record.Value.стандВычеты;
                newRow.Cell(8).Value = record.Value.соцВычеты;
                newRow.Cell(9).Value = record.Value.имущВычеты;
                newRow.Cell(10).Value = record.Value.профВычеты;
                newRow.Cell(11).Value = record.Value.налоговаяБаза;
                newRow.Cell(12).Value = record.Value.подоходНалог;
                newRow.Cell(13).Value = record.Value.иныеУдержания;
                newRow.Cell(14).Value = record.Value.итогоУдержано;
                newRow.Cell(15).Value = record.Value.кВыплате;
            }

            ws.Cell(2, 3).Value = empl.employee.ФИО;
            ws.Cell(2, 6).Value = "за " + startPeriod.Year + " год";
            ws.Cell(4, 3).Value = empl.employee.Документы_вычеты;
            ws.Cell(6, 3).Value = DateToString(empl.employee.Дата_договора) + ", " + empl.employee.Номер_договора;

            wb.Save();
        }
        public void ПодоходныйНалогПеречислено(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("ПодоходныйНалогПеречислено.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            List<Reports.Data.ПодоходныйНалогПеречислForPrint> list = new Data().Get_ПодоходныйНалогПеречисл(startPeriod, endPeriod);

            int i = 1;
            foreach (var record in list)
            {
                newRow = InsertRow(newRow, 1, 4, 10);
                newRow.Cell(1).Value = i;
                newRow.Cell(2).Value = record.date.ToString("MMMM");
                newRow.Cell(3).Value = record.Начислено;

                string платежныеИнстр = "";
                foreach(var plat in record.платежки)
                {
                    платежныеИнстр += plat.Перечислено;
                    платежныеИнстр += ", " + DateToString(plat.дата);
                    платежныеИнстр += ", " + plat.номер + ";   ";
                }
                newRow.Cell(4).Value = платежныеИнстр;
                i++;
            }
            wb.Save();
        }
        public void Дивиденты(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Дивиденты.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            List<view_Дивиденты> list = new Data().Get_Дивиденты(startPeriod, endPeriod);

            int i = 1;
            foreach (var record in list)
            {
                newRow = InsertRow(newRow, 1, 9, 8);
                newRow.Cell(1).Value = i;
                newRow.Cell(2).Value = record.Наименование_организации;
                newRow.Cell(3).Value = DateToString(record.Дата_начисления);
                newRow.Cell(4).Value = record.Сумма;
                newRow.Cell(5).Value = record.Налоговая_база;
                newRow.Cell(6).Value = record.Ставка_налога;
                newRow.Cell(7).Value = record.Сумма_налога;
                newRow.Cell(8).Value = DateToString(record.Дата_плат_инстр) + ", " +
                    record.Номер_плат_инстр;
                newRow.Cell(9).Value = record.Перечислено_налога;
                i++;
            }
            wb.Save();
        }
        public void НалоговыйАгент(DateTime startPeriod, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("НалоговыйАгент.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            List<view_НалоговыйАгент> list = new Data().Get_НалоговыйАгент(startPeriod, endPeriod);

            int i = 1;
            foreach (var record in list)
            {
                newRow = InsertRow(newRow, 1, 13, 8);
                newRow.Cell(1).Value = i;
                newRow.Cell(2).Value = record.Наименование_организации + ", " + record.Страна;
                newRow.Cell(3).Value = record.Вид_дохода;
                newRow.Cell(4).Value = DateToString(record.Дата_начисления_платежа);
                newRow.Cell(5).Value = record.Сумма_платежа;
                newRow.Cell(6).Value = record.Сумма_затрат_для_исчисления_налога;
                newRow.Cell(7).Value = record.Сумма_дохода_осв_от_налога_по_зак_РБ;
                newRow.Cell(8).Value = record.по_международному_договору;
                newRow.Cell(9).Value = record.облагаемый_доход;
                newRow.Cell(10).Value = record.Ставка_налога_по_зак_РБ;
                newRow.Cell(11).Value = record.ставка_по_международному_договору;
                newRow.Cell(12).Value = record.Подлежит_уплате;
                newRow.Cell(13).Value = record.Перечислено + ", " + DateToString(record.Дата_плат_инстр) + ", " +
                    record.Номер_плат_инстр;
                i++;
            }
            wb.Save();
        }
        public void Кооператив()
        {
            XLWorkbook wb = GetCopyTemplate("Кооператив.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(3);
            List<Производственный_кооператив> list = new Data().Get_Кооператив();

            int i = 1;
            foreach (var record in list)
            {
                newRow = InsertRow(newRow, 1, 7, 10);
                newRow.Cell(1).Value = i;
                newRow.Cell(2).Value = record.ФИО;
                newRow.Cell(3).Value = record.Размер_пая;
                newRow.Cell(4).Value = record.Размер_паевых_взносов;
                newRow.Cell(5).Value = record.Выплачена_стоимость_пая;
                newRow.Cell(6).Value = record.Выдано_иное_имущество;
                newRow.Cell(7).Value = record.Иные_выплаты_при_выходе_из_кооператива;
                i++;
            }
            wb.Save();
        }
        public void СтраховойВзнос(int year, int emplID)
        {
            XLWorkbook wb = GetCopyTemplate("СтраховойВзнос.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(10);
            List<Reports.Data.СтраховойВзносForPrint> list = new Data().Get_СтраховойВзнос(year, emplID);

            foreach (Reports.Data.СтраховойВзносForPrint record in list)
            {
                newRow = ws.Row(GetRowIndexOn_СтраховойВзнос(record.info.Дата.Month, 10));
                newRow.Cell(1).Value = record.info.Дата.ToString("MMMM");
                newRow.Cell(2).Value = record.выплаты[0];
                newRow.Cell(3).Value = record.выплаты[1];
                newRow.Cell(4).Value = record.выплаты[2];
                newRow.Cell(5).Value = record.выплаты[3];

                newRow.Cell(6).Value = record.info.Общая_сумма_выплат;
                newRow.Cell(7).Value = record.info.Сумма_на_которую_начисл_страх_взносы;
                newRow.Cell(8).Value = record.info.Сумма_начисл_страх_взносов_всего;
                newRow.Cell(9).Value = record.info.в_том_числе_1_процент;

                newRow.Cell(10).Value = record.пособия[0];
                newRow.Cell(11).Value = record.пособия[1];
                newRow.Cell(12).Value = record.пособия[2];
                newRow.Cell(13).Value = record.пособия[3];
                newRow.Cell(14).Value = record.пособия[4];
                newRow.Cell(15).Value = record.пособия[5];

                newRow.Cell(16).Value = record.info.За_месяц;
                newRow.Cell(17).Value = record.info.Количество_рабочих_дней;
                newRow.Cell(18).Value = record.количествоПособий;
            }

            Работник employee = new Data().Get_Работник(emplID);
            ws.Cell(2, 1).Value = employee.ФИО;
            ws.Cell(4, 1).Value = DateToString(employee.Дата_договора) + ", " + employee.Номер_договора + ", " + DateToString(employee.Дата_выплаты_вознаграждения);
            ws.Cell(4, 11).Value = employee.Инвалидность;

            wb.Save();
        }
        public void СтраховыеВзносыПеречислено(int year)
        {
            XLWorkbook wb = GetCopyTemplate("ПеречисленныйСтраховойВзнос.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            List<Reports.Data.СтрахВзносПеречислForPrint> list = new Data().Get_СтраховыеВзносыПеречислено(year);

            foreach (Reports.Data.СтрахВзносПеречислForPrint record in list)
            {
                newRow = ws.Row(GetRowIndexOn_СтраховойВзнос(record.info.Дата.Month, 4));
                newRow.Cell(1).Value = record.info.Дата.ToString("MMMM");
                newRow.Cell(2).Value = record.info.Общая_сумма_выплат;
                newRow.Cell(3).Value = record.info.Сумма_на_которую_начисл_страх_взносы;
                newRow.Cell(4).Value = record.info.Сумма_начисл_страх_взносов_всего;
                newRow.Cell(5).Value = record.info.в_том_числе_1_процент;
                newRow.Cell(6).Value = record.info.Иные_платежи;
                newRow.Cell(7).Value = record.info.Перечислено_фондом_плательщику;
                newRow.Cell(8).Value = record.info.Сумма_начисленных_пособий;
                newRow.Cell(9).Value = record.info.Остаток_задолженности_за_пред_период;
                newRow.Cell(10).Value = record.info.Подлежит_уплате;
                newRow.Cell(11).Value = record.info.Перечислено_в_Фонд;

                string платежныеИнстр = "";
                foreach (var plat in record.платежки)
                {
                    платежныеИнстр += DateToString(plat.дата);
                    платежныеИнстр += ", " + plat.номер + ";   ";
                }
                newRow.Cell(12).Value = платежныеИнстр;
            }

            wb.Save();
        }
        public void ПенсионныйВзнос(int year, int emplID)
        {
            XLWorkbook wb = GetCopyTemplate("ПенсионныйВзнос.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(6);
            List<Reports.Data.ПенсВзносыForPrint> list = new Data().Get_ПенсионныеВзносы(year, emplID);

            foreach(Reports.Data.ПенсВзносыForPrint record in list)
            {
                newRow = ws.Row(GetRowIndexOn_СтраховойВзнос(record.info.Дата.Month, 6));
                newRow.Cell(1).Value = record.info.Дата.ToString("MMMM");
                newRow.Cell(2).Value = record.выплаты[0];
                newRow.Cell(3).Value = record.выплаты[1];
                newRow.Cell(4).Value = record.выплаты[2];
                newRow.Cell(5).Value = record.выплаты[3];
                newRow.Cell(6).Value = record.итого;
                newRow.Cell(7).Value = record.info.Сумма_на_которую_начисл_пенс_взносы;
                newRow.Cell(6).Value = record.info.Сумма_начисленных_пенс_взносов;

            }
            Работник employee = new Data().Get_Работник(emplID);
            ws.Cell(2, 1).Value = employee.ФИО;
            ws.Cell(2, 8).Value = employee.Тариф_пенс_взносов;

            wb.Save();
        }
        public void ПенсВзносыПеречислено(int year)
        {
            XLWorkbook wb = GetCopyTemplate("ПенсионныйВзносПеречислено.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            List<Reports.Data.ПенсВзносПеречисленоForPrint> list = new Data().Get_ПенсВзносыПеречислено(year);

            foreach (Reports.Data.ПенсВзносПеречисленоForPrint record in list)
            {
                newRow = ws.Row(GetRowIndexOn_СтраховойВзнос(record.info.Дата.Month, 4));
                newRow.Cell(1).Value = record.info.Дата.ToString("MMMM");
                newRow.Cell(2).Value = record.info.Сумма_на_которую_начисл_пенс_взносы;
                newRow.Cell(3).Value = record.info.Сумма_начисленных_пенс_взносов;
                newRow.Cell(4).Value = record.info.Остаток_задолженности_за_пред_период;
                newRow.Cell(5).Value = record.info.Иные_платежи;
                newRow.Cell(6).Value = record.info.Подлежит_уплате;
                newRow.Cell(7).Value = record.info.Перечислено_в_Фонд;

                string платежныеИнстр = "";
                foreach (var plat in record.платежки)
                {
                    платежныеИнстр += DateToString(plat.дата);
                    платежныеИнстр += ", " + plat.номер + ";   ";
                }
                newRow.Cell(8).Value = платежныеИнстр;
            }

            wb.Save();
        }
        public void РасходыФонда(int year)
        {
            XLWorkbook wb = GetCopyTemplate("УчетРасходовФонда.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(10);
            List<Reports.Data.РасходыФондаForPrint> list = new Data().Get_РасходыФонда(year);

            foreach (Reports.Data.РасходыФондаForPrint record in list)
            {
                newRow = ws.Row(GetRowIndexOn_СтраховойВзнос(record.месяц, 4));
                newRow.Cell(1).Value = new DateTime(2015, record.месяц, 1).ToString("MMMM");
                newRow.Cell(2).Value = record.нетрудоспособности;
                newRow.Cell(3).Value = record.беременности;
                newRow.Cell(4).Value = record.рождение;
                newRow.Cell(5).Value = record.учет;
                newRow.Cell(6).Value = record.погребение;
                newRow.Cell(7).Value = record.уход;
                newRow.Cell(8).Value = record.категорииСемей;
                newRow.Cell(9).Value = record.инвалид;
            }
            wb.Save();
        }

        public void Строение(int year, int id)
        {
            XLWorkbook wb = GetCopyTemplate("УчетСтроений.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(14);
            Reports.Data.СтроениеForPrint build = new Data().Get_Строение(year, id);

            foreach (Стоимость_строения record in build.info)
            {
                newRow = ws.Row(14 + record.Период.Value.Month);
                newRow.Cell(2).Value = record.Площадь_всего;
                newRow.Cell(3).Value = record.Площадь_аренда;
                newRow.Cell(4).Value = record.Первоначальная_стоимость;
                newRow.Cell(5).Value = record.Сумма_армотизации;
            }
            ws.Cell(2, 1).Value = build.Name;
            ws.Cell(4, 1).Value = build.Priobr;
            ws.Cell(6, 1).Value = build.Reg;
            ws.Cell(8, 1).Value = build.Vvod;
            ws.Cell(10, 1).Value = build.Exit;

            if(build.type != -1)
            {
                ws.Cell(7+build.type, 6).Value = "X";
            }

            wb.Save();
        }

        #endregion
    }
}
