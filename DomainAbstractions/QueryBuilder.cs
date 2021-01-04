using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// Waiting for query command build request, check query keyword stored as the first value in request dictionary and compile request, response query command string.
    /// Input ports:
    /// 1. IRequestResponseDataFlow<Dictionary<string, object>, string>: query command build request.
    /// Output ports:
    /// IRequestResponseDataFlow<string, DataTable> sqliteDbRequest: request to get information from database when compile query request.
    /// TODO: Considering move sqliteDbRequest out, validate query outside, make this abstraction only responsible for compiling.
    /// </summary>
    public class QueryBuilder : IRequestResponseDataFlow_B<Dictionary<string, object>, string>
    {
        // public properties
        public string InstanceName = "Default";
        
        // ports
        private IRequestResponseDataFlow_B<string, DataTable> sqliteDbRequest;

        private string selectOperatorComm;
        private readonly List<string> dateTypeConvertList;
        private string dateConvertFormat;

        // constructor
        public QueryBuilder()
        {
            
        }

        /// <summary>
        /// Receive SQL operation request dictionary which contains query operation key words, return compiled query command string.
        /// </summary>
        /// <param name="selectOperator"> Select option when compile select query request. Support "ALL" or "DISTINCT". </param>
        /// <param name="convertDateTypeList"> A list contains all date format keywords, will use dateType as convert format. </param>
        /// <param name="dateType"> Date type to convert date format query contents. </param>
        public QueryBuilder(string selectOperator,
            List<string> convertDateTypeList = null, string dateType = "%d/%m/%Y")
        {
            selectOperatorComm = selectOperator;

            if (convertDateTypeList != null)
            {
                dateTypeConvertList = convertDateTypeList;
            }
            else
            {
                dateTypeConvertList = new List<string>()
                {
                    "sessionstartdate",
                    "sessionenddate",
                    "dob",
                    "earliestdomesticslaughterdate",
                    "earliestexportslaughterdate",
                    "drugexpiry",
                    "domesticslaughterwithholdingdate",
                    "exportslaughterwithholdingdate",
                    "datestamp"
                };
            }

            dateConvertFormat = dateType;
        }

        // IRequestResponseDataFlow implementation
        async Task<string> IRequestResponseDataFlow_B<Dictionary<string, object>, string>.SendRequest(Dictionary<string, object> sqlRequest)
        {
            var sqlBuilder = new System.Text.StringBuilder();

            // extract request from dictionary
            var operationKeyWord = sqlRequest["OPERATION"] as string;

            switch (operationKeyWord)
            {
                case "SELECT":
                    sqlBuilder = await SelectQueryBuilder(sqlRequest, sqlBuilder);
                    break;
                case "INSERT":
                    sqlBuilder = InsertQueryBuilder(sqlRequest, sqlBuilder);
                    break;
                case "UPDATE":
                    sqlBuilder = UpdateQueryBuilder(sqlRequest, sqlBuilder);
                    break;
                case "ALTER":
                    sqlBuilder = AlterQueryBuilder(sqlRequest, sqlBuilder);
                    break;
            }

            return sqlBuilder.ToString();
        }


        // implementation of IRequestResponseDataFlow<List<KeyValuePair<string, object>>, string>
        // async Task<string> IRequestResponseDataFlow<List<KeyValuePair<string, object>>, string>.SendRequest(List<KeyValuePair<string, object>> queryOperation)
        // {
        //     // TODO: modify the structure of data to contain operation type as "SELECT", "UPDATE", "INSERT" or "ALTER", etc.
        //     // For now, support select
        //     var selectOperationSyntax = CreateSqlSelectSyntax();
        //
        //     // parse information in queryOperation
        //     var columnsList = queryOperation
        //         .Where(kvp => kvp.Key.Equals("SELECT"))
        //         .Select(kvp => kvp.Value as List<string>)
        //         .ToList();
        //
        //     var aliasList = queryOperation
        //         .Where(kvp => kvp.Key.Equals("ALIAS"))
        //         .Select(kvp => kvp.Value as List<string>)
        //         .ToList();
        //
        //     var isDistinct = queryOperation
        //         .Any(kvp => kvp.Key.Equals("DISTINCT"));
        //
        //     var table = queryOperation
        //         .Where(kvp => kvp.Key.Equals("FROM"))
        //         .Select(kvp => kvp.Value.ToString())
        //         .FirstOrDefault();
        //
        //     var limit = queryOperation
        //         .Where(kvp => kvp.Key.Equals("LIMIT"))
        //         .Select(kvp => kvp.Value)
        //         .FirstOrDefault();
        //
        //     var offset = queryOperation
        //         .Where(kvp => kvp.Key.Equals("OFFSET"))
        //         .Select(kvp => kvp.Value)
        //         .FirstOrDefault();
        //
        //
        //     // fill information into select syntax
        //     if (columnsList.Any())
        //     {
        //         var selectList = new List<Tuple<string, string>>();
        //
        //         for (int i = 0; i < columnsList.Count; i++)
        //         {
        //             for (int j = 0; j < columnsList[i].Count; j++)
        //             {
        //                 selectList.Add(new Tuple<string, string>($"{columnsList[i][j]}", $"{aliasList[i][j]}"));
        //             }
        //         }
        //
        //         SelectQuery(selectOperationSyntax, selectList);
        //     }
        //     else
        //     {
        //         SelectQuery(selectOperationSyntax, new List<Tuple<string, string>>());
        //     }
        //
        //
        //
        //
        //
        //
        //     return "";
        // }

        // private methods

        /// <summary>
        /// Create SQL SELECT syntax
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
        /// SQL select query compile method, used to compile operations stored in the dictionary, returns a command string.
        /// </summary>
        /// <param name="selectSqlRequest"> query operation request dictionary </param>
        /// <param name="selectSqlBuilder"> a string builder for compiling </param>
        /// <returns></returns>
        async Task<StringBuilder> SelectQueryBuilder(Dictionary<string, object> selectSqlRequest, StringBuilder selectSqlBuilder)
        {
            selectSqlBuilder.Clear();

            var selectSql = selectSqlRequest["SELECT"] as List<List<Tuple<string, string>>>;
            var fromSql = selectSqlRequest["FROM"] as List<string>;
            var joinSql = selectSqlRequest["LEFT JOIN"] as List<string>;
            var onSql = selectSqlRequest["ON"] as List<string>;
            var filterSql = selectSqlRequest["FILTER"] as List<Tuple<string, string, string>>;
            var sortSql = selectSqlRequest["SORT"] as List<Tuple<string, string>>;
            var limitSql = selectSqlRequest["LIMIT"] as List<string>;
            var offsetSql = selectSqlRequest["OFFSET"] as List<string>;

            // SQL request validation
            // rule 1: from table should not be empty
            if (fromSql == null) throw new InvalidOperationException();
            if (!fromSql.Any()) throw new ArgumentException("SQL builder from table value should not be null.");

            // rule 2: number of JOIN and ON should match
            if (joinSql == null || onSql == null) throw new InvalidOperationException();
            if (joinSql.Count != onSql.Count) throw new ArgumentException("Joined table and join condition does not match.");

            // rule 3: number of LIMIT and OFFSET should match
            if (limitSql == null || offsetSql == null) throw new InvalidOperationException();

            // rule 4: select columns should exist in tables, otherwise, change select into: SELECT NULL AS display name
            // select from table source to get columns
            selectSqlBuilder.AppendLine("SELECT *");
            selectSqlBuilder.AppendLine($"FROM {fromSql.First()}");
            if (joinSql.Any())
                for (int i = 0; i < joinSql.Count; i++)
                    selectSqlBuilder.AppendLine($"LEFT JOIN {joinSql[i]} ON {onSql[i]}");
            selectSqlBuilder.AppendLine("LIMIT 1");
            var preQueryColumnCheckComm = selectSqlBuilder.ToString();

            var preQueryColumnCheck = await sqliteDbRequest.SendRequest(preQueryColumnCheckComm);

            var preQueryColumnList = preQueryColumnCheck.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName.ToLower())
                .ToList();
            preQueryColumnList.AddRange(new List<string>() { "null", "'true'", "'false'" });

            // rule 5: if dictionary is empty, means no previous operations are needed.

            // ---------------------------------Begin compile SQL command----------------------------------------
            selectSqlBuilder.Clear();
            // step 1: compile select part
            // select validation, convert date time format based on an index list
            var validSelectOperator = new string[2] { "ALL", "DISTINCT" };
            selectOperatorComm = validSelectOperator.Contains(selectOperatorComm) ? selectOperatorComm : "ALL";

            selectSqlBuilder.AppendLine($"SELECT {selectOperatorComm}");
            if (selectSql != null && selectSql.Any())
            {
                foreach (List<Tuple<string, string>> selectList in selectSql)
                {
                    foreach (Tuple<string, string> selectTuple in selectList)
                    {
                        var selectColumnFullName = selectTuple.Item1; // Tablename.columnname
                        var selectColumnName = selectColumnFullName.Contains(".") ? selectColumnFullName.Split('.')[1] : selectColumnFullName; // columnname
                        var selectColumnDisplayName = selectTuple.Item2; // ttf0001_dosage

                        if (dateTypeConvertList.Any()) // check when need to convert some date format
                        {
                            if (preQueryColumnList.Contains(selectColumnName.ToLower())
                            ) // check whether column exists in select result
                            {
                                if (dateTypeConvertList.Any(s => selectColumnFullName.Contains(s))
                                ) // check whether columnname format need to change
                                {
                                    selectSqlBuilder.AppendLine(string.IsNullOrEmpty(selectColumnDisplayName)
                                        ? $"strftime('{dateConvertFormat}', {selectColumnFullName}, 'unixepoch'), "
                                        : $"strftime('{dateConvertFormat}', {selectColumnFullName}, 'unixepoch') AS '{selectColumnDisplayName}', ");
                                }
                                else
                                {
                                    selectSqlBuilder.AppendLine(string.IsNullOrEmpty(selectColumnDisplayName)
                                        ? $"{selectColumnFullName}, "
                                        : $"{selectColumnFullName} AS '{selectColumnDisplayName}', ");
                                }
                            }
                            else
                            {
                                selectSqlBuilder.AppendLine(string.IsNullOrEmpty(selectColumnDisplayName)
                                    ? "NULL, "
                                    : $"NULL AS '{selectColumnDisplayName}', ");
                            }
                        }
                        else
                        {
                            if (preQueryColumnList.Contains(selectColumnName.ToLower())
                            ) // check whether column exists in select result
                            {
                                selectSqlBuilder.AppendLine(string.IsNullOrEmpty(selectColumnDisplayName)
                                    ? $"{selectColumnFullName}, "
                                    : $"{selectColumnFullName} AS '{selectColumnDisplayName}', ");
                            }
                            else
                            {
                                selectSqlBuilder.AppendLine(string.IsNullOrEmpty(selectColumnDisplayName)
                                    ? "NULL, "
                                    : $"NULL AS '{selectColumnDisplayName}', ");
                            }
                        }
                    }
                }

                selectSqlBuilder.Remove(selectSqlBuilder.ToString().LastIndexOf(','), 1);
            }
            else
            {
                selectSqlBuilder.AppendLine("*");
            }

            // step 2: compile from part
            selectSqlBuilder.AppendLine("FROM");
            selectSqlBuilder.AppendLine($"{fromSql.First()}");

            // step 3: compile LEFT JOIN part
            if (joinSql.Any())
            {
                for (int i = 0; i < joinSql.Count; i++)
                    selectSqlBuilder.AppendLine($"LEFT JOIN {joinSql[i]} ON {onSql[i]}");
            }

            // step 4: compile filter part
            if (filterSql.Any())
            {
                selectSqlBuilder.AppendLine("WHERE");
                foreach (Tuple<string, string, string> filterCondition in filterSql)
                    selectSqlBuilder.AppendLine($"{filterCondition.Item1} {filterCondition.Item2} {filterCondition.Item3} AND ");

                selectSqlBuilder.Remove(selectSqlBuilder.ToString().LastIndexOf("AND", StringComparison.Ordinal), 3);
            }

            // step 5: compile sort part
            if (sortSql.Any())
            {
                selectSqlBuilder.AppendLine("ORDER BY");
                selectSqlBuilder.AppendLine($"{sortSql.Last().Item1} {sortSql.Last().Item2}");
            }

            // step 6: compile LIMIT part
            if (limitSql.Any())
            {
                selectSqlBuilder.AppendLine("LIMIT");
                selectSqlBuilder.AppendLine(offsetSql.Any()
                    ? $"{limitSql.First()} \n OFFSET \n {offsetSql.First()}"
                    : $"{limitSql.First()}");
            }

            return selectSqlBuilder;
        }

        /// <summary>
        /// SQL select query compile method, used to compile operations stored in the dictionary, returns a command string.
        /// </summary>
        /// <param name="insertSqlRequest"> query operation request dictionary </param>
        /// <param name="insertSqlBuilder"> a string builder for compiling </param>
        /// <returns></returns>
        private StringBuilder InsertQueryBuilder(Dictionary<string, object> insertSqlRequest, StringBuilder insertSqlBuilder)
        {
            insertSqlBuilder.Clear();

            var insertTableName = insertSqlRequest["INSERT TABLE"] as string;
            var insertColumns = insertSqlRequest["INSERT COLUMNS"] as List<List<string>>;
            var insertValues = insertSqlRequest["VALUES"] as List<List<string>>;

            // validation dictionary content
            var validateFlag = false;

            if (insertColumns != null && insertValues != null)
                if (!string.IsNullOrEmpty(insertTableName) && insertColumns.Count == insertValues.Count)
                    for (var i = 0; i < insertColumns.Count; i++)
                        if (insertColumns[i].Count == insertValues[i].Count)
                            validateFlag = true;

            if (!validateFlag) return insertSqlBuilder;

            // begin compile insert query command

            for (int i = 0; i < insertColumns.Count; i++)
            {
                var columns = insertColumns[i];
                var values = insertValues[i];

                if (columns.Any() && values.Any())
                {
                    insertSqlBuilder.AppendLine($"INSERT INTO {insertTableName}");
                    insertSqlBuilder.Append("(");
                    for (var j = 0; j < columns.Count; j++)
                    {
                        insertSqlBuilder.Append(j != columns.Count - 1 ? $"{columns[j]}, " : $"{columns[j]}");
                    }
                    insertSqlBuilder.AppendLine(")");
                    insertSqlBuilder.AppendLine("VALUES");
                    insertSqlBuilder.Append("(");
                    for (var j = 0; j < values.Count; j++)
                    {
                        insertSqlBuilder.Append(j != values.Count - 1 ? $"'{values[j]}', " : $"'{values[j]}'");
                    }
                    insertSqlBuilder.AppendLine(");");
                }
            }

            return insertSqlBuilder;
        }

        /// <summary>
        /// SQL select query compile method, used to compile operations stored in the dictionary, returns a command string.
        /// </summary>
        /// <param name="updateSqlRequest"> query operation request dictionary </param>
        /// <param name="updateSqlBuilder"> a string builder for compiling </param>
        /// <returns></returns>
        private StringBuilder UpdateQueryBuilder(Dictionary<string, object> updateSqlRequest, StringBuilder updateSqlBuilder)
        {
            updateSqlBuilder.Clear();

            var updateTableName = updateSqlRequest["UPDATE TABLE"] as string;
            var updateColumns = updateSqlRequest["UPDATE COLUMN"] as List<List<string>>;
            var updateValues = updateSqlRequest["UPDATE VALUE"] as List<List<string>>;
            var updateFilters = updateSqlRequest["FILTER"] as List<Tuple<string, string, string>>;

            // validation
            var validateFlag = false;
            if (updateTableName != null && updateColumns != null && updateValues != null && updateFilters != null)
                if (updateColumns.Count == updateValues.Count)
                    for (var i = 0; i < updateColumns.Count; i++)
                        if (updateColumns[i].Count == updateValues[i].Count)
                            validateFlag = true;

            if (!validateFlag) return updateSqlBuilder;

            // begin compile update query command
            for (int i = 0; i < updateColumns.Count; i++)
            {
                var columns = updateColumns[i];
                var values = updateValues[i];

                updateSqlBuilder.AppendLine($"UPDATE {updateTableName}");
                updateSqlBuilder.AppendLine("SET");

                for (var j = 0; j < columns.Count; j++)
                {
                    updateSqlBuilder.AppendLine(j != columns.Count - 1
                        ? $"{columns[j]} = '{values[j]?.Replace("'", "''")}',"
                        : $"{columns[j]} = '{values[j]?.Replace("'", "''")}'");
                }

                if (string.IsNullOrEmpty(updateFilters[i].Item1) || string.IsNullOrEmpty(updateFilters[i].Item2) || string.IsNullOrEmpty(updateFilters[i].Item3))
                    continue;

                updateSqlBuilder.AppendLine("WHERE");

                updateSqlBuilder.AppendLine($"{updateFilters[i].Item1} {updateFilters[i].Item2} '{updateFilters[i].Item3}';");
            }

            return updateSqlBuilder;
        }

        /// <summary>
        /// SQL select query compile method, used to compile operations stored in the dictionary, returns a command string.
        /// </summary>
        /// <param name="alterSqlRequest"> query operation request dictionary </param>
        /// <param name="alterSqlBuilder"> a string builder for compiling </param>
        /// <returns></returns>
        private StringBuilder AlterQueryBuilder(Dictionary<string, object> alterSqlRequest, StringBuilder alterSqlBuilder)
        {
            alterSqlBuilder.Clear();

            var alterTableNames = alterSqlRequest["ALTER TABLE"] as List<string>;
            var alterColumns = alterSqlRequest["ADD COLUMNS"] as List<string>;
            var alterTypes = alterSqlRequest["COLUMN TYPES"] as List<string>;
            var alterIndex = alterSqlRequest["INDEX"] as List<bool>;

            // validation dictionary content
            var validateFlag = false;

            if (alterTableNames != null && alterColumns != null && alterTypes != null && alterIndex != null)
                if (alterTableNames.Count == alterColumns.Count &&
                    alterTableNames.Count == alterTypes.Count &&
                    alterTableNames.Count == alterIndex.Count)
                    validateFlag = true;

            if (!validateFlag) return alterSqlBuilder;

            // begin compile alter query command
            for (var i = 0; i < alterTableNames.Count; i++)
            {
                alterSqlBuilder.AppendLine($"ALTER TABLE {alterTableNames[i]}");
                alterSqlBuilder.AppendLine($"ADD '{alterColumns[i]}' {alterTypes[i]};");
            }

            for (var i = 0; i < alterTableNames.Count; i++)
            {
                if (alterIndex[i])
                {
                    alterSqlBuilder.AppendLine(
                        $"CREATE UNIQUE INDEX '{alterColumns[i]}Idx' ON {alterTableNames[i]} ({alterColumns[i]})");
                }
            }
            
            return alterSqlBuilder;
        }

        /// <summary>
        /// Add Select operation into Select syntax.
        /// </summary>
        /// <param name="sqlSelect"></param>
        /// <param name="selectTupleList"></param>
        private void SelectQuery(Dictionary<string, object> sqlSelect,
            List<Tuple<string, string>> selectTupleList)
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
        private void FilterQuery(Dictionary<string, object> sqlFilter,
            Tuple<string, string, string> filterTuple)
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
        private void SortQuery(Dictionary<string, object> sqlSort,
            Tuple<string, string> sortTuple)
        {
            var validSortOperator = new[] { "ASC", "DESC" };
            sortTuple = validSortOperator.Contains(sortTuple.Item2)
                ? sortTuple
                : new Tuple<string, string>(sortTuple.Item1, "ASC");

            var sortValue = sqlSort["SORT"] as List<Tuple<string, string>>;

            sortValue?.Add(sortTuple);

            sqlSort["SORT"] = sortValue;
        }

    }
}
