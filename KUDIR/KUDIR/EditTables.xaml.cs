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

namespace KUDIR
{
    /// <summary>
    /// Interaction logic for EditTables.xaml
    /// </summary>
    public partial class EditTables : Window
    {
        public EditTables()
        {
            InitializeComponent();
        }
        Data data;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data = new Data(Data.DataTypes.Выручка, @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\VirtualBox\BBD\SHARE\KUDIR1.mdf;Integrated Security=True;Connect Timeout=30");
            dgTable.ItemsSource = data.Table.DefaultView;
            HideColumns(data.HiddenColumns, dgTable);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            data.Update();
        }

        void HideColumns(List<int> columns, DataGrid grid)
        {
            foreach(int index in columns)
            {
                grid.Columns[index].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
