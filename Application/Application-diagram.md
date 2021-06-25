
# Application-diagram.md

This is a companion file to Application-diagram.pdf to enable first time readers of the diagram to understand how it works.
Application-diagram is an abstraction in the top most concrete layer of ALA (see AbstractionLayeredArchitecture.md)

## ALA diagrams in general

The diagram expresses the user stories and all the specific details of the requirements (but no implementation).
In ALA, implementation must be inside domain abstractions.
The diagram is executable. In this case it has been hand translated into wiring code in Application.cs.


An ALA diagram expresses user stories in terms of a composition of instances of domain abstractions (the boxes in the diagram) wired together by their ports. Ports are instances of programming paradigms implemented as interfaces. An ALA diagram is polyglot in programming paradigms so some wires represent an event-driven programming paradigm, some represent a UI layout programming paradigm, some represent a data-flow programming paradigm, etc. You will see the different programming paradigms at play as we look at how each user story works in the diagram.


Another version of this diagram is in the GALADE branch (GALADE stands for Graphical ALA Development Environment) and features automatically generated code and tight integration with Domain Abstraction classes to know the ports and configuration data that are used by the diagram for instances of domain abstractions.


## How the diagram expresses user stories

### Yellow user stories

The yellow user stories of the diagram express the base UI layout and the Exit menu item causes the application to close.

### Green user stories

The green user stories of the diagram sense if a device is connected and displays "Searching for a device" or "Connected" in the statusbar. SCPSense knows how to send an SCP (Serial Command Protocol) command out on the serial port to poll for the presence of a device. It does this every 3 seconds when it gets an event from the Timer, which starts when the MainWindow sends an event to start the application. Also note that SCPSense uses an instance of an Arbitrator to ensure it has exclusive use of the device while using a command. The device is considered a one-at-a time resource. When SCPSense gets a response from a device, it outputs a boolean data-flow signal which changes the message on the statusbar, and signals the sessions Grid to start.

### Blue user stories

The blue user stories display data from the device. When the sessions grid gets its start signal, it requests data on a table-data-flow paradigm. SCPSessions knows how to get columns and rows of data from devices that describe the session files in that device. When the user clicks on one of the session rows, it outputs the index on its rowSelected port, which is wired to SCPSessions. SCPsession knows how to to tell the device to select a specific session. An event is also routed to the data Grid to tell it to start. Note that these two Grids are instances of the same domain abstraction configured with two different instance names. The data Grid now requests data using the table-data-flow programming paradigm, which will come from SCDData. SCPData knows how to get columns and rows of data from a single session in the device.

### Brown user stories

The brown user stories of the diagram get data off the device and write it to a CSV file. There is an "Import from device" menu item in the File menu, which when clicked outputs an event. This event opens up a Wizard which contains RadioButtons for selecting where the data is to go. When the user makes his selection, the relevant RadioButton outputs an event which opens a SaveFileBrowser. When the filepath is selected it informs a CSVFileReaderWriter. It also causes an event to go to a Transfer domain abstraction that knows how to pull all the rows and columns from a table-data-flow, and push it to another table-data-flow. table-data-flow supports both pull and push modes, but does so in small batches in case there is a lot of data. 
