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

namespace KUDIR
{
    /// <summary>
    /// Interaction logic for EditTables.xaml
    /// </summary>
    public partial class EditTables : Window
    {
        string strConnect = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR.mdf;Integrated Security=True;Connect Timeout=30";
        
        public EditTables()
        {
            InitializeComponent();
        }
        Data data;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data = new Data(Data.DataTypes.Выручка, strConnect);
            DataGridConfig grid = new DataGridConfig(dgTable);
            grid.ShowData(data);


            PrintReport pr = new PrintReport("D:\\Отчет.xlsx");
            pr.СтраховыеВзносыПеречислено(2014);
            //pr.СтраховойВзнос(2014, 1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            data.Update();
        }


    }
}