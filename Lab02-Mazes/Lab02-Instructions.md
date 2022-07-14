Lab 02 - Injection Patterns & Cross-Cutting Concerns
====================================================

Objectives
-----------
This lab incorporates useful design patterns (such as the Decorator pattern) as well as various dependency injection patterns (including constructor injection and property injection).

Application Overview
--------------------

The "Starter" folder contains the code files for this lab. 

**Visual Studio Code:** Open the "Starter" folder in VS Code.  
**Visual Studio 2022:** Open the "Mazes.sln" solution.

This is a console application that produces mazes. There are a number of algorithms that generate mazes with different speeds and biases (a bias could be a tendency to include diagonal paths, longer/shorter paths, or twistier/straighter paths). 

The "DrawMaze" project contains the console application that creates and shows the mazes.

The "MazeGeneration" project contains the "MazeGenerator" that we will be working with.

The "Algorithms" project contains a number of different maze algorithms.

The "MazeGrid" project contains the infrastructure for the grid, cells, text outputs, and graphical outputs. This code was originally taken from "Mazes for Programmers" and translated from Ruby to C#.

Build the solution and run the application. A console window will pop up showing a 15 x 15 maze with a path traced from the center to the lower right corner. In addition, a bitmap (in png format) will be generated and automatically opened. This shows a "heat map" with the darker areas being furthest from the center of the maze.

*Note: The bitmap version will only automatically open on Windows. If you are using macOS or Linux, you will need to open the "DrawMaze/Program.cs" file and comment out the indicated code. You can find the bitmap in the output folder location: [working_directory]/DataProcessor/bin/Debug/net6.0/mazes.png*

Lab Goals
---------
The "MazeGenerator" class creates the grid and "new"s up the maze algorithm that it wants to use. We want to move this responsibility for creating the grid and selecting a maze algorithm outside of the class.

In addition, we want to add logging to the "MazeGenerator". Instead of adding a logging object directly to the class, we will add logging as a cross-cutting concern.

Current Classes
---------------

DrawMaze/Program.cs:
```c#
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MazeGenerator();
            CreateAndShowMaze(generator);
            Console.ReadLine();
        }
        ...
    }
```

MazeGeneration/MazeGenerator.cs:
```c#
    public class MazeGenerator
    {
        private Grid mazeGrid;
        private bool isGenerated = false;

        public MazeGenerator()
        {
            mazeGrid = new ColorGrid(15, 15);
        }

        private void GenerateMaze()
        {
            var algorithm = new BinaryTree();
            algorithm?.CreateMaze(mazeGrid);
            isGenerated = true;
        }
        ...
    }
```

Hints
-----
In MazeGenerator.cs:
* Remove the "new ColorGrid()" call from the constructor by injecting a grid instead.

* Remove the "new" call from the "GenerateMaze" method by injecting the algorithm object into the class. Note: there is already an "IMazeAlgorithm" interface in the "Algorithms" project.

In Program.cs:
* Compose the objects in the "Main" method.

For Logging:
* Create a new Logging Decorator class to wrap the MazeGenerator. This should log "Enter Method" and "Exit Method" messages for "GetTextMaze" and "GetGraphicalMaze".

* Update the "Main" method to compose the objects using the decorator.

BONUS CHALLENGE: Add the Ninject DI container to compose the objects.

*SPECIAL BONUS*: The "Completed" folder contains an additional project, "MazeWeb". This is a website that generates mazes based on parameters (size, color, algorithm). Instructions for using this project are at the end of the lab.

If you need more assistance, step-by-step instructions are included below. Otherwise, **STOP READING NOW**


Grid & Algorithms: Step-By-Step
-------------------------------
1. Add readonly fields for the Grid and IMazeAlgorithm.

In the "MazeGenerator" project, open the "MazeGenerator.cs" file and add the following fields to the "MazeGenerator" class.

```c#
    private readonly Grid mazeGrid;
    private readonly IMazeAlgorithm algorithm;
    private bool isGenerated = false;
```

*Note: these fields are set to "readonly" so that they can only be set in the constructor.*

2. Update the constructor to accept these as parameters and set the private fields.

```c#
    public MazeGenerator(Grid grid, IMazeAlgorithm algorithm)
    {
        this.mazeGrid = grid;
        this.algorithm = algorithm;
    }
```

3. Update the "GenerateMaze" method to remove the local algorithm variable. Instead, it will use the private field.

```c#
    public void GenerateMaze()
    {
        algorithm?.CreateMaze(mazeGrid);
        isGenerated = true;
    }
```

