using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLink_ALA.Application
{
    public class SCPSimulator : IDataFlow<string>, IEvent
    {
        public string InstanceName = "SCPSimulator";

        private IDataFlow<List<string>> listOfPorts;
        private IDataFlow<char> charFromPort;
        private IDataFlow_B<string> selectedCOM;

        private Queue<string> scpCommands = new Queue<string>();

        public SCPSimulator()
        {
        }

        string IDataFlow<string>.Data 
        {
            set
            {
                SendResultAsync(value);
            }
        }

        private async Task SendResultAsync(string command)
        {
            await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (responseDic.ContainsKey(command))
                {
                    var response = responseDic[command];
                    foreach (var c in response.ToCharArray())
                        charFromPort.Data = c;
                    return;
                }

                if (command == "{FGDD}")
                {
                    _listIndex = 0;
                    charFromPort.Data = '^';
                    return;
                }

                if (command == "{FL}")
                {
                    if (_listIndex >= indexData.Count)
                    {
                        SendString("");
                    }
                    else
                    {
                        SendString(_listIndex++.ToString());
                    }
                    return;
                }

                if (command.StartsWith("{FPNA") || command.StartsWith("{FPDA") || command.StartsWith("{FPNR"))
                {
                    var index = int.Parse(command.Trim('{').Trim('}').Substring(4));
                    var data = indexData[index];
                    var result = command.StartsWith("{FPNA") ? data.Item1 : command.StartsWith("{FPDA") ? data.Item2 : data.Item3;
                    SendString(result);
                    return;
                }

                if (command.StartsWith("{FF"))
                {
                    _dataIndex = int.Parse(command.Trim('{').Trim('}').Substring(2));
                    _headerIndex = 0;
                    charFromPort.Data = '^';
                    return;
                }

                if (command == "{FH}")
                {
                    var result = _headerIndex < dataHeader.Count ? dataHeader[_headerIndex++] : string.Empty;
                    SendString(result);
                    return;
                }

                if (command == "{FD}")
                {
                    _dataRowIndex = 0;
                    charFromPort.Data = '^';
                    return;
                }

                if (command.StartsWith("{FR"))
                {
                    _dataRowIndex = int.Parse(command.Trim('{').Trim('}').Substring(2));
                    charFromPort.Data = '^';
                    return;
                }

                if (command.StartsWith("{FN"))
                {
                    var pageSize = int.Parse(command.Trim('{').Trim('}').Substring(2));
                    var data = contentData[_dataIndex].ToArray();
                    var endIndex = _dataRowIndex + pageSize < data.Length ? _dataRowIndex + pageSize : data.Length - 1;

                    var dataList = new List<string>();
                    for (var index = _dataRowIndex; index <= endIndex; index++)
                        dataList.Add(data[index]);

                    var dataContent = string.Join(";", dataList.ToArray());
                    _headerIndex = 0;
                    _dataRowIndex = 0;
                    SendString(dataContent);
                }

                SendString($"Unknown command: {command}");
            }));
        }

        private int _listIndex = 0;
        private int _dataIndex = 0;
        private int _headerIndex = 0;
        private int _dataRowIndex = 0;

        private void SendString(string data, bool addBrackets = true)
        {
            var dataString = addBrackets ? $"[{data}]" : data;

            foreach (var c in dataString.ToCharArray())
                charFromPort.Data = c;
        }

        void IEvent.Execute()
        {
            listOfPorts.Data = new List<string>() { "COM-1" };
        }

        private Dictionary<string, string> responseDic = new Dictionary<string, string>()
        {
            { "{ZA1}", "^" },
            { "{ZN}", "[Simulated device]" },
            { "{SOCU}", "[0]" },
        };

        private Dictionary<int, Tuple<string, string, string>> indexData = new Dictionary<int, Tuple<string, string, string>>()
        {
            {0, new Tuple<string, string, string>("session0", "1/1/2021", "3")},
            {1, new Tuple<string, string, string>("session1", "2/1/2021", "3")},
            {2, new Tuple<string, string, string>("session2", "3/1/2021", "3")},
        };

        private Dictionary<int, List<string>> contentData = new Dictionary<int, List<string>>()
        {
            {0, new List<string>() {
                "0,FID00000000000000,EID0000000000000,300",
                "1,FID00000000000001,EID0000000000001,303",
                "2,FID00000000000002,EID0000000000002,298",
            }},
            {1, new List<string>() {
                "0,FID11111111111110,EID11111111111110,200",
                "1,FID11111111111111,EID11111111111112,203",
                "2,FID11111111111112,EID11111111111112,198",
            }},
            {2, new List<string>() {
                "0,FID22222222222220,EID2222222222220,320",
                "1,FID22222222222221,EID2222222222221,253",
                "2,FID22222222222222,EID2222222222222,299",
            }},
        };

        private List<string> dataHeader = new List<string>()
        {
            "F01FID",
            "F11EID",
            "F10Weight"
        };
    }
}
