using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace KUDIR.Code
{
    public class PrintRepotrs
    {
        //static XLWorkbook GetCopyTemplate(string template, string fileName)
        //{
        //    XLWorkbook temp = new XLWorkbook(@"..\..\ReportTemplates\\" + template);
        //    temp.SaveAs(fileName);
        //    XLWorkbook wb = new XLWorkbook(fileName);
        //    return wb;
        //}
        //static IXLRow InsertRow(IXLRow rowBefore, int startColumn, int endColumn)
        //{
        //    IXLRow newRow = rowBefore.InsertRowsBelow(1).Last();
        //    newRow.Cells(startColumn, endColumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //    newRow.Cells(startColumn, endColumn).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        //    newRow.Cells(startColumn, endColumn).Style.Alignment.WrapText = true;
        //    newRow.Cells(startColumn, endColumn).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //    newRow.Cells(startColumn, endColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //    newRow.Cells(startColumn, endColumn).Style.Font.FontName = "Calibri";
        //    return newRow;
        //}


        //#region Отчеты

        //public static void Выручка(string fileName, DataTable table, Decimal ИтогоГод_Реализ, Decimal ИтогоГод_Внереализ)
        //{
        //    XLWorkbook wb = GetCopyTemplate("Выручка.xlsx", "D:\\Отчет.xlsx");
        //    IXLWorksheet ws = wb.Worksheet(1);
        //    IXLRow newRow = ws.Row(4);
        //    Decimal SumRealiz = 0;
        //    Decimal SumOthers = 0;
        //    for (int i = 0; i < table.Rows.Count; ++i)
        //    {
        //        newRow = InsertRow(newRow, 1, 6);
        //        try { newRow.Cell(1).Value = table.Rows[i]["Дата_записи"].CastTo<DateTime>().ToString("dd:MM:yyyy"); }
        //        catch { }
        //        string dateV = "";
        //        try
        //        {
        //            dateV = table.Rows[i]["Док_выручка_Дата"].CastTo<DateTime>().ToString("dd:MM:yyyy");
        //        }
        //        catch { }
        //        newRow.Cell(2).Value = table.Rows[i]["Док_выручка_Наим"].ToString() + ", " + table.Rows[i]["Док_выручка_Номер"].ToString() + ", " + dateV;
        //        newRow.Cell(3).Value = table.Rows[i]["Содержание_операции"].ToString();
        //        newRow.Cell(4).Value = table.Rows[i]["Выручка_от_реализации"] == DBNull.Value ? 0 : table.Rows[i]["Выручка_от_реализации"].CastTo<Decimal>();
        //        newRow.Cell(5).Value = table.Rows[i]["Внереализационные_доходы"] == DBNull.Value ? 0 : table.Rows[i]["Внереализационные_доходы"].CastTo<Decimal>();
        //        newRow.Cell(6).Value = "X";

        //        SumRealiz += table.Rows[i]["Выручка_от_реализации"] == DBNull.Value ? 0 : table.Rows[i]["Выручка_от_реализации"].CastTo<Decimal>();
        //        SumOthers += table.Rows[i]["Внереализационные_доходы"] == DBNull.Value ? 0 : table.Rows[i]["Внереализационные_доходы"].CastTo<Decimal>();
        //    }


        //    newRow.RowBelow(1).Cell(4).Value = SumRealiz;
        //    newRow.RowBelow(2).Cell(4).Value = ИтогоГод_Реализ;
        //    newRow.RowBelow(1).Cell(5).Value = SumOthers;
        //    newRow.RowBelow(2).Cell(5).Value = ИтогоГод_Внереализ;
        //    newRow.RowBelow(1).Cell(6).Value = SumRealiz + SumOthers;
        //    newRow.RowBelow(2).Cell(6).Value = ИтогоГод_Реализ + ИтогоГод_Внереализ;

        //    wb.Save();
        //}

        //public static void Отгрузка(string fileName, DataTable table)
        //{
        //    XLWorkbook wb = GetCopyTemplate("Отгрузка.xlsx", "D:\\Отчет.xlsx");
        //    IXLWorksheet ws = wb.Worksheet(1);
        //    IXLRow newRow = ws.Row(5);
            
        //    for (int i = 0; i < table.Rows.Count; ++i)
        //    {
        //        newRow = InsertRow(newRow, 1, 32);
        //        try { newRow.Cell(1).Value = table.Rows[i]["Дата отгрузки"].CastTo<DateTime>().ToString("dd:MM:yyyy"); }
        //        catch { continue; }
        //    }
        //}

        //static Dictionary<Отгрузка_общее, List<Отгрузка_оплата>> Get_Отгрузка(DataTable table)
        //{
        //    Dictionary<Отгрузка_общее, List<Отгрузка_оплата>> dict = new Dictionary<Отгрузка_общее, List<Отгрузка_оплата>>();
        //    List<Отгрузка_оплата> list = new List<Отгрузка_оплата>();
        //    for (int i = 0; i < table.Rows.Count; ++i)
        //    {
        //        Отгрузка_общее otgr = new Отгрузка_общее();
        //        Отгрузка_оплата opl = new Отгрузка_оплата();
        //        try
        //        {
        //            otgr.date = table.Rows[i]["Дата отгрузки"].CastTo<DateTime>().Date;
        //            otgr.docNumber = table.Rows[i]["Номер док отгрузки"].ToString();
        //            opl.date = table.Rows[i]["Дата оплаты"].CastTo<DateTime>().Date;
        //        }
        //        catch { continue; }
        //        otgr.User = table.Rows[i]["Лицо которому реализ товар"].ToString();
        //        otgr.docName = table.Rows[i]["Наим док отгрузки"].ToString();
        //        try
        //        {
        //            otgr.docDate = table.Rows[i]["Дата док отгрузки"].CastTo<DateTime>().Date;
        //        }
        //        catch { }
        //        otgr.sumBR = table.Rows[i]["Стоимость отгр товаров в бр"] == DBNull.Value ? 0 : table.Rows[i]["Стоимость отгр товаров в бр"].CastTo<Decimal>();
        //        otgr.currencyName = table.Rows[i]["Наим валюты"].ToString();
        //        otgr.sumCurrency = table.Rows[i]["Стоимость в валюте"] == DBNull.Value ? 0 : table.Rows[i]["Стоимость в валюте"].CastTo<Decimal>();

        //        opl.docDate = 
                

        //        foreach (Отгрузка key in dict.Keys)
        //        {
        //            if(key.date.Date.CompareTo() == 0)
        //        }
        //    }
        //}

        //#endregion

        #region ДопТипы

        //struct Отгрузка_общее
        //{
        //    public DateTime date;
        //    public string User;
        //    public string docName;
        //    public string docNumber;
        //    public DateTime docDate; 
        //    public Decimal sumBR;
        //    public string currencyName;
        //    public Decimal sumCurrency;
        //}
        //struct Отгрузка_оплата
        //{
        //    public DateTime docDate;
        //    public string docName;
        //    public string docNumber;
        //    public DateTime date;
        //}

        #endregion
    }
}
