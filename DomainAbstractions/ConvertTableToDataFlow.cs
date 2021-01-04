using ProgrammingParadigms;
using System.Data;
using System;
using System.Linq;

namespace DomainAbstractions
{
    /// <summary>
    /// Converts DataTable to DataFlow. To be explicit, it picks one cell from the DataTable with the given Column name and Row Index (or Primary Key).
    /// When it's instantiated, the parameter 'Column' is required to pick one column. As for the Row, it can
    /// be assigned programmatically when instantiated or, using the IDataFlow<string> input to assign the 
    /// primary key to then find the right Row.
    /// 
    /// Note: When wanting to use the input IDataFlow<string> port, the IDataFlow<DataTable> port must have first been used to instantiate the table
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<DataTable> incomingDataTable: the data table that wants to be extracted for a specific cell
    /// 2. IDataFlow<string> rowIndex: row index as a string. Specifies the row in which the cell is located in the DataTable.
    /// 3. IEvent clearTable: event to clear the internal .NET data table within this abstraction
    /// 4. IDataFlow<string> cellOutput: output cell result as a string from the column and row selection
    /// </summary>
    public class ConvertTableToDataFlow : IDataFlow<DataTable>, IDataFlow<string>, IEvent // incomingDataTable, rowIndex, clearDataTable
    {
        // properties --------------------------------------------------------------
        public string InstanceName = "Default";
        public string Column;
        public int ColumnIndex = 0;
        public string KeyWord;

        // -1 means the Row will not be used. Instead, we use the primary key to find the Row we want.
        public int Row = -1;

        public bool knowPrimaryKey { set; get; } = true; // this public property is used for the DataFlow<string> input to determine how the row search works

        // ports -----------------------------------------------------------------
        private IDataFlow<string> cellOutput;

        // private fields ----------------------------------------------------------
        private DataTable dataTable;


        /// <summary>
        /// Converts DataTable to DataFlow. To be explicit, it picks one cell from the DataTable with 
        /// the giving Column Name and Row Index(or Primary Key).
        /// </summary>
        public ConvertTableToDataFlow() {}


        // IDataFlow<DataTable> implementation --------------------------------------
        DataTable IDataFlow<DataTable>.Data
        {
            set
            {
                dataTable = value;
                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["index"] };

                if (Row != -1)
                {
                    cellOutput.Data = dataTable.Rows[Row][Column].ToString();
                }

                // Find the first row that contains a keyword, and return string format cell value
                // e.g. KeyWord = "FileNo", following part will search Column 0, return the first matching cell content and row index
                // this part would help to validate session content: new FileReader().WireTo(new ConvertTableToDataFlow(){KeyWord = "FileNo"}) ...
                if (!string.IsNullOrEmpty(KeyWord))
                {
                    if (!string.IsNullOrEmpty(Column))
                    {
                        var firstMatchRow = dataTable.AsEnumerable().FirstOrDefault(dr => dr[Column].ToString().ToLower().Contains(KeyWord.ToLower()));

                        if (firstMatchRow == null)
                        {
                            if (cellOutput != null) cellOutput.Data = ""; // did not find the content matching the keyword
                        }
                        else
                        {
                            if (cellOutput != null) cellOutput.Data = firstMatchRow[Column].ToString();
                        }
                    }

                    if (ColumnIndex >= 0)
                    {
                        var firstMatchRow = dataTable.AsEnumerable().FirstOrDefault(dr => dr[ColumnIndex].ToString().ToLower().Contains(KeyWord.ToLower()));

                        if (firstMatchRow == null)
                        {
                            if (cellOutput != null) cellOutput.Data = ""; // did not find the content matching the keyword
                        }
                        else
                        {
                            if (cellOutput != null) cellOutput.Data = firstMatchRow[ColumnIndex].ToString();
                        }
                    }
                }
            }
        }

        // IDataFlow<string> implementation -----------------------------------------
        string IDataFlow<string>.Data {
            set
            {
                // here value is the primary key
                if (dataTable != null && cellOutput != null)
                {
                    DataRow row;

                    if (knowPrimaryKey) row = dataTable.Rows.Find(value);
                    else row = dataTable.Rows.Find(Convert.ToInt32(value));
                    
                    cellOutput.Data = row == null ? null : row[Column].ToString();
                }
            }
        }

        // IEvent implementation -----------------------------------------
        // clears the internal dataTable
        void IEvent.Execute()
        {
            if (dataTable != null)
            {
                dataTable.Clear();
                dataTable = null;
            }
        }
    }
}
