using System;
using System.Windows.Media;
using DomainAbstractions;
using ProgrammingParadigms;
using Libraries;

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

            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR main
            MainWindow id_887ca6dc36c048308b37ab9577149b58 = new MainWindow() {InstanceName="id_887ca6dc36c048308b37ab9577149b58"}; /* {"IsRoot":false} */
            Vertical id_73d30fcfb26c4dbfadb344a36d22d7ea = new Vertical() {InstanceName="id_73d30fcfb26c4dbfadb344a36d22d7ea"}; /* {"IsRoot":false,"Description":"Vertivcally arrange the title, grids, & status bar in the main window\r\n"} */
            // END AUTO-GENERATED INSTANTIATIONS FOR main

            // BEGIN AUTO-GENERATED WIRING FOR main
            id_887ca6dc36c048308b37ab9577149b58.WireTo(id_73d30fcfb26c4dbfadb344a36d22d7ea, "child"); /* {"SourceType":"MainWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false,"Description":"","SourceGenerics":[],"DestinationGenerics":[]} */
            // END AUTO-GENERATED WIRING FOR main


        }
    }
}