*Note: the "algorithm" has a null check (the "?." operator). If the field is null, then the "GenerateMaze" method will do nothing, and the grid will remain in its default state.*

4. Compose the objects in the "Main" method.

In the "Program.cs" file, update "var generator = new MazeGenerator()" with the new parameters. *Note: You will need to add some using statements for "MazeGrid" and "Algorithms".*

```c#
    static void Main(string[] args)
    {
        MazeGenerator generator = 
            new MazeGenerator(
                new ColorGrid(15, 15),
                new BinaryTree());

        CreateAndShowMaze(generator);
        Console.ReadLine();
    }
```

5. Build and run the application.

Try updating the parameters of the ColorGrid constructor to generate mazes of different sizes. Note: large mazes take a while for the bitmap version to generate.

Try changing the "BinaryTree" algorithm for one of the other maze algorithms.

Logging: Step-By-Step
----------------------
1. Extract an interface from the "MazeGenerator" class.

**Visual Studio Code:**  Click on "MazeGenerator" (the class name), and press "Ctrl+." to bring up a list of actions. Select "Extract Interface". This will create an "IMazeGenerator" interface in the same file.

**Visual Studio 2022:** Right-click on the "MazeGenerator" class name, select "Quick Actions and Refactorings..." (or press "Ctrl+."), then "Extract interface". Name the interface "IMazeGenerator" and make sure that the all 3 methods are selected. *Note: these steps may be slightly different if you have ReSharper installed and it is set as the primary refactoring tool.*

This will create a separate interface file ("IMazeGenerator.cs") and add the interface to the existing "MazeGenerator" class. Make sure that "IMazeGenerator" is "public" so it will be available throughout the solution.

2. Create a new logging decorator class that implements the new interface.

```c#
    public class ConsoleLoggingDecorator : IMazeGenerator
```

3. Add the interface implementation code.  
The easiest way to implement the interface is to click on "IMazeGenerator", use "Ctrl+." to bring up the actions, and select "Implement Interface". This will create the 3 methods.  

```c#
    public void GenerateMaze()
    {
        throw new NotImplementedException();
    }

    public Image GetGraphicalMaze(bool includeHeatMap = false)
    {    
        throw new NotImplementedException();
    }
    
    public string GetTextMaze(bool includePath = false)
    {    
        throw new NotImplementedException();
    }
```

4. Add a field and constructor parameter to wrap a real MazeGenerator.

```c#
    private readonly IMazeGenerator wrappedGenerator;

    public ConsoleLoggingDecorator(IMazeGenerator mazeGenerator)
    {
        wrappedGenerator = mazeGenerator;
    }
```

5. Add the code to log to the console. Here is one way of implementing that functionality:

```c#
    private void LogToConsole(string message)
    {
        Console.WriteLine($"{DateTime.Now:s}: {message}");
    }

    private void LogEnterMethod([CallerMemberName] string? methodName = null)
    {
        LogToConsole($"Entering '{methodName}'");
    }

    private void LogExitMethod([CallerMemberName] string? methodName = null)
    {
        LogToConsole($"Exiting '{methodName}'");
    }
```

