using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// DeviceSimulator is an ALA domain abstraction  (see AbstractionLayeredArchitecture.md for more details)
    /// Abstraction description follows:
    /// DeviceSimulator simulates a real device that a farmer uses so that an example application can still execute.
    /// The real devices are Electronic ID readers or livestock weighing scales or other devices that measure or collect data.
    /// These device organise their data by sessions (which are equivalent to files with homogeneous rows of data).
    /// They are called sessions becasue teh farmer has a session of handling his livestock one by one.
    /// Here we create a soft version of such a device and configure it with data.
    /// The real devices connect to a PC's COM port and use a serial protocol called SCP (Serial Command Protocol)
    /// The commands themselves come from a long history of actual commands that were implemented on these embedded devices going back 25 years so dont worry about the details of them too much.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Configurations: (configurations are for use by the application when it instantiates a domain abstraction)
    ///     AddSession: See the function. Use it to add sessions and define the sessions's fields
    ///     AddSessionData: See the function. Use it to add data to sessions
    ///     InstanceName property: As with all domain abstractions, we have an instance name. (Because there can be multiple instances of this abstraction, the application gives us an object name which is not generally used by the abstraction internal logic. It is only used during debugging so you can tell which object you are break-pointed on.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    ///     commandInput (IDataFlow<string>): Receives serial commands
    ///     responseOutput (IDataFlow<char>): Send responses to serial comamnds.
    /// Both these ports should be wired to/from the same domain abstraction. We implement than as a separate char stream in each direction to beter simulate a connection to a real serial COM port.
    /// Future generalization:
    ///     Add capabilities to get its data from a disk files
    ///     Add capabilities to receive data uploaded to it
    ///     Add capabilities to allow it to be used for accptance testing, including scenarios such as with badly formatted data, not responding to commands, etc
    /// </summary>
    public class RealFarmingDeviceSimulator : IDataFlow<char>, IEvent  // commandInput (IEvent not relevant to this abstraction to be removed) // also commandInput should be changed to IDataFlow<char> to better simulate a serial communications
    {
        public string InstanceName { get; set; } = "SCPDevice";

        private IDataFlow<List<string>> listOfPorts;  // not relevant to this abstraction to be removed
        private IDataFlow<char> responseOutput;

        private int ResponseDelay = 10;   // This makes the simulated device respond asynchronously and slowly to SCP commands to better simulate a real device
        private string _commandBuffer;

        private bool _shouldResponse;

        public RealFarmingDeviceSimulator()
        {
            var cts = new CancellationTokenSource(5000);
            cts.Token.Register(() =>
            {
                _shouldResponse = true;
            });
        }

        char IDataFlow<char>.Data 
        {
            set
            {
                if (value == '{')
                {
                    _commandBuffer = value.ToString();
                    return;
                }

                _commandBuffer += value;

                if (value == '}')
                    SendResultAsync(_commandBuffer);
            }
        }

        private async Task SendResultAsync(string command)
        {
            if (!_shouldResponse) 
                return;

            await System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (responseDic.ContainsKey(command))
                {
                    var response = responseDic[command];
                    foreach (var c in response.ToCharArray())
                        responseOutput.Data = c;
                    return;
                }

                if (command == "{FGDD}")
                {
                    _listIndex = 0;
                    await Task.Delay(ResponseDelay);  // simulate delay in the device
                    responseOutput.Data = '^';
                    return;
                }

                if (command == "{FL}")
                {
                    if (_listIndex >= indexData.Count)
                    {
                        await SendString("");
                    }
                    else
                    {
                        await SendString(_listIndex++.ToString());
                    }
                    return;
                }

                if (command.StartsWith("{FPNA") || command.StartsWith("{FPDA") || command.StartsWith("{FPNR"))
                {
                    var index = int.Parse(command.Trim('{').Trim('}').Substring(4));
                    var data = indexData[index];
                    var result = command.StartsWith("{FPNA") ? data.Item1 : command.StartsWith("{FPDA") ? data.Item2 : data.Item3;
                    await SendString(result);
                    return;
                }

                if (command.StartsWith("{FF"))
                {
                    _dataIndex = int.Parse(command.Trim('{').Trim('}').Substring(2));
                    _headerIndex = 0;
                    await Task.Delay(ResponseDelay);  // simulate delay in the device
                    responseOutput.Data = '^';
                    return;
                }

                if (command == "{FH}")
                {
                    var dataHeader = headerData[_dataIndex];
                    var result = _headerIndex < dataHeader.Length ? dataHeader[_headerIndex++] : string.Empty;
                    await SendString(result);
                    return;
                }

                if (command == "{FD}")
                {
                    _dataRowIndex = 0;
                    await Task.Delay(ResponseDelay);  // simulate delay in the device
                    responseOutput.Data = '^';
                    return;
                }

                if (command.StartsWith("{FR"))
                {
                    _dataRowIndex = int.Parse(command.Trim('{').Trim('}').Substring(2));
                    await Task.Delay(ResponseDelay);  // simulate delay in the device
                    responseOutput.Data = '^';
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
                    await SendString(dataContent);
                    return;
                }

                await SendString($"Unknown command: {command}");
            }));
        }

        private int _listIndex = 0;
        private int _dataIndex = 0;
        private int _headerIndex = 0;
        private int _dataRowIndex = 0;

        async private Task SendString(string data, bool addBrackets = true)
        {
            var dataString = addBrackets ? $"[{data}]" : data;

            await Task.Delay(ResponseDelay);  // simulate delay in the device
            foreach (var c in dataString.ToCharArray())
            {
                responseOutput.Data = c;
            }
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

        public void AddSession(string name, string date, string[] columns)
        {
            var index = indexData.Count;
            indexData.Add(index, new Tuple<string, string, string>(name, date, "0"));
            headerData.Add(index, columns);
        }

        public void AddSessionData(int index, string[] sessionData)
        {
            if (!contentData.ContainsKey(index))
                contentData.Add(index, new List<string>());

            var list = contentData[index];
            var dataIndex = list.Count;
            var data = $"{dataIndex}";
            foreach (var dataDetails in sessionData)
            {
                data += $",{dataDetails}";
            }
            
            list.Add(data);

            var indexDetails = indexData[index];
            indexDetails = new Tuple<string, string, string>(indexDetails.Item1, indexDetails.Item2, (dataIndex + 1).ToString());
            indexData[index] = indexDetails;
        }

        private Dictionary<int, Tuple<string, string, string>> indexData = new Dictionary<int, Tuple<string, string, string>>();

        private Dictionary<int, List<string>> contentData = new Dictionary<int, List<string>>();

        private Dictionary<int, string[]> headerData = new Dictionary<int, string[]>();
    }
}
