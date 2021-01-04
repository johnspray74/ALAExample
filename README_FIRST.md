# Getting Started with DataLink_ALA

* **Last updated on 30/06/2020**

## Abstraction Layered Architecture (ALA)

Please read about the underlying architecture of ALA being used first.

http://www.abstractionlayeredarchitecture.com/ by John R Spray

### Beginning of DataLink_ALA

Originally we used the "Old_Datalink_Wiring.XMind" to express our wiring for the code before we then converted the wiring to code in Application.cs file on Visual Studio following specific conventions referenced in ---.

The old wiring diagram is found in ***~\DataLink_ALA\Application\Diagrams\Old Wiring Diagrams\Conversion_DataLink_ALA.xmind.xmind***

The *Main* sheet of the diagram file displays a basic overview with just instances being wired to instances e.g.

![Old wiring diagram wiring example](Application/Documentation/images/Old-wiring-diagram-wiring-example.PNG)


Whereas the *Summaries* sheet of the diagram begins to express the diagram with the ports that wire between each instance. e.g.

![Example of old wiring](Application/Documentation/images/Old-wiring-ports.PNG)


With the inconvenience of having an inconsistent one to one with diagram and code and requiring constant review led to the creation and usage of the 'XMind Parser' which is currently used and explained more below.

### Current DataLink_ALA
Currently, DataLink_ALA project has four main folders inside, *ProgrammingParadigms*, *Libraries*, *DomainAbstractions* and *Appliacation* which related to different layers in ALA.

##### <u>XMind Parser</u> 

![Example of old wiring](Application/Documentation/images/XmindParser.PNG)

We currently (30/06/2020) use the XMind Parser to help us convert the diagram to code. This way we are expected to have a 1 to 1 relationship with code to diagram. To use the XMind Parser, select XMind Parser to Build on Visual Studios within the DataLinkALA solutions package.	

​	For more more information about how the XMind Parser conventions, please refer to 
​		***~\DataLink_ALA\Application\Documentation\ZenParserConventions.md***

In the *Application* folder, only one file *Application.cs* exists, and it was converted from the new Xmind diagrams ***~\DataLink_ALA\Application\Diagrams\NewDiagrams\Conversion_DataLink_ALA.xmind*** (This is the main diagram) and ***~\DataLink_ALA\Application\Diagrams\NewDiagrams\MethodSheet.xmind*** (This sheet is for seperating user stories to keep the main diagram from bloating)

All of the domain abstractions are in the *DomainAbstractions* folder, and *~\DataLink_ALA\Application\Diagrams\NewDiagrams\Templates.xmind* is used to store related Xmind templates for these abstractions.

*Libraries* folder consists of several important methods for wiring, logging, etc.

*ProgrammingParadigms* folder consists of DataFlowConnectors, EventConnectors, TableDataFlowConnectors, IUI, ITableDataFlow and other interfaces implemented in abstractions.

To colour the Xmind diagram, use the "Add color to all instance nodes" on the XMind Parser and the coloured diagram will be found in the folder of:
**~\DataLink_ALA\Application\Diagrams\RecolourDiagramNodes**

## Managing the XMIND files

Currently, there is no simple way to efficiently merge changes in Xmind diagrams, in order to change the diagram safely, please make sure only one person can change the diagram at the same time.

## ALA Issues

1. Diamond Problem: where there is a split of a data flow and when it re-joins, one comes before another and displays incorrect information.

## Parser Issues

