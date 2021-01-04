using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProgrammingParadigms;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.RegularExpressions;

namespace Libraries
{
    public static class StaticUtilities
    {
        /// <summary>
        /// <para>An extension method for objects, particularly anonymous objects, to get a named property.</para>
        /// <para>T : The type of the property to return.</para>
        /// <para>obj : The object containing the desired property.</para>
        /// <para>propertyName : the variable name of the property.</para>
        /// <para>Returns : A property cast as type T.</para>
        /// </summary>
        public static T GetProperty<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName)?.GetValue(obj);
        }

        /// <summary>
        /// <para>A helper method to convert an object to any type T. The initial purpose of this was to enable casting from an anonymous type into an object while keeping the accessibility of its properties.</para>
        /// <para>T : The type to cast to.</para>
        /// <para>typeHolder : Any instance can be passed into this parameter and its type T will be extracted.</para>
        /// <para>obj : The object to cast.</para>
        /// <para>Returns : obj cast as type T.</para>
        /// </summary>
        public static T Cast<T>(T typeHolder, object obj)
        {
            return (T)obj;
        }

        public static void EnumerablePrinter(this IEnumerable enumerable)
        {
            foreach (var element in enumerable)
            {
                Logging.Log(element.ToString());
            }
        }

        public static string FormatObjectAsString(object obj)
        {
            if (obj is DataTable)
            {
                var dt = (DataTable)obj;
                var dataTableStringBuilder = new StringBuilder();
                int itemSizeLimit = 16;
                
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    var dataColumn = dt.Columns[i].ColumnName;
                    dataColumn = Regex.Replace(dataColumn, @"\n", " ");

                    if (dataColumn.Length > itemSizeLimit)
                    {
                        dataTableStringBuilder.Append($"{dataColumn.Substring(0, itemSizeLimit)} ");
                    }
                    else
                    {
                        dataTableStringBuilder.Append($"{dataColumn}{new string(' ', itemSizeLimit - dataColumn.Length)} ");
                    }
                }

                var finalColumnName = dt.Columns[dt.Columns.Count - 1].ColumnName;
                finalColumnName = Regex.Replace(finalColumnName, @"\n", " ");

                if (finalColumnName.Length > itemSizeLimit)
                {
                    dataTableStringBuilder.Append($"{finalColumnName.Substring(0, itemSizeLimit)} ");
                }
                else
                {
                    dataTableStringBuilder.Append($"{finalColumnName}");
                }

                dataTableStringBuilder.AppendLine("\n");

                foreach (DataRow dataRow in dt.Rows)
                {
                    for (int i = 0; i < dataRow.ItemArray.Length - 1; i++)
                    {
                        var item = dataRow.ItemArray[i].ToString();
                        item = Regex.Replace(item, @"\n", " ");

                        if (item.Length > itemSizeLimit)
                        {
                            dataTableStringBuilder.Append($"{item.ToString().Substring(0, itemSizeLimit)} ");
                        }
                        else
                        {
                            dataTableStringBuilder.Append($"{item}{new string(' ', itemSizeLimit - item.ToString().Length)} ");
                        }
                    }

                    var finalDataRowItem = dataRow.ItemArray[dataRow.ItemArray.Length - 1].ToString();
                    finalDataRowItem = Regex.Replace(finalDataRowItem, @"\n", " ");

                    if (finalDataRowItem.Length > itemSizeLimit)
                    {
                        dataTableStringBuilder.Append($"{finalDataRowItem.ToString().Substring(0, itemSizeLimit)} ");
                    }
                    else
                    {
                        dataTableStringBuilder.Append($"{finalDataRowItem}");
                    }

                    dataTableStringBuilder.AppendLine("\n");
                }

                return dataTableStringBuilder.ToString();
            }

            try
            {
                if (obj is JToken)
                {
                    return JsonConvert.SerializeObject(obj);
                }
                var objectToConvert = obj ?? "null";
                var jsonSerializer = JsonSerializer.CreateDefault();
                JToken jToken;
                using (JTokenWriter jsonWriter = new JTokenWriter())
                {
                    jsonSerializer.Serialize(jsonWriter, objectToConvert);
                    jToken = jsonWriter.Token!;
                }

                return JsonConvert.SerializeObject(jToken);
            }
            catch
            {
                return obj.ToString();
            }
        }

        public static void LogDataChange(this object obj)
        {
            Logging.Log($"Value change observed:\n{FormatObjectAsString(obj)}");
        }

        public static void LogDataChange(this object obj, string objName)
        {
            Logging.Log($"{objName}:\n{FormatObjectAsString(obj)}");
        }

        public static void LogDataChange(this object obj, string A, string B, string interfaceName)
        {
            Logging.Log($"\"{A}\" {interfaceName} {B} changed to: {FormatObjectAsString(obj)}");
        }

        public static void LogDataChange(this object obj, string A, string B, string interfaceName, object oldValue)
        {
            Logging.Log($"\"{A}\" {interfaceName} {B} changed: {FormatObjectAsString(oldValue)} --> {FormatObjectAsString(obj)}");
        }

        public static JObject CreateJObjectMessage(string msgKeyword, string msgContent)
        {
            return new JObject(new JProperty(msgKeyword.ToUpper(), new JValue(msgContent)));
        }
    }
}
