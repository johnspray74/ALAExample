using System;
using System.Windows.Media;
using DomainAbstractions;
using ProgrammingParadigms;
using Libraries;

namespace Application
{
    public class Application
    {
        // Application.cs is code hand-generated from Application-diagram (in the same folder). The diagram is the real source.
        // The application diagram is an abstraction in the top most concrete layer of ALA (see AbstractionLayeredArchitecture.md for more details)
        // The diagram expresses the user stories and all the specific details of the requirements (but no implementation).
        // In ALA, implementation must be inside domain abstractions.

        // The code in the constructor of this class is just hand-compiled wiring code accurately following that diagram.
        // The diagram does its job by a composition of configured instances of domain abstractions.
        // Hence the constructor code just instantiates domain abstractions, configures them with constructor arguments or properties, and wires them together according to the diagram, nothing more.
        // This class is also where you will find the main() entry point to the application, however all it does is instantiate this class to get the constructor run, which does all the work.
        // Then main simply tells the MainWindow instance to Run. 
        // As far as the code inside the constructor below is concerned, it is not meant to be human readable from the point of view of what it does - use the application-diagram for that plus its accompanying application.md explanation.
        // However, the code in the constructor below does need to be human readable from the point of view of being an accurate reflection of the diagram.

        private MainWindow mainWindow = new MainWindow("ALAExample");

        [STAThread]
        public static void Main()
        {
            // fairly standard way of starting an ALA application that uses windows.
            new Application().Initialize().mainWindow.Run();
        }

        private Application Initialize()
        {
            Wiring.PostWiringInitialize();
            return this;
        }

        private Application()
        {

            // First part of the code is to set up a real farming device simulator instance that we configure with mock data.
            // In the real application, this would be a real farming device connected to the physical COM port of the PC.
            // This simulated device gets wired to the COMPort abstraction and pretends to be connected to a real serial COM port.
            // The simulated device is not part of the application, so is not shown on the application-diagram even though we instantiate and configure it here.
            // The simulated device emulates the real devices ability to receive SCP commands and respond to them.

            var scpSimulator = new RealFarmingDeviceSimulator();

            // Configure the simulated device with some session data
            // farmer call their files inside devices "sessions". Thay can configure the fields that they want to use, so here we have three sessions, each with different fields.
            scpSimulator.AddSession(name: "session0", date: "2021-02-28", columns: new[] { "F01FID", "F11EID" });
            scpSimulator.AddSession(name: "session1", date: "2021-03-31", columns: new[] { "F01FID", "F11EID", "F10Weight" });
            scpSimulator.AddSession(name: "session2", date: "2021-04-30", columns: new[] { "F01FID", "F11EID", "F10Weight", "F12Remark" });
            scpSimulator.AddSessionData(index: 0, sessionData: new[] { "1", "EID0000000000000" });
            scpSimulator.AddSessionData(index: 0, sessionData: new[] { "2", "EID0000000000001" });
            scpSimulator.AddSessionData(index: 1, sessionData: new[] { "013", "EID0000000000010", "342" });
            scpSimulator.AddSessionData(index: 1, sessionData: new[] { "001", "EID0000000000011", "373" });
            scpSimulator.AddSessionData(index: 1, sessionData: new[] { "002", "EID0000000000012", "304" });
            scpSimulator.AddSessionData(index: 2, sessionData: new[] { "0123", "EID0000000000021", "405", "healthy" });
            scpSimulator.AddSessionData(index: 2, sessionData: new[] { "1023", "EID0000000000022", "376", "pregnant" });
            scpSimulator.AddSessionData(index: 2, sessionData: new[] { "0412", "EID0000000000023", "354", "black spot" });
            scpSimulator.AddSessionData(index: 2, sessionData: new[] { "0219", "EID0000000000024", "395", "lame" });






            // -------------------------- CODE MANUALLY GENERATED FROM DIAGRAM --------------------------------------------------------------
            // The following code has been manually generated from the diagram in application-diagram.drawio (read-only in application-diagram.pdf)
            // Refer to the diagram for how the application works, not this code.
            // Also application.md is a commentary on reading the diagram.
            // Take an interest in this code if
            // 1. You want to understand how the diagram was hand-coded
            // 2. You want to know all the mechanics of how the ALA diagram was made to actually execute
            // 3. You have modified the diagram and need to correspondingly modify this code


            // First instantiate any domain abstractions that we can't instantiate anonymously in the later code they need to be referred to by name, because the wiring diagram has loops in it:

            var saveFileBrowser = new SaveFileBrowser("Save file", "CSV");
            var textConnected = new Text("Connected", false) { Color = Brushes.Green };
            var textSearching = new Text("Searching for a device...") { Color = Brushes.Red };
            var scpProtocol = new SCPProtocol();
            var arbitrator = new Arbitrator() { InstanceName = "scpDevice"};
            var ignoredDataFlowConnector = new DataFlowConnector<string>();
            var sessionListScp = new SCPSessions() { InstanceName = "sessionList" };
            var sessionDataScp = new SCPData() { InstanceName = "forGrid" };
            var sessionDataScpImport = new SCPData() { InstanceName = "import" };
            var saveToCsvFileTransact = new Transfer() { InstanceName = "save to csv file transact", AutoLoadNextBatch = true };
            var sessionListGrid = new Grid() { InstanceName = "sessions", RowHeight = 50, PrimaryKey = "index" };
            var sessionDataGrid = new Grid() { InstanceName = "data" };
            var csvFileReaderWriter = new CSVFileReaderWriter();
            var comPort = new COMPort();


            // Now do all the wiring of the diagram
            // Note any instances of domain abstractions not already instantiated above are anonymous in this code.
            // Note that a.wireTo(b) is an extension method that uses reflection to wire 'a' to 'b' via 'ports'. The ports on 'a' and 'b' must be a programmming paradigm interface of the same interface type - 'a' must have a private field of the interface and 'b' must implement the interface.
            // Note that the fluent style is used: wireTo returns its first argument, allowing you to call wireTo again to wire in another instance of a domain abstraction.
            // Note that InstanceName properties are not needed by the code - they are just to help when debugging because if there are multiple instances of the same domain abstraction you often dont know ehich instance you have break-pointed into.
            // Sometimes wireTo has a second parameter which is the name of the specific port it is wiring to. This ensures wiring to the correct port is there is more than one port of a given type.
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
                                        .WireTo(scpProtocol, "requestResponseDataFlow")
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
                            .WireTo(scpProtocol, "requestResponseDataFlow")
                            .WireTo(arbitrator, "arbitrator")
                        , "dataSource")
                        .WireTo(sessionListScp, "dataFlowSelectedPrimaryKey")
                        .WireTo(sessionDataGrid
                            .WireTo(sessionDataScp
                                .WireTo(scpProtocol, "requestResponseDataFlow")
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
                    .WireTo(scpProtocol
                        .WireTo(comPort
                            .WireTo(scpProtocol, "charFromPort")
                            .WireTo(scpSimulator
                                .WireTo(comPort, "responseOutput")
                            , "charToPort")
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
            , "appStart");
        }
    }
}