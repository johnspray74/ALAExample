using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// SessionTableValidation abstraction is used to validate upload session file table contents. TODO: separate into different validate abstractions.
    /// Input ports:
    /// 1. ITableDataFlow: usually wired from Transact as data source.
    /// Output ports:
    /// 1. IRequestResponseDataFlow<string, DataTable> dbRequest: Database request when validate session file content.
    /// 2. IRequestResponseDataFlow<Dictionary<string, object>, string> queryRequest: SQL query command request interface.
    /// 3. IDataFlowB<string> dbFilePath: Used to get drive path and read information in polaris.xml.
    /// 4. ITableDataFlow sourceTableDataFlow: Interface used to wire to source abstractions, such as sessionFileReaderWriter or other validation abstractions.
    /// </summary>
    public class SessionTableValidation : ITableDataFlow 
    {
        // Instance Name
        public string InstanceName = "Default";
        
        // ports
        private IRequestResponseDataFlow_B<string, DataTable> dbRequest;
        private IRequestResponseDataFlow_B<Dictionary<string, object>, string> queryRequest;
        private IDataFlowB<string> dbFilePath;
        private ITableDataFlow sourceTableDataFlow; // source ITableDataFlow

        // private fields.
        private DataTable uploadSessionDataTable = new DataTable();
        private bool dataTransactFlag = false;

        // step 1: validate polaris settings

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => uploadSessionDataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            uploadSessionDataTable.Rows.Clear();
            uploadSessionDataTable.Columns.Clear();

            dataTransactFlag = false;

            await sourceTableDataFlow.GetHeadersFromSourceAsync();

            // uploadSessionDataTable.Clear();

            uploadSessionDataTable = sourceTableDataFlow.DataTable;
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            var sessionFileSize = await sourceTableDataFlow.GetPageFromSourceAsync();
            var deviceDrivePath = dbFilePath.Data.Substring(0, dbFilePath.Data.LastIndexOf("db", StringComparison.Ordinal));
            // Begin validation and formatting DataTable

            if (dataTransactFlag)
                return new Tuple<int, int>(uploadSessionDataTable.Rows.Count, uploadSessionDataTable.Rows.Count);

            // step 1: validate device polaris settings, return true if device support upload session
            if (await ValidatePolarisSettings(deviceDrivePath, uploadSessionDataTable))
            {
                // step 1: check duplicate animal ids
                var duplicateRows = CheckDuplicateAnimalsInDataTable(uploadSessionDataTable);

                // step 2: remove duplicate rows in data table
                RemoveRows(uploadSessionDataTable, duplicateRows, true);
            }
                
            dataTransactFlag = true;

            return new Tuple<int, int>(0, uploadSessionDataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        // private field
        async Task<bool> ValidatePolarisSettings(string deviceDrive, DataTable sessionData)
        {
            // read polaris file for limitation check
            var infoFilePath = Path.Combine(deviceDrive, "polaris.xml");
            var polaris = XElement.Load(infoFilePath);
            var polarisElements = polaris.Elements();
            var polarisSetting = polarisElements.ToDictionary(element => element.Name.ToString(), element => element.Value);
            var dbResponse = new DataTable();

            // get database status and validate
            var sessionCountQuery = "SELECT COUNT(*) AS 'sessionCount' FROM SessionTable";
            dbResponse = await dbRequest.SendRequest(sessionCountQuery);
            var sessionCount = Convert.ToInt32(dbResponse.Rows[0][0]);
            var maxSessions = polarisSetting.ContainsKey("maxSessions") ? Int32.Parse(polarisSetting["maxSessions"]) : 0;
            if (sessionCount + 1 > maxSessions)
            {
                System.Diagnostics.Debug.WriteLine($"Device does not support upload more than {maxSessions} sessions.");
                throw new ArgumentException($"Device does not support upload more than {maxSessions} sessions.");
            }

            var weightRecordsCountQuery = "SELECT COUNT(*) AS 'weightRecordsCount' FROM WeightTable";
            dbResponse = await dbRequest.SendRequest(weightRecordsCountQuery);
            var weightRecordsCount = Convert.ToInt32(dbResponse.Rows[0][0]);
            var maxWeightRecords = polarisSetting.ContainsKey("maxWeightRecords") ? Int32.Parse(polarisSetting["maxWeightRecords"]) : 0;
            if (weightRecordsCount + sessionData.Rows.Count - 5 > maxWeightRecords) // 5 is now specified as non-record rows.
            {
                System.Diagnostics.Debug.WriteLine($"Device does not support upload more than {maxWeightRecords} weight table records.");
                throw new ArgumentException($"Device does not support upload more than {maxWeightRecords} weight table records.");
            }

            var animalCountQuery = "SELECT COUNT(*) FROM AnimalTable";
            dbResponse = await dbRequest.SendRequest(animalCountQuery);
            var animalCount = Convert.ToInt32(dbResponse.Rows[0][0]);
            var maxAnimalRecords = polarisSetting.ContainsKey("maxAnimalRecords") ? Int32.Parse(polarisSetting["maxAnimalRecords"]) : 0;
            if (animalCount > maxAnimalRecords)
            {
                System.Diagnostics.Debug.WriteLine($"Animal records exceed max animal number limit {maxAnimalRecords}.");
                throw new ArgumentException($"Animal records exceed max animal number limit {maxAnimalRecords}.");
            }
            // TODO: count new animal records and then compare

            var customiseColumnQuery = "SELECT key , value" +
                                       " FROM SettingTable" +
                                       " WHERE key = 'AnimalTableModel_customColumnCounter'" +
                                       " OR key = 'WeightTableModel_customColumnCounter'";
            dbResponse = await dbRequest.SendRequest(customiseColumnQuery);

            var customiseColumnCount = Convert.ToInt32(dbResponse.Rows[0][1])
                                       + Convert.ToInt32(dbResponse.Rows[1][1]);
            var maxCustomFields = polarisSetting.ContainsKey("maxCustomFields") ? Int32.Parse(polarisSetting["maxCustomFields"]) : 0;
            if (customiseColumnCount > maxCustomFields)
            {
                System.Diagnostics.Debug.WriteLine($"Device does not support more than {maxCustomFields} customise columns.");
                throw new ArgumentException($"Device does not support more than {maxCustomFields} customise columns.");
            }
            // TODO: count customise upload columns and then compare

            return true;
        }

        /// <summary>
        /// Check duplicate animal ids in all DataRows of dataTable
        /// </summary>
        /// <param name="dataTable">session DataTable for duplication check</param>
        /// <returns>return a list of DataRow index for further remove operation</returns>
        private List<int> CheckDuplicateAnimalsInDataTable(DataTable dataTable)
        {
            var duplicateRows = new List<int>();
            var animalIdColumns = new Dictionary<string, int>(); // e.g. EID, (in column) 1; VID, (in column) 2
            var animalIds = new List<string>() { "VID", "EID", "IDE", "FID", "RFID" };

            int totalRowCount = dataTable.Rows.Count;
            int headerRowIndex = -1;
            bool headerRowSeen = false;

            // check how many animal ids are in DataTable, and which columns are they in
            for (int i = 0; i < 5; i++)
            {
                var row = (DataRow)dataTable.Rows[i];

                if (IsEmptyRow(row))
                    continue;

                for (var columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    var cellValue = row[dataTable.Columns[columnIndex]].ToString();
                    if (animalIds.Any(id => id.ToLower() == cellValue.ToLower()))
                    {
                        // we need to check if this column has duplicate animals in datatable
                        animalIdColumns.Add(cellValue, columnIndex);
                        headerRowSeen = true;
                    }
                }

                if (headerRowSeen)
                {
                    headerRowIndex = i;
                    break;
                }
            }

            foreach (var animalId in animalIdColumns.Keys)
            {
                var tempDuplicateIndex = new Dictionary<string, List<int>>(); // ID string, list of rows
                var columnName = dataTable.Columns[animalIdColumns[animalId]];

                for (var i = totalRowCount - 1; i > headerRowIndex; i--)
                {
                    var row = dataTable.Rows[i];
                    if (IsEmptyRow(row))
                        continue;
                    var idValue = row[columnName].ToString();
                    if (string.IsNullOrEmpty(idValue)) continue;

                    if (tempDuplicateIndex.ContainsKey(idValue))
                        tempDuplicateIndex[idValue].Add(i);
                    else
                        tempDuplicateIndex.Add(idValue, new List<int>() { i });
                }

                foreach (var kvp in tempDuplicateIndex.Where(kvp => kvp.Value.Count > 1))
                {
                    duplicateRows.AddRange(kvp.Value.GetRange(1, kvp.Value.Count - 1));
                }
            }

            return duplicateRows;
        }

        /// <summary>
        /// Check whether a DataRow is empty or not
        /// </summary>
        /// <param name="dr">one DataRow in a DataTable</param>
        /// <returns>true when DataRow is empty, false if not</returns>
        private bool IsEmptyRow(DataRow dr)
        {
            if (dr == null)
                return true;
            else
                foreach (DataColumn col in dr.Table.Columns)
                    if (dr[col] != DBNull.Value && !String.IsNullOrEmpty(dr[col].ToString()))
                        return false;
            return true;
        }

        /// <summary>
        /// remove duplicate rows in session data table, implement ITableDataFlow
        /// </summary>
        /// <param name="inputDataTable"> input DataTable for remove operation</param>
        /// <param name="rowIndexes"> a list of DataRow index</param>
        /// <param name="remove"> operation flag, remove DataRows when true</param>
        private void RemoveRows(DataTable inputDataTable, List<int> rowIndexes, bool remove = false)
        {
            if (!remove || rowIndexes.Count < 1) return;

            if (rowIndexes.Max() > inputDataTable.Rows.Count) throw new ArgumentException("Input row index exceeds DataTable count.");
            foreach (var rowIndex in rowIndexes.OrderByDescending(r => r)) inputDataTable.Rows.RemoveAt(rowIndex);
        }
    }
}
