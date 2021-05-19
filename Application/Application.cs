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
            var sessionListScp = new SessionListSCP();
            var sessionDataScp = new SessionDataSCP();
            var sessionListTransact = new Transact() { InstanceName = "session list transact", AutoLoadNextBatch = true };
            var sessionDataTransact = new Transact() { InstanceName = "session data transact", AutoLoadNextBatch = true };
            var saveToCsvFileTransact = new Transact() { InstanceName = "save to csv file transact", AutoLoadNextBatch = true };
            var sessionDataGrid = new Grid() { InstanceName = "session data grid" };
            var csvFileReaderWriter = new CSVFileReaderWriter();

            var scpSimulator = new SCPSimulator();

            mainWindow
            // UI
            .WireTo(new Vertical(true) { Layouts = new int[] { 0, 0, 2, 0 } }
                .WireTo(new Horizontal() { InstanceName = "menubar" }
                    .WireTo(new Menubar()
                        .WireTo(new Menu("File")
                            .WireTo(new MenuItem("Import from device")
                                .WireTo(new Wizard("Where do you want to put it?")
                                    .WireTo(new WizardItem("Local CSV file")
                                        .WireTo(saveFileBrowser
                                            .WireTo(new CSVFileReaderWriter())
                                        )
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
                                    .WireTo(sessionDataScp, "tableDataFlowSource")
                                    .WireTo(csvFileReaderWriter, "tableDataFlowDestination")
                                , "fileSelected")
                            )
                        )
                    )
                )
                .WireTo(new Horizontal() { InstanceName = "mainPanel", Ratios = new int[] { 1, 3 } }
                    .WireTo(new Grid() { InstanceName = "sessions", RowHeight = 50, PrimaryKey = "index" }
                        .WireFrom(sessionListTransact, "tableDataFlowDestination")
                        .WireTo(sessionDataScp, "dataFlowSelectedPrimaryKey")
                        .WireTo(new EventConnector()
                            .WireTo(sessionDataGrid)
                            .WireTo(sessionDataTransact
                                .WireTo(sessionDataGrid, "tableDataFlowDestination")
                                .WireTo(sessionDataScp
                                    .WireTo(scpProtocal, "requestResponseDataFlow")
                                    .WireTo(arbitrator, "arbitrator")
                                , "tableDataFlowSource")
                            )
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
            .WireTo(new EventConnector()
                .WireTo(new Timer() { Delay = 3000 }
                    .WireTo(scpSimulator
                        .WireTo(new SCPDeviceSense()
                            .WireTo(arbitrator, "arbitrator")
                            .WireTo(scpProtocal
                                .WireTo(scpSimulator, "scpCommand")
                            , "requestResponseDataFlow")
                            .WireTo(new DataFlowConnector<string>(), "selectedCOMPort")
                            .WireTo(ignoredDataFlowConnector, "connectedDeviceVersion")
                            .WireTo(ignoredDataFlowConnector, "connectedDeviceSerialNumber")
                            .WireTo(new ConvertToEvent<string>()
                                .WireTo(new EventConnector()
                                    .WireTo(new ConvertEventToBoolean() { KeepTrue = true }
                                        .WireTo(new DataFlowConnector<bool>()
                                            .WireTo(textConnected)
                                            .WireTo(new Not()
                                                .WireTo(textSearching)
                                            )
                                        )
                                    )
                                )
                            , "connectedDeviceName")
                        , "listOfPorts")
                        .WireTo(scpProtocal, "charFromPort")
                        .WireTo(new DataFlowConnector<string>(), "selectedCOM")
                    )
                )
                .WireTo(sessionListTransact
                    .WireTo(sessionListScp
                        .WireTo(scpProtocal, "requestResponseDataFlow")
                        .WireTo(arbitrator, "arbitrator")
                        .WireTo(ignoredDataFlowConnector, "sessionListCount")
                    , "tableDataFlowSource")
                )
            , "appStartsRun");
        }
    }
}