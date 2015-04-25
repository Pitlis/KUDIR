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
    /// Interaction logic for ВыручкаГенерация.xaml
    /// </summary>
    public partial class ВыручкаГенерация : Window
    {
        public ВыручкаГенерация()
        {
            InitializeComponent();
            cbYear.ItemsSource = new List<int>() { 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017 };
            cbMonth.ItemsSource = new List<string>() { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        }
        public DateTime date;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(cbMonth.SelectedValue != null && cbYear.SelectedValue != null)
            {
                date = new DateTime((int)cbYear.SelectedValue, cbMonth.SelectedIndex + 1, 15);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Необходимо выбрать месяц и год");
            }
        }
    }
}