1. <u>Reusable methods</u> - With the use of the parser, methods that were previously used in the Application.cs are not reusable. In the coding sense, variables created in code blocks of  'methods' are a black box viewed from the outside however currently the parser opens this up meaning that we cannot re-use similar and duplicate wiring with the only difference being instances that it uses.

   ​	<u>Possible solution:</u> Have a Macro abstraction (like the C# macros) which repeats the code wiring. This means that it has to be executed after the wiring and before the post-wiring.


## Database Manipulation

### SQLite Database Tool
An useful tool to check and manage SQLite database is [DB Browser for SQLite](https://sqlitebrowser.org/).

### XR5000 Database Structure
A basic structure about the XR5000 database can be found in ["~\Application\Diagrams\XR5000 Database Structure.xmind"](Application/Diagrams/XR5000%20Database%20Structure.xmind), it contains table, column and primary key information from the database. An example is shown below:

<center>
  <img src="Application/Documentation/images/Xr5000-db-structure.PNG" alt="XR5000 db structure" style="zoom:60%;" />
</center>

### SQLite Query Operation Container
Currently, we support JSON format query operation container, the structure of the container is in ["~\Application\JSON Format Query Container Structure.xmind"](Application/Diagrams/JSON%20Format%20Query%20Container%20Structure.xmind). Part of the container structure can be seen below:

![JSON format query container](Application/Documentation/images/Json_query_container_example.PNG)

Element names and types, together with implementation examples are listed in the structure diagram. When implementing a query selection task (e.g. SelectColumns, From, Sort), one instance will first check whether there is a container in the `ITableDataFlow`, if there is, add information into the container, if not, initialise a container and add information into it. After that, the container would be passed to the next instance.

### Database Manipulation Issues
1. <u>Nested Query</u> - In some scenarios, we may need to use complex nested query clauses, this feature should be added in the future.

2. <u>JSON Format Container for Insert, Updtae and Alter</u> - Currently, the JSON format container does not support Insert, Update and Alter, this should be added when needed.

3. <u>Materialized View</u> - Consider whether a materialized view could be used to handle complex query tasks.


## Misc
* The version number for the current release is stored in Libraries.Constants.DataLinkPCVersionNumber. Please update this variable, and only this variable, when a new version is released.


* Settings, configurations, logs, and any other small bits of data that you wish to persist can be stored in "C:\ProgramData\Tru-Test\DataLink_ALA\\".


* MiHub Livestock has been rebranded to Datamars Livestock. You will see mentions of both in the code (e.g. in variable names).


* Datamars Livestock user info (currently only access/refresh tokens) is stored in "C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json"


* A runtime log of the application is stored in "C:\ProgramData\Tru-Test\DataLink_ALA\DataLink_ALA.log". This file is cleared every time the application runs, however an archive of each log file is kept in "C:\ProgramData\Tru-Test\DataLink_ALA\Logs\\" with a timestamped name. Feel free to clear this folder from time to time if its contents are occupying too much space.


* A log of the application's wiring is stored in "C:\ProgramData\Tru-Test\DataLink_ALA\wiringLog.txt". Like the runtime log, this file is cleared every time the application is run. Unlike the runtime log, this log is not archived anywhere. The ZenParser also produces a log next to each line of auto-generated wiring code as a comment.


* The InstanceName property were still added to instances with variable names in the Xmind Parser although InstanceName would overwrite to the variable name this is because of allowing flexibility to possibly using it in the future. (Due to easier debugging in Visual Studios and being able to set breakpoints with the condition of the InstanceName property) However if this is not used, then feel free to deprecate this feature. 


* When uploading session files to XR5000, before manipulation with the database, a backup of the database would be saved in  "%ProgramData%\Tru-Test\DataLink_ALA". The namw of the backup database will have a name with format **backupdb[ddmmyyyy]_hhmmss**, for example, *backupdb05062020_102630*. You may need to clean some of these backups after a period of time if they consume a lot of space.

* Basic thinking of message collection during running time:
  * Logs, errors, useful messages should be collected during application running time, instead of using public methods, we want to create an abstraction for this.
  * Currently, `TaskManager` abstraction has been created to handle this. When a new task begins, for example, upload favourite setting files to XR5000, `IEvent` or `IDataFlow<bool>` (`true`) should be sent to `TaskManager` to begin a new task, then this task manager will wait and collect messges related to this specific task.
  * Received messages would be added into a `JSON` format container, which is first initialised with `NAME` (`JValue` type), `DESCRIPTION` (`JValue` type), `INFORMATION` (`JArray` type) and `ERROR` (`JArray` type). Messages start with these keywords would be updated or added into rge container, otherwise, a new `JProperty` would be added for that message.
  * A public method in `StaticUtilities` class called `CreateJObjectMessage(string msgKeyword, string msgContent)` can be used to create message object sent to `TaskManager`.
  * For the purpose of collecting many kinds of information during various kinds of tasks, we may need to consider adding `IDataFlow<JObject> messageObj` port in many abstractions, currently, this port has been added in `CopyFiles` abstraction for testing.
  * The Xmind implementation for the test of TaskManager can be found in "\Application\Diagrams\NewDiagrams\MethodSheet.xmind" by searching "TEST TaskManager Abstraction" or "TaskManager", this can be an example for collecting information during application running time.
  * After collected related information for a task, the `JSON` format container can be saved locally as a file or passed to other abstractions to parse information and show up in the UI.
  * `TaskManager` can be started and stopped in two ways, one is `IEvent`, the first `IEvent` will start a new task while the second would stop it. The other way is using `IDataFlow<bool>`, data `true` will start a new task while `false` will stop it. 

<center>
  <img src="Application/Documentation/images/TaskManager.PNG" alt="XR5000 db structure" style="zoom:60%;" />
</center>
