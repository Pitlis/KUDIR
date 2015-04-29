using KUDIR.Code;
using System;
using System.Collections.Generic;
using System.Data;
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
using Reports;
using KUDIR.Forms;

namespace KUDIR
{
    /// <summary>
    /// Interaction logic for EditTables.xaml
    /// </summary>
    public partial class EditTables : Window
    {
        public string strConnect;
        public MainWindow mainMenu;
        public Forms.Работник formEmpl;
        public Forms.Строения formBuild;
        public EditTables()
        {
            InitializeComponent();
        }
        Data data;


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            data.Update();
        }

        private void dgTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ChangeButtonStatus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.Update();
                ChangeButtonStatus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            data.Refresh();
            ChangeButtonStatus();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (data == null)
                return;
            if(data.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Все несохраненные изменения будут потеряны. Продолжить?", "Обновление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    data.Refresh();
                    ChangeButtonStatus();
                }
            }
        }

        void ChangeButtonStatus()
        {
            if(data != null)
            {
                bool changed = data.HasChanges();
                btnSave.IsEnabled = changed;
                btnCancel.IsEnabled = changed;
            }
        }
        bool LoadData(Data.DataTypes type)
        {
            try
            {
                data = new Data(type, strConnect);
                DataGridConfig grid = new DataGridConfig(dgTable);
                grid.ShowData(data);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных!\n\n\nПодробно:\n" + ex.Message);
                return false;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if(LoadData(Data.DataTypes.Выручка))
            {
                gridName.Text = "Учет валовой выручки";
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if(LoadData(Data.DataTypes.Отгрузка))
            {
                gridName.Text = "Сведения об отгрузке товаров";
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.Предоплата))
            {
                gridName.Text = "Сведения о предварительной оплате";
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.Кредитор))
            {
                gridName.Text = "Учет кредиторской задолженности";
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.НалоговыйАгент))
            {
                gridName.Text = "Налог на доходы иностранных организаций, не осуществляющих деятельность в Республике Беларусь через постоянное представительство";
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.Дивиденты))
            {
                gridName.Text = "Налог на прибыль по дивидентам, начисленным белорусским организациям";
            }
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            formBuild = new Forms.Строения();
            formBuild.mainMenu = this;
            formBuild.strConnect = strConnect;
            formBuild.Show();
            this.Hide();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.НезавершенныеСтроения))
            {
                gridName.Text = "Учет зданий, сооружений и передаточных устройств сверхнормативного незавершенного строительства";
            }
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            formEmpl = new Forms.Работник();
            formEmpl.mainMenu = this;
            formEmpl.strConnect = strConnect;
            formEmpl.Show();
            this.Hide();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.НДС_Приобретение))
            {
                gridName.Text = "Суммы налога на добавленную стоимость, уплаченные при приобретении (ввозе) товаров (работ, услуг), имущественных прав";
            }
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.НДС_Реализация))
            {
                gridName.Text = "Суммы налога на добавленную стоимость, исчисленные по оборотам по реализации (приобретению) товаров (работ, услуг), имущественных прав";
            }
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            if (LoadData(Data.DataTypes.ТоварыТС))
            {
                gridName.Text = "Учет товаров, в том числе сырья и материалов, основных средств и отдельных предметов в составе оборотных средств, ввозимых на территорию Республики Беларусь из государств - членов Таможенного Союза";
            }
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            if(LoadData(Data.DataTypes.Кооператив))
            {
                gridName.Text = "Учет стоимости паев членов производственного кооператива";
            }
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            if(data != null && data.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Все несохраненные изменения будут потеряны.\nПродолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            mainMenu.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (data != null && data.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Все несохраненные изменения будут потеряны.\nПродолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (mainMenu != null)
            {
                mainMenu.editTables = null;
                mainMenu.Show();
            }
            if(formBuild != null)
            {
                formBuild.mainMenu = null;
            }
            if (formEmpl != null)
            {
                formEmpl.mainMenu = null;
            }
        }

        private void MenuItem_Click_Generate(object sender, RoutedEventArgs e)
        {
            ВыручкаГенерация wind = new ВыручкаГенерация();
            if (wind.ShowDialog() == true)
            {
                try
                {
                    new StoredProcedure(strConnect).Generate_Выручка(wind.date);
                    btnRefresh_Click(new Object(), new RoutedEventArgs());
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к базе данных!\n\n\nПодробно:\n" + ex.Message);
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (Authentication.Security)
            {
                if (Authentication.UserAccess == Authentication.Access.Продавец)
                {
                    miBuild.Visibility = System.Windows.Visibility.Collapsed;
                    miNalog.Visibility = System.Windows.Visibility.Collapsed;
                    miOthers.Visibility = System.Windows.Visibility.Collapsed;
                    miWork.Visibility = System.Windows.Visibility.Collapsed;
                    miGenerate.IsEnabled = false;
                }
            }
        }
    }
}