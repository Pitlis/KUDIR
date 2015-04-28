using KUDIR.Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ChangeServer.xaml
    /// </summary>
    public partial class ChangeServer : Window
    {
        public ChangeServer()
        {
            InitializeComponent();
            
        }

        string GetServerName()
        {
            SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder(Authentication.GetSqlConnectionString());
            return connect == null ? "" : connect.DataSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBaseConfig.ChangeServerName(txtServerName.Text);
            DialogResult = true;
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtServerName.Text = GetServerName();
        }
    }
}
