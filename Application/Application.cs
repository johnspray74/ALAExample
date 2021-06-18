using System;
using System.Windows.Media;
using DomainAbstractions;
using Libraries;
using ProgrammingParadigms;

namespace Application
{
    public class Application
    {
        private MainWindow mainWindow = new MainWindow("ALAExample");

        [STAThread]
        public static void Main()
        {
            new Application().Initialize().mainWindow.Run();
        }

        private Application Initialize()
        {
            Wiring.PostWiringInitialize();
            return this;
        }

        private Application()
        {
            var saveFileBrowser = new SaveFileBrowser("Save file", "CSV");
            var textConnected = new Text("Connected") { Color = Brushes.Green };
            var textSearching = new Text("Searching for a device") { Color = Brushes.Red };
            var scpProtocal = new SCPProtocol();
            var arbitrator = new Arbitrator() { InstanceName = "scpDevice"};
            var ignoredDataFlowConnector = new DataFlowConnector<string>();
            var sessionListScp = new SCPSessions() { InstanceName = "sessionList" };
            var sessionDataScp = new SCPData() { InstanceName = "forGrid" };
            var sessionDataScpImport = new SCPData() { InstanceName = "import" };
            var saveToCsvFileTransact = new Transfer() { InstanceName = "save to csv file transact", AutoLoadNextBatch = true };
            var sessionListGrid = new Grid() { InstanceName = "sessions", RowHeight = 50, PrimaryKey = "index" };
            var sessionDataGrid = new Grid() { InstanceName = "data" };
            var csvFileReaderWriter = new CSVFileReaderWriter();

            // This is a simulator abstraction that we configure with mock data.
            // This simulated device is wired to the 
            // In the real application, this would be a real farming device connected to the physical COM port of the PC.
            // to recieve command and return data.
            var scpSimulator = new DeviceSimulator();
            scpSimulator.AddSession(name:"session0", date:"2021-02-28", new[] { "F01FID","F11EID" });
            scpSimulator.AddSession("session1", "2021-03-31", new[] { "F01FID", "F11EID", "F10Weight" });
            scpSimulator.AddSession("session2", "2021-04-30", new[] { "F01FID", "F11EID", "F10Weight", "F12Remark" });
            scpSimulator.AddSessionData(0, new[] { "1", "EID0000000000000" });
            scpSimulator.AddSessionData(0, new[] { "2", "EID0000000000001" });
            scpSimulator.AddSessionData(1, new[] { "013", "EID0000000000010", "342" });
            scpSimulator.AddSessionData(1, new[] { "001", "EID0000000000011", "373" }); 
            scpSimulator.AddSessionData(1, new[] { "002", "EID0000000000012", "304" });
            scpSimulator.AddSessionData(2, new[] { "0123", "EID0000000000021", "405", "healthy" }); 
            scpSimulator.AddSessionData(2, new[] { "1023", "EID0000000000022", "376", "pregnant" });
            scpSimulator.AddSessionData(2, new[] { "0412", "EID0000000000023", "354", "black spot" });
            scpSimulator.AddSessionData(2, new[] { "0219", "EID0000000000024", "395", "lame" });

            mainWindow
            // UI
            .WireTo(new Vertical(true) { Layouts = new int[] { 0, 0, 2, 0 } }
                .WireTo(new Horizontal() { InstanceName = "menubar" }
                    .WireTo(new Menubar()
                        .WireTo(new Menu("File")
                            .WireTo(new MenuItem("Import from device")
                                .WireTo(new Wizard("Where do you want to put it?")
                                    .WireTo(new RadioButton("Local CSV file")
                                        .WireTo(saveFileBrowser)
                                    )
                                    .WireTo(new RadioButton("Cloud"))
                                )
                            )
                            .WireTo(new MenuItem("Exit")
                                .WireTo(mainWindow)
                            )
                        )
                    )
                )
                .WireTo(new Horizontal() { InstanceName = "toolbar" }
                    .WireTo(new Toolbar()
                        .WireTo(new Tool("5000Import.png")
                            .WireTo(saveFileBrowser
                                .WireTo(csvFileReaderWriter, "dataFlowOutputFilePathNames")
                                .WireTo(saveToCsvFileTransact
                                    .WireTo(sessionDataScpImport
                                        .WireTo(scpProtocal, "requestResponseDataFlow")
                                        .WireTo(arbitrator, "arbitrator")
                                    , "tableDataFlowSource")
                                    .WireTo(csvFileReaderWriter, "tableDataFlowDestination")
                                , "fileSelected")
                            )
                        )
                    )
                )
                .WireTo(new Horizontal() { InstanceName = "mainPanel", Ratios = new int[] { 1, 3 } }
                    .WireTo(sessionListGrid
                        .WireTo(sessionListScp
                            .WireTo(scpProtocal, "requestResponseDataFlow")
                            .WireTo(arbitrator, "arbitrator")
                        , "dataSource")
                        .WireTo(sessionListScp, "dataFlowSelectedPrimaryKey")
                        .WireTo(sessionDataGrid
                            .WireTo(sessionDataScp
                                .WireTo(scpProtocal, "requestResponseDataFlow")
                                .WireTo(arbitrator, "arbitrator")
                            , "dataSource")
                        , "eventRowSelected")
                    )
                    .WireTo(sessionDataGrid)
                )
                .WireTo(new Horizontal() { InstanceName = "statusbar" }
                    .WireTo(new Statusbar()
                        .WireTo(textConnected)
                        .WireTo(textSearching)
                    )
                )
            )
            .WireTo(new Timer() { Delay = 3000 }
                .WireTo(new SCPSense() { InstanceName = "scpSence"}
                    .WireTo(arbitrator, "arbitrator")
                    .WireTo(scpProtocal
                        .WireTo(scpSimulator
                            .WireTo(scpProtocal, "charFromPort")
                        , "scpCommand")
                    , "requestResponseDataFlow")
                    .WireTo(new DataFlowConnector<bool>()
                        .WireTo(textConnected)
                        .WireTo(new Not()
                            .WireTo(textSearching)
                        )
                        .WireTo(new ToEvent<bool>()
                            .WireTo(sessionListGrid)
                        )
                    , "IsDeviceConnected")
                )
            , "appStartsRun");
        }
    }
}