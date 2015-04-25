﻿using KUDIR.Forms;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KUDIR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EditTables editTables;
        public Отчеты reports;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (editTables == null)
            {
                editTables = new EditTables();
                editTables.mainMenu = this;
                editTables.Show();
                this.Hide();
            }
            else
            {
                editTables.Activate();
            }
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            if (reports == null)
            {
                reports = new Отчеты();
                reports.mainMenu = this;
                reports.Show();
                this.Hide();
            }
            else
            {
                reports.Activate();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (editTables != null)
                editTables.mainMenu = null;
            if (reports != null)
                reports.mainMenu = null;
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (editTables == null)
            {
                if (reports != null && reports.mainMenu != null)
                    reports.mainMenu = null;
                Application.Current.Shutdown();
            }
            if(editTables != null)
            {
                editTables.Show();
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
