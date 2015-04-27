using KUDIR.Code;
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

namespace KUDIR.Forms
{
    /// <summary>
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }


        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            string path = GetPathForSave();
            if(path != null)
            {
                try
                {
                    DataBaseConfig.CreateCopyOfCurrentDB(path);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка создания резервной копии!\nПопробуйте перезапустить приложение.\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("Копия успешно создана!");
            }
        }

        string GetPathForSave()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "DB_backup " + DateTime.Now.ToString("dd-MM-yyyy");
            dlg.DefaultExt = ".mdf";
            dlg.Filter = "DataBase (.mdf)|*.mdf";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }
        string GetPathForLoad()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mdf";
            dlg.Filter = "DataBase (.mdf)|*.mdf";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }

        private void btnNewBase_Click(object sender, RoutedEventArgs e)
        {
            string path;
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                path = dialog.SelectedPath;
            }
            else
                path = null;
            if (path != null)
            {
                try
                {
                    DataBaseConfig.CreateNewDB(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка создания базы!\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("База успешно создана и подключена!!");
            }
        }

        private void btnOpenBase_Click(object sender, RoutedEventArgs e)
        {
            string path = GetPathForLoad();
            if (path != null)
            {
                try
                {
                    DataBaseConfig.ChangeDB(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка открытия базы!\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("База успешно подключена!");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                context.SubmitChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка!\n\n" + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (rbAll.IsChecked == true)
            {
                PrintTable(Filter.All);
                return;
            }
            if (rbDel.IsChecked == true)
            {
                PrintTable(Filter.Deleted);
                return;
            }
            if (rbEx.IsChecked == true)
            {
                PrintTable(Filter.Exist);
                return;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e) 
        {
            cbTables.ItemsSource = new List<string>() { "Не выбрана", "Выплаты работника", "Выплаты", "Выручка", "Кредитор", "Налоги на дивиденты", "Налоговые вычеты", "Налоговый агент", "Налоговый вычет работника", "НДС", "Незавершенное строение", "Отгрузка товаров", "Пенсионный взнос", "Платежные документы", "Подоходный налог", "Пособие работника", "Пособия", "Производственный кооператив", "Работник", "Стоимость строения", "Страховой взнос", "Строение", "Товары из ТС", "Удержания"};
        }

        Reports.DataClassesDataContext context = new Reports.DataClassesDataContext(DataBaseConfig.GetSqlConnectionString());
        void PrintTable(Filter filter)
        {
            bool deleted = false;
            if(filter == Filter.Deleted)
            {
                deleted = true;
            }
            try
            {

                switch ((string)cbTables.SelectedValue)
                {
                    case "Выплаты работника":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Выплата_работникуs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Выплата_работникуs select v;
                        break;
                    case "Выплаты":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Выплатыs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Выплатыs select v;
                        break;
                    case "Выручка":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Выручкаs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Выручкаs select v;
                        break;
                    case "Кредитор":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Кредиторs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Кредиторs select v;
                        break;
                    case "Налоги на дивиденты":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Налоги_на_дивидентыs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Налоги_на_дивидентыs select v;
                        break;
                    case "Налоговые вычеты":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Налоговые_вычетыs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Налоговые_вычетыs select v;
                        break;
                    case "Налоговый агент":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Налоговый_агентs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Налоговый_агентs select v;
                        break;
                    case "Налоговый вычет работника":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Налоговый_вычет_работникаs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Налоговый_вычет_работникаs select v;
                        break;
                    case "НДС":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.НДСs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.НДСs select v;
                        break;
                    case "Незавершенное строение":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Незавершенное_строениеs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Незавершенное_строениеs select v;
                        break;
                    case "Отгрузка товаров":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Отгрузка_товаровs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Отгрузка_товаровs select v;
                        break;
                    case "Пенсионный взнос":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Пенсионный_взносs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Пенсионный_взносs select v;
                        break;
                    case "Платежные документы":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Платежный_документs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Платежный_документs select v;
                        break;
                    case "Подоходный налог":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Подоходный_налогs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Подоходный_налогs select v;
                        break;
                    case "Пособие работника":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Пособие_работникаs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Пособие_работникаs select v;
                        break;
                    case "Пособия":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Пособияs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Пособияs select v;
                        break;
                    case "Производственный кооператив":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Производственный_кооперативs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Производственный_кооперативs select v;
                        break;
                    case "Работник":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Работникs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Работникs select v;
                        break;
                    case "Стоимость строения":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Стоимость_строенияs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Стоимость_строенияs select v;
                        break;
                    case "Страховой взнос":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Страховой_взносs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Страховой_взносs select v;
                        break;
                    case "Строение":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Строениеs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Строениеs select v;
                        break;
                    case "Товары из ТС":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Товары_из_ТСs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Товары_из_ТСs select v;
                        break;
                    case "Удержания":
                        if (filter != Filter.All)
                            grTable.ItemsSource = from v in context.Удержанияs where v.DEL == deleted select v;
                        else
                            grTable.ItemsSource = from v in context.Удержанияs select v;
                        break;
                    default:
                        grTable.ItemsSource = null;
                        break;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе!");
            }
        }

        enum Filter
        {
            All, Deleted, Exist
        }

        private void cbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rbAll.IsChecked == true)
            {
                PrintTable(Filter.All);
                return;
            }
            if(rbDel.IsChecked == true)
            {
                PrintTable(Filter.Deleted);
                return;
            }
            if(rbEx.IsChecked == true)
            {
                PrintTable(Filter.Exist);
                return;
            }
        }
            
    }
}
