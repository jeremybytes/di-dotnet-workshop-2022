# Dependency Injection in C# [Full-Day Workshop]

## Description  

Loosely coupled code is easier to maintain, extend, and test. Dependency Injection (DI) help us get there. In this workshop, we'll see how interfaces can add "seams" to our code that makes it more flexible and maintainable. From there, we'll dig into loose coupling with Dependency Injection. DI doesn't have to be complicated. With just a few simple changes to our constructors or properties, we can have code that is easy to extend and test.  

After laying a good foundation, we'll take a closer look by diving into various DI patterns (such as constructor injection and property injection) as well as other patterns that help us handle interception and optional dependencies. Along the way, we'll see how DI helps us adhere to the SOLID principles in our code. We'll also we'll look at common stumbling blocks like dealing with constructor over-injection, managing static dependencies, and handling disposable dependencies.  

Throughout the day, we'll go hands-on with labs to give you a chance to put the concepts into action.  

If you're a C# developer who wants to get better with concepts like abstraction, loose coupling, extensibility, and unit testing, then this is the workshop for you.  

---  

## Pre-requisites

To get the most out of the workshop, you should have an understanding of the basics of C# and object oriented programming (classes, inheritance, methods, and properties). No prior experience with dependency injection is necessary. Hardware & Software To participate in the hands-on portion, you will need a computer (Windows, macOS, or Linux) with the following installed:  
* **.NET 6.0 SDK**  
[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)  
The .NET SDK (Software Development Kit) allows you to build .NET applications.  
* **Visual Studio Code**  
[https://code.visualstudio.com/download](https://code.visualstudio.com/download)  
This is a great all-around editor.  
* **VS Code C# Extension**  
[https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)  
This extension to Visual Studio Code adds syntax highlighing and code completion for C#.  

As an alternative to Visual Studio Code, you can use **Visual Studio 2022** (any edition).  

---

## Labs

These are the hands-on portions of the workshop. Labs can be completed with Visual Studio Code or Visual Studio 2022. All labs run on Windows, macOS, and Linux. Each lab consists of the following:

* **Labxx-Instructions** (Markdown)  
A markdown file containing the lab instructions. This includes the scenario, a set of goals, and step-by-step instructions.  
This can be viewed in on GitHub or in Visual Studio Code (just click the "Open Preview to the Side" button in the upper right corner).

* **Starter** (Folder)  
This folder contains the starting code for the lab.

* **Completed** (Folder)  
This folder contains the completed solution. If at any time, you get stuck during the lab, you can check this folder for a solution.

--- 

## Topics + Code  

The following connects the topics with the sample code.  

### Basics 
**Constructor Injection**  
* [PeopleController.cs](/Basics/MainDemo/PeopleViewer/Controllers/PeopleController.cs) - Web application controller
* [PeopleViewerWindow.xaml.cs](/Basics/MainDemo/PeopleViewer.View/PeopleViewerWindow.xaml.cs) - Desktop application main window
* [PeopleViewModel.cs](/Basics/MainDemo/PeopleViewer.Presentation/PeopleViewModel.cs) - Desktop application view model

**Object Composition / Composition Root**  
* [Program.cs](/Basics/MainDemo/PeopleViewer/Program.cs) - Web application program file (no DI container)
* [App.xaml.cs](/Basics/MainDemo/PeopleViewer.Desktop/App.xaml.cs) - Desktop application main window (no DI container)
* [App.xaml.cs](/Basics/MainDemo/PeopleViewer.Ninject/App.xaml.cs) - Desktop application main window (using Ninject)

**Decorators**
* [CachingReader.cs](/04-diving-deeper-into-dependency-injection/MainDemo/PersonDataReader.Decorators/CachingReader.cs) - Local cache decorator  

**Unit Testing**
* [PeopleViewModelTests.cs](/Basics/MainDemo/PeopleViewer.Presentation.Tests/PeopleViewModelTests.cs) - View Model unit tests (constructor injection)  

**Dependency Injection Containers**  
* [App.xaml.cs](/Basics/MainDemo/PeopleViewer.Ninject/App.xaml.cs) - Desktop application main window (using Ninject)

**Property Injection**  
* [CSVReader.cs](/Basics/MainDemo/PersonDataReader.CSV/CSVReader.cs) - CSV File data reader  
* [CSVReaderTests.cs](/Basics/MainDemo/PersonDataReader.CSV.Test/CSVReaderTests.cs) - CSV Reader unit tests (property injection)  


### Deeper Dive / Advanced Topics  

**Property Injection**  
* [CSVReader.cs](/Advanced/MainDemo/PersonDataReader.CSV/CSVReader.cs) - CSV File data reader  

**Method Injection**  
* [MainWindow.xaml.cs](/Advanced/MethodInjection/PeopleViewer/MainWindow.xaml.cs) - person.ToString(selectedFormatter)  

**Read-Only / Guard Clause**  
* [PeopleViewModel.cs](/Advanced/MainDemo/PeopleViewer.Presentation/PeopleViewModel.cs) - Desktop application view model
* [PeopleViewerWindow.xaml.cs](/Advanced/MainDemo/PeopleViewer.View/PeopleViewerWindow.xaml.cs) - Desktop application main window

**Decorators**
* [CachingReader.cs](/Advanced/MainDemo/PersonDataReader.Decorators/CachingReader.cs) - Local cache decorator  
* [ExceptionLoggingReader.cs](/Advanced/MainDemo/PersonDataReader.Decorators/ExceptionLoggingReader.cs) - Exception logging decorator  
* [RetryReader.cs](/Advanced/MainDemo/PersonDataReader.Decorators/RetryReader.cs) - Retry decorator  
* [Program.cs](/Advanced/MainDemo/PeopleViewer/Program.cs) - Web application composition (with decorators)  
* [App.xaml.cs](/Advanced/MainDemo/PeopleViewer.Desktop/App.xaml.cs) - Desktop application composition (with decorators)  

**Proxy / IDisposable**  
* [SQLReaderProxy.cs](/Advanced/MainDemo/PersonDataReader.SQL/SQLReaderProxy.cs) - Proxy to wrap IDisposable SQL Reader  

**Static Dependencies**  
* [ITimeProvider.cs](/Advanced/StaticDependencies/HouseControl.Library/Schedules/ITimeProvider.cs) - Wrapper interface
* [CurrentTimeProvider.cs](/Advanced/StaticDependencies/HouseControl.Library/Schedules/CurrentTimeProvider.cs) - Default that uses "DateTimeOffset.Now"  
* [ScheduleHelper.cs](/Advanced/StaticDependencies/HouseControl.Library/Schedules/ScheduleHelper.cs) - Static property that uses current time by default  
* [ScheduleHelperTests.cs](/Advanced/StaticDependencies/HouseControl.Library.Test/ScheduleHelperTests.cs) - Tests that swap out time provider for deterministic tests

**Configuration Strings**  
* [ServiceReader.cs](/Advanced/MainDemo/PersonDataReader.Service/ServiceReader.cs) - Service reader that has 'string' dependency
* [ServiceReaderUri.cs](/Advanced/MainDemo/PersonDataReader.Service/ServiceReaderUri.cs) - Strongly-typed wrapper around 'string'
* [Program.cs](/Advanced/MainDemo/PeopleViewer/Program.cs) - Web application composition (with ServiceReaderUri)  
* [App.xaml.cs](/Advanced/MainDemo/PeopleViewer.Desktop/App.xaml.cs) - Desktop application composition (with ServiceReaderUri)  
* [App.xaml.cs](/Advanced/MainDemo/PeopleViewer.Desktop.Ninject/App.xaml.cs) - Desktop application composition using Ninject container  

---

## Resources

**DI Patterns**  
* [Dependency Injection: The Property Injection Pattern](http://jeremybytes.blogspot.com/2014/01/dependency-injection-property-injection.html)  
* [Property Injection: Simple vs. Safe](http://jeremybytes.blogspot.com/2015/06/property-injection-simple-vs-safe.html)  
* [Dependency Injection: The Service Locator Pattern](http://jeremybytes.blogspot.com/2013/04/dependency-injection-service-locator.html)  

**Decorators and Async Interfaces**
* [Async Interfaces, Decorators, and .NET Standard](https://jeremybytes.blogspot.com/2019/01/more-di-async-interfaces-decorators-and.html)  
* [Async Interfaces](https://jeremybytes.blogspot.com/2019/01/more-di-async-interfaces.html)  
* [Adding Retry with the Decorator Pattern](https://jeremybytes.blogspot.com/2019/01/more-di-adding-retry-with-decorator.html)  
* [Unit Testing Async Methods](https://jeremybytes.blogspot.com/2019/01/more-di-unit-testing-async-methods.html)  
* [Adding Exception Logging with the Decorator Pattern](https://jeremybytes.blogspot.com/2019/01/more-di-adding-exception-logging-with.html)  
* [Adding a Client-Side Cache with the Decorator Pattern](https://jeremybytes.blogspot.com/2019/01/more-di-adding-client-side-cache-with.html)  
* [The Real Power of Decorators -- Stacking Functionality](https://jeremybytes.blogspot.com/2019/01/more-di-real-power-of-decorators.html)  

**Challenges**  
* Static Objects: [Mocking Current Time with a Simple Time Provider](https://jeremybytes.blogspot.com/2015/01/mocking-current-time-with-time-provider.html)  

**Related Topics**
* Session: [DI Why? Getting a Grip on Dependency Injection](http://www.jeremybytes.com/Demos.aspx#DI)
* Pluralsight: [Getting Started with Dependency Injection in .NET](https://app.pluralsight.com/library/courses/using-dependency-injection-on-ramp/table-of-contents) 

More information at [http://www.jeremybytes.com](http://www.jeremybytes.com)  

---  