# ALAExample

An example application of ALA (Abstraction Layered Architecture).
Look at the diagram in the Application folder: Application.pdf. It describes a small desktop application (which, incidentally, displays data loaded from a device and saves it to a CSV file). Then download and execute the solution in Visual Studio to see the diagram itself actually run (it uses a software simulation of a real device).
Then look at the code in Application.cs to see how it was done.


# Background

ALAExample is a sample of a research project for a software Reference Architecture called ALA [(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)
ALA is theroetically optimized for the maintainabilty quality attribute.
This research was to measure if this is true in paractice.

The example Windows desktop application is a cutdown application used by farmers to get data to/from their EID readers, livestock weighing devices etc.
The solution includes a simulated device so it actually gets some data off it, displays and can write it to a disk file.



<!---
[(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)
-->

# Knowledge dependencies

ALA (Abstraction Layered Architecture) always makes it clear what knowledge is needed to understand a given piece of code.

Knowledge of ALA comes from the Introduction and Chapter 2 of the web site <http://www.abstractionlayeredarchitecture.com>.

In this solution, start with the *application* folder. It has application.pdf and its hand tranlation into code, application.cs.
The dependencies only go down abstraction layers as follows:
The application.cs uses classes in the *domain abstractions* folder.
The domain abstractions use interfaces in the *programming paradigms* folder.
The application.cs uses a *wireTo* extension method in the *libraries* folder.

This example project is to show the actual working code for the Application, Domain Abstractions, Programming Paradigms layers.


## To run the example application

1. Clone this repository or download as a zip.
2. Open the solution in Visual Studio 2019 or later
3. When the application runs you will see data already loaded from a simulated real device.
4. Click on the row on the left, and it will show data from different sessions on the right.
5. Click on File, Import from Device, to open a Wizard, then select Local CSV File, then enter a filename to save data to a CSV file.
6. Click on the Toolbar icon to do the same thing.


### Built With

C#, Visual Studio 2019, GALADE v1.6.1


## Future work

Swift, Java, Python and Rust versions needed.

## Authors

Rosman Cheng, John Spray, Roopak Sinha, Arnab Sen


## License

No Liciense. Use as you please.

## Acknowledgments


