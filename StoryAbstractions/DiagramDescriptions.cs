namespace StoryAbstractions
{
    /// <summary>
    /// 
    /// Datalink_ALA is split into many separate diagrams for maintainability and readability for different user stories.
    /// 
    /// ---------------------------------------------- MainDiagram ------------------------------------------------------
    /// Begins with the 'MainWindow' which has UI (top section) and an event that fires when the application is run which is
    /// what starts the data flow (bottom section).
    /// Contains the shared UI and resources within all devices (SCP or USB) and at the leaf nodes of e.g. WizardItems; are
    /// commonly reference points (common bus points) that are located in other diagrams and are expressed as user stories.
    /// 
    /// 
    /// ---------------------------------------------- SCP_DeviceDetection ----------------------------------------------
    /// This diagram begins with the application start event from the MainWindow being passed to the 'COMPortAdapter'.
    /// Then SCP commands are sent through 'SCPDeviceSense' to check for a response and device name.
    /// Once a SCP device is detected it will be passed to that device's specific boolean connector e.g. 'XRS2ConnectedConnector'
    /// and well as flag other boolean or event connectors related to whether a SCP device is connected or disconnected.
    /// 
    /// 
    /// ---------------------------------------------- Ethernet_DeviceDetection -----------------------------------------
    /// Begins with the application start event from the MainWindow being passed to a Timer
    /// 
    /// 
    /// ---------------------------------------------- XRS2_Functionalities ----------------------------------------------
    /// Contains all the UI related to an XRS2 device when it is connected. At the leaf nodes
    /// 
    /// 
    /// ---------------------------------------------- XR5000_Functionalities ---------------------------------------------
    /// Contains all the UI related to an XR5000 device when it is connected. At the leaf nodes
    /// 
    /// 
    /// ---------------------------------------------- FirmwareUpdate -----------------------------------------------------
    /// 
    /// 
    /// 
    /// </summary>
    public class DiagramDescriptions
    {
    }
}
