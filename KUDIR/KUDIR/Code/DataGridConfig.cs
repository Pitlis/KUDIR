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
            DGrid = grid;
        }

        public void ShowData(Data data, string filter)
        {
            DGrid.ItemsSource = new DataView(data.Table, filter, "ID", DataViewRowState.CurrentRows);
            ConfigColumns(data);
        }
        public void ShowData(Data data)
        {
            DGrid.ItemsSource = data.Table.DefaultView;
            ConfigColumns(data);
        }

        void ConfigColumns(Data data)
        {
            if (data.ColumnPositions != null)
            {
                ChangeColumnPosition(data.Table, data.ColumnPositions, data.ColumnNames);
            }
            HideColumns(data.HiddenColumns, DGrid);
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
    }
}
