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
            data = new Data(Data.DataTypes.НалоговыйАгент, strConnect);
            DataGridConfig grid = new DataGridConfig(dgTable);
            grid.ShowData(data);


            PrintReport pr = new PrintReport("D:\\Отчет.xlsx");
            pr.НалоговыйАгент(new DateTime(2014, 1, 1), new DateTime(2015, 10, 10));
            //DateTime date1 = new DateTime(2015, 4, 1);
            //DateTime date2 = new DateTime(2015, 4, 17);
            //IEnumerable<DataRow> query =
            //    from row in data.Table.AsEnumerable()
            //    where row["Дата_записи"] != DBNull.Value && row.Field<DateTime>("Дата_записи") >= date1 && row.Field<DateTime>("Дата_записи") <= date2
            //    select row;
            //PrintRepotrs.Выручка("D:\\Отчет.xlsx", query.CopyToDataTable<DataRow>(), 300, 500);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            data.Update();
        }


    }
}