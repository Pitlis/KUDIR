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

            newRow.RowBelow(1).Cell(4).Value = records.SumRealiz;
            newRow.RowBelow(2).Cell(4).Value = records.SumRealizYear;
            newRow.RowBelow(1).Cell(5).Value = records.SumOthers;
            newRow.RowBelow(2).Cell(5).Value = records.SumOthersYear;
            newRow.RowBelow(1).Cell(6).Value = records.SumRealiz + records.SumOthers;
            newRow.RowBelow(2).Cell(6).Value = records.SumRealizYear + records.SumOthersYear;

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
        public void ПодоходныйНалогПеречислено(DateTime startPeriod, DateTime endPeriod, int emplID)
        {
            XLWorkbook wb = GetCopyTemplate("ПодоходныйНалогПеречислено.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            List<view_ПодоходныйНалогПеречисл> list = new Data().Get_ПодоходныйНалогПеречисл(startPeriod, endPeriod, emplID);

            int i = 1;
            foreach (var record in list)
            {
                newRow = InsertRow(newRow, 1, 4, 10);
                newRow.Cell(1).Value = i;
                newRow.Cell(2).Value = record.Месяц.ToString("MMMM");
                newRow.Cell(3).Value = record.Начислено;
                newRow.Cell(4).Value = record.Перечислено + ", " +
                    DateToString(record.Дата) + ", " +
                    record.Номер_платежной_инструкции;
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

        #endregion
    }
}
