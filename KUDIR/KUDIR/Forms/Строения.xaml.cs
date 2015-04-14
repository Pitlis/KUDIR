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
    /// Interaction logic for Строения.xaml
    /// </summary>
    public partial class Строения : Window
    {
        string strConnect = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR.mdf;Integrated Security=True;Connect Timeout=30";
        Data dataBuild;
        Data dataBuildInfo;
        Data_Строение build;
        int currentBuild = -2;
        DataGridConfig grid;

        public Строения()
        {
            InitializeComponent();
            cbPravo.ItemsSource = Enum.GetValues(typeof(Data_Строение.Type_Право));
            cbPravo.SelectedIndex = 0;
        }


        void FillForm(Data_Строение build)
        {
            tbxAddress.Text = build.Адрес;
            tbxName.Text = build.Наименование;
            dpPriobrDate.SelectedDate = build.Дата_приобретения;
            tbxDocPriobrName.Text = build.Док_Наименование_Приобр;
            tbxDocPriobrNumber.Text = build.Док_Номер_Приобр;
            dpDocPriobrDate.SelectedDate = build.Док_Дата_Приобр;

            dpEntrDate.SelectedDate = build.Дата_ввода_в_эксплуатацию;

            dpRegDate.SelectedDate = build.Дата_гос_регистрации;
            tbxDocRegName.Text = build.Док_Наименование_Рег;
            tbxDocRegNumber.Text = build.Док_Номер_Рег;
            dpDocRegDate.SelectedDate = build.Док_Дата_Рег;

            dpExitDate.SelectedDate = build.Дата_выбытия;
            tbxDocExitName.Text = build.Док_Наименование_Выб;
            tbxDocExitNumber.Text = build.Док_Номер_Выб;
            dpDocExitDate.SelectedDate = build.Док_Дата_Выб;

            cbPravo.SelectedValue = build.Право;
        }
        void LoadFromForm(Data_Строение build)
        {
            build.Адрес = tbxAddress.Text;
            build.Наименование = tbxName.Text;
            build.Дата_приобретения = dpPriobrDate.SelectedDate;
            build.Док_Наименование_Приобр = tbxDocPriobrName.Text;
            build.Док_Номер_Приобр = tbxDocPriobrNumber.Text;
            build.Док_Дата_Приобр = dpDocPriobrDate.SelectedDate;

            build.Дата_ввода_в_эксплуатацию = dpEntrDate.SelectedDate;

            build.Дата_гос_регистрации = dpRegDate.SelectedDate;
            build.Док_Наименование_Рег = tbxDocRegName.Text;
            build.Док_Номер_Рег = tbxDocRegNumber.Text;
            build.Док_Дата_Рег = dpDocRegDate.SelectedDate;

            build.Дата_выбытия = dpExitDate.SelectedDate;
            build.Док_Наименование_Выб = tbxDocExitName.Text;
            build.Док_Номер_Выб = tbxDocExitNumber.Text;
            build.Док_Дата_Выб = dpDocExitDate.SelectedDate;

            build.Право = (Data_Строение.Type_Право)cbPravo.SelectedValue;
        }
        void InitForm()
        {
            dataBuild = new Data(Data.DataTypes.Строение, strConnect);
            dataBuildInfo = new Data(Data.DataTypes.СтоимостьСтроения, strConnect);
            grid = new DataGridConfig(dgTable);
            if (dataBuild.Table.Rows.Count == 0)
            {
                build = new Data_Строение(dataBuild);
                currentBuild = -1;
            }
            else
            {
                build = new Data_Строение(dataBuild, 0);
                currentBuild = 0;
                FillForm(build);
                grid.ShowData(dataBuildInfo, "ID_строение = " + build.ID, "ID");
                dataBuildInfo.Table.Columns["ID_строение"].DefaultValue = build.ID;
            }
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            dgTable.CanUserAddRows = true;
            if(currentBuild + 1 < dataBuild.Table.Rows.Count && currentBuild != -1)//Следующая запись есть
            {
                currentBuild++;
                build = new Data_Строение(dataBuild, currentBuild);
                FillForm(build);
                ReloadBuildInfo();
                return;
            }
            if (currentBuild + 1 == dataBuild.Table.Rows.Count && currentBuild != -1)//Это была последняя запись
            {
                currentBuild = 0;
                build = new Data_Строение(dataBuild, 0);
                FillForm(build);
                ReloadBuildInfo();
                return;
            }
            if(currentBuild == -1 && dataBuild.Table.Rows.Count > 0)//Переход со страницы новой записи
            {
                currentBuild = 0;
                build = new Data_Строение(dataBuild, 0);
                FillForm(build);
                ReloadBuildInfo();
                return;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbxName.Text == null)
            {
                MessageBox.Show("Требуется наименование строения!");
                return;
            }
            LoadFromForm(build);
            build.Update();
            dataBuildInfo.Update();
            ReloadBuildInfo();
            if(currentBuild == -1 && dataBuild.Table.Rows.Count > 0)
            {
                currentBuild = dataBuild.Table.Rows.Count - 1;
            }
        }

        void ReloadBuildInfo()//грид обновляется при каждом переключении между строениями, и при создании новой записи
        {
            dataBuildInfo = new Data(Data.DataTypes.СтоимостьСтроения, strConnect);
            grid = new DataGridConfig(dgTable);
            grid.ShowData(dataBuildInfo, "ID_строение = " + build.ID, "ID");
            dataBuildInfo.Table.Columns["ID_строение"].DefaultValue = build.ID;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            build.Delete();
            InitForm();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            build = new Data_Строение(dataBuild);
            ReloadBuildInfo();
            currentBuild = -1;
            FillForm(build);
            dgTable.CanUserAddRows = false;
        }


        private void lo_ContentRendered(object sender, EventArgs e)
        {
            InitForm();
        }

    }
}
