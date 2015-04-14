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
            employee.Update();
            dataEmplInfo.Update();
            ReloadGridsInfo();
            if (currentEmpl == -1 && dataEmpl.Table.Rows.Count > 0)
            {
                currentEmpl = dataEmpl.Table.Rows.Count - 1;
            }

        }
        Data.DataTypes currentTypeInfo;
        void ReloadGridsInfo()//грид обновляется при каждом переключении между работниками, и при создании новой записи
        {
            dataEmplInfo = new Data(currentTypeInfo, strConnect);
            grid = new DataGridConfig(dgTable1);
            grid.ShowData(dataEmplInfo, "работникID = " + employee.работникID, "работникID");
            dataEmplInfo.Table.Columns["работникID"].DefaultValue = employee.работникID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitForm();
            FormLoaded = true;
        }
        bool FormLoaded = false;
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                    default:
                        break;
                }
            }
        }

        private void dgTable1_Loaded(object sender, RoutedEventArgs e)
        {
            InitForm();
        }
    }
}
