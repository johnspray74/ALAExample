using System;
using System.Windows.Media;
using DomainAbstractions;
using Libraries;
using ProgrammingParadigms;

namespace Application
{
    public class Application
    {
        // Application.cs is code hand-generated from Application-diagram in the same folder. They are the real source code.
        // This class is an abstraction in the top most concrete layer of ALA (Abstraction Layered Architecture)
        // (for bmore background refer https://www.github.com/johnspray74/ALAExample http://www.abstractionlayeredarchitecture.com)
        // Actually the diagram (application-diagram in the same folder) is the source code - read that instead.
        // This Application.cs class is just hand compiled wiring code following that diagram.

        // (ALA is Abstraction Layered Architecture - see www.abstractionlayeredarchitecture.com)
        // It knows all the specifics and details of the requirements of the application (but no implementation)
        // Its job is to express the user stories and requirements by instantiating domain abstractions and wiring them together.
        // This class has a line of code in Main() to instnatiate this Application class, which causes all the instantiating and wiring to be done in its constructor, then do any other initialization that is needed, then start the MainWindow.
        // That's pretty much all there is apart from the details in the constructor below, but you only need to read that to see how it faithfully follows the diagram.
        // Go back to the diagram to see how the application user stories work.

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
            // This is a real farming device simulator abstraction that we configure with mock data.
            // This simulated device gets wired to the COMPort abstraction but is not itself part of the application. 
            // In the real application, this would be a real farming device connected to the physical COM port of the PC.
            // to receive command and return data.
            var scpSimulator = new RealFarmingDeviceSimulator();

            // Configure the simulated device with soem data
            scpSimulator.AddSession(name: "session0", date: "2021-02-28", new[] { "F01FID", "F11EID" });
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





            // -------------------------- CODE MANUALLY GENERATED FROM DIAGRAM --------------------------------------------------------------
            // The following code has been manually generated from the diagram in application-diagram.drawio, also in application-diagram.pdf
            // Refer to the diagram for how the application works, not this code.
            // Also application.md is a commentary on reading the diagram.
            // Take an interest in this code if
            // 1. You want to understand how the diagram was hand-coded
            // 2. You want to know how the ALA diagram was made to actually execute
            // 3. You have modified the diagram and need to correspondingly modify this code


            // First instantiate any domain abstractions that we can't leave anonymous because the wiring diagram has loops in it:

            var saveFileBrowser = new SaveFileBrowser("Save file", "CSV");
            var textConnected = new Text("Connected") { Color = Brushes.Green };
            var textSearching = new Text("Searching for a device") { Color = Brushes.Red };
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


            // Now do all the wiring of the diagram
            // Note any instances of domain abstractions not already instantiated above are anonymous in this code.
            // Note that a.wireTo(b) is an extension method that uses reflection to wire 'a' to 'b' via 'ports'. The ports on 'a' and 'b' must be of the same interface type. 'a' must have a private field of the interface and 'b' must implement the interface.
            // Note that the fluent style is used: wireTo returns its first argument, allowing you to call wireTo again to wire in another instance of a domain abstraction.
            // Note that InstanceName properties are not needed by the code - they are just to help when debugging becasue they give instances of the same domain abstraction in different places different names.
            // Sometime wireTo has a second parameter which is the name of the specific port it is wiring to.
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
                        .WireTo(scpSimulator
                            .WireTo(scpProtocol, "responseOutput")
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