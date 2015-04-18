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
    /// Interaction logic for Add_ПенсионныйВзнос.xaml
    /// </summary>
    public partial class Add_ПенсионныйВзнос : Window
    {
        public string EmplName { get; set; }

        public DateTime date;
        public Decimal? Others;
        public Decimal? Dolg;

        public string docNumber;
        public DateTime? docDate;
        public Decimal? docMoney;

        public Add_ПенсионныйВзнос()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpDate.SelectedDate == null)
                    throw new Exception();
                date = dpDate.SelectedDate.Value;
                docNumber = txtNumDoc.Text.Length > 2? txtNumDoc.Text : null;
                try
                {
                    docMoney = Decimal.Parse(txtMoney.Text);
                }
                catch
                {
                    docMoney = null;
                }
                try
                {
                    Others = Decimal.Parse(txtOthers.Text);
                }
                catch
                {
                    Others = null;
                }
                try
                {
                    Dolg = Decimal.Parse(txtDolg.Text);
                }
                catch
                {
                    Dolg = null;
                }
                docDate = dpDateDoc.SelectedDate;
            }
            catch
            {
                MessageBox.Show("Некорректные значения!");
                return;
            }
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            lbName.Content = EmplName;
        }
    }
}
