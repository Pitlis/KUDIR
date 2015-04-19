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
        IXLRow InsertRow(IXLRow rowBefore, int startColumn, int endColumn)
        {
            IXLRow newRow = rowBefore.InsertRowsBelow(1).Last();
            newRow.Cells(startColumn, endColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            newRow.Cells(startColumn, endColumn).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            newRow.Cells(startColumn, endColumn).Style.Alignment.WrapText = true;
            newRow.Cells(startColumn, endColumn).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            newRow.Cells(startColumn, endColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            newRow.Cells(startColumn, endColumn).Style.Font.FontName = "Calibri";
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
                newRow = InsertRow(newRow, 1, 6);
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

        #endregion
    }
}
