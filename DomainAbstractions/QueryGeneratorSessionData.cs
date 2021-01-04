using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    ///  QueryGeneratorSessionData abstraction is used to get session data from query devices and upload session file to them.
    /// Input ports:
    /// 1. IDataFlow<string>: session file primary key, sessionid for USB device.
    /// 2. ITableDataFlow: Usually wired from Transact as data source to get session data from device.
    /// Output ports:
    /// 1. IRequestResponseDataFlow<string, DataTable> dbRequestQuery: database query request.
    /// 2. IRequestResponseDataFlow<string, int> dbRequestNonQuery: database execute non query request.
    /// 3. IRequestResponseDataFlow<string, SQLiteTransaction> dbRequestTransaction: database Transact request, TODO: can be used to support Rollback database operation.
    /// 4. IRequestResponseDataFlow<Dictionary<string, object>, string> sessionDataQueryRequest: query command request to Query builder.
    /// 5. IDataFlowB<string> dbFilePath: database file path.
    /// 6. IDataFlow<string> workingDbFilePath: working database file path. When operating database, need to backup and change its name first.
    /// 
    /// New added: 2020-05-29 by Shuwei
    /// 7. IDataFlowB<string> sessionNumberDataFlow: session number data from connector
    /// 8. IDataFlowB<string> sessionNameDataFlow: session name data from connector
    /// 9. IDataFlowB<string> sessionDateDataFlow: session date data from connector
    /// 10. IDataFlowB<string> sessionDateStampDataFlow: session time stamp data from connector
    /// </summary>
    public class QueryGeneratorSessionData : IDataFlow<string>, ITableDataFlow
    {
        // public properties
        public string InstanceName = "Default";
        public string deviceName = "XR5000";

        // ports
        private IRequestResponseDataFlow_B<string, DataTable> dbRequestQuery;
        private IRequestResponseDataFlow_B<string, int> dbRequestNonQuery;
        private IRequestResponseDataFlow_B<string, SQLiteTransaction> dbRequestTransaction;
        private IRequestResponseDataFlow_B<Dictionary<string, object>, string> sessionDataQueryRequest;
        private IDataFlowB<string> dbFilePath;
        // private IDataFlowB<string> sessionFilePath;
        private IDataFlow<string> workingDbFilePath;
        private IDataFlowB<string> sessionNumberDataFlow;
        private IDataFlowB<string> sessionNameDataFlow;
        private IDataFlowB<string> sessionDateDataFlow;
        private IDataFlowB<string> sessionDateStampDataFlow;

        // private fields
        private Dictionary<string, DataTable> cachedSessionData = new Dictionary<string, DataTable>();
        private Dictionary<string, int> sessionRecordsCount = new Dictionary<string, int>();
        private DataTable sessionDataHeader;
        private long rowId = 1;
        private string sessionNumber;
        private string sessionName;
        private string sessionDate;
        private string sessionDateStamp;
        private long currentSessionIndex;

        // cache flag
        private bool cacheFlag = false;
        private string currentSessionPrimaryKey;
        private DataTable currentDataTable = new DataTable();

        public QueryGeneratorSessionData() { }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => currentSessionPrimaryKey = value; }

        // ITableDataFlow implementation
        DataTable ITableDataFlow.DataTable => currentDataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            // string format session primary key: sessionid from database
            string primaryKey = currentSessionPrimaryKey;

            // cache data if has queried
            if (cachedSessionData.ContainsKey(primaryKey)
                && cachedSessionData[primaryKey].Rows.Count > 0
                && cachedSessionData[primaryKey].Columns.Count > 0)
            {
                // cache data table exists.
                cacheFlag = true;
                currentDataTable = cachedSessionData[primaryKey];
            }
            else
            {
                // cache data table for current session
                currentDataTable = new DataTable();
                cachedSessionData[primaryKey] = currentDataTable.Copy();

                #region Get dynamic session information from database
                // 1. Get dynamic session columns from database using old format query container (dictionary container, TBC: change into JSON format)
                var sessionHeaderQuerySyntax = CreateSqlSelectSyntax();
                // 1.1 add query column information
                SelectQuery(sessionHeaderQuerySyntax, new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("FieldAttributeTable.labelname", ""),
                    new Tuple<string, string>("FieldAttributeTable.location", "location"),
                    new Tuple<string, string>("FieldAttributeTable.fieldname", "fieldname"),
                    new Tuple<string, string>("SessionTable.sessionname", "name"),
                    new Tuple<string, string>("SessionTable.sessioncount", "count"),
                    new Tuple<string, string>("SessionTable.sessionstartdate", "date")
                });
                // 1.2 add query table information
                var fromTable = sessionHeaderQuerySyntax["FROM"] as List<string>;
                fromTable?.Add("FieldAttributeTable");
                sessionHeaderQuerySyntax["FROM"] = fromTable;
                // 1.3 add join constraint
                var joinTable = sessionHeaderQuerySyntax["LEFT JOIN"] as List<string>;
                joinTable?.Add("FieldLayoutTable");
                joinTable?.Add("SessionTable");
                sessionHeaderQuerySyntax["LEFT JOIN"] = joinTable;
                // 1.4 add join condition
                var onCondition = sessionHeaderQuerySyntax["ON"] as List<string>;
                onCondition?.Add("FieldAttributeTable.fieldname = FieldLayoutTable.columnname");
                onCondition?.Add("FieldLayoutTable.sessionid = SessionTable.sessionid");
                sessionHeaderQuerySyntax["ON"] = onCondition;
                // 1.4 add filtering condition
                FilterQuery(sessionHeaderQuerySyntax, new Tuple<string, string, string>("FieldLayoutTable.sessionid", "=", primaryKey));
                // FilterQuery(sessionHeaderQuerySyntax, new Tuple<string, string, string>("FieldLayoutTable.layoutkey", "=", "DataTableFieldLayout"));
                FilterQuery(sessionHeaderQuerySyntax, new Tuple<string, string, string>("FieldLayoutTable.layoutkey", "IN", @"('DataTableFieldLayout', '')"));
                // 1.5 add sorting condition
                SortQuery(sessionHeaderQuerySyntax, new Tuple<string, string>("FieldLayoutTable.fieldorder", "ASC"));
                // 2. Compile query command using query builder
                var sessionDataHeaderComm = await sessionDataQueryRequest.SendRequest(sessionHeaderQuerySyntax);
                // 3. Query from database for result
                sessionDataHeader = await dbRequestQuery.SendRequest(sessionDataHeaderComm);
                #endregion

                #region Process with query result
                // Add columns into current data table
                foreach (DataRow headerRow in sessionDataHeader.Rows)
                {
                    var sessionLabelName = headerRow["labelname"].ToString();
                    currentDataTable.Columns.Add(sessionLabelName);
                }
                // Add a index column and hide it
                currentDataTable.Columns.Add(new DataColumn("index") { Prefix = "hide" });
                // Store session count information into dictionary
                if (sessionDataHeader.Rows.Count > 0)
                {
                    sessionRecordsCount[primaryKey] = int.Parse(sessionDataHeader.Rows[0]["count"].ToString());
                    currentDataTable.TableName = $"FileNo: {primaryKey};Name: {sessionDataHeader.Rows[0]["name"].ToString()};Date: {sessionDataHeader.Rows[0]["date"].ToString()}";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"==========> Empty query result for session {primaryKey}, using query command \n{sessionDataHeaderComm}");
                    throw new ArgumentException($"Empty query result for session {primaryKey}");
                }
                #endregion
            }
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            // string format session primary key: sessionid from database
            string primaryKey = currentSessionPrimaryKey; 

            // cache exist and the current table is going to be displayed
            if (cacheFlag)
            {
                cacheFlag = false;
                return new Tuple<int, int>(0, currentDataTable.Rows.Count);
            }
            if (sessionRecordsCount.ContainsKey(primaryKey)
                && currentDataTable.Rows.Count >= sessionRecordsCount[primaryKey])
            {
                return new Tuple<int, int>(currentDataTable.Rows.Count, currentDataTable.Rows.Count);
            }
            int rowStartIndex = currentDataTable.Rows.Count;

            // Get session data from database
            var sessionDataQuerySyntax = CreateSqlSelectSyntax();
            var sessionDataSelectList = new List<Tuple<string, string>>();

            foreach (DataRow sessionDataRow in sessionDataHeader.Rows)
            {
                if (sessionDataRow["location"].ToString().ToLower().Contains("treatmenttable")
                    || sessionDataRow["location"].ToString().ToLower().Contains("weighttable"))
                {
                    sessionDataSelectList.Add(new Tuple<string, string>($"NewWeightTable.{sessionDataRow["fieldname"]}", sessionDataRow["labelname"].ToString()));
                }
                else
                {
                    sessionDataSelectList.Add(new Tuple<string, string>($"{sessionDataRow["location"]}.{sessionDataRow["fieldname"]}", sessionDataRow["labelname"].ToString()));
                }
            }

            SelectQuery(sessionDataQuerySyntax, sessionDataSelectList);

            string newWeightTableComm = await CreateNewWeightTable(primaryKey);

            var fromTable = sessionDataQuerySyntax["FROM"] as List<string>;
            fromTable?.Add($"({newWeightTableComm}) AS NewWeightTable");
            sessionDataQuerySyntax["FROM"] = fromTable;

            var joinTable = sessionDataQuerySyntax["LEFT JOIN"] as List<string>;
            joinTable?.Add("AnimalTable");
            sessionDataQuerySyntax["LEFT JOIN"] = joinTable;

            var onCondition = sessionDataQuerySyntax["ON"] as List<string>;
            onCondition?.Add("NewWeightTable.animalid = AnimalTable.animalid");
            sessionDataQuerySyntax["ON"] = onCondition;

            FilterQuery(sessionDataQuerySyntax, new Tuple<string, string, string>("NewWeightTable.sessionid", "=", primaryKey));

            // var limitCondition = new List<string>(){$"{limitNumber}" };
            // sessionDataQuerySyntax["LIMIT"] = limitCondition;
            //
            // var offsetCondition = new List<string>(){ { $"{offsetIndex}" } };
            // sessionDataQuerySyntax["OFFSET"] = offsetCondition;
            // offsetIndex += limitNumber;

            var sessionDataComm = await sessionDataQueryRequest.SendRequest(sessionDataQuerySyntax);

            var sessionData = await dbRequestQuery.SendRequest(sessionDataComm);

            sessionData.Columns.Add(new DataColumn("index") { Prefix = "hide" });

            foreach (DataRow dataRow in sessionData.Rows)
            {
                dataRow["index"] = rowId++;
                currentDataTable.Rows.Add(dataRow.ItemArray);
            }

            return new Tuple<int, int>(rowStartIndex, currentDataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            // currentDataTable = new DataTable();

        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex,
            GetNextPageDelegate getNextPage)
        {
            var uploadSessionDataTable = currentDataTable.Copy();
            // var uploadSessionFilePath = sessionFilePath.Data;
            var fieldAttributeTable = await CreateFieldAttributeTable(dbRequestQuery);

            // step 1: check duplicate animal ids
            // var duplicateRows = CheckDuplicateAnimalsInDataTable(uploadSessionDataTable);

            // step 2: remove duplicate rows in data table
            // RemoveRows(uploadSessionDataTable, duplicateRows, true);

            // step 3: remove empty and duplicate columns
            // ValidateColumnLabels(uploadSessionDataTable, 4);

            // step 4: delete table header
            // step 4.1 initialise new session IntializeNewSession
            // InitialiseSession(uploadSessionFilePath, uploadSessionDataTable, out sessionName, out sessionDate);

            // step 4.2 remove empty columns
            // remove empty columns
            // var skipRowIndex = 5;
            // RemoveEmptyColumns(uploadSessionDataTable, skipRowIndex);

            // step 4.3 get dynamic fields from table
            var dynamicFields = GetDynamicFields(uploadSessionDataTable, 2);
            // var dynamicFields = GetDynamicFields(uploadSessionDataTable, 5);

            // step 4.4 remove headers in session data table
            for (int i = 0; i < 2; i++)
            {
                uploadSessionDataTable.Rows.RemoveAt(0);
            }

            // step 5: create and complete upload session
            // dynamicField is from upload session, dynamicFieldRow is new created or existed filed.
            // step 5.1: update dynamicFields and change uploadSessionDataTable column names
            foreach (DataRow sessionDynamicField in dynamicFields.Rows)
            {
                var dynamicFieldRow = FindOrCreateDynamicField(deviceName, fieldAttributeTable, sessionDynamicField);

                // update dynamicFields row
                sessionDynamicField.ItemArray = dynamicFieldRow.ItemArray;

                PerformTableChange(uploadSessionDataTable, dynamicFieldRow, sessionDynamicField);
            }

            // step 5.2: add dateStamp column if does not exist
            DataRow dateRow = fieldAttributeTable.AsEnumerable().Single(dr =>
                dr["TableType"].ToString().Equals("1") &&
                dr["Name"].ToString().ToLower().Equals("datestamp"));

            if (!dynamicFields.AsEnumerable().Any(dr => dr["Name"].ToString().ToLower().Equals("datestamp")))
            {
                DataRow sessionDateRow = dynamicFields.NewRow();
                sessionDateRow.ItemArray = dateRow.ItemArray;
                dynamicFields.Rows.Add(sessionDateRow);

                if (uploadSessionDataTable != null)
                {
                    uploadSessionDataTable.Columns.Add(dateRow["Name"].ToString(), typeof(string));
                    // foreach (DataRow row in uploadSessionDataTable.Rows)
                    //     if (row.ItemArray.Where(x => !String.IsNullOrWhiteSpace(x.ToString())).ToArray().Length > 0)
                    //         row[dateRow["Name"].ToString()] = sessionDate;
                }
            }
            foreach (DataRow row in uploadSessionDataTable.Rows)
                if (row.ItemArray.Where(x => !String.IsNullOrWhiteSpace(x.ToString())).ToArray().Length > 0)
                    row[dateRow["Name"].ToString()] = sessionDateStamp;

            // step 5.3: update dynamicFields DisplayLength and Length columns
            var dynamicFieldLength = new Dictionary<string, Tuple<string, string>>()
                {
                    {"generaltype", new Tuple<string, string>("16", "50")},
                    {"timetype", new Tuple<string, string>("8", "8")},
                    {"datetype", new Tuple<string, string>("10", "10")},
                    {"numerictype", new Tuple<string, string>("8", "8")},
                    {"numericpositivetype", new Tuple<string, string>("8", "8")},
                    {"listofvaluetype", new Tuple<string, string>("16", "16")}
                };

            foreach (DataRow sessionDynamicField in dynamicFields.Rows)
            {
                string dynamicFieldType = sessionDynamicField["DeviceFieldType"].ToString();

                sessionDynamicField["DisplayLength"] = dynamicFieldLength.ContainsKey(dynamicFieldType)
                    ? dynamicFieldLength[dynamicFieldType].Item1
                    : "16";

                sessionDynamicField["Length"] = dynamicFieldLength.ContainsKey(dynamicFieldType)
                    ? dynamicFieldLength[dynamicFieldType].Item2
                    : "50";

                // update data in fieldAttributeTable.
                foreach (DataRow fieldAttribute in fieldAttributeTable.Rows)
                {
                    if (fieldAttribute["Name"].ToString().Equals(sessionDynamicField["Name"].ToString()))
                        fieldAttribute.ItemArray = sessionDynamicField.ItemArray;
                }
            }

            // step 6: Begin upload session
            // sessionName, sessionDate and dynamic fields contain information of session file
            // fileAttributeTable contains database field information
            // uploadSessionDataTable contains upload session records

            BackupDb(dbFilePath.Data);
            var newDbFilePath = RenameFile(dbFilePath.Data, "db_working");
            workingDbFilePath.Data = newDbFilePath;

            // step 6.1: prepare insert data

            // ==========> INSERT new dynamic fields into FieldAttributeTable
            // ADD column in AnimalTable or WeightTable
            var fieldAttributeInsertTable = CreateDataTable("FieldAttributeTable", new List<Tuple<string, Type>>()
                {
                    new Tuple<string, Type>("FieldName", null),
                    new Tuple<string, Type>("LabelName", null),
                    new Tuple<string, Type>("FieldType", null),
                    new Tuple<string, Type>("Location", null),
                    new Tuple<string, Type>("ListOfValues", null),
                    new Tuple<string, Type>("isID", null),
                    new Tuple<string, Type>("maxNumOfDisplayChars", null),
                    new Tuple<string, Type>("maxinputlen", null),
                    new Tuple<string, Type>("isRepeat", null),
                    new Tuple<string, Type>("isAutoInc", null),
                    new Tuple<string, Type>("isMandatory", null),
                    new Tuple<string, Type>("Description", null),
                    new Tuple<string, Type>("isDeletedFlag", null)
                });

            foreach (DataRow insertDynamicRow in dynamicFields.AsEnumerable().Where(dr => dr["State"].ToString().Equals("1")))
            {
                var insertFieldAttributeRow = fieldAttributeInsertTable.NewRow();
                insertFieldAttributeRow.ItemArray = insertDynamicRow.ItemArray.ToList().GetRange(0, insertFieldAttributeRow.ItemArray.Length).ToArray();
                fieldAttributeInsertTable.Rows.Add(insertFieldAttributeRow);
            }

            var fieldAttributeTableInsertOperation = CreateSqlInsertSyntax();
            var fieldAttributeTableInsertTableName = fieldAttributeTableInsertOperation["INSERT TABLE"] as string;
            var fieldAttributeTableInsertColumns = fieldAttributeTableInsertOperation["INSERT COLUMNS"] as List<List<string>>;
            var fieldAttributeTableInsertValues = fieldAttributeTableInsertOperation["VALUES"] as List<List<string>>;

            var alterOperation = CreateSqlAlterSyntax();
            var alterTable = alterOperation["ALTER TABLE"] as List<string>;
            var addColumns = alterOperation["ADD COLUMNS"] as List<string>;
            var columnTypes = alterOperation["COLUMN TYPES"] as List<string>;
            var alterIndex = alterOperation["INDEX"] as List<bool>;

            fieldAttributeTableInsertTableName = "FieldAttributeTable";
            foreach (DataRow row in fieldAttributeInsertTable.Rows)
            {
                // Animal table and Weight Table alter
                alterTable.Add(row["FieldName"].ToString().ToLower().StartsWith("atf") ? "AnimalTable" : "WeightTable");
                addColumns.Add(row["FieldName"].ToString());
                columnTypes.Add("VARCHAR(255)");
                alterIndex.Add(row["isID"].ToString().ToLower().Equals("true"));

                // field attribute table insert
                var columnList = new List<string>();
                var valueList = new List<string>();
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.AutoIncrement || row[col] == DBNull.Value || row[col].ToString() == string.Empty)
                        continue;
                    columnList.Add(col.ColumnName);
                    valueList.Add(row[col].ToString());
                }

                fieldAttributeTableInsertColumns.Add(columnList);
                fieldAttributeTableInsertValues.Add(valueList);
            }

            fieldAttributeTableInsertOperation["INSERT TABLE"] = fieldAttributeTableInsertTableName;
            fieldAttributeTableInsertOperation["INSERT COLUMNS"] = fieldAttributeTableInsertColumns;
            fieldAttributeTableInsertOperation["VALUES"] = fieldAttributeTableInsertValues;

            alterOperation["ALTER TABLE"] = alterTable;
            alterOperation["ADD COLUMNS"] = addColumns;
            alterOperation["COLUMN TYPES"] = columnTypes;
            alterOperation["INDEX"] = alterIndex;

            var fieldAttributeInsertComm = await sessionDataQueryRequest.SendRequest(fieldAttributeTableInsertOperation);
            var fieldAttributeInsert = await dbRequestNonQuery.SendRequest(fieldAttributeInsertComm);

            var animalAndWeightTableAlterComm = await sessionDataQueryRequest.SendRequest(alterOperation);
            var animalAndWeightTableAlter = await dbRequestNonQuery.SendRequest(animalAndWeightTableAlterComm);

            // compile and do INSERT, ALTER and INDEX

            // ==========> UPDATE Field Attribute Table
            var fieldAttributeUpdateTable = CreateDataTable("FieldAttributeTable", new List<Tuple<string, Type>>()
                {
                    new Tuple<string, Type>("FieldName", null),
                    new Tuple<string, Type>("LabelName", null),
                    new Tuple<string, Type>("FieldType", null),
                    new Tuple<string, Type>("Location", null),
                    new Tuple<string, Type>("ListOfValues", null),
                    new Tuple<string, Type>("isID", null),
                    new Tuple<string, Type>("maxNumOfDisplayChars", null),
                    new Tuple<string, Type>("maxinputlen", null),
                    new Tuple<string, Type>("isRepeat", null),
                    new Tuple<string, Type>("isAutoInc", null),
                    new Tuple<string, Type>("isMandatory", null),
                    new Tuple<string, Type>("Description", null),
                    new Tuple<string, Type>("isDeletedFlag", null)
                });

            foreach (DataRow updateDynamicRow in dynamicFields.AsEnumerable().Where(dr => dr["State"].ToString().Equals("2")))
            {
                var updateFieldAttributeRow = fieldAttributeUpdateTable.NewRow();
                updateFieldAttributeRow.ItemArray = updateDynamicRow.ItemArray.ToList().GetRange(0, updateFieldAttributeRow.ItemArray.Length).ToArray();
                fieldAttributeUpdateTable.Rows.Add(updateFieldAttributeRow);
            }

            var updateOperation = CreateSqlUpdateSyntax();
            var updateTable = updateOperation["UPDATE TABLE"] as string;
            var updateColumn = updateOperation["UPDATE COLUMN"] as List<List<string>>;
            var updateValue = updateOperation["UPDATE VALUE"] as List<List<string>>;
            var updateFilter = updateOperation["FILTER"] as List<Tuple<string, string, string>>;

            updateTable = fieldAttributeUpdateTable.TableName;

            foreach (DataRow row in fieldAttributeUpdateTable.Rows)
            {
                var tmpUpdateColumn = new List<string>();
                var tmpUpdateValue = new List<string>();

                foreach (DataColumn col in row.Table.Columns)
                {
                    tmpUpdateColumn.Add(col.ColumnName);
                    tmpUpdateValue.Add(row[col] == DBNull.Value ? null : row[col].ToString());
                }

                updateColumn.Add(tmpUpdateColumn);
                updateValue.Add(tmpUpdateValue);
                updateFilter.Add(new Tuple<string, string, string>("FieldName", "=", $"{row["FieldName"]}"));
            }

            updateOperation["UPDATE TABLE"] = updateTable;
            updateOperation["UPDATE COLUMN"] = updateColumn;
            updateOperation["UPDATE VALUE"] = updateValue;
            updateOperation["FILTER"] = updateFilter;

            // compile and do UPDATE

            var fieldAttributeUpdateComm = await sessionDataQueryRequest.SendRequest(updateOperation);
            var fieldAttributeUpdate = await dbRequestNonQuery.SendRequest(fieldAttributeUpdateComm);

            // ==========> INSERT session record
            var sessionTable = CreateDataTable("SessionTable", new List<Tuple<string, Type>>()
                {
                    new Tuple<string, Type>("SessionName", null),
                    new Tuple<string, Type>("SessionStartDate", null),
                    new Tuple<string, Type>("SessionStartTime", null),
                    new Tuple<string, Type>("SessionEndDate", null),
                    new Tuple<string, Type>("SessionEndTime", null),
                    new Tuple<string, Type>("sessiondataentryformfieldlayout", null),
                    new Tuple<string, Type>("sessiondatatablefieldlayout", null)
                });

            DataRow sessionRow = sessionTable.NewRow();
            sessionRow["SessionName"] = sessionName;
            DateTime sessionDateTime = Convert.ToDateTime(sessionDate);
            // sessionRow["SessionStartDate"] = sessionDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            sessionRow["SessionStartDate"] = sessionDateStamp;
            sessionRow["SessionStartTime"] = sessionDateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            // sessionRow["SessionEndDate"] = sessionDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            sessionRow["SessionEndDate"] = sessionDateStamp;
            sessionRow["SessionEndTime"] = sessionDateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

            var sessionInsertOperation = CreateSqlInsertSyntax();
            var sessionInsertTableName = sessionInsertOperation["INSERT TABLE"] as string;
            var sessionInsertColumns = sessionInsertOperation["INSERT COLUMNS"] as List<List<string>>;
            var sessionInsertValues = sessionInsertOperation["VALUES"] as List<List<string>>;

            var sessionColumnList = new List<string>();
            var sessionValueList = new List<string>();
            foreach (DataColumn col in sessionRow.Table.Columns)
            {
                if (col.AutoIncrement || sessionRow[col] == DBNull.Value || sessionRow[col].ToString() == string.Empty)
                    continue;
                sessionColumnList.Add(col.ColumnName);
                sessionValueList.Add(sessionRow[col].ToString());
            }

            // set session dirty column to true
            sessionColumnList.Add("sessiondirty");
            sessionValueList.Add("true");

            sessionInsertTableName = sessionTable.TableName;
            sessionInsertColumns.Add(sessionColumnList);
            sessionInsertValues.Add(sessionValueList);

            sessionInsertOperation["INSERT TABLE"] = sessionInsertTableName;
            sessionInsertOperation["INSERT COLUMNS"] = sessionInsertColumns;
            sessionInsertOperation["VALUES"] = sessionInsertValues;

            // compile and do INSERT

            var sessionInsertComm = await sessionDataQueryRequest.SendRequest(sessionInsertOperation);
            var sessionInsert = await dbRequestNonQuery.SendRequest(sessionInsertComm);

            var currentSessionIdQuery =
                await dbRequestQuery.SendRequest("SELECT seq FROM SQLITE_SEQUENCE WHERE name = 'SessionTable'");
            currentSessionIndex = Convert.ToInt64(currentSessionIdQuery.Rows[0][0]);
            uploadSessionDataTable.Columns.Add("sessionid", typeof(long), currentSessionIndex.ToString());

            // ==========> INSERT or UPDATE Animals
            // insert new animals: does not have matching id
            // update old animals: has matching id
            var currentSessionAnimalIdFields = dynamicFields.AsEnumerable()
                .Where(dr => dr["isID"].ToString().ToLower().Equals("true")).Select(dr => dr["Name"].ToString());
            // search id columns in uploadSessionDataTable, fill with corresponding animal ids
            var animalIdInfoQuery = CreateSqlSelectSyntax();
            var selectList = animalIdInfoQuery["SELECT"] as List<List<Tuple<string, string>>>;
            var fromAnimalTable = animalIdInfoQuery["FROM"] as List<string>;

            selectList.Add(new List<Tuple<string, string>>() { new Tuple<string, string>("animalid", "") });
            foreach (var allAnimalId in currentSessionAnimalIdFields)
            {
                selectList.Add(new List<Tuple<string, string>>() { new Tuple<string, string>(allAnimalId, "") });
            }

            fromAnimalTable.Add("AnimalTable");
            animalIdInfoQuery["SELECT"] = selectList;
            animalIdInfoQuery["FROM"] = fromAnimalTable;

            // compile and do SELECT
            var allAnimalIdComm = await sessionDataQueryRequest.SendRequest(animalIdInfoQuery);
            var allAnimalIdInfo = await dbRequestQuery.SendRequest(allAnimalIdComm);

            // find animal ids
            uploadSessionDataTable.Columns.Add("animalid", typeof(long));
            foreach (DataRow dataRow in uploadSessionDataTable.Rows)
            {
                foreach (var currentSessionAnimalIdField in currentSessionAnimalIdFields)
                {
                    long foundAnimalId = allAnimalIdInfo.AsEnumerable()
                        .Where(dr =>
                            dr[currentSessionAnimalIdField].ToString().Equals(dataRow[currentSessionAnimalIdField]))
                        .Select(dr => Convert.ToInt64(dr["animalid"])).FirstOrDefault();

                    if (foundAnimalId != 0L)
                    {
                        dataRow["animalid"] = foundAnimalId;
                        break;
                    }
                }
            }

            var animalFieldList = dynamicFields.AsEnumerable()
                .Where(dr => dr["location"].ToString().Equals("AnimalTable")).Select(dr => dr["Name"].ToString());

            foreach (DataRow animalRow in uploadSessionDataTable.Rows)
            {
                // INSERT new animals
                if (string.IsNullOrEmpty(animalRow["animalid"].ToString().Trim()))
                {
                    var animalInsertOperation = CreateSqlInsertSyntax();
                    var animalTableInsertTableName = animalInsertOperation["INSERT TABLE"] as string;
                    animalTableInsertTableName = "AnimalTable";
                    var animalTableInsertColumns = animalInsertOperation["INSERT COLUMNS"] as List<List<string>>;
                    var animalTableInsertValues = animalInsertOperation["VALUES"] as List<List<string>>;
                    var tmpAnimalInsertColumn = new List<string>();
                    var tmpAnimalInsertValue = new List<string>();

                    foreach (var animalField in animalFieldList)
                    {
                        if (uploadSessionDataTable.Columns[animalField].AutoIncrement ||
                            animalRow[animalField] == DBNull.Value ||
                            animalRow[animalField].ToString().Equals(string.Empty))
                            continue;
                        tmpAnimalInsertColumn.Add(animalField);
                        tmpAnimalInsertValue.Add(animalRow[animalField].ToString());
                    }

                    animalTableInsertColumns.Add(tmpAnimalInsertColumn);
                    animalTableInsertValues.Add(tmpAnimalInsertValue);

                    animalInsertOperation["INSERT TABLE"] = animalTableInsertTableName;
                    animalInsertOperation["INSERT COLUMNS"] = animalTableInsertColumns;
                    animalInsertOperation["VALUES"] = animalTableInsertValues;

                    var animalInsertComm = await sessionDataQueryRequest.SendRequest(animalInsertOperation);
                    var animalInsert = await dbRequestNonQuery.SendRequest(animalInsertComm);

                    // compile and do INSERT

                    var currentAnimalIdQuery =
                        await dbRequestQuery.SendRequest("SELECT seq FROM SQLITE_SEQUENCE WHERE name = 'AnimalTable'");

                    var currentAnimalId = Convert.ToInt64(currentAnimalIdQuery.Rows[0][0]);
                    animalRow["animalid"] = currentAnimalId;
                }
                // Update existed animals
                else
                {
                    var animalUpdateOperation = CreateSqlUpdateSyntax();
                    var animalTableUpdateName = animalUpdateOperation["UPDATE TABLE"] as string;
                    animalTableUpdateName = "AnimalTable";
                    var animalTableUpdateColumns = animalUpdateOperation["UPDATE COLUMN"] as List<List<string>>;
                    var animalTableUpdateValues = animalUpdateOperation["UPDATE VALUE"] as List<List<string>>;
                    var animalTableUpdateFilter =
                        animalUpdateOperation["FILTER"] as List<Tuple<string, string, string>>;
                    var tmpAnimalUpdateColumn = new List<string>();
                    var tmpAnimalUpdateValue = new List<string>();

                    foreach (var animalField in animalFieldList)
                    {
                        if (animalRow[animalField] == DBNull.Value)
                        {
                            tmpAnimalUpdateColumn.Add(animalField);
                            tmpAnimalUpdateValue.Add(null);
                        }
                        else
                        {
                            tmpAnimalUpdateColumn.Add(animalField);
                            tmpAnimalUpdateValue.Add(animalRow[animalField].ToString());
                        }
                    }

                    animalTableUpdateColumns.Add(tmpAnimalUpdateColumn);
                    animalTableUpdateValues.Add(tmpAnimalUpdateValue);
                    animalTableUpdateFilter.Add(new Tuple<string, string, string>("animalid", "=",
                        animalRow["animalid"].ToString()));

                    animalUpdateOperation["UPDATE TABLE"] = animalTableUpdateName;
                    animalUpdateOperation["UPDATE COLUMN"] = animalTableUpdateColumns;
                    animalUpdateOperation["UPDATE VALUE"] = animalTableUpdateValues;
                    animalUpdateOperation["FILTER"] = animalTableUpdateFilter;

                    // compile and update animal
                    var animalUpdateComm = await sessionDataQueryRequest.SendRequest(animalUpdateOperation);
                    var animalUpdate = await dbRequestNonQuery.SendRequest(animalUpdateComm);
                }
            }

            // ==========> INSERT WeightRecords
            var weightFieldList = dynamicFields.AsEnumerable()
                .Where(dr => dr["location"].ToString().Equals("WeightTable")).Select(dr => dr["Name"].ToString()).ToList();
            weightFieldList.Add("sessionid");
            weightFieldList.Add("animalid");

            var weightInsertOperation = CreateSqlInsertSyntax();
            var weightTableInsertTableName = weightInsertOperation["INSERT TABLE"] as string;
            weightTableInsertTableName = "WeightTable";
            var weightTableInsertColumns = weightInsertOperation["INSERT COLUMNS"] as List<List<string>>;
            var weightTableInsertValues = weightInsertOperation["VALUES"] as List<List<string>>;

            foreach (DataRow weightRow in uploadSessionDataTable.Rows)
            {
                var tmpWeightInsertColumn = new List<string>();
                var tmpWeightInsertValue = new List<string>();

                foreach (var weightField in weightFieldList)
                {
                    if (uploadSessionDataTable.Columns[weightField].AutoIncrement ||
                        weightRow[weightField] == DBNull.Value ||
                        weightRow[weightField].ToString().Equals(string.Empty))
                        continue;
                    tmpWeightInsertColumn.Add(weightField);
                    tmpWeightInsertValue.Add(weightRow[weightField].ToString());
                }

                weightTableInsertColumns.Add(tmpWeightInsertColumn);
                weightTableInsertValues.Add(tmpWeightInsertValue);
            }

            weightInsertOperation["INSERT TABLE"] = weightTableInsertTableName;
            weightInsertOperation["INSERT COLUMNS"] = weightTableInsertColumns;
            weightInsertOperation["VALUES"] = weightTableInsertValues;

            var weightInsertComm = await sessionDataQueryRequest.SendRequest(weightInsertOperation);
            var weightInsert = await dbRequestNonQuery.SendRequest(weightInsertComm);


            // ==========> Begin update field layout table
            int orderCnt = 0;
            var displayFields = dynamicFields.AsEnumerable().Select(dr => dr["Name"].ToString());

            var fieldLayoutTableInsertOperation = CreateSqlInsertSyntax();
            var fieldLayoutTableInsertTableName = fieldLayoutTableInsertOperation["INSERT TABLE"] as string;
            var fieldLayoutTableInsertColumns = fieldLayoutTableInsertOperation["INSERT COLUMNS"] as List<List<string>>;
            var fieldLayoutTableInsertValues = fieldLayoutTableInsertOperation["VALUES"] as List<List<string>>;

            fieldLayoutTableInsertTableName = "FieldLayoutTable";

            foreach (var fileField in displayFields)
            {
                var displayFieldProperty = await dbRequestQuery.SendRequest(
                    $"SELECT maxnumofdisplaychars, fieldtype, actualMaxDataWidth FROM FieldAttributeTable WHERE fieldname= '{fileField}'");

                var maxNumOfDisplayCharsInDb =
                    displayFieldProperty.Rows[0]["maxnumofdisplaychars"].ToString() == string.Empty
                        ? "16"
                        : displayFieldProperty.Rows[0]["maxnumofdisplaychars"].ToString();
                var fieldType = displayFieldProperty.Rows[0]["fieldtype"].ToString();
                var actualMaxDataWidthInDb = displayFieldProperty.Rows[0]["actualMaxDataWidth"].ToString();

                // field attribute table insert
                var columnList = new List<string>();
                var valueList = new List<string>();

                columnList.AddRange(new string[5] { "sessionid", "columnname", "layoutkey", "isvisible", "fieldorder" });
                valueList.AddRange(new string[5] { currentSessionIndex.ToString(), fileField, "", "true", orderCnt.ToString()});

                orderCnt += 1;

                fieldLayoutTableInsertColumns.Add(columnList);
                fieldLayoutTableInsertValues.Add(valueList);
            }

            fieldLayoutTableInsertOperation["INSERT TABLE"] = fieldLayoutTableInsertTableName;
            fieldLayoutTableInsertOperation["INSERT COLUMNS"] = fieldLayoutTableInsertColumns;
            fieldLayoutTableInsertOperation["VALUES"] = fieldLayoutTableInsertValues;

            var fieldLayoutTableInsertComm = await sessionDataQueryRequest.SendRequest(fieldLayoutTableInsertOperation);
            var fieldLayoutInsert = await dbRequestNonQuery.SendRequest(fieldLayoutTableInsertComm);

            // rename database after manipulation
            newDbFilePath = RenameFile(newDbFilePath, "db");
            workingDbFilePath.Data = newDbFilePath;
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { throw new NotImplementedException(); }

        // POST WIRING: get session name, session date and other information from connectors
        private void PostWiringInitialize()
        {
            if (sessionNumberDataFlow != null) sessionNumberDataFlow.DataChanged += () => sessionNumber = sessionNumberDataFlow.Data;
            if (sessionNameDataFlow != null) sessionNameDataFlow.DataChanged += () => sessionName = sessionNameDataFlow.Data;
            if (sessionDateDataFlow != null) sessionDateDataFlow.DataChanged += () => sessionDate = sessionDateDataFlow.Data;
            if (sessionDateStampDataFlow != null) sessionDateStampDataFlow.DataChanged += () => sessionDateStamp = sessionDateStampDataFlow.Data;
        }


        #region Processing region: Create table, Parse dynamic fields, etc.
        /// <summary>
        /// Query and create field attribute table from database, the content in the table should match with the database FieldAttributeTable structure
        /// </summary>
        /// <param name="dbQuery"> db query request </param>
        /// <returns></returns>
        async Task<DataTable> CreateFieldAttributeTable(IRequestResponseDataFlow_B<string, DataTable> dbQuery)
        {
            var fieldAttributeQuery = "SELECT fieldname AS 'Name'," +
                                      " labelname AS 'Label'," +
                                      " fieldtype AS 'DeviceFieldType'," +
                                      " location," +
                                      " listofvalues AS 'Listofvalues'," +
                                      " isID AS 'IsID'," +
                                      " maxNumOfDisplayChars AS 'DisplayLength'," +
                                      " maxinputlen AS 'Length'," +
                                      " isRepeat AS 'IsRepeat'," +
                                      " isAutoInc AS 'IsAutoIncrement'," +
                                      " isMandatory AS 'IsMandatory'," +
                                      " description AS 'Description'," +
                                      " isDeletedFlag AS 'IsDeleted'," +
                                      " isAutoCalculation," +
                                      " isNewFieldFlag AS 'IsNew'," +
                                      " actualMaxDataWidth," +
                                      " 0 AS 'TableType'," +
                                      " 0 AS 'State'," +
                                      " '' AS 'DynamicFieldKey'," +
                                      " '' AS 'Format'," +
                                      " '' AS 'DisplayOrder'," +
                                      " '' AS 'IsVisible'," +
                                      " 'false' AS 'ReuseDataType'" +
                                      " FROM FieldAttributeTable" +
                                      " WHERE location in ('AnimalTable', 'WeightTable', 'TreatmentTable')";

            var fieldAttributeTable = await dbQuery.SendRequest(fieldAttributeQuery);

            if (fieldAttributeTable == null)
                throw new ArgumentException($"Field Attribute Table is empty using query command {fieldAttributeQuery}.");

            foreach (DataRow dataRow in fieldAttributeTable.Rows)
            {
                var location = dataRow["location"];
                dataRow["TableType"] = (location.Equals("WeightTable")
                    ? 1
                    : (location.Equals("AnimalTable") ? 2 : (location.Equals("TreatmentTable") ? 3 : 0)));
            }

            return fieldAttributeTable;
        }

        /// <summary>
        /// Use header information and header label value such as DW2Weight() to search and complete field information. This method is used to translate the headers and header information.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="infoRowCount"> real record row begin index</param>
        /// <returns>return a DataTable contains all field information got from upload session file.</returns>
        private DataTable GetDynamicFields(DataTable dataTable, int infoRowCount)
        {
            DataTable dynamicFieldsTable = CreateDynamicFieldTable();
            var deviceFieldTypeDict = new Dictionary<string, string>()
            {
                {"2", "numerictype" },
                {"3", "listofvaluetype" },
                {"4", "datetype" }
            };

            foreach (DataColumn col in dataTable.Columns)
            {
                DataRow dynamicRow = dynamicFieldsTable.NewRow();

                if (dataTable.Rows[infoRowCount - 2][col].ToString().ToLower().StartsWith("dw"))
                {
                    dynamicRow["Name"] = "DW";
                    dynamicRow["DeviceFieldType"] = "numerictype";
                    dynamicRow["IsID"] = false;
                    dynamicRow["TableType"] = 1;
                    dynamicRow["Label"] = dataTable.Rows[infoRowCount - 1][col].ToString();
                    dynamicRow["Listofvalues"] = string.Empty;
                }
                else if (dataTable.Rows[infoRowCount - 1][col].ToString().ToLower().Contains("draft"))
                {
                    dynamicRow["Name"] = "DR";
                    dynamicRow["IsID"] = false;
                    dynamicRow["TableType"] = 1;
                    dynamicRow["Label"] = dataTable.Rows[infoRowCount - 1][col].ToString();
                }
                else if (dataTable.Rows[infoRowCount - 2][col].ToString().ToUpper().StartsWith("T"))
                {
                    string colDescription = dataTable.Rows[infoRowCount - 2][col].ToString();
                    dynamicRow["Label"] = dataTable.Rows[infoRowCount - 1][col].ToString().Trim();
                    dynamicRow["Name"] = colDescription.Substring(0,
                        colDescription.ToLower().IndexOf(dynamicRow["Label"].ToString().ToLower(),
                            StringComparison.Ordinal) - 1);
                    var dynamicFieldDeviceFieldType = colDescription.Substring(
                        colDescription.ToLower().IndexOf(dynamicRow["Label"].ToString().ToLower(),
                            StringComparison.Ordinal) - 1, 1);
                    dynamicRow["DeviceFieldType"] = deviceFieldTypeDict.ContainsKey(dynamicFieldDeviceFieldType)
                        ? deviceFieldTypeDict[dynamicFieldDeviceFieldType]
                        : "generaltype";
                    dynamicRow["TableType"] = 3;
                    dynamicRow["Listofvalues"] = string.Empty;
                }
                else
                {
                    var colDescription = dataTable.Rows[infoRowCount - 2][col].ToString();
                    var dynamicFieldLabel = dataTable.Rows[infoRowCount - 1][col].ToString().Trim();


                    int filedNum = 0;
                    if ((!colDescription.ToLower().StartsWith("f") &&
                         !colDescription.ToLower().StartsWith("c")) ||
                        (!int.TryParse(colDescription.Substring(1, 1), out filedNum)))
                        continue;

                    int openBracketIndex = colDescription.IndexOf("(", StringComparison.Ordinal);
                    var dynamicFieldName = colDescription.Substring(0,
                        colDescription.ToLower().LastIndexOf(dynamicFieldLabel.ToLower(), StringComparison.Ordinal) - 1);
                    dynamicRow["Name"] = dynamicFieldName;
                    dynamicRow["Label"] = dynamicFieldLabel;

                    var dynamicFieldDeviceFieldType = colDescription.Substring(
                        colDescription.ToLower().LastIndexOf(dynamicFieldLabel.ToLower(), StringComparison.Ordinal) - 1,
                        1);
                    dynamicRow["DeviceFieldType"] = deviceFieldTypeDict.ContainsKey(dynamicFieldDeviceFieldType)
                        ? deviceFieldTypeDict[dynamicFieldDeviceFieldType]
                        : "generaltype";

                    int length = 0;
                    string columnParam = "";
                    try
                    {
                        columnParam = colDescription.Substring(openBracketIndex + 1,
                            colDescription.LastIndexOf(")", StringComparison.Ordinal) - openBracketIndex - 1);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    if (columnParam == "" || !int.TryParse(columnParam, out length))
                        length = dataTable.AsEnumerable()
                            .Where(r => dataTable.Rows.IndexOf(r) >= infoRowCount)
                            .Max(r => r[col].ToString().Length);
                    dynamicRow["Length"] = length;

                    dynamicRow["IsID"] = colDescription.ToLower().Contains("isid") ? true : false;
                    if (colDescription.ToLower().StartsWith("f"))
                        dynamicRow["TableType"] = 2;
                    else
                        dynamicRow["TableType"] = 1;
                    if (dynamicRow["DeviceFieldType"].ToString().Equals("listofvaluetype"))
                        dynamicRow["Listofvalues"] = string.Join(",",
                            dataTable.AsEnumerable()
                                .Where(r => dataTable.Rows.IndexOf(r) >= infoRowCount &&
                                            !string.IsNullOrEmpty(r[col].ToString())).Select(r => r[col].ToString())
                                .Select(r => r).Distinct().ToList());
                    else
                        dynamicRow["Listofvalues"] = string.Empty;
                }

                // Handle Draft Field
                if (dataTable.Rows[infoRowCount - 1][col].ToString().ToLower().Contains("draft"))
                {
                    dynamicRow["DeviceFieldType"] = "listofvaluetype";
                    dynamicRow["Listofvalues"] = string.Join(",",
                        dataTable.AsEnumerable()
                            .Where(r => dataTable.Rows.IndexOf(r) >= infoRowCount &&
                                        !string.IsNullOrEmpty(r[col].ToString()))
                            .Select(r => r[col].ToString())
                            .Distinct().ToList());
                }

                dynamicFieldsTable.Rows.Add(dynamicRow);
                col.ColumnName = dynamicRow["Label"].ToString();

            }

            return dynamicFieldsTable;
        }

        /// <summary>
        /// Create an empty DataTable to contain dynamic fields, this is used after preprocess with upload session files, begin to search all fields in session DataTable.
        /// </summary>
        /// <returns>an empty DataTable for containing dynamic fields information use.</returns>
        private DataTable CreateDynamicFieldTable()
        {
            // !!! This table format should match with field attribute table query format
            DataTable table = new DataTable();
            table.Columns.Add("Name"); // field name => fieldname
            table.Columns.Add("Label"); // shown name => labelname
            table.Columns.Add("DeviceFieldType"); // numerictype, listofvaluetype, datetype or generaltype => fieldtype
            table.Columns.Add("location"); // => location
            table.Columns.Add("Listofvalues"); // => listofvalues
            table.Columns.Add("IsID").DefaultValue = "false"; // true or false => isID
            table.Columns.Add("DisplayLength"); // => maxNumOfDisplayChars
            table.Columns.Add("Length"); // => maxinputlen
            table.Columns.Add("IsRepeat").DefaultValue = "false"; // => isRepeat
            table.Columns.Add("IsAutoIncrement").DefaultValue = "false"; // => isAutoInc
            table.Columns.Add("IsMandatory").DefaultValue = "false"; // => isMandatory
            table.Columns.Add("Description"); // => description
            table.Columns.Add("IsDeleted").DefaultValue = "false"; // => isDeletedFlag
            table.Columns.Add("isAutoCalculation").DefaultValue = "false"; // => isAutoCalculation
            table.Columns.Add("IsNew").DefaultValue = "false"; // => isNewFieldFlag
            table.Columns.Add("actualMaxDataWidth"); // => actualMaxDataWidth
            table.Columns.Add("TableType"); // 1 for WeightTable, 2 for AnimalTable, 3 for TreatmentTable. => location, need translate from int
            table.Columns.Add("State").DefaultValue = 0; // 1 for inserting new field, 2 for updating field
            table.Columns.Add("DynamicFieldKey");
            table.Columns.Add("Format");
            table.Columns.Add("DisplayOrder");
            table.Columns.Add("IsVisible").DefaultValue = "false";
            table.Columns.Add("ReuseDataType");

            return table;
        }


        /// <summary>
        /// Find Or Create Dynamic Field, return a DataRow contains all dynamic field information
        /// </summary>
        /// <param name="fieldAttributeTable"> query from database table: FieldAttributeTable, contains all field information in database, added some columns for operation</param>
        /// <param name="dynamicFields"> get and created by GetDynamicFields method from upload session file</param>
        /// <param name="dynamicField"> a dynamic field DataRow in dynamicFields</param>
        /// <returns> return a DataRow contains all needed information, an existed DataRow in fieldAttributeTable or a new created DataRow</returns>
        private DataRow FindOrCreateDynamicField(string deviceName, DataTable fieldAttributeTable, DataRow dynamicField)
        {
            var dynamicFieldLabel = dynamicField["Label"].ToString();
            var animalEidCaptions = new string[3] { "EID", "IDE", "RFID" };
            var animalVidCaptions = new string[2] { "VID", "FID" };

            // try to search dynamic field row in fieldAttributeTable
            DataRow dynamicFieldRow = fieldAttributeTable.AsEnumerable().FirstOrDefault(dr =>
                dr["Label"].ToString().ToLower().Equals(dynamicFieldLabel.ToLower()));

            if (dynamicFieldRow == null && deviceName.Contains("ID5000"))
            {
                if (animalEidCaptions.Contains(dynamicFieldLabel.ToUpper()))
                {
                    dynamicFieldRow = fieldAttributeTable.AsEnumerable()
                        .Where(dr => dr["Name"].ToString().Equals("eid"))
                        .Select(dr => dr).FirstOrDefault();
                }
                else if (animalVidCaptions.Contains(dynamicFieldLabel.ToUpper()))
                {
                    dynamicFieldRow = fieldAttributeTable.AsEnumerable()
                        .Where(dr => dr["Name"].ToString().Equals("vid"))
                        .Select(dr => dr).FirstOrDefault();
                }
            }

            // field exists in current fieldAttributeTable, do update operation
            if (dynamicFieldRow != null)
            {
                const int LOVListSizeLimitation = 30;
                dynamicFieldRow["State"] = 2;

                if (dynamicFieldRow["DeviceFieldType"].ToString().Equals(dynamicField["DeviceFieldType"].ToString()))
                    dynamicFieldRow["ReuseDataType"] = true;
                else
                    dynamicFieldRow["ReuseDataType"] = false;

                if (dynamicFieldRow["DeviceFieldType"].ToString().Equals(dynamicField["DeviceFieldType"].ToString()) &&
                    dynamicField["DeviceFieldType"].ToString().Equals("listofvaluetype") &&
                    !dynamicField["Listofvalues"].ToString().Equals(dynamicFieldRow["Listofvalues"].ToString()))
                {
                    List<string> exisitingLov =
                        dynamicFieldRow["Listofvalues"].ToString().Split(',').Where(v => !String.IsNullOrEmpty(v)).ToList();
                    List<string> fileLov =
                        dynamicField["Listofvalues"].ToString().Split(',').Where(v => !String.IsNullOrEmpty(v)).ToList();
                    foreach (string lov in fileLov)
                    {
                        if (exisitingLov.Count < LOVListSizeLimitation && exisitingLov.All(v => !String.Equals(v, lov, StringComparison.CurrentCultureIgnoreCase)))
                            exisitingLov.Add(lov);
                    }
                    dynamicFieldRow["Listofvalues"] = string.Join(",", exisitingLov.Distinct().ToArray());
                }

                if (bool.Parse(dynamicFieldRow["IsDeleted"].ToString()))
                    dynamicFieldRow["IsDeleted"] = "false";


            }
            else // field does not exist in current fieldAttributeTable, do insert operation
            {
                // check customise columns number limit
                // int allCustomFieldCount = fieldAttributeTable.AsEnumerable().Count(dr =>
                //     dr["Name"].ToString().StartsWith("wtf") || dr["Name"].ToString().StartsWith("atf"));
                // if (allCustomFieldCount >= maxCustomFields) throw new ArgumentException($"Device does not support more than {maxCustomFields} customise columns.");

                // create new dynamic field, fill information in fieldAttributeTable
                // dynamicFieldRow = dynamicFields.AsEnumerable().FirstOrDefault(dr => dr["Label"].ToString().Equals(dynamicFieldLabel));
                dynamicFieldRow = dynamicField;

                dynamicFieldRow["State"] = 1;
                string newFieldName;

                // does not support TreatmentTable insert currently, old DataLink - ExportManagerScale5000 - Line107
                if (Convert.ToInt32(dynamicFieldRow["TableType"]) == 3)
                {
                    dynamicFieldRow["TableType"] = 1;
                    dynamicFieldRow["location"] = "WeightTable";
                }

                // new animal table field
                if (Convert.ToInt32(dynamicFieldRow["TableType"]) == 2)
                {
                    dynamicFieldRow["location"] = "AnimalTable";

                    // get max atf count index in FieldAttributeTable
                    var maxAtfIndexList = fieldAttributeTable.AsEnumerable()
                        .Where(dr => dr["Name"].ToString().StartsWith("atf"))
                        .Select(dr => Int32.Parse(Regex.Replace(dr["Name"].ToString(), @"^atf0*", "")));

                    var newAtfIndex = maxAtfIndexList.Any() ? maxAtfIndexList.Max() + 1 : 1;

                    newFieldName = $"atf{newAtfIndex.ToString().PadLeft(4, '0')}";

                }
                else // new weight table field
                {
                    dynamicFieldRow["location"] = "WeightTable";

                    // get max wtf count index in FieldAttributeTable
                    var maxWtfIndexList = fieldAttributeTable.AsEnumerable()
                        .Where(dr => dr["Name"].ToString().StartsWith("wtf"))
                        .Select(dr => Int32.Parse(Regex.Replace(dr["Name"].ToString(), @"^wtf0*", "")));

                    var newWtfIndex = maxWtfIndexList.Any() ? maxWtfIndexList.Max() + 1 : 1;

                    newFieldName = $"wtf{newWtfIndex.ToString().PadLeft(4, '0')}";
                }

                dynamicFieldRow["Name"] = newFieldName;
                dynamicFieldRow["IsRepeat"] = false;
                dynamicFieldRow["IsAutoIncrement"] = false;
                dynamicFieldRow["IsMandatory"] = false;
                dynamicFieldRow["Description"] = string.Empty;
                dynamicFieldRow["IsNew"] = true;

                // add a new row in fieldAttributeTable
                DataRow newDynamicFieldRow = fieldAttributeTable.NewRow();
                newDynamicFieldRow.ItemArray = dynamicFieldRow.ItemArray;
                fieldAttributeTable.Rows.Add(newDynamicFieldRow);
            }

            if (!string.IsNullOrEmpty(dynamicFieldRow["Format"].ToString()))
                dynamicFieldRow["Format"] = dynamicField["Format"].ToString().Replace("#", "0");
            else
                dynamicFieldRow["Format"] = string.Empty;

            return dynamicFieldRow;
        }

        /// <summary>
        /// Change session DataTable column name and perform change with "listofvalue" type.
        /// </summary>
        /// <param name="sessionDataTable"> session DataTable need to be performed </param>
        /// <param name="dynamicRow"> dynamic row got from FindOrCreateDynamicField method</param>
        /// <param name="sessionDynamicField"> session dynamic field read from session DataTable using GetDynamicFields method</param>
        private void PerformTableChange(DataTable sessionDataTable, DataRow dynamicRow, DataRow sessionDynamicField)
        {
            var dynamicFieldName = dynamicRow["Name"].ToString();

            sessionDataTable.Columns[sessionDynamicField["Label"].ToString()].ColumnName = dynamicFieldName;

            //Resolve LOV case sensitive
            if (dynamicRow["DeviceFieldType"].ToString().Equals("listofvaluetype"))
            {
                // remove duplicated list of values (ignore character case)
                List<string> lovs = dynamicRow["Listofvalues"].ToString().Split(',').ToList();
                List<string> resolvedLov = new List<string>();
                foreach (string lovValue in lovs)
                {
                    if (!resolvedLov.Contains(lovValue))
                    {
                        if (resolvedLov.Any(v => v.ToLower() == lovValue.ToLower()))
                        {
                            string existinglovValue = resolvedLov.First(v => v.ToLower() == lovValue.ToLower());
                            foreach (DataRow row in sessionDataTable.AsEnumerable()
                                        .Where(r => !string.IsNullOrEmpty(r[dynamicFieldName].ToString()) &&
                                                    r[dynamicFieldName].ToString() == lovValue))
                                row[dynamicFieldName] = existinglovValue;
                        }
                        else
                            resolvedLov.Add(lovValue);
                    }
                }
                dynamicRow["Listofvalues"] = string.Join(",", resolvedLov.Distinct().ToArray());

                // for file device dynamic field, use existing LOV if only character case is different 
                List<string> devicelovs = dynamicRow["Listofvalues"].ToString().Split(',').ToList();
                List<string> filelovs = sessionDynamicField["Listofvalues"].ToString().Split(',').ToList();
                foreach (string lovValue in filelovs)
                {
                    if (devicelovs.Any(v => String.Equals(v, lovValue, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string existinglovValue = devicelovs.First(v => v.ToLower() == lovValue.ToLower());
                        foreach (DataRow row in sessionDataTable.AsEnumerable()
                                    .Where(r => !string.IsNullOrEmpty(r[dynamicFieldName].ToString()) &&
                                                r[dynamicFieldName].ToString() == lovValue))
                            row[dynamicFieldName] = existinglovValue;
                    }
                }
            }
        }

        /// <summary>
        /// Create a DataTable
        /// </summary>
        /// <param name="tableName"> table name</param>
        /// <param name="columns"> column names and types</param>
        /// <returns></returns>
        private DataTable CreateDataTable(string tableName, List<Tuple<string, Type>> columns)
        {
            DataTable table = new DataTable(tableName);

            foreach (var column in columns)
            {
                if (column.Item2 == null)
                {
                    table.Columns.Add(column.Item1, typeof(string));
                }
                else
                {
                    table.Columns.Add(column.Item1, column.Item2);
                }
            }

            return table;
        }
        #endregion

        #region SQL query creation and operation region: Create query package and operate database
        /// <summary>
        /// Create sql SELECT syntax
        /// </summary>
        /// <param name="operation"> SELECT, INSERT ALTER or UPDATE</param>
        /// <returns></returns>
        private Dictionary<string, object> CreateSqlSelectSyntax(string operation = "SELECT")
        {
            return new Dictionary<string, object>()
            {
                {
                    "OPERATION", operation
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "SELECT", new List<List<Tuple<string, string>>>() { }
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "DISTINCT", false
                }, // stores whether return distinct result or not
                {
                    "FROM", new List<string>() { }
                }, // stores a list of table names, use the first one as the main data source table
                {
                    "LEFT JOIN", new List<string>() { }
                }, // stores a list join tables need to be joined with main table
                {
                    "ON", new List<string>() { }
                }, // stores a list of join conditions match with join tables
                {
                    "FILTER", new List<Tuple<string, string, string>>() { }
                }, // stores a list of filter conditions in tuple format: <"SessionTable.sessionid", "=", "{primary key}">
                {
                    "GROUP BY", new List<string>() { }
                }, // stores a list of group conditions
                {
                    "HAVING", new List<string>() { }
                }, // stores a list of having conditions, should use with GROUP BY
                {
                    "SORT", new List<Tuple<string, string>>() { }
                }, // stores a list of sort conditions in tuple format, but only the last one will be used, <"sort key", "ASC/DESC">
                {
                    "LIMIT", new List<string>() { }
                }, // stores a list of limit numbers of selection
                {
                    "OFFSET", new List<string>() { }
                } // stores a list of index, use with LIMIT
            };
        }

        /// <summary>
        /// Create sql INSERT syntax
        /// </summary>
        /// <param name="operation">SELECT, INSERT ALTER or UPDATE</param>
        /// <returns></returns>
        private Dictionary<string, object> CreateSqlInsertSyntax(string operation = "INSERT")
        {
            return new Dictionary<string, object>()
            {
                {
                    "OPERATION", operation
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "INSERT TABLE", string.Empty
                }, // stores the table to insert
                {
                    "INSERT COLUMNS", new List<List<string>>()
                }, // stores the list of column names list to insert
                {
                    "VALUES", new List<List<string>>()
                } // stores multiple value list to insert into the table
            };
        }

        /// <summary>
        /// Create sql ALTER syntax
        /// </summary>
        /// <param name="operation">SELECT, INSERT ALTER or UPDATE</param>
        /// <returns></returns>
        private Dictionary<string, object> CreateSqlAlterSyntax(string operation = "ALTER")
        {
            return new Dictionary<string, object>()
            {
                {
                    "OPERATION", operation
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "ALTER TABLE", new List<string>()
                }, // stores the tables to alter column  
                {
                    "ADD COLUMNS", new List<string>()
                }, // stores the list of column names list to create
                {
                    "COLUMN TYPES", new List<string>()
                }, // stores column types
                {
                    "INDEX", new List<bool>()
                } // stores index flag of new created columns
            };
        }

        /// <summary>
        /// Create sql UPDATE syntax
        /// </summary>
        /// <param name="operation">SELECT, INSERT ALTER or UPDATE</param>
        /// <returns></returns>
        private Dictionary<string, object> CreateSqlUpdateSyntax(string operation = "UPDATE")
        {
            return new Dictionary<string, object>()
            {
                {
                    "OPERATION", operation
                }, // stores a list of selections, each selection have multiple input, <TableName.columnname, display column name>
                {
                    "UPDATE TABLE", string.Empty
                }, // stores the table name to update
                {
                    "UPDATE COLUMN", new List<List<string>>()
                }, // stores the list of column names list to update
                {
                    "UPDATE VALUE", new List<List<string>>()
                }, // stores values to update with columns
                {
                    "FILTER", new List<Tuple<string, string, string>>()
                } // stores a list of filter conditions in tuple format: <"SessionTable.sessionid", "=", "{primary key}">
            };
        }

        /// <summary>
        /// Add Select operation into Select syntax.
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <param name="selectTupleList"></param>
        private void SelectQuery(Dictionary<string, object> sqlSelect, List<Tuple<string, string>> selectTupleList)
        {
            var selectValue = sqlSelect["SELECT"] as List<List<Tuple<string, string>>>; // get value from dictionary and assign the type

            selectValue?.Add(selectTupleList);

            sqlSelect["SELECT"] = selectValue;
        }

        /// <summary>
        /// Add Filter operation into Select syntax.
        /// </summary>
        /// <param name="sqlFilter"></param>
        /// <param name="filterTuple"></param>
        private void FilterQuery(Dictionary<string, object> sqlFilter, Tuple<string, string, string> filterTuple)
        {
            var validFilterOperator = new[] { "=", "!=", ">", "<", ">=", "<=", "IN", "NOT IN", "IS", "IS NOT" };

            if (string.IsNullOrEmpty(filterTuple.Item3)
                || string.IsNullOrEmpty(filterTuple.Item3.Trim())
                || string.Equals(filterTuple.Item3.Trim().ToLower(), "null")) // in case the input is "null", "NULL" or "Null" etc.
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, "is", "null");
            }
            else if (string.Equals(filterTuple.Item2.ToLower(), "not null"))
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, "is", "not null");
            }
            else if (string.Equals(filterTuple.Item2.ToLower(), "in"))
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, filterTuple.Item2, $"{filterTuple.Item3}");
            }
            else
            {
                filterTuple = new Tuple<string, string, string>(filterTuple.Item1, filterTuple.Item2, $"'{filterTuple.Item3}'");
            }

            filterTuple = validFilterOperator.Contains(filterTuple.Item2.ToUpper())
                ? filterTuple
                : new Tuple<string, string, string>(filterTuple.Item1, "=", filterTuple.Item3);

            var filterValue = sqlFilter["FILTER"] as List<Tuple<string, string, string>>;

            filterValue?.Add(filterTuple);

            sqlFilter["FILTER"] = filterValue;
        }

        /// <summary>
        /// Add Sort operation into Select syntax.
        /// </summary>
        /// <param name="sqlSort"></param>
        /// <param name="sortTuple"></param>
        private void SortQuery(Dictionary<string, object> sqlSort, Tuple<string, string> sortTuple)
        {
            var validSortOperator = new[] { "ASC", "DESC" };
            sortTuple = validSortOperator.Contains(sortTuple.Item2)
                ? sortTuple
                : new Tuple<string, string>(sortTuple.Item1, "ASC");

            var sortValue = sqlSort["SORT"] as List<Tuple<string, string>>;

            sortValue?.Add(sortTuple);

            sqlSort["SORT"] = sortValue;
        }

        // For XR5000 device, the treatment table in database needs further modification to join with weight table.
        // temp use, need to use syntax and ALA. Create a new table that not exists in current database
        async Task<string> CreateNewWeightTable(string sessionPrimaryKey)
        {
            System.Text.StringBuilder preSqlBuilder = new System.Text.StringBuilder();
            List<string> drugs = new List<string>();
            // use select structure instead
            string drugIdQueryComm =
                $"SELECT DISTINCT drugid FROM TreatmentTable WHERE sessionid='{sessionPrimaryKey}' ORDER BY drugid";
            var drugId = await dbRequestQuery.SendRequest(drugIdQueryComm);
        
        
            if (drugId != null && drugId.Rows.Count > 0)
            {
                foreach (DataRow row in drugId.Rows)
                {
                    drugs.Add(row[0].ToString());
                }
            }
        
            // for session list, do not need this .
            // for session data, need pass this new weight table as the main source for query.
        
            // For XR5000, this new weight table is used to gather related data together for query use. Especially get treatment records and drug information.
        
            // read xml content
            // XElement polaris = XElement.Load(@"C:\Projects\XR5000TestData\XR50001206db\polaris.xml");
            XElement polaris = XElement.Load(Path.Combine(dbFilePath.Data.Substring(0, dbFilePath.Data.LastIndexOf("db")), "polaris.xml"));
        
            // get object content in xml
            IEnumerable<string> model = from item in polaris.Elements("Database_SchemaVersion")
                                        select item.Value;
            var databaseSchemaVersion = Convert.ToInt32(model.First()); // get model name in model element
            var hasDrugExpiry = databaseSchemaVersion >= 45;
        
            string sql = @"SELECT W.*,";
        
            foreach (string item in drugs)
            {
                sql += $"\nT{item}.treatmentid AS 'ttf{item.PadLeft(4, '0')}_treatmentid',";
                sql += $"\nT{item}.drugid AS 'ttf{item.PadLeft(4, '0')}_drugid',";
                sql += $"\nT{item}.drugbatch AS 'ttf{item.PadLeft(4, '0')}_drugbatch',";
                sql += $"\nT{item}.drugdosage AS 'ttf{item.PadLeft(4, '0')}_drugdosage',";
                if (hasDrugExpiry)
                    sql += $"\nT{item}.drugexpiry AS 'ttf{item.PadLeft(4, '0')}_drugexpiry',";
                sql +=
                    $"\nT{item}.domesticslaughterwithholdingdate AS 'ttf{item.PadLeft(4, '0')}_domesticslaughterwithholdingdate',";
                sql +=
                    $"\nT{item}.exportslaughterwithholdingdate AS 'ttf{item.PadLeft(4, '0')}_exportslaughterwithholdingdate',";
            }
            sql = sql.Substring(0, sql.Length - 1);
        
            sql += "\nFROM WeightTable W ";
            foreach (string item in drugs)
            {
                sql += String.Format(
                    "\nLEFT OUTER JOIN TreatmentTable T{0} ON  W.weightid=T{0}.weightid and T{0}.drugid={0} ", item);
            }
            sql += $"\nWHERE W.SessionID = '{sessionPrimaryKey}' ";
        
            return sql;
        }

        #endregion

        #region DB backup and rename region: backup database before manipulation
        /// <summary>
        /// Backup a sqlite database in C:\ProgramData\Tru-Test\DataLink_ALA
        /// </summary>
        /// <param name="sqliteDbFilePath"> full file path to a database file</param>
        private void BackupDb(string sqliteDbFilePath)
        {
            var dbBackupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Tru-Test", "DataLink_ALA");

            if (!Directory.Exists(dbBackupFolder))
            {
                Directory.CreateDirectory(dbBackupFolder);
            }

            var dbBackupFilePath = Path.Combine(dbBackupFolder, "backupdb" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));

            var connIn = new SQLiteConnection($"Data Source= {sqliteDbFilePath}");
            var connOut = new SQLiteConnection($"Data Source= {dbBackupFilePath}");

            try
            {
                connIn.Open();
                connOut.Open();
                connIn.BackupDatabase(connOut, connOut.Database, connIn.Database, -1, null, -1);
            }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                connIn.Close();
                connOut.Close();
            }
        }

        /// <summary>
        /// Rename a file and return new file name if operation success.
        /// </summary>
        /// <param name="oldFilePath"></param>
        /// <param name="newName"></param>
        /// <returns>new file path if success, old file path if not</returns>
        private string RenameFile(string oldFilePath, string newName)
        {
            if (File.Exists(oldFilePath))
            {
                var oldFileInfo = new FileInfo(oldFilePath);
                var oldFolder = oldFileInfo.Directory;

                if (oldFolder != null)
                {
                    var newFilePath = Path.Combine(oldFolder.ToString(), newName);
                    if (File.Exists(newFilePath))
                    {
                        File.Delete(newFilePath);
                        File.Delete(newFilePath + "-journal");
                    }

                    try
                    {
                        File.Move(oldFilePath, newFilePath);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    return newFilePath;
                }

            }
            else
            {
                throw new ArgumentException($"{oldFilePath} does not exist.");
            }
            return oldFilePath;
        }

        #endregion

        #region Methods backup region (NOT IN USE NOW)
        /// <summary>
        /// check DataRow content from skipRowIndex until the end of DataTable, remove columns have no contents or only spaces.
        /// </summary>
        /// <param name="table"> DataTable to check</param>
        /// <param name="skipRowIndex"> the begin index for check</param>
        private void RemoveEmptyColumns(DataTable table, int skipRowIndex)
        {
            List<DataColumn> emptyColumnList = new List<DataColumn>();

            if (table != null)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (!table.AsEnumerable().Any(row => !String.IsNullOrEmpty(row[col].ToString().Trim()) && table.Rows.IndexOf(row) >= skipRowIndex))
                    {
                        emptyColumnList.Add(col);
                    }
                }
            }
            foreach (DataColumn col in emptyColumnList)
            {
                table?.Columns.Remove(col);
            }
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

        /// <summary>
        /// validate column labels in a data table, remove duplicate columns and empty columns
        /// </summary>
        /// <param name="inputDataTable">input DataTable</param>
        /// <param name="headerColumnIndex"> DataRow index to locate headers in DataTable</param>
        private void ValidateColumnLabels(DataTable inputDataTable, int headerColumnIndex)
        {
            var removeColumns = new List<DataColumn>();
            var duplicateColumns = new List<string>();
            foreach (DataColumn col in inputDataTable.Columns)
            {
                var columnName = inputDataTable.Rows[headerColumnIndex][col].ToString();

                if (string.IsNullOrEmpty(columnName))
                    removeColumns.Add(col);
                else if (!duplicateColumns.Contains(columnName))
                {
                    duplicateColumns.Add(columnName);
                }
                else
                {
                    removeColumns.Add(col);
                }
            }

            foreach (var removeCol in removeColumns)
            {
                inputDataTable.Columns.Remove(removeCol);
            }

        }

        /// <summary>
        /// initialise upload session, get session name and session date
        /// </summary>
        /// <param name="filePath">file path to the session file, used to generate date information when no date information found in the session file</param>
        /// <param name="sessionDataTable"> upload session DataTable</param>
        /// <param name="tmpSessionName"> output session name of a session file</param>
        /// <param name="tmpSessionDate"> output session date of a session file</param>
        private void InitialiseSession(string filePath, DataTable sessionDataTable, out string tmpSessionName, out string tmpSessionDate)
        {
            // session name
            const string validFileChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789^&'@{}[],$=!-#()%.+!_ ";
            var fullName = sessionDataTable.Rows[1][0].ToString();
            tmpSessionName = fullName.Substring(fullName.IndexOf(":") + 1).Trim();
            tmpSessionDate = string.Empty;

            List<char> invalidChars = tmpSessionName.Where(c => !validFileChar.Contains(c)).Distinct().ToList();
            foreach (char invalidChar in invalidChars)
                tmpSessionName = tmpSessionName.Replace(invalidChar.ToString(), string.Empty);

            // session date
            bool hasValidDate = false;
            var dateFromFile = sessionDataTable.Rows[2][0].ToString();
            var rawDate = dateFromFile.Substring(dateFromFile.IndexOf(":", StringComparison.Ordinal) + 1).Trim();
            var tmpDate = TryParseDate(rawDate, out bool parseSuccessful);
            if (parseSuccessful)
            {
                tmpSessionDate = tmpDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                sessionDateStamp = (tmpDate - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(CultureInfo.InvariantCulture);
                hasValidDate = true;
            }
            if (!hasValidDate)
            {
                tmpSessionDate = new FileInfo(filePath).CreationTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime.TryParseExact(rawDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmpDate);
                sessionDateStamp = (tmpDate - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Parse date format 
        /// </summary>
        /// <param name="rawDate"> raw date format</param>
        /// <param name="parseSuccessful">parse Successful flag</param>
        /// <param name="firstTryDateFormat"></param>
        /// <returns>parsed datetime</returns>
        private DateTime TryParseDate(string rawDate, out bool parseSuccessful, string firstTryDateFormat = null)
        {
            DateTime tmpDate = DateTime.Now;
            parseSuccessful = false;
            if (!string.IsNullOrWhiteSpace(firstTryDateFormat))
            {
                parseSuccessful = DateTime.TryParseExact(rawDate, firstTryDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out tmpDate);
            }
            if (!parseSuccessful)
            {
                parseSuccessful = DateTime.TryParse(rawDate, out tmpDate);
            }
            if (!parseSuccessful)
            {
                parseSuccessful = DateTime.TryParseExact(rawDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out tmpDate);
            }
            if (!parseSuccessful)
            {
                parseSuccessful = DateTime.TryParseExact(rawDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out tmpDate);
            }
            if (!parseSuccessful)
            {
                parseSuccessful = DateTime.TryParseExact(rawDate, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out tmpDate);
            }
            if (!parseSuccessful)
            {
                parseSuccessful = DateTime.TryParseExact(rawDate, "yyyy/MM/dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out tmpDate);
            }
            return tmpDate;
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

        #endregion
    }
}
