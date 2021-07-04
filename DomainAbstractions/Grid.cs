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
    /// Grid is an ALA domain abstraction (see AbstractionLayeredArchitecture.md for more details)
    /// Abstraction description follows:
    /// This is a UI abstraction that displays rows and columns of data, and allows users to select a specific row.
    /// Grid pulls data from its dataSource port which uses the TableDataFlow programming paradigm, and is implemented by ITableDataFlow.
    /// It pulls the data when it receives an event on the fetchDataFromSource input port
    /// In the underlying implementation it uses a WPF DataGrid.
    /// 
    /// Currently the only supported input data source is an IDataTableFlow which internally is implemented using a .NET DataTable
    /// We assume that the dataSource has a column named the value of PrimaryKey (which is to be configured when instantiated), so when a row selected, 
    /// we can find the PrimaryKey value as the primary key of the selection and generate an IDataFlow<string> as an output.
    /// This PrimaryKey column is not displayed on the grid by default.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Notes: 
    /// 1. If there is a checkbox column, please use "checkbox" as the column name.
    /// 2. The visibility of a column is configured in the DataTable (DataColumn.Prefix = 'hide').
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI parent: connection from the containing IUI element such as a window, panel, horizontal, vertical.
    /// 2. ITableDataFlow dataSource: the main data input/output for the grid
    /// 3. IEvent start: causes the grid to configure itself according to the columns in the ITableDataFlow
    /// 4. IEvent eventRowSelected: outputs an event when the currently selected row changes (by the user click or the default) TBD: deprecate this, use dataFlowSelectedPrimaryKey instead
    /// 5. IDataFlow<string> dataFlowSelectedPrimaryKey: string output of the current row primary key selected (by the user click or the default)
    /// </summary>

    public class Grid : IUI, IEvent // parent, fetchDataFromSource
    {
        // Configurations ---------------------------------------------------------------------
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
        /// A Grid which is used to displaying the data from its ITableDataFlow port
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
        // Event input port tells us to that the dataSource is ready to give us data or that the data source may have different columns
        void IEvent.Execute()
        {
            var _fireAndForgot = FetchDataAsync();
        }

        private async Task FetchDataAsync()
        {
            // here we want to fetch data from the dataSource programming paradigm and display it on the grid
            // what we actually do is bind the WPF DataGrid to the underlying DataTable in the ITableDataFlow interface

            dataGrid.Columns.Clear();

            await dataSource.GetHeadersFromSourceAsync();
            
            dataGrid.ItemsSource = null; // prevent the dataGrid from reading the source while we are changing its columns
            foreach (DataColumn c in dataSource.DataTable.Columns)
                dataGrid.Columns.Add(GetInitializedColumn(c));

            var batch = await dataSource.GetPageFromSourceAsync();
            while (batch.Item1 < batch.Item2)
                batch = await dataSource.GetPageFromSourceAsync();

            dataGrid.ItemsSource = dataSource.DataTable.DefaultView;
        }





        // user selecting a row calls this function -------------------------------------------------------
        private void RowSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshRowSelection();
        }

        private void RefreshRowSelection()
        {
            // When a row is selected, there are tow output ports, one just outputs an event and one outputs the primarty key of the selected row
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





        // specify and returun a style for the cells of the underlying WPF DataGrid
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



        // Create a WPF DatGridColum for configuring a WPF DataGrid from a DataColumn, which is what describes a columns of a DataTable, which is the undelying representation of data in the ITableDataFlow programming paradigm
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
