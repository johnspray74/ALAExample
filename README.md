# ALAExample

An example application of ALA (Abstraction Layered Architecture).
Look at the diagram in the Application folder: *application diagram.pdf* also shown below. It describes a small desktop application (which, incidentally, displays data loaded from a device and saves it to a CSV file).

![Application diagram](Application/Application-diagram.png)

Then download and execute the solution in Visual Studio to see the diagram itself actually run (it uses a software simulation of a real device).

![Application screenshot](Application/Application-demo.gif)

<!---
![Application screenshot](Application/Application-screenshot.png)
-->

Then look at the code in *application.cs* to see how it was done by using [(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)


### Background

ALAExample is a cut-down example of an application used by farmers to get data to/from their EID readers, livestock weighing devices, etc.

The full Windows PC desktop application was a research project for a software reference architecture called ALA [(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com). ALA is theoretically optimized for the maintainabilty quality attribute by telling you how to organise code. This research was to measure if this is true in paractice.

The project was done by a masters student and internship students over two internships. By using the architecture they were able to write quality code to replace a legacy degenerate application with approximately 2 man-years of effort compared with approximately 12 man-years of effort for the legacy application. To give an idea of size, the ALA version contains 55 KLOC and the legacy version 70 KLOC for approximately the same functionality.



<!---
[(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)
-->

### Knowledge dependencies

ALA (Abstraction Layered Architecture) always makes it clear what knowledge is needed to understand a given unit of code.

Knowledge of ALA comes from the Introduction and Chapter 2 of the web site <http://www.abstractionlayeredarchitecture.com>.
In ALA, the only unit of code is an abstraction. Dependencies must be on abstractions that are more abstract. This gives rise to abstraction layers as follows.

In this solution, start with the *application* folder which is the top layer. It has application.pdf and its hand translation into code, application.cs.
The application.cs uses classes in the *domain abstractions* folder, which is the second layer.
The domain abstractions use interfaces in the *programming paradigms* folder, which is the third layer.
The application.cs uses a *wireTo* extension method in the *libraries* folder, which is the bottom layer.

There are no dependencies within layers, so all abstractions are like standalone programs given knowledge of the abstractions they use.

This example project is to show the actual working code for the Application, Domain Abstractions, Programming Paradigms and Libraries layers.


### To run the example application

1. Clone this repository or download as a zip.
2. Open the solution in Visual Studio 2019 or later
3. When the application runs you will see data already loaded from a simulated real device.
4. Click on the row on the left, and it will show data from different sessions on the right.
5. Click on File, Import from Device, to open a Wizard, then select Local CSV File, then enter a filename to save data to a CSV file.
6. Click on the Toolbar icon to do the same thing.


### Built With

C#, Visual Studio 2019, GALADE v1.6.1

### Contributing

1. Fork it (<https://github.com/johnspray74/ALAExample/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request


### Future work

Swift, Java, Python and Rust versions needed.

### Authors

Rosman Cheng, John Spray, Roopak Sinha, Arnab Sen


### License

This project is licensed under the terms of the MIT license. See [License.txt](License.txt)

[![GitHub license](https://img.shields.io/github/license/johnspray74/ALAExample)](https://github.com/johnspray74/ALAExample/blob/master/License.txt)

### Acknowledgments


