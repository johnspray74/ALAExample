# ALAExample

An example application of ALA [(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com).
Look at the source diagram in the Application folder: *application diagram.pdf* also shown below. (You can click on the diagram to get a clearer view.) It describes the user stories of a small desktop application, and is executable.

![Application diagram](Application/Application-diagram.png)

An ALA diagram simply expresses user stories in terms of instances of domain abstractions (the boxes in the diagram) wired together by their ports, which are programming paradigms implemented as interfaces. An ALA diagram is polyglot in programming paradigms so some wires represent an event-driven programming paradigm, some represent a UI layout programming paradigm, some represent a data-flow programming paradigm, etc. 

The yellow user stories of the diagram express the base UI layout and the Exit menu item causes the application to close.

The green user stories of the diagram sense if a device is connected and displays "Searching for a device" or "Connected" in the statusbar. SCPSense knows how to send an SCP (Serial Command Protocol) command out on the serial port to poll for the presence of a device. It does this every 3 seconds when it gets an event from the Timer, which starts when the MainWindow sends an event to start the application. Also note that SCPSense uses an instance of an Arbitrator to ensure it has exclusive use of the device while using a command. The device is considered a one-at-a time resource. When SCPSense gets a response from a device, it outputs a boolean data-flow signal which changes the message on the statusbar, and signals the sessions Grid to start.

The blue user stories display data from the device. When the sessions grid gets its start signal, it requests data on a table-data-flow paradigm. SCPSessions knows how to get columns and rows of data from devices that describe the session files in that device. When the user clicks on one of the session rows, it outputs the index on its rowSelected port, which is wired to SCPSessions. SCPsession knows how to to tell the device to select a specific session. An event is also routed to the data Grid to tell it to start. Note that these two Grids are instances of the same domain abstraction configured with two different instance names. The data Grid now requests data using the table-data-flow programming paradigm, which will come from SCDData. SCPData knows how to get columns and rows of data from a single session in the device.

The brown user stories of the diagram get data off the device and write it to a CSV file. There is an "Import from device" menu item in the File menu, which when clicked outputs an event. This event opens up a Wizard which contains RadioButtons for selecting where the data is to go. When the user makes his selection, the relevant RadioButton outputs an event which opens a SaveFileBrowser. When the filepath is selected it informs a CSVFileReaderWriter. It also causes an event to go to a Transfer domain abstraction that knows how to pull all the rows and columns from a table-data-flow, and push it to another table-data-flow. table-data-flow supports both pull and push modes, but does so in small batches in case there is a lot of data. 

Question for thought: How many applications could you read the source code and understand it like we have just done right here in the readme? This diagram is not just documentation, it is source code. In this case we have hand translated the diagram into readable code which you can see in Application.cs. We have done this to connect the dots on how we get the diagram to execute without any magic. However, if the application diagram gets very larger, we would want to make things even easier by auto-generating the wiring code.

To see the diagram executing, just download and execute the solution in Visual Studio. You will be able to use the application as shown by the gif below. (Not shown in the diagram is that we have wired in a software simulation of a real device to the COM port).

![Application screenshot](Application/Application-demo.gif)

<!---
![Application screenshot](Application/Application-screenshot.png)
-->


### Background

ALAExample is a cut-down example of an application used by farmers to get data to/from their EID readers, livestock weighing devices, etc.

The full Windows PC desktop application was a research project for a software reference architecture called ALA [(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com). ALA is theoretically optimized for the maintainabilty quality attribute by telling you how to organise code. This research was to measure if this is true in paractice.

The project was done by a masters student and internship students over two internships. By using the architecture, they were able to write quality code to replace a legacy degenerate application with approximately 2 man-years of effort compared with approximately 12 man-years of effort for the legacy application. To give an idea of size, the ALA version contains 55 KLOC and the legacy version 70 KLOC for approximately the same functionality.

<!---
[(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)
-->

### Abstraction Layered Architecture brief description

In ALA, the only unit of code is an abstraction. Dependencies must be on abstractions that are more abstract. This gives rise to abstraction layers as follows:

Start with the *application* folder which is the top layer. It has application.pdf and its hand translation into code, application.cs.
The application.cs uses classes in the *domain abstractions* folder, which is the second layer. Domain abstractions must be more abstract (and therefore more reuseable) than the specific application. We even use multiple instances of some of them in the same diagram.
The domain abstractions use interfaces in the *programming paradigms* folder, which is the third layer. Programming paradigms must be even more abstract (and therefore even more reusable) than domain abstractions. 
The application.cs uses a *wireTo* extension method in the *libraries* folder, which is the bottom layer. wireTo supports this whole pattern of expressing user stories through instances of domain abstractions wired togther using programming paradigms. This pattern is one way to conform to the constraints provided by the fundamental rules of ALA.

There are no dependencies within layers, so all abstractions are like standalone programs given knowledge of the abstractions they use. Through the use of abstraction, the internals of all abstractions are zero-coupled in ALA, even going down the layers.

ALA (Abstraction Layered Architecture) always makes it clear what knowledge is needed to understand a given unit of code.

Knowledge of ALA itself comes from the Introduction and Chapter 2 of the web site <http://www.abstractionlayeredarchitecture.com>.

The only purpose of this example project is to show the actual working code for an application conforming to ALA. 


### To run the example application

1. Clone this repository or download as a zip.
2. Open the solution in Visual Studio 2019 or later
3. When the application runs, you will see data already loaded from a simulated real device.
4. Click on the row on the left, and it will show data from different sessions on the right.
5. Click on File, Import from Device, to open a Wizard, then select Local CSV File, then enter a filename to save data to a CSV file.
6. Click on the Toolbar icon to do the same thing.


### Built With

C#, Visual Studio 2019

### Contributing

1. Fork it (<https://github.com/johnspray74/ALAExample/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request


### Future work

Swift, Java, Python and Rust versions needed.

We have a graphical tool called GALADE (Graphical ALA development environment) (not used for this example) written by Arnab Sen here [https://github.com/arnab-sen/GALADE](https://github.com/arnab-sen/GALADE).

GALADE automatically generates code of course. GALADE also features automatic layout when drawing the diagram so that the developer can concentrate on expressing user stories and inventing the needed abstractions - feature that we find critcally important becasue most drawing packages ask you to spend too much time working on layout. It also presents the boxes representing instances of domain abstractions with their configuration constructor parameters and properties ready to be filled in. It also shows all the ports ready to be wired. Importantly, it acheives this by reading the code in the classes in the DomainAbstractions folder. This is really powerful and supports round trip of both the application wiring code and the domain abstractions code.
 
We need the Galade version doing in a branch so you can see the two types of lines of code its generates. 
 
We would love help to further develop GALADE to support ALA development, for example, a routing algorithm for its wiring, or to show errors for illegal dependencies according to the ALA fundmental rules.

We would also love to see other graphical tools in based on Visual Studio and Eclipse graphical environments to gnerally support ALA and specifically to generate wiring code.

### Authors

Rosman Cheng, John Spray, Roopak Sinha, Arnab Sen


### License

This project is licensed under the terms of the MIT license. See [License.txt](License.txt)

[![GitHub license](https://img.shields.io/github/license/johnspray74/ALAExample)](https://github.com/johnspray74/ALAExample/blob/master/License.txt)

### Acknowledgments


