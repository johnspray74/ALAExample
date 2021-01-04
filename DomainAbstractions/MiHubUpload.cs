using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace DomainAbstractions
{
    public class MiHubUpload : IEvent, IDataFlow<string>, ITableDataFlow
    {
        /// <summary>
        /// [DEPRECATED]
        /// Uploads a CSV (in the XR3000 format) to MiHub
        /// </summary>

        // Properties
        public string InstanceName = "Default";

        // Fields
        //private string filePath = "";
        private string userinfoPath = @"C:\ProgramData\Tru-Test\DataLink_ALA\" + "userinfo.json";
        private string accessToken;
        private string refreshToken;
        private string fileData = "";
        private List<string> filePaths = new List<string>();
        private List<string> fileNames = new List<string>();

        // Outputs
        private IEvent uploadCompleted;
        private IDataFlow<string> uploadResultMessage;
        private List<string> uploadResultMessages = new List<string>();
        private IDataFlow<List<string>> uploadResultMessagesOutput;
        private IDataFlow<string> uploadResultSummary;
        private bool uploadResultsSent = false;
        private IDataFlowB<string> userSelectedFarm;

        public MiHubUpload() { }

        #region classes
        public enum DataType
        {
            Unknown = 0,
            Alphanumeric,
            Numeric,
            Custom,
            Date,
            Time
        }

        public class Session
        {
            public DateTime CreatedAt { get; set; }
            public SessionMeta Meta = null;
            public Table SessionData { get; set; }
            public string SessionAuditKey = null;

            public double? Latitude = null;
            public double? Longitude = null;
        }

        public class SessionMeta
        {

        }

        public class Table
        {
            public List<Header> Headers { get; set; }
            // The Row dictionary is keyed with the Header's FieldId
            public List<Dictionary<Header, ValueType>> Rows { get; set; }
        }

        public class Header
        {
            public string UserReadableName { get; set; }
            public string RawFormat { get; set; }
            public string FieldId { get; set; }
            public bool IsId { get; set; }
            public DataType DataType { get; set; }
        }

        public class ValueType
        {
            public DataType DataType { get; set; }

            public DateTime? DateStore { get; set; }
            public string StringStore { get; set; }

            // Decimal values are stored in string format to preserve the correct number of decimal places.
            // IE we don't want to add or remove extra zeroes of precision in the value stored.  Ref: DLMA-558
            public string DecimalStore { get; set; }

            public ValueType()
            {
                // Parameterless constructor for JSON deserialization
                this.DataType = DataType.Unknown;
            }
            public ValueType(Decimal val)
            {
                DecimalStore = val.ToString("F2");
                DataType = DataType.Numeric;
            }
            public ValueType(int val)
            {
                DecimalStore = val.ToString();
                DataType = DataType.Numeric;
            }

            public ValueType(DateTime dateTime, bool isTime = false)
            {
                //DateStore = (isTime ? new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second) : dateTime).CastToUnspecifiedTimeZone();
                //DataType = isTime ? DataType.Time : DataType.Date;
            }
            public ValueType(string alphanumeric)
            {
                StringStore = alphanumeric;
                DataType = DataType.Alphanumeric;
            }
        }

        #endregion

        private async Task ClearFarmAsync()
        {
            string baseURL = "https://livestock.mihub.tru-test.com";
            var request = new HttpRequestMessage(HttpMethod.Post, baseURL + "/odata/Farms/Services.DeleteAll");
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpResponseMessage response = await (new HttpClient()).SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
        }

        private async Task PushDataAsync()
        {
            HttpClient client = new HttpClient();

            // Get access and refresh tokens from local storage
            var jsonBuffer = File.ReadAllText(userinfoPath);
            dynamic localJsonResponse = JsonConvert.DeserializeObject(jsonBuffer);
            accessToken = localJsonResponse.access_token;
            refreshToken = localJsonResponse.refresh_token;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            // Get farm name
            string farmsURI = @"https://livestock.mihub.tru-test.com/odata/Farms";
            var farmsResponse = await client.GetAsync(farmsURI);
            string farmsResponseString = await farmsResponse.Content.ReadAsStringAsync();

            string usersURI = @"https://livestock.mihub.tru-test.com/odata/Users";
            var usersResponse = await client.GetAsync(usersURI);
            string usersResponseString = await usersResponse.Content.ReadAsStringAsync();

            dynamic jsonAllUserFarms = JsonConvert.DeserializeObject(farmsResponseString);
            dynamic jsonUser = JsonConvert.DeserializeObject(usersResponseString);
            string chosenFarm = "Default";

            if (userSelectedFarm != null && !string.IsNullOrEmpty(userSelectedFarm.Data))
            {
                chosenFarm = userSelectedFarm.Data;
            }
            var farms = jsonAllUserFarms.value;
            var user = jsonUser.value;

            string farmId;
            string farmName = "None";

            string currentFarmId = "";
            if (chosenFarm == "Default")
            {
                currentFarmId = user[0]["currentFarm_FarmId"];
                for (int i = 0; i < farms.Count; i++)
                {
                    farmId = farms[i]["farmId"];
                    if (farmId == currentFarmId)
                    {
                        farmId = currentFarmId;
                        farmName = farms[i]["farmName"];
                        break;
                    }
                }
            }
            else
            {
                farmName = chosenFarm;
            }

            #region test uploading with JSON
            //string jsonSession = File.ReadAllText(@"C:\Projects\AnimalManagementDesktopApp\jsonSession1.json");
            //string baseURL = "https://livestock.mihub.tru-test.com";
            //string jsonURL = "http://staging.livestock.mihub.tru-test.com/json/session";
            ////var request = new HttpRequestMessage(HttpMethod.Post, jsonURL);
            ////request.Content = new System.Net.Http.StringContent(jsonSession, Encoding.UTF8, "application/json");
            ////request.Headers.Add("Accept-Language", "en");


            //var request = new HttpRequestMessage(HttpMethod.Post, baseURL + "/odata/Farms/Services.DeleteAll");

            //request.Headers.Add("Authorization", "Bearer " + accessToken);
            //HttpResponseMessage response = await client.SendAsync(request);
            //var responseJson = await response.Content.ReadAsStringAsync();

            //var postContent = new Dictionary<string, string>();
            //fileData = File.ReadAllText(@"C:\Projects\AnimalManagementDesktopApp\jsonSession1.json");
            ////fileNames.Add(filePath.Split('\\').Last<string>());
            //string postURI = "http://staging.livestock.mihub.tru-test.com/json/session"; // http if proxy is enabled, https otherwise
            //postContent.Add("FarmName", farmName);
            //postContent.Add("Content", fileData); // JSON file contents
            //var postResponse = await client.PostAsync(postURI, new FormUrlEncodedContent(postContent));

            //// Handle upload result
            //string postResponseString = await postResponse.Content.ReadAsStringAsync();
            #endregion

            fileNames.Clear();
            uploadResultMessages.Clear();
            foreach (string filePath in filePaths)
            {
                // Push data to MiHub
                string postURI = "https://livestock.mihub.tru-test.com/Csv/Session";
                var postContent = new Dictionary<string, string>();
                fileData = File.ReadAllText(filePath);
                //fileNames.Add(filePath.Split('\\').Last<string>());
                fileNames.Add(Regex.Match(filePath, @"(?<=(\\))[^\\]*(?=(.csv))").Value);

                postContent.Add("FarmName", farmName);
                postContent.Add("Content", fileData); // CSV file contents (if successive uploads of the same data are done then their imports will fail)

                var request = new HttpRequestMessage(HttpMethod.Post, postURI);
                request.Content = new FormUrlEncodedContent(postContent);
                HttpResponseMessage response = await client.SendAsync(request);

                // Handle upload result
                string postResponseString = await response.Content.ReadAsStringAsync();
                dynamic uploadResult = JsonConvert.DeserializeObject(postResponseString);
                
                var error = uploadResult.error;
                if (error == null)
                {
                    uploadResultMessages.Add("Upload successful! ");
                }
                else
                {
                    uploadResultMessages.Add("Error: " + error.innererror[0].description + " ");
                }
            }

            // Generate summary of transfer results
            int numSuccess = 0;
            int numFailed = 0;
            foreach (string message in uploadResultMessages)
            {
                if (message.Contains("Error"))
                {
                    numFailed++;
                }
                else
                {
                    numSuccess++;
                }
            }

            string summary = "";
            if (uploadResultSummary != null)
            {
                if (numSuccess > 0) summary += $"{numSuccess} file(s) successfully transferred\n";
                if (numFailed > 0) summary += $"{numFailed} file(s) failed to transfer";
                uploadResultSummary.Data = summary;
            }

            filePaths.Clear();
            uploadCompleted.Execute();

            Debug.WriteLine($"Sent file to MiHub!");
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            var fireAndForget1 = ClearFarmAsync();
            var fireAndForget2 = PushDataAsync();
        }

        // IDataFlow<string> implementation
        //string IDataFlow<string>.Data { set => filePath = value; }
        string IDataFlow<string>.Data { set => filePaths.Add(value); }

        // ITableDataFlow implementation ------------------------------------------------------
        private DataTable dataTable = new DataTable();
        DataTable ITableDataFlow.DataTable => dataTable;

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            //if (uploadResultsSent) return;
            dataTable.Columns.Clear();
            dataTable.Rows.Clear();
            dataTable.Columns.Add("File");
            dataTable.Columns.Add("Message");
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (!uploadResultsSent)
            {
                DataRow row;
                int index = 0;
                //foreach (string message in uploadResultMessages)
                //{
                //    row = dataTable.NewRow();
                //    row["File"] = index++.ToString();
                //    row["Message"] = message;
                //    dataTable.Rows.Add(row);
                //}

                for (int i = 0; i < fileNames.Count; i++)
                {
                    row = dataTable.NewRow();
                    row["File"] = fileNames[i];
                    row["Message"] = uploadResultMessages[i];
                    dataTable.Rows.Add(row);
                }
                uploadResultsSent = true;
                return new Tuple<int, int>(0, dataTable.Rows.Count);
            }
            else
            {
                uploadResultsSent = false; // reset flag;
                return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);
            }
        }

        Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
            throw new NotImplementedException();
        }

        Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            throw new NotImplementedException();
        }

    }
}
