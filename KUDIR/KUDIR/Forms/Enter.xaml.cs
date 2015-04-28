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
    /// Interaction logic for Enter.xaml
    /// </summary>
    public partial class Enter : Window
    {
        public Enter()
        {
            InitializeComponent();
            if (!Authentication.Security)
            {
                MainWindow wind = new MainWindow();
                wind.strConnect = KUDIR.Code.Authentication.GetSqlConnectionString();
                wind.Show();
                this.Close();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Authentication user = new Authentication(login.Text, pass.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MainWindow wind = new MainWindow();
            wind.strConnect = KUDIR.Code.Authentication.GetSqlConnectionString();
            wind.Show();
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void btnChangeServer_Click(object sender, RoutedEventArgs e)
        {
            ChangeServer wind = new ChangeServer();
            wind.ShowDialog();
        }

    }
}
