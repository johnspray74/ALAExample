﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Exports a column in a DataTable as a List&lt;string&gt;. The ColumnName can be set either as a public property or from an IDataFlow&lt;string&gt;, with the latter overwriting the former.</para>
    /// <para>A Lambda can also be set, as a public property, to output particular DataRows based on some condition described by the Lambda.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;string&gt; columnName : the name of the column to export.</para>
    /// <para>2. IDataFlow&lt;DataTable&gt; dataTableInput : the DataTable to export from.</para>
    /// <para>3. IDataFlowB&lt;DataTable&gt; dataTable: like dataTableInput, but for pulling from an input instead of having the DataTable pushed to this instance.</para>
    /// <para>4. IDataFlow&lt;List&lt;string&gt;&gt; exportedColumn : the exported column, where each element has has ToString() applied to them, i.e. they may have been types other than strings.</para>
    /// </summary>
    public class ExportDataTableColumn : IDataFlow<string>, IDataFlow<DataTable>
    {
        // Properties
        public string InstanceName = "Default";
        public string ColumnName;
        public FilterLambdaDelegate Lambda;
        public bool ClearOnOutput = true;

        // Private Fields
        private DataTable dt = new DataTable();
        private List<string> exportList = new List<string>();

        // Ports
        private IDataFlowB<DataTable> dataTable;
        private IDataFlow<List<string>> exportedColumn;

        /// <summary>
        /// <para>Exports a column in a DataTable as a List&lt;string&gt;. The ColumnName can be set either as a public property or from an IDataFlow&lt;string&gt;, with the latter overwriting the former.</para>
        /// <para>A Lambda can also be set, as a public property, to output particular DataRows based on some condition described by the Lambda.</para>
        /// </summary>
        public ExportDataTableColumn() { }

        public void Export()
        {
            if (ColumnName == null || !dt.Columns.Contains(ColumnName)) return;
            foreach (DataRow dataRow in dt.Rows)
            {
                exportList.Add(dataRow[ColumnName].ToString());
            }

            var output = exportList;
            if (ClearOnOutput)
            {
                output = exportList.Select(s => s).ToList();
                exportList.Clear();
            }

            if (exportedColumn != null) exportedColumn.Data = output;
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                ColumnName = value;
                Export();
            }
        }

        // IDataFlow<DataTable> implementation
        DataTable IDataFlow<DataTable>.Data
        {
            set
            {
                dt = value;
                Export();
            }
        }
    }
}
