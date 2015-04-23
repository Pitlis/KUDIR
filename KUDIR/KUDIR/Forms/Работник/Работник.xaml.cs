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
    /// Interaction logic for Работник.xaml
    /// </summary>
    public partial class Работник : Window
    {
        string strConnect = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR.mdf;Integrated Security=True;Connect Timeout=30";
        Data dataEmpl;
        Data dataEmplInfo;
        Data_Работник employee;
        int currentEmpl = -2;
        DataGridConfig grid;

        public Работник()
        {
            InitializeComponent();
        }

        void FillForm(Data_Работник empl)
        {
            tbxName.Text = empl.ФИО;
            tbxNumber.Text = empl.Номер_договора;
            tbxInv.Text = empl.Инвалидность;
            tbxTarif.Text = empl.Тариф_пенс_взносов.ToString();
            tbxTarifStrah.Text = empl.Тариф_страх_взносов.ToString();
            tbxDogovor.Text = empl.Условия_договора;
            dpDateDog.SelectedDate = empl.Дата_договора;
            dpDateVozn.SelectedDate = empl.Дата_выплаты_вознаграждения;
            tbxDocV.Text = empl.Документы_вычеты;
        }
        void LoadFromForm(Data_Работник empl)
        {
            int tarif = 0;
            if (tbxTarif.Text != null && !Int32.TryParse(tbxTarif.Text, out tarif))
            {
                empl.Тариф_пенс_взносов = null;
            }
            int tarif1 = 0;
            if (tbxTarifStrah.Text != null && !Int32.TryParse(tbxTarifStrah.Text, out tarif1))
            {
                empl.Тариф_страх_взносов = null;
            }
            empl.Тариф_пенс_взносов = (short)tarif;
            empl.Тариф_пенс_взносов = (short)tarif1;
            empl.ФИО = tbxName.Text;
            empl.Номер_договора = tbxNumber.Text;
            empl.Инвалидность = tbxInv.Text;
            empl.Условия_договора = tbxDogovor.Text;
            empl.Дата_договора = dpDateDog.SelectedDate;
            empl.Дата_выплаты_вознаграждения = dpDateVozn.SelectedDate;
            empl.Документы_вычеты = tbxDocV.Text;
        }

        void InitForm()
        {
            dataEmpl = new Data(Data.DataTypes.Работник, strConnect);
            dataEmplInfo = new Data(Data.DataTypes.Выплаты, strConnect);
            currentTypeInfo = Data.DataTypes.Выплаты;
            grid = new DataGridConfig(dgTable1);
            if (dataEmpl.Table.Rows.Count == 0)
            {
                employee = new Data_Работник(dataEmpl);
                currentEmpl = -1;
            }
            else
            {
                employee = new Data_Работник(dataEmpl, 0);
                currentEmpl = 0;
                FillForm(employee);
                grid.ShowData(dataEmplInfo, "работникID = " + employee.работникID, "работникID");
                dataEmplInfo.Table.Columns["работникID"].DefaultValue = employee.работникID;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            dgTable1.CanUserAddRows = true;
            if (currentEmpl + 1 < dataEmpl.Table.Rows.Count && currentEmpl != -1)//Следующая запись есть
            {
                currentEmpl++;
                employee = new Data_Работник(dataEmpl, currentEmpl);
                FillForm(employee);
                currentTypeInfo = Data.DataTypes.Выплаты;
                ReloadGridsInfo();
                tabControl.SelectedIndex = 0;
                return;
            }
            if (currentEmpl + 1 == dataEmpl.Table.Rows.Count && currentEmpl != -1)//Это была последняя запись
            {
                currentEmpl = 0;
                employee = new Data_Работник(dataEmpl, 0);
                FillForm(employee);
                currentTypeInfo = Data.DataTypes.Выплаты;
                ReloadGridsInfo();
                tabControl.SelectedIndex = 0;
                return;
            }
            if (currentEmpl == -1 && dataEmpl.Table.Rows.Count > 0)//Переход со страницы новой записи
            {
                currentEmpl = 0;
                employee = new Data_Работник(dataEmpl, 0);
                FillForm(employee);
                currentTypeInfo = Data.DataTypes.Выплаты;
                ReloadGridsInfo();
                tabControl.SelectedIndex = 0;
                return;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            employee = new Data_Работник(dataEmpl);
            currentTypeInfo = Data.DataTypes.Выплаты;
            ReloadGridsInfo();
            tabControl.SelectedIndex = 0;
            currentEmpl = -1;
            FillForm(employee);
            dgTable1.CanUserAddRows = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            employee.Delete();
            InitForm();
            tabControl.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbxName.Text == null || tbxNumber.Text == null)
            {
                MessageBox.Show("Требуется ФИО и номер договора!");
                return;
            }
            LoadFromForm(employee);
            try
            {
                employee.Update();
                dataEmplInfo.Update();
                ReloadGridsInfo();
                if (currentEmpl == -1 && dataEmpl.Table.Rows.Count > 0)
                {
                    currentEmpl = dataEmpl.Table.Rows.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        Data.DataTypes currentTypeInfo;
        void ReloadGridsInfo()//грид обновляется при каждом переключении между работниками, и при создании новой записи
        {
            if (currentTypeInfo == Data.DataTypes.ПодоходныйНалогПеречислено || currentTypeInfo == Data.DataTypes.СтраховойВзнос || currentTypeInfo == Data.DataTypes.ПенсионныйВзнос)
            {
                dataEmplInfo = new DataPartialTables(currentTypeInfo, strConnect);
            }
            else
            {
                dataEmplInfo = new Data(currentTypeInfo, strConnect);
            }
            grid = new DataGridConfig(dgTable1);
            grid.ShowData(dataEmplInfo, "работникID = " + employee.работникID, "работникID");
            dataEmplInfo.Table.Columns["работникID"].DefaultValue = employee.работникID;
        }

        bool FormLoaded = false;
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataEmplInfo != null && dataEmplInfo.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Все несохраненные изменения будут потеряны.\nДля сохранения нажмите \"Нет\" и кнопку \"Сохранить\"\n Продолжить без сохранения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            dgTable1.CanUserAddRows = true;
            dgTable1.Margin = new Thickness(0, 23, 0, 0);
            if (FormLoaded)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        currentTypeInfo = Data.DataTypes.Выплаты;
                        ReloadGridsInfo();
                        break;
                    case 1:
                        currentTypeInfo = Data.DataTypes.Пособия;
                        ReloadGridsInfo();
                        break;
                    case 2:
                        currentTypeInfo = Data.DataTypes.Вычеты;
                        ReloadGridsInfo();
                        break;
                    case 3:
                        currentTypeInfo = Data.DataTypes.Удержания;
                        ReloadGridsInfo();
                        break;
                    case 4:
                        currentTypeInfo = Data.DataTypes.ПодоходныйНалогПеречислено;
                        ReloadGridsInfo();
                        dgTable1.CanUserAddRows = false;
                        dgTable1.Margin = new Thickness(0, 54, 0, 0);
                        break;
                    case 5:
                        currentTypeInfo = Data.DataTypes.СтраховойВзнос;
                        ReloadGridsInfo();
                        dgTable1.CanUserAddRows = false;
                        dgTable1.Margin = new Thickness(0, 54, 0, 0);
                        break;
                    case 6:
                        currentTypeInfo = Data.DataTypes.ПенсионныйВзнос;
                        ReloadGridsInfo();
                        dgTable1.CanUserAddRows = false;
                        dgTable1.Margin = new Thickness(0, 54, 0, 0);
                        break;
                    default:
                        break;
                }
            }
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitForm();
            FormLoaded = true;
        }

        private void btnAddRecord_Nalog(object sender, RoutedEventArgs e)
        {
            Add_ПодоходныйНалог wind = new Add_ПодоходныйНалог();
            wind.EmplName = employee.ФИО;
            if(wind.ShowDialog() == true)
            {
                ((DataPartialTables)dataEmplInfo).AddNewRecord(employee.работникID, wind.date, wind.nalog, wind.docNumber, wind.docDate, wind.docMoney);
                ReloadGridsInfo();
            }
        }

        private void btnAddRecord_Insurance(object sender, RoutedEventArgs e)
        {
            Add_СтраховойВзнос wind = new Add_СтраховойВзнос();
            wind.EmplName = employee.ФИО;
            if(wind.ShowDialog() == true)
            {
                ((DataPartialTables)dataEmplInfo).AddNewRecord(employee.работникID, wind.date, (short)wind.Days, wind.Others, wind.Fond, wind.Dolg, wind.Month, wind.docNumber, wind.docDate, wind.docMoney);
                ReloadGridsInfo();
            }
        }

        private void btnAddRecord_Pens(object sender, RoutedEventArgs e)
        {
            Add_ПенсионныйВзнос wind = new Add_ПенсионныйВзнос();
            if (wind.ShowDialog() == true)
            {
                ((DataPartialTables)dataEmplInfo).AddNewRecord(employee.работникID, wind.date, wind.Others, wind.Dolg, wind.docNumber, wind.docDate, wind.docMoney);
                ReloadGridsInfo();
            }
        }

        public EditTables tablesForm;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Все несохраненные изменения будут потеряны.\nПродолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            tablesForm.Show();
        }

    }
}
