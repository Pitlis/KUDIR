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
                PrintReport pr = new PrintReport(fileName);
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
                PrintReport pr = new PrintReport(fileName);
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

        #endregion

        string strConnect = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR.mdf;Integrated Security=True;Connect Timeout=30";
        private void tabChart2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabChart2.SelectedIndex == 1)
            {
                cb1_empl.ItemsSource = GetEmployees();
            }
            //cb1_empl.ItemsSource = 
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
                catch{}
            }
            return dict;
        }
    }
}
