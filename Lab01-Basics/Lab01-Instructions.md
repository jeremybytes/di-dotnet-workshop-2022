Lab 01 - Refactoring to Constructor Injection for Unit Testing
===============================================================

Objectives
-----------
This lab shows some core dependency injection (DI) concepts, including constructor injection and object composition. This sets a foundation for deeper dives into DI.

Application Overview
---------------------

The "Starter" folder contains the code files for this lab. 

**Visual Studio Code:** Open the "Starter" folder in VS Code.  
**Visual Studio 2022:** Open the "DataProcessor.sln" solution.

This is a console application that processes data from a text file. The "DataLoader" class creates a list of strings from the contents of the file ("data.txt"). These strings are passed to the "DataParser" class. The DataParser is responsible for parsing the strings. Any errors are logged to a file using the "FileLogger" class.

Build the application (in Debug mode), and open the output folder: *[working_directory]/DataProcessor/bin/Debug/net6.0/*. Open the "data.txt" file. This contains a number of comma-separated value records along with some invalid records. Run the application by double-clicking "DataProcessor.exe" (or run from inside Visual Studio). Notice that the output states that 7 records were processed successfully. Now open the "log.txt" file in the same folder. It contains the errors and bad records.

Lab Goals
----------

Our goal is to create unit tests for the "DataParser" class in the "DataProcessor.Library" project. The "DataProcessor.Library.Tests" project contains a test class ("DataParserTest") and a static "TestData" class which contains sample records that we can use for testing.

Basic Tests
------------

Create tests for the following:
1. When the "ParseData" method is run with the entire data set, the result should match the expected number of good records (7 records).

2. When the "ParseData" method is run with a single good record, the result should be 1.

3. When the "ParseData" method is run with a single bad record, the result should be 0.

4. When the "ParseData" method is run with a record with an invalid date, the result should be 0.

5. When "ParseData" is run with a record with an invalid integer field, the result should be 0.

Hints
------

* Break the tight coupling between the "DataParser" class and the "FileLogger" by creating a logger interface and then using constructor injection.

* Create a "FakeLogger" class that implements the interface and can be used for unit testing (hint: the class does not need to do any actual logging).

* The Test project has a "TestData" class with hard-coded records that can be used for testing.

If you need more assistance, step-by-step instructions are included below (after the Advanced Tests section).


Advanced Tests
---------------

If you still have time after completing the Basic Tests (or you just want more of a challenge), here are some additional tests for the "DataParser" class.

1. When the "ParseData" method is run with a single good record, the logger's "Log" method should not be called.

2. When the "ParseData" method is run with a single bad record, the logger's "Log" method should be called 1 time.

3. When the "ParseData" method is run with a record with an invalid date field, the logger's "Log" method should be called 1 time.

4. When the "ParseData" method is run with a record with an invalid integer field, the logger's "Log" method should be called 1 time.

5. When the "ParseData" method is run with mixed records (including both good and bad data), the logger's "Log" method should be called 4 times.

Hints
------

* These tests are still on the "ParseData" class (not on the logger class).

* A mocking framework makes it possible to create an object that tracks calls to the method(s) of an interface. "Moq" can be added with NuGet.

* For more information on how Moq can help with these tests, look at the "Verification" section of the QuickStart: https://github.com/Moq/moq4/wiki/Quickstart

If you need more assistance, step-by-step instructions are included below. Otherwise, **STOP READING NOW**


Basic Tests: Step-By-Step
--------------------------

1. Create the "ILogger" interface.

Open the "FileLogger.cs" file. 

**Visual Studio Code:**  Click on "FileLogger" (the class name), and press "Ctrl+." to bring up a list of actions. Select "Extract Interface". This will create an "IFileLogger" interface in the same file. Rename "IFileLogger" to "ILogger" (be sure to rename both the interface and the implementation on "FileLogger").

**Visual Studio 2022:** Right-click on the class name, select "Quick Actions and Refactorings..." (or press "Ctrl+."), then "Extract interface". Name the interface "ILogger" and make sure that the "Log" method is selected. *Note: these steps may be slightly different if you have ReSharper installed and it is set as the primary refactoring tool.*

This will create a separate interface file ("ILogger.cs") and add the interface to the existing "FileLogger" class. Make sure that "ILogger" is "public" so it will be available throughout the solution.

```c#
namespace DataProcessor.Library;

public interface ILogger
{
    void Log(string message, string data);
}
```

2. Break the coupling between the "DataParser" and "FileLogger" classes.

To break the coupling, we'll use a combination of abstraction (the ILogger interface) and constructor injection.

In the "DataParser" class, replace the existing "FileLogger logger" field with a class-level field of type "ILogger":

```c#
    // FileLogger logger = new();
    private ILogger logger;
```

To inject the logger, create a constructor for the "DataParser" class. This should take an "ILogger" parameter and assign it to the class-level field.

```c#
    public DataParser(ILogger logger)
    {
        this.logger = logger;
    }
```

If we build at this point, we will get an error in "Program.cs" (where we use the DataParser class) because we no longer have a no-parameter constructor.

3. Compose the objects in the console application.

Open the "Program.cs" file in the "DataProcessor" project.  

In the "ProcessData" method (where the build error occurs), create an instance of the FileLogger class and pass it as a parameter to the DataParser.

```c#
    ILogger logger = new FileLogger();
    var parser = new DataParser(logger);
```

Build and run the application. Ensure that the output is the same ("Successfully processed 7 records") and verify that the log.txt file contains the error messages.

Now it's time to move to the test project.

4. Create a Fake Logger for testing

In the "DataProcessor.Library.Tests" project, create a new class called "FakeLogger". The easiest way to do this is to right-click on the project, select "Add", then "Class", and type in "FakeLogger" for the name.

Make sure that the class is "public" and add the interface.

```c#
    public class FakeLogger : ILogger
```

The easiest way to implement the interface is to click on "ILogger", use "Ctrl+." to bring up the actions, and select "Implement Interface". This will create the "Log" method. Be sure to remove the "throw" statement that is included by default. The method itself does not need any contents.

```c#
    public void Log (string message, string data)
    {
        // Just a placeholder for testing
    }
```

With this in place, we can move on to our unit tests.

5. Verify that the unit tests work  

In the "DataParserTest.cs" file, there is a "Test1" that always returns true.

**Visual Studio Code:** Open a command prompt and navigate to the test project folder: *[working_directory]/DataProcessor.Library.Tests/*, and type "dotnet test". This will show the passing test.

**Visual Studio 2022:** Open the Visual Studio Test Explorer. From the menu, choose "Test" -> "Test Explorer".

Expand the tree to show "Test1". Click the "Run All" link in the Test Explorer to run the test. When it comes up green, we know that the testing framework is set up properly.

6. Create the unit tests

In the "DataParserTest.cs" file, rename "Test1" to something more appropriate. I like to use a 3-part naming recommended by Roy Osherove (author of "The Art of Unit Testing"). This includes (a) the unit under test, (b) the action performed, and (c) the expected result.

This type of naming (or some other regular naming scheme) makes it easier to know what's going on when we have failing tests -- especially when we have hundreds of tests in our solution.

Our first test is making sure that the number of records processed equals the number of good records in our test data (7 records). So, here's a proposed name:

```c#
    [Test]
    public void ParseData_WithMixedData_ReturnsGoodRecords()
```

For unit tests, I like the "Arrange / Act / Assert" layout. For the "Arrange" part, we just create an instance of our DataParser class and pass in an instance of our FakeLogger.

```c#
    // Arrange
    var logger = new FakeLogger();
    var parser = new DataParser(logger);
```

For the "Act", we call the "ParseData" method and give it our test data.

```c#
    // Act
    int processedRecords = parser.ParseData(TestData.Data);
```

And for the "Assert" portion, we compare the output value to the expected value.

```c#
    // Assert
    Assert.That(processedRecords, Is.EqualTo(7));
```

By using constructor injection, we decouple our data parsing class from a specific logging class. This makes our unit tests very easy to put together.

For the other tests, we use the same "Arrange" section. For the "Act" section, we simply pass in the test data for that test ("GoodRecord", "BadRecord", "BadStartDate", "BadRating"). The "Assert" section would compare the result to 1 or 0 as appropriate.

To see the completed unit tests, check the solution in the "Completed" folder. The "DataParserTests" class includes the Basic Tests.


Advanced Tests: Step-By-Step
-----------------------------

For the advanced tests, we will take advantage of Moq as a mocking framework. A mocking framework allows us to create in-memory objects that we can use for unit testing. This means that we do not need to use the "FakeLogger" class that we created earlier.

Moq has a number of good features that can help us with testing. We'll specifically look at the "Verify" method that allows us to check whether a particular method has been called and also specify how many times it should be called.

1. Add Moq to the test project

**Visual Studio Code:** Open a command prompt and navigate to the test project folder. Type the following to add the "Moq" NugetPackage:

```
dotnet add package Moq
```

**Visual Studio 2022:** In the "DataProcessor.Library.Tests" project, right-click on the Dependencies and select "Manage NuGet Packages". Then use the search box to locate "Moq" and install it.

2. Create a new file / class for the logger tests: "DataParserLoggerTests.cs"

3. In the "DataParserLoggerTests.cs" file, add a using statement for Moq. (Alternately, we can add this to the "Usings.cs" file to add it globally in the project).

```c#
    using Moq;
```

4. Create a unit test to ensure that the "Log" method is called if there is a bad record.

```c#
    [Test]
    public void ParseData_WithBadRecord_LoggerIsCalledOnce()
```

5. In the "Arrange" section, create a mock of our ILogger interface.

```c#
    // Arrange
    var mockLogger = new Mock<ILogger>();
    var parser = new DataParser(mockLogger.Object);
```

The first line creates a new mock variable based on our ILogger interface. 

Moq allows us to add custom behavior to the mock object without creating a separate "fake" class. For this test, the default mock object behavior is fine, so no further setup is required.

To get an actual "ILogger" from our mock, we use the "Object" property. We can pass this to our DataParser constructor.

6. In the "Act" section, call "Parse" on a bad record.

```c#
    // Act
    parser.ParseData(TestData.BadRecord);
```

*Note, we don't need the return value from the "Parse" method for this test.*

7. In the "Assert", verify that the "Log" method was actually called on our logger.

```c#
    // Assert
    mockLogger.Verify(m => 
        m.Log(It.IsAny<string>(), TestData.BadRecord[0]), Times.Once());
```

This is where things get interesting. We call the "Verify" method on our mock object -- notice that we are using "mockLogger". This takes a delegate where we specify which method we're calling. In this case, we are specifying that we are checking whether the "Log" method is called.

We can also check to see if the "Log" method was called with specific parameters. In this case, we want to check for any message (the first parameter). Moq offers the "It.IsAny<T>()" method that we can use with this. The second parameter is the "BadRecord" that is used (it is a single-record collection, so we select the first one). In this "Verify" call, we are asking whether the "Log" method is called with any string for the first parameter and the contents of "BadRecord". We could also use "It.IsAny<string>()" for the second parameter to capture all calls to "Log".

The "Verify" method has a second parameter to indicate how many times the method is called. In this case, we are using the Moq method "Times.Once()". This means that "Verify" will return true if "Log" is called one time (and only one time).

There are other useful options such as Times.Never(), Times.Exactly(int), Times.AtLeast(int), Times.AtMost(int) and several others. So, we could check to see if the method was called 4 times by using "Times.Exactly(4)".

With this information, you should be able to create all of the unit tests in the Advanced Tests section.

To see the completed unit tests, check the solution in the "Completed" folder. The "DataParserLoggerTests" class includes the Advanced Tests.

***
*End of Lab 01 - Refactoring to Constructor Injection for Unit Testing*
***
