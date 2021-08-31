using System;
using System.Windows.Media;
using System.Collections.Generic;

using System.Diagnostics;

using DomainAbstractions;
using ProgrammingParadigms;
using Foundation;

namespace Application
{
    public class Application
    {
        // Application.cs contains code hand-generated from Application-diagram (in the same folder).
        // Application-diagram is the real source along with its commentary document, Application-diagram.md
        // Application-diagram is drawn in draw.io but can be viewed in the pdf version.
        // Application-diagram is an abstraction in the top most concrete layer of ALA (see AbstractionLayeredArchitecture.md)

        // The code in the constructor of this class is just hand-compiled wiring code directly following that diagram.
        // The constructor code instantiates domain abstractions, configures them with constructor arguments or properties from details in the boxes on the diagram,
        // and wires them together by the correct ports according to the diagram.
        // This class is also where you will find the main() entry point to the application, however all it does is instantiate this class to get the constructor run,
        // which does all the work. Then main() simply tells the MainWindow instance to start running. 
        // As far as the code inside the constructor below is concerned, it is not meant to be human readable from the point of view of what it does - use the application-diagram for that plus its accompanying application.md explanation.
        // However, the code in the constructor below does need to be human readable from the point of view of being an accurate reflection of the diagram.
        //
        // _Always_ modify the diagram first, then this code.


        private MainWindow mainWindow = new MainWindow("ALAExample");

        [STAThread]
        public static void Main()
        {
            // You don't need to do this, but just demonstrates how to get logging of all the WireTos that are done in case you need to check everything was wired correctly or chase down a bug in the Wiring.cs
            Wiring.DiagnosticOutput += (s) => { Debug.WriteLine(s); };
            Wiring.DiagnosticOutput += (s) => { new LoggerToFile(@"C:\ProgramData\Example_ALA\wiringLog.txt") { InstanceName = "wiringLogging" }.WriteLine(s); };

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

            var simulatedExternalDevice = new RealFarmingDeviceSimulator();

            // Configure the simulated device with some session data
            // Farmers call their files inside devices "sessions". They can configure the fields that they want to use, so here we have three sessions, each with different fields.
            simulatedExternalDevice.AddSession(name: "session0", date: "2021-02-28", columns: new[] { "F01FID", "F11EID" });
            simulatedExternalDevice.AddSession(name: "session1", date: "2021-03-31", columns: new[] { "F01FID", "F11EID", "F10Weight" });
            simulatedExternalDevice.AddSession(name: "session2", date: "2021-04-30", columns: new[] { "F01FID", "F11EID", "F10Weight", "F12Remark" });
            simulatedExternalDevice.AddSessionData(index: 0, sessionData: new[] { "1", "EID0000000000000" });
            simulatedExternalDevice.AddSessionData(index: 0, sessionData: new[] { "2", "EID0000000000001" });
            simulatedExternalDevice.AddSessionData(index: 1, sessionData: new[] { "013", "EID0000000000010", "342" });
            simulatedExternalDevice.AddSessionData(index: 1, sessionData: new[] { "001", "EID0000000000011", "373" });
            simulatedExternalDevice.AddSessionData(index: 1, sessionData: new[] { "002", "EID0000000000012", "304" });
            simulatedExternalDevice.AddSessionData(index: 2, sessionData: new[] { "0123", "EID0000000000021", "405", "healthy" });
            simulatedExternalDevice.AddSessionData(index: 2, sessionData: new[] { "1023", "EID0000000000022", "376", "pregnant" });
            simulatedExternalDevice.AddSessionData(index: 2, sessionData: new[] { "0412", "EID0000000000023", "354", "black spot" });
            simulatedExternalDevice.AddSessionData(index: 2, sessionData: new[] { "0219", "EID0000000000024", "395", "lame" });





// -------------------------- BEGIN CODE MANUALLY GENERATED FROM DIAGRAM --------------------------------------------------------------
            // The following code has been manually generated from the diagram in application-diagram.drawio (read-only version in application-diagram.pdf)
            // Refer to that diagram for how this application works, not this code.
            // Also application.md is a commentary on reading the diagram.
            // Take an interest in this code if:
            // 1. You want to understand how the diagram was hand-coded
            // 2. You want to know all the mechanics of how the ALA diagram was made to actually execute
            // 3. You have modified the diagram and need to correspondingly modify this code


            // First instantiate any domain abstractions that we can't instantiate anonymously in the later code because they need to be referred to by name because the diagram has circular wiring:

            var saveFileBrowser = new SaveFileBrowser(title: "Save file", extension: "CSV");
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
            // Note any instances of domain abstractions not already instantiated anonymously in this code.
            // Note that a.wireTo(b) is an extension method that uses reflection to wire 'a' to 'b' via 'ports'. The ports on 'a' and 'b' must be a programmming paradigm interface of the same interface type - 'a' must have a private field of the interface and 'b' must implement the interface.
            // Note that the fluent style is used: wireTo returns its first argument, allowing you to call wireTo again to wire it some thing else.
            // Note that InstanceName properties are not needed by the code - they are just to help when debugging because if there are multiple instances of the same domain abstraction you often dont know which instance you have break-pointed into.
            // Sometimes WireTo has a second parameter which is the name of the specific port it is wiring to. This ensures wiring to the correct port if there is more than one port of a given type.
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
                            .WireTo(scpProtocol, "outputForCharactersReceiveFromTheCOMPort")
                            .WireTo(simulatedExternalDevice
                                .WireTo(comPort, "responseOutput")
                            , "virtualComPortTx")
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
            // -------------------------- END CODE MANUALLY GENERATED FROM DIAGRAM --------------------------------------------------------------



            // See logging.cs for an explantion of why we wire up logging instead of using a programming paradigm layer abstraction for it

            // This logging instance is configured to output to the specified file and to the output window
            // It needs all the logging output ports of all the programming paradigms wiring to it
            var logging = new LoggerToFile(@"C:\ProgramData\Example_ALA\debugLog.txt") { InstanceName = "debugLogging" };



            // This wires the static diagnostic output port of WireManyPorts abstraction to logging so we get diagnostic output of it wiring up all the other diagnostic outputs
            WireMany.DiagnosticOutput = (string s) => { logging.WriteLine(s); };
            
            // this method will look through all the domain abstractions and wire all their DiagnosticOutput ports (which must be static) to logging
            // doing the equivalent of the lines below without us having to do every one individually
            new WireMany().WireManyPortsTo("DomainAbstractions", "DiagnosticOutput", logging, "WriteLine");

            // These manual wirings are how we used to do it - they are now done by WireStaticPorts above
            // Do it this way to override anything that you want to go to a special place
            // Arbitrator.DiagnosticOutput = (string s) => { logging.WriteText(s); };
            // SCPProtocol.DiagnosticOutput = (string s) => { logging.WriteText(s); };
            // Grid.DiagnosticOutput = (string s) => { logging.WriteText(s); };
            // CSVFileReaderWriter.DiagnosticOutput =  (string s) => { logging.WriteText(s); };
            // Transfer.DiagnosticOutput =  (string s) => { logging.WriteText(s); };
        }
    }
}