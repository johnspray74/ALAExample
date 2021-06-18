# ALAExample

An example application of ALA (Abstraction Layered Architecture).
Look at the diagram in the Application folder: Application.pdf. It describes a small desktop application, which incidentally displays data loaded from a device and saves it to a CSV file. Then download and execute the solution in Visual Studio to see the diagram itself actually run (it uses a software simulation of a real device).
Then look at the code in Application.cs to see how it was done.


# Background

ALAExample is a sample of a research project for a software Reference Architecture called ALA [(Abstraction Layered Architecture)](abstractionlayeredarchitecture.com).
Abstraction Layered Architecture is theretically optimized for the maintainabilty quality attribute.
This research was to measure if this is true in paractice.

The example Windows desktop application is a cutdown application used by farmers to get data to/from their EID readers, livestock weighing devices etc.
The solution includes a simulated device so it actually gets some data off it, displays and can write it to a disk file.

# Knowledge dependencies

ALA (Abstraction Layered Architecture) always makes it clear what knowledge is needed to understand a given piece of code.

Knowledge of ALA comes from the Introduction and Chapter 2 of the web site  [(Abstraction Layered Architecture)](abstractionlayeredarchitecture.com)

In this solution, start with the *application* folder. It has Application.pdf and its hand tranlation into code, Application.cs.
The dependencies only go down the abstraction layers as follows:
The application uses classes in the *domain abstractions* folder.
The domain abstractions use interfaces in the *programming paradigms* folder.
The application uses a *wireTo* exetension method in the *libraries* folder.

This example project is to show the actual working code for the Application, Domain Abstractions, Programming Paradigms layers.

## Work-in-progress

It is a work-in-progress to make it build and run.
Will need a device simulator to run without a real device on the com port.
Swift and Java versions to follow.

## Getting Started


### Prerequisites


### Installing


### Running the code


### Built With

C#, Visual Studio 2019, GALADE v1.6.1


## Authors

John Spray, Roopak Sinha, Rosman Cheng


## License


## Acknowledgments