*Note: for the "CallerMemberName" attribute to work, you will need to add a using statement for "System.Runtime.CompilerServices". (You an also click on "CallerMemberName", press "Ctrl+." and choose to have the using statement added for you.  
See next note for what "CallerMemberName" does.*

6. Update the interface members to call the logging methods and the "real" methods from the wrapped repository.

```c#
    public void GenerateMaze()
    {
        LogEnterMethod();
        wrappedGenerator.GenerateMaze();
        LogExitMethod();
    }

    public Image GetGraphicalMaze(bool includeHeatMap = false)
    {
        LogEnterMethod();
        var result = wrappedGenerator.GetGraphicalMaze(includeHeatMap);
        LogExitMethod();
        return result;
    }

    public string GetTextMaze(bool includePath = false)
    {
        LogEnterMethod();
        var result = wrappedGenerator.GetTextMaze(includePath);
        LogExitMethod();
        return result;
    }
```

*Note: the "CallerMemberName" attribute will automatically set the "methodName" parameter based on where it is called. For example, when the "LogEnterMethod" is called from within the "GenerateMaze" method, the "methodName" parameter is set to "GenerateMaze". This is why we do not need to pass the parameter in explicitly when we call "LogEnterMethod" and "LogExitMethod".*

7. In the "Program.cs" file, update the composition root with the logging decorator.

```c#
    static void Main(string[] args)
    {
        IMazeGenerator generator = 
            new ConsoleLoggingDecorator(
                new MazeGenerator(
                    new ColorGrid(15, 15),
                    new Sidewinder()));

        CreateAndShowMaze(generator);
        Console.ReadLine();
    }
```

Note that the type of the "generator" was changed to the interface "IMazeGenerator". We also need to change the signature of the "CreateAndShowMaze" method to use the interface.

```c#
   private static void CreateAndShowMaze(IMazeGenerator generator)
```

8. Build and run the application. The console should contain messages such as the following:

```
    2022-08-10T19:25:58: Entering 'GenerateMaze'
    2022-08-10T19:25:58: Exiting 'GenerateMaze'
    2022-08-10T19:25:58: Entering 'GetTextMaze'
    2022-08-10T19:25:58: Exiting 'GetTextMaze'
    ... [Text maze output here]
    2022-08-10T19:25:58: Entering 'GetGraphicalMaze'
    2022-08-10T19:25:58: Exiting 'GetGraphicalMaze'
```

BONUS CHALLENGE - Ninject: Step-By-Step
---------------------------------------
1. Add the Ninject NuGet package to the "DrawMaze" project.

**Visual Studio Code:** Open a command prompt and navigate to the "DrawMaze" project folder. Type the following to add the "Ninject" NugetPackage:

```
dotnet add package Ninject
```

**Visual Studio 2022:** In the "DrawMaze" project, right-click on the Dependencies and select "Manage NuGet Packages". Then use the search box to locate "Ninject" and install it.

2. Add the assembly references to the top of the "Program.cs" file.

```c#
    using Ninject;
```

3. Instantiate and configure the container.

```c#
    static void Main(string[] args)
    {
        // Ninject DI Container
        IKernel Container = new StandardKernel();
        Container.Bind<Grid>().ToMethod(c => new ColorGrid(15, 15));
        Container.Bind<IMazeAlgorithm>().To<Sidewinder>();
        Container.Bind<IMazeGenerator>().To<ConsoleLoggingDecorator>()
            .WithConstructorArgument<IMazeGenerator>(Container.Get<MazeGenerator>());
```

This contains the following:
* Instantiate the Ninject DI container.
* Map the "Grid" abstract class to a factory method (in this case, a constructor with parameters).
* Map the "IMazeAlgorithm" to a concrete implementation.
* Map the "IMazeGenerator" to the logging decorator. Since the logging decorator constructor needs an "IMazeGenerator" to wrap, we include "WithConstructorArgument" to inject the real maze generator and to prevent circular references.

4. Ask the container for an "IMazeGenerator".

```c#
    IMazeGenerator generator = Container.Get<IMazeGenerator>();
```

5. Build and run the application.

The results should be the same as when we wired up the objects manually. Change the "IMazeAlgorithm" mapping to try out different implementations.

Special Bonus - Web Applicaton
------------------------------
The "Completed" folder contains an additional project "MazeWeb" that shows graphical mazes in the browser based on parameters (size, color, and algorithm). This is in the "Completed" folder because it relies on the final state of the MazeGenerator and other classes.  

This web applications works on Windows, macOS, and Linux.  

1. Open a command prompt to the "Completed" folder and navigate to the "MazeWeb" folder.

```
[Lab02 folder]/Completed/MazeWeb
```

2. Start the web application.

```
dotnet run
```

Note: You may get a runtime error if you do not have a developer certificate on your machine (for HTTPS). To create a developer certificate, type the following:

```
dotnet dev-certs https
```

Then to trust the certificate, type the following:  

```
dotnet dev-certs https --trust
```

You will get a popup verifying that you want to trust the certificate on your machine. Click "Yes".

*Note: the trust step works on Windows and macOS only. Check the Microsoft docs for "dotnet dev-certs" for more information on using this on Linux.*  

3. Start the web application with ```dotnet run``` if it is not already started.

You should see the following:

```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Development\Sessions\Workshop-DI-NetCore\Lab02 - Mazes\Completed\MazeWeb
```

This tells us the web application is at "http://localhost:5000" & "https://localhost:5001".  

4. Open a browser and navigate to one of those addresses.

Note: the web application has an HTTPS redirect, so if you type in the "http" address, it will automatically be redirected to the "https" one.

5. Use the text boxes and drop-downs to select the parameters for the maze: size, color, algorithm.

6. The browser will show the generated maze.

7. Click the "Back" button to get to the parameter screen and experiment with different values.

If you're interested in this web application, you can read more about it here: [https://jeremybytes.blogspot.com/2020/01/building-web-front-end-to-show-mazes-in.html](https://jeremybytes.blogspot.com/2020/01/building-web-front-end-to-show-mazes-in.html).


***
*End of Lab 02 - Injection Patterns & Cross-Cutting Concerns*
***
