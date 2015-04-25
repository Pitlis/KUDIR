using Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KUDIR.Code;
using System.Data;

namespace KUDIR.Forms
{
    /// <summary>
    /// Interaction logic for Отчеты.xaml
    /// </summary>
    public partial class Отчеты : Window
    {
        public Отчеты()
        {
            InitializeComponent();
        }
        public MainWindow mainMenu;
        public string strConnect;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (tab1.SelectedIndex)
                {
                    case 0:
                        chart1();
                        break;
                    case 1:
                        chart2();
                        break;
                    case 2:
                        chart3();
                        break;
                    case 3:
                        chart4();
                        break;
                    case 4:
                        chart5();
                        break;
                    case 5:
                        chart6();
                        break;
                    case 6:
                        chart7();
                        break;
                    case 7:
                        chart8();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        void chart1()
        {
            string fileName = null;
            switch (tabChart1.SelectedIndex)
            {
                case 0:
                    fileName = Report(dp1_start.SelectedDate, dp1_end.SelectedDate, "выручка");
                    break;
                case 1:
                    fileName = Report(dp2_start.SelectedDate, dp2_end.SelectedDate, "отгрузка");
                    break;
                case 2:
                    fileName = Report(dp3_start.SelectedDate, dp3_end.SelectedDate, "предоплата");
                    break;
                case 3:
                    fileName = Report(dp4_start.SelectedDate, dp4_end.SelectedDate, "кредитор");
                    break;
                default:
                    break;
            }
            OpenReport(fileName);
        }
        void chart2()
        {
            string fileName = null;
            switch (tabChart2.SelectedIndex)
            {
                case 0:
                    fileName = Report(dp5_start.SelectedDate, dp5_end.SelectedDate, "налоговыйАгент");
                    break;
                case 1:
                    fileName = Report(dp6_start.SelectedDate, dp6_end.SelectedDate, (cb1_empl.SelectedItem == null ? -1 : (int)cb1_empl.SelectedValue), "подоходный налог");
                    break;
                case 2:
                    fileName = Report(dp7_start.SelectedDate, dp7_end.SelectedDate, "перечисл подоходные налоги");
                    break;
                case 3:
                    fileName = Report(dp8_start.SelectedDate, dp8_end.SelectedDate, "дивиденты");
                    break;
                default:
                    break;
            }
            OpenReport(fileName);
        }
        void chart3()
        {
            string fileName = null;
            fileName = Report("кооператив");
            OpenReport(fileName);
        }
        void chart4()
        {
            string fileName = null;
            switch (tabChart4.SelectedIndex)
            {
                case 0:
                    fileName = Report(cb1_year.SelectedValue == null ? -1 : (int)cb1_year.SelectedValue, cb2_empl.SelectedValue == null ? -1 : (int)cb2_empl.SelectedValue, "страховой взнос");
                    break;
                case 1:
                    fileName = Report(cb2_year.SelectedValue == null ? -1 : (int)cb2_year.SelectedValue, "перечисл страх взносы");
                    break;
                case 2:
                    fileName = Report(cb3_year.SelectedValue == null ? -1 : (int)cb3_year.SelectedValue, cb3_empl.SelectedValue == null ? -1 : (int)cb3_empl.SelectedValue, "пенсионный взнос");
                    break;
                case 3:
                    fileName = Report(cb4_year.SelectedValue == null ? -1 : (int)cb4_year.SelectedValue, "перечисл пенс взносы");
                    break;
                default:
                    break;
            }
            OpenReport(fileName);
        }
        void chart5()
        {
            string fileName = null;
            fileName = Report(cb5_year.SelectedValue == null ? -1 : (int)cb5_year.SelectedValue, "расходы фонда");
            OpenReport(fileName);
        }
        void chart6()
        {
            string fileName = null;
            switch (tabChart5.SelectedIndex)
            {
                case 0:
                    fileName = Report((cb6_year.SelectedItem == null ? -1 : (int)cb6_year.SelectedValue), (cb1_build.SelectedItem == null ? -1 : (int)cb1_build.SelectedValue), "строение");
                    break;
                case 1:
                    fileName = Report("незавершенные строения");
                    break;
                default:
                    break;
            }
            OpenReport(fileName);
        }
        void chart7()
        {
            string fileName = null;
            fileName = Report(dp9_start.SelectedDate, dp9_end.SelectedDate, "товарыТС");
            OpenReport(fileName);
        }
        void chart8()
        {
            string fileName = null;
            switch (tabChart6.SelectedIndex)
            {
                case 0:
                    fileName = Report(dp10_start.SelectedDate, dp10_end.SelectedDate, "НДС при приобретении");
                    break;
                case 1:
                    fileName = Report(dp11_start.SelectedDate, dp11_end.SelectedDate, "НДС при реализации");
                    break;
                default:
                    break;
            }
            OpenReport(fileName);
        }

        string GetPathForSave(string name)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Отчет " + name + " " + DateTime.Now.ToString("dd-MM-yyyy");
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Excel (.xlsx)|*.xlsx";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }
        void OpenReport(string fileName)
        {
            if (fileName != null)
            {
                MessageBoxResult result = MessageBox.Show("Отчет успешно создан. Открыть?", "Отчет", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
        }

        #region Отчеты

        string Report(DateTime? startPeriod, DateTime? endPeriod, string reportType)
        {
            if (startPeriod == null || endPeriod == null)
                throw new Exception("Должны быть введены даты начала и конца периода!");
            string fileName = GetPathForSave(reportType);
            if (fileName != null)
            {
                PrintReport pr = new PrintReport(fileName, strConnect);
                try
                {
                    switch (reportType)
                    {
                        case "выручка":
                            pr.Выручка(startPeriod.Value, endPeriod.Value);
                            break;
                        case "отгрузка":
                            pr.Отгрузка(startPeriod.Value, endPeriod.Value);
                            break;
                        case "предоплата":
                            pr.Предоплата(startPeriod.Value, endPeriod.Value);
                            break;
                        case "кредитор":
                            pr.Кредитор(startPeriod.Value, endPeriod.Value);
                            break;
                        case "дивиденты":
                            pr.Дивиденты(startPeriod.Value, endPeriod.Value);
                            break;
                        case "налоговыйАгент":
                            pr.НалоговыйАгент(startPeriod.Value, endPeriod.Value);
                            break;
                        case "перечисл подоходные налоги":
                            pr.ПодоходныйНалогПеречислено(startPeriod.Value, endPeriod.Value);
                            break;
                        case "товарыТС":
                            pr.ТоварыТС(startPeriod.Value, endPeriod.Value);
                            break;
                        case "НДС при приобретении":
                            pr.НДСприобретение(startPeriod.Value, endPeriod.Value);
                            break;
                        case "НДС при реализации":
                            pr.НДСреализация(startPeriod.Value, endPeriod.Value);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при формировании отчета!");
                }
            }
            return fileName;
        }
        string Report(DateTime? startPeriod, DateTime? endPeriod, int emplID, string reportType)
        {
            if (startPeriod == null || endPeriod == null || emplID == -1)
                throw new Exception("Должны быть введены даты начала, конца периода и выбран работник!");
            string fileName = GetPathForSave(reportType);
            if (fileName != null)
            {
                PrintReport pr = new PrintReport(fileName, strConnect);
                try
                {
                    switch (reportType)
                    {
                        case "подоходный налог":
                            pr.ПодоходныйНалог(startPeriod.Value, endPeriod.Value, emplID);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при формировании отчета!");
                }
            }
            return fileName;
        }
        string Report(string reportType)
        {
            string fileName = GetPathForSave(reportType);
            if (fileName != null)
            {
                PrintReport pr = new PrintReport(fileName, strConnect);
                try
                {
                    switch (reportType)
                    {
                        case "кооператив":
                            pr.Кооператив();
                            break;
                        case "незавершенные строения":
                            pr.НезавершСтроение();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при формировании отчета!");
                }
            }
            return fileName;
        }
        string Report(int year, int emplID, string reportType)
        {
            if (year == -1 || emplID == -1)
                throw new Exception("Заполнены не все поля!");
            string fileName = GetPathForSave(reportType);
            if (fileName != null)
            {
                PrintReport pr = new PrintReport(fileName, strConnect);
                try
                {
                    switch (reportType)
                    {
                        case "страховой взнос":
                            pr.СтраховойВзнос(year, emplID);
                            break;
                        case "пенсионный взнос":
                            pr.ПенсионныйВзнос(year, emplID);
                            break;
                        case "строение":
                            pr.Строение(year, emplID);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при формировании отчета!");
                }
            }
            return fileName;
        }
        string Report(int year, string reportType)
        {
            if (year == -1)
                throw new Exception("Должен быть выбран год!");
            string fileName = GetPathForSave(reportType);
            if (fileName != null)
            {
                PrintReport pr = new PrintReport(fileName, strConnect);
                try
                {
                    switch (reportType)
                    {
                        case "перечисл страх взносы":
                            pr.СтраховыеВзносыПеречислено(year);
                            break;
                        case "перечисл пенс взносы":
                            pr.ПенсВзносыПеречислено(year);
                            break;
                        case "расходы фонда":
                            pr.РасходыФонда(year);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при формировании отчета!");
                }
            }
            return fileName;
        }

        #endregion

        
        private void tabChart2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabChart2.SelectedIndex == 1)
            {
                cb1_empl.ItemsSource = GetEmployees();
            }
            //cb1_empl.ItemsSource = 
        }
        private void tabChart4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabChart4.SelectedIndex == 0 || tabChart4.SelectedIndex == 2)
            {
                var empl = GetEmployees();
                cb2_empl.ItemsSource = empl;
                cb3_empl.ItemsSource = empl;
            }
        }
        private void tabChart5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabChart5.SelectedIndex == 0)
            {
                var build = GetBuilds();
                cb1_build.ItemsSource = build;
            }
        }

        Dictionary<string, int> GetEmployees()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Data data = new Data(Data.DataTypes.Работник, strConnect);
            for (int i = 0; i < data.Table.Rows.Count; i++)
            {
                int id = (int)data.Table.Rows[i]["работникID"];
                string name = data.Table.Rows[i]["ФИО"].ToString();
                try
                {
                    dict.Add(name, id);
                }
                catch { }
            }
            return dict;
        }
        Dictionary<string, int> GetBuilds()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Data data = new Data(Data.DataTypes.Строение, strConnect);
            for (int i = 0; i < data.Table.Rows.Count; i++)
            {
                int id = (int)data.Table.Rows[i]["ID"];
                string name = data.Table.Rows[i]["Наименование"].ToString();
                try
                {
                    dict.Add(name, id);
                }
                catch { }
            }
            return dict;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //глупое место
            List<int> years = new List<int>();
            for (int i = 2000; i <= DateTime.Now.Year + 1; ++i)
                years.Add(i);
            cb1_year.ItemsSource = years;
            cb2_year.ItemsSource = years;
            cb3_year.ItemsSource = years;
            cb4_year.ItemsSource = years;
            cb5_year.ItemsSource = years;
            cb6_year.ItemsSource = years;
            //----
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mainMenu != null)
            {
                mainMenu.reports = null;
                mainMenu.Show();
            }
        }


    }
}
