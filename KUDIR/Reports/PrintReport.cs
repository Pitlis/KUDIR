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

        public void Выручка(DateTime startPerion, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Выручка.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            Data.ВыручкаForPrint records = new Data().Get_Выручка(startPerion, endPeriod);

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

        public void Отгрузка(DateTime startPerion, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Отгрузка.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            Dictionary<Reports.Data.Отгрузка_Key, Reports.Data.Отгрузка_info> records = new Data().Get_Отгрузка(startPerion, endPeriod);

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
        public void Предоплата(DateTime startPerion, DateTime endPeriod)
        {
            XLWorkbook wb = GetCopyTemplate("Предоплата.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(5);
            Dictionary<Reports.Data.Предоплата_Key, Reports.Data.Предоплата_info> records = new Data().Get_Предоплата(startPerion, endPeriod);

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

        #endregion
    }
}
