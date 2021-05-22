using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DomainAbstractions
{

    /// <summary>
    /// This is a UI abstraction that displays the data in the style of a data grid (rows and columns) and
    /// allows users to select the specific row and column they want.
    /// An WPF Data Grid is used for displaying the data of an ITableDataFlow and keeps a copy of the data itself.
    /// The Primary key column is not displayed by default.
    /// 
    /// Currently the only supported input data source is a DataTable, and we assume that every
    /// DataTable binded to this grid will have the column PrimaryKey which was assigned when instantiated, so when any row selected, 
    /// we can find the PrimaryKey value as the primary key of the selection and generate an IDataFlow<string> as an output.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Notice: 
    /// 1. If there is a checkbox column, please use the key word "checkbox" as the column name.
    /// 2. The visibility of a column was configured in the DataTable (DataColumn.Prefix = 'hide').
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI inputUI: connection from the containing IUI element such as a window, panel, horizontal, vertical.
    /// 2. ITableDataFlow inputOutputTableData: mainTableDataFlow, the main data input/output for the grid
    /// 3. IEvent clearTable: Clear the data table and internal copy
    /// 4. IDataFlow<bool> visible: toggle the visibility of the grid with a boolean value (visible or collapsed)
    /// 5. IDataFlow<int> gridSelectedIndex: will set the internal datagrid index
    /// 6. IEvent eventRowSelected: outputs an event when the currently selected row changes (by the user click or the default) TBD: deprecate this, use dataFlowSelectedPrimaryKey instead
    /// 6. IDataFlow<string> dataFlowSelectedPrimaryKey: string output of the current row primary key selected (by the user click or the default)
    /// 7. IDataFlow<string> dataFlowNumberOfRecords: string output of the total number of records displayed
    /// 8. IDataFlow<bool> dataFlowShowRecordStateTitle: boolean output to toggle the finished recording on the grid title
    /// 9. IDataFlow<bool> dataFlowShowDownloadingStateTitle: boolean output to toggle the downloading state title
    /// </summary>

    public class Grid : IUI, IEvent // inputUI, fetchDataFromSource
    {
        // properties ---------------------------------------------------------------------
        public string InstanceName = "Default";
        public bool ShowHeader = true;
        public double RowHeight = 22;
        public Thickness Margin;
        public string PrimaryKey;
        public DataGridSelectionMode selectionMode { set => dataGrid.SelectionMode = value;}

        // ports ---------------------------------------------------------------------
        private IEvent eventRowSelected;  
        private IDataFlow<string> dataFlowSelectedPrimaryKey;
        private ITableDataFlow dataSource;

        // private fields ---------------------------------------------------------------------
        private DataGrid dataGrid;
        private int selectedIndex = 0;

        /// <summary>
        /// An WPF Data Grid which used for displaying the data of an ITableDataFlow.
        /// It keeps a copy of the data itself.
        /// </summary>
        public Grid(bool autoColumnWidth = true)
        {
            dataGrid = new DataGrid() { AutoGenerateColumns = false };
            dataGrid.HorizontalGridLinesBrush = new SolidColorBrush(Color.FromRgb(150, 150, 150));
            dataGrid.VerticalGridLinesBrush = new SolidColorBrush(Color.FromRgb(150, 150, 150));
            dataGrid.ColumnWidth = new DataGridLength(1, autoColumnWidth? DataGridLengthUnitType.Auto : DataGridLengthUnitType.Star);
            dataGrid.IsReadOnly = true;
            dataGrid.CellStyle = GetCellStyle();
            dataGrid.SelectionChanged += RowSelectionChanged;
            dataGrid.Background = Brushes.White;
            dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            dataGrid.SelectionMode = DataGridSelectionMode.Single;
        }

        // IUI implementation ---------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            dataGrid.Margin = Margin;
            dataGrid.RowHeight = RowHeight;
            dataGrid.HeadersVisibility = ShowHeader ? DataGridHeadersVisibility.All : DataGridHeadersVisibility.None;
            return dataGrid;
        }

        // IEvent implementation ---------------------------------------------------------
        // this ask data source to pull data
        void IEvent.Execute()
        {
            var _fireAndForgot = FetchDataAsync();
        }

        private async Task FetchDataAsync()
        {
            dataGrid.Columns.Clear();

            await dataSource.GetHeadersFromSourceAsync();
            foreach (DataColumn c in dataSource.DataTable.Columns)
                dataGrid.Columns.Add(GetInitializedColumn(c));

            var batch = await dataSource.GetPageFromSourceAsync();
            while (batch.Item1 < batch.Item2)
                batch = await dataSource.GetPageFromSourceAsync();

            dataGrid.ItemsSource = dataSource.DataTable.DefaultView;
        }

        // data grid row selection event - select a row manually or programactically
        private void RowSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshRowSelection();
        }

        private void RefreshRowSelection()
        {
            if (dataGrid.SelectedIndex >= 0)
            {
                selectedIndex = dataGrid.SelectedIndex;
                Debug.WriteLine($"{InstanceName} Selected index is SET: {selectedIndex}");
                // output the primary key of the selected row
                if (dataFlowSelectedPrimaryKey != null)
                {
                    dataFlowSelectedPrimaryKey.Data = dataSource.DataTable.Rows[dataGrid.SelectedIndex][PrimaryKey].ToString();
                }
                eventRowSelected?.Execute();
            }
        }

        // cell initialized style
        private Style GetCellStyle()
        {
            var cellStyle = new Style();
            cellStyle.Setters.Add(new Setter() { Property = Control.FontSizeProperty, Value = 14.0 });
            cellStyle.Setters.Add(new Setter() { Property = FrameworkElement.VerticalAlignmentProperty, Value = VerticalAlignment.Stretch });
            cellStyle.Setters.Add(new Setter() { Property = FrameworkElement.HorizontalAlignmentProperty, Value = HorizontalAlignment.Stretch });

            // cell selection style
            cellStyle.Resources.Add(SystemColors.InactiveSelectionHighlightBrushKey, Brushes.DodgerBlue);
            var trigger = new Trigger() { Property = DataGridCell.IsSelectedProperty, Value = true };
            trigger.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White)); // Transparent
            trigger.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)))); // Transparent
            trigger.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
            trigger.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.DodgerBlue));
            cellStyle.Triggers.Add(trigger);

            // cell un-selection style
            var unselectTrigger = new Trigger() { Property = DataGridCell.IsSelectedProperty, Value = false };
            unselectTrigger.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black)); // Transparent
            unselectTrigger.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)))); // Transparent
            unselectTrigger.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
            unselectTrigger.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.White));
            cellStyle.Triggers.Add(unselectTrigger);

            return cellStyle;
        }


        private DataGridColumn GetInitializedColumn(DataColumn column)
        {
            return column.ColumnName.Equals("checkbox") || column.DataType == typeof(bool) ?
            // checkbox column
            (DataGridColumn)new DataGridCheckBoxColumn()
            {
                Header = column.ColumnName,
                Width = 50,
                Binding = new Binding(column.ColumnName)
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                ElementStyle = new Style()
                {
                    Setters = {
                        new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center),
                        new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center)
                    }
                }
            } :
            // text column
            new DataGridTextColumn()
            {
                Header = column.ColumnName,
                Binding = new Binding(column.ColumnName),
                ElementStyle = new Style()
                {
                    Setters = {
                        new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center),
                        new Setter(FrameworkElement.MarginProperty, new Thickness(5, 0, 0, 0))
                    }
                },
                Visibility = "hide".Equals(column.Prefix) ? Visibility.Collapsed : Visibility.Visible
            };
        }
    }
}
