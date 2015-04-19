using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;

namespace KUDIR.Code
{
    public class PrintRepotrs
    {
        static XLWorkbook GetCopyTemplate(string template, string fileName)
        {
            XLWorkbook temp = new XLWorkbook(@"..\..\ReportTemplates\\" + template);
            temp.SaveAs(fileName);
            XLWorkbook wb = new XLWorkbook(fileName);
            return wb;
        }
        static IXLRow InsertRow(IXLRow rowBefore, int startColumn, int endColumn)
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


        #region Отчеты

        public static void Выручка(string fileName, DataTable table, Decimal ИтогоГод_Реализ, Decimal ИтогоГод_Внереализ)
        {
            XLWorkbook wb = GetCopyTemplate("Выручка.xlsx", "D:\\Отчет.xlsx");
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow newRow = ws.Row(4);
            Decimal SumRealiz = 0;
            Decimal SumOthers = 0;
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                newRow = InsertRow(newRow, 1, 6);
                try { newRow.Cell(1).Value = table.Rows[i]["Дата_записи"].CastTo<DateTime>().ToString("dd:MM:yyyy"); }
                catch { }
                string dateV = "";
                try
                {
                    dateV = table.Rows[i]["Док_выручка_Дата"].CastTo<DateTime>().ToString("dd:MM:yyyy");
                }
                catch { }
                newRow.Cell(2).Value = table.Rows[i]["Док_выручка_Наим"].ToString() + ", " + table.Rows[i]["Док_выручка_Номер"].ToString() + ", " + dateV;
                newRow.Cell(3).Value = table.Rows[i]["Содержание_операции"].ToString();
                newRow.Cell(4).Value = table.Rows[i]["Выручка_от_реализации"] == DBNull.Value ? 0 : table.Rows[i]["Выручка_от_реализации"].CastTo<Decimal>();
                newRow.Cell(5).Value = table.Rows[i]["Внереализационные_доходы"] == DBNull.Value ? 0 : table.Rows[i]["Внереализационные_доходы"].CastTo<Decimal>();
                newRow.Cell(6).Value = "X";

                SumRealiz += table.Rows[i]["Выручка_от_реализации"] == DBNull.Value ? 0 : table.Rows[i]["Выручка_от_реализации"].CastTo<Decimal>();
                SumOthers += table.Rows[i]["Внереализационные_доходы"] == DBNull.Value ? 0 : table.Rows[i]["Внереализационные_доходы"].CastTo<Decimal>();
            }


            newRow.RowBelow(1).Cell(4).Value = SumRealiz;
            newRow.RowBelow(2).Cell(4).Value = ИтогоГод_Реализ;
            newRow.RowBelow(1).Cell(5).Value = SumOthers;
            newRow.RowBelow(2).Cell(5).Value = ИтогоГод_Внереализ;
            newRow.RowBelow(1).Cell(6).Value = SumRealiz + SumOthers;
            newRow.RowBelow(2).Cell(6).Value = ИтогоГод_Реализ + ИтогоГод_Внереализ;

            wb.Save();
        }

        #endregion
    }
}
