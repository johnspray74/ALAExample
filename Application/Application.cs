using System;
using System.Windows.Media;
using DataLink_ALA.Application;
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
            var arbitrator = new Arbitrator();
            var ignoredDataFlowConnector = new DataFlowConnector<string>();
            var sessionListScp = new SCPSessions();
            var sessionDataScp = new SCPData() { InstanceName = "forGrid" };
            var sessionDataScpImport = new SCPData() { InstanceName = "import" };
            var saveToCsvFileTransact = new Transfer() { InstanceName = "save to csv file transact", AutoLoadNextBatch = true };
            var sessionListGrid = new Grid() { InstanceName = "sessions", RowHeight = 50, PrimaryKey = "index" };
            var sessionDataGrid = new Grid() { InstanceName = "data" };
            var csvFileReaderWriter = new CSVFileReaderWriter();

            // This is a simulator abstraction that been initialized with mock data. In the real application, this is replaced with a SCP device
            // to recieve command and return data.
            var scpSimulator = new SCPSimulator();
            scpSimulator.AddSession("session0", "1/1/2021", new[] { "F01FID","F11EID" });
            scpSimulator.AddSession("session1", "1/2/2021", new[] { "F01FID", "F11EID", "F10Weight" });
            scpSimulator.AddSession("session2", "1/3/2021", new[] { "F01FID", "F11EID", "F10Weight", "F12Remark" });
            scpSimulator.AddSessionData(0, new[] { "0000", "EID0000000000000" });
            scpSimulator.AddSessionData(0, new[] { "0001", "EID0000000000001" });
            scpSimulator.AddSessionData(1, new[] { "1000", "EID0000000000010", "302" });
            scpSimulator.AddSessionData(1, new[] { "1001", "EID0000000000011", "303" }); 
            scpSimulator.AddSessionData(1, new[] { "1002", "EID0000000000012", "304" });
            scpSimulator.AddSessionData(2, new[] { "2000", "EID0000000000021", "305", "tested" }); 
            scpSimulator.AddSessionData(2, new[] { "2001", "EID0000000000022", "306", "recorded" });
            scpSimulator.AddSessionData(2, new[] { "2002", "EID0000000000023", "307", "uploaded" });
            scpSimulator.AddSessionData(2, new[] { "2003", "EID0000000000024", "308", "saved" });

            mainWindow
            // UI
            .WireTo(new Vertical(true) { Layouts = new int[] { 0, 0, 2, 0 } }
                .WireTo(new Horizontal() { InstanceName = "menubar" }
                    .WireTo(new Menubar()
                        .WireTo(new Menu("File")
                            .WireTo(new MenuItem("Import from device")
                                .WireTo(new Wizard("Where do you want to put it?")
                                    .WireTo(new WizardItem("Local CSV file")
                                        .WireTo(saveFileBrowser)
                                    )
                                    .WireTo(new WizardItem("Cloud"))
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
                .WireTo(new SCPSense()
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