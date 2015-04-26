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
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }


        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            string path = GetPathForSave();
            if(path != null)
            {
                try
                {
                    DataBaseConfig.CreateCopyOfCurrentDB(path);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка создания резервной копии!\nПопробуйте перезапустить приложение.\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("Копия успешно создана!");
            }
        }

        string GetPathForSave()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "DB_backup " + DateTime.Now.ToString("dd-MM-yyyy");
            dlg.DefaultExt = ".mdf";
            dlg.Filter = "DataBase (.mdf)|*.mdf";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }
        string GetPathForLoad()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mdf";
            dlg.Filter = "DataBase (.mdf)|*.mdf";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }

        private void btnNewBase_Click(object sender, RoutedEventArgs e)
        {
            string path;
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                path = dialog.SelectedPath;
            }
            else
                path = null;
            if (path != null)
            {
                try
                {
                    DataBaseConfig.CreateNewDB(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка создания базы!\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("База успешно создана и подключена!!");
            }
        }

        private void btnOpenBase_Click(object sender, RoutedEventArgs e)
        {
            string path = GetPathForLoad();
            if (path != null)
            {
                try
                {
                    DataBaseConfig.ChangeDB(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка открытия базы!\n\n" + ex.Message);
                    return;
                }
                MessageBox.Show("База успешно подключена!");
            }
        }

    }
}
