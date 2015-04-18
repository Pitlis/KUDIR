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
    /// Interaction logic for Add_СтраховойВзнос.xaml
    /// </summary>
    public partial class Add_СтраховойВзнос : Window
    {
        public string EmplName { get; set; }

        public DateTime date;
        public int Days;
        public Decimal? Others;
        public int Month;
        public Decimal? Fond;
        public Decimal? Dolg;

        public string docNumber;
        public DateTime? docDate;
        public Decimal? docMoney;

        public Add_СтраховойВзнос()
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
                Days = Int32.Parse(txtDays.Text);
                if (Days <= 0 || Days > 30)
                    throw new Exception();
                docNumber = txtNumDoc.Text.Length > 2 ? txtNumDoc.Text : null;
                try
                {
                    docMoney = Decimal.Parse(txtMoney.Text);
                }
                catch
                {
                    docMoney = null;
                }
                docDate = dpDateDoc.SelectedDate;

                try
                {
                    Others = Decimal.Parse(txtOthers.Text);
                }
                catch
                {
                    Others = null;
                }
                Month = cbMonth.SelectedIndex;

                try
                {
                    Fond = Decimal.Parse(txtFond.Text);
                }
                catch
                {
                    Fond = null;
                }

                try
                {
                    Dolg = Decimal.Parse(txtDolg.Text);
                }
                catch
                {
                    Dolg = null;
                }
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
            cbMonth.ItemsSource = new List<string>() {"не указывать", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            cbMonth.SelectedIndex = 0;
        }
    }
}
