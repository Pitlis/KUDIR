using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KUDIR.Code
{
    public class DataGridConfig
    {
        public DataGrid DGrid { get; private set; }

        public DataGridConfig(DataGrid grid)
        {
            grid.Columns.Clear();
            DGrid = grid;
        }

        public void ShowData(Data data, string filter, string IdName)
        {
            DGrid.ItemsSource = new DataView(data.Table, filter, IdName, DataViewRowState.CurrentRows);
            ConfigColumns(data);
        }
        public void ShowData(Data data)
        {
            DGrid.ItemsSource = data.Table.DefaultView;
            ConfigColumns(data);
        }

        void ConfigColumns(Data data)
        {
            AddDatePicker(data.Table);
            if (data.ColumnPositions != null)
            {
                ChangeColumnPosition(data.Table, data.ColumnPositions, data.ColumnNames);
            }
            HideColumns(data.HiddenColumns, DGrid);
            DGrid.IsReadOnly = !data.CanEdit;
        }

        void HideColumns(List<int> columns, DataGrid grid)
        {
            foreach (int index in columns)
            {
                grid.Columns[index].Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void ChangeColumnPosition(DataTable table, string[] columnNames, string[] newNames)
        {
            int i = 0;
            foreach (string name in columnNames)
            {
                int t = table.Columns.IndexOf(name);
                DGrid.Columns[t].DisplayIndex = i;
                if (newNames != null)
                    DGrid.Columns[t].Header = (string)newNames[i];
                i++;
            }
        }

        void AddDatePicker(DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                 if (table.Columns[i].DataType.IsEquivalentTo(typeof(DateTime)))
                 {
                     DataGridTemplateColumn dgct = new DataGridTemplateColumn();
                     System.Windows.FrameworkElementFactory factory = new System.Windows.FrameworkElementFactory(typeof(DatePicker));
                     System.Windows.Data.Binding b = new System.Windows.Data.Binding(table.Columns[i].ColumnName);
                     b.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged;
                     factory.SetValue(DatePicker.SelectedDateProperty, b);
                     System.Windows.DataTemplate cellEditingTemplate = new System.Windows.DataTemplate();
                     cellEditingTemplate.VisualTree = factory;
                     dgct.CellTemplate = cellEditingTemplate;
                     dgct.Header = table.Columns[i].ColumnName;
                     DGrid.Columns[i] = dgct;
                 }
            }
        }
    }
}
