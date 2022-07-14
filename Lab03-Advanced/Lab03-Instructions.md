Lab 03 - Decorators, Parameters, and Ninject
=============================================

Objectives
-----------
This lab incorporates a number of dependency injection concepts. This includes changing a service provider, creating a decorator to reduce the number of service calls, using the Ninject container, factory methods, and custom parameter types.

The code is from a working home automation project, so the application code is a bit more involved.

Application Overview
---------------------

The "Starter" folder contains the code files for this lab. Open the "HouseControl" solution (Visual Studio 2022) or open the "Starter" folder (Visual Studio Code).

This project is a home automation application that uses a (local) web service to get the time of sunset for a particular date and location.

The HouseControl solution consists of a number of projects that control the scheduling of home automation events (such as turning lights on 30 minutes before sunset). Here are the relevant classes:

**HouseController** (in the HouseControl.Library project)

This is the controller class that reads events on the schedule and calls into the hardware system to execute those events. 

The constructor requires a Schedule:

```c#
    public HouseController(Schedule schedule)
    {
        this.schedule = schedule;
        ...
    }
```

**Schedule** (in the HouseControl.Library project, Schedules folder)

This is the list of events (ScheduleItems). It uses separate classes to load/save events to the file system and has a method to roll the schedule forward -- meaning, taking past events and rolling them forward to the next applicable day. As an example, if an event is scheduled for every day and the event time is today at 6:00 a.m. (i.e. in the past), it will roll the event forward to tomorrow at 6:00 a.m. (the next applicable time).

The details of rolling forward are not vital to understand, but it does require the sunrise and sunset times for particular dates in order to function properly.

The constructor requires a file name and a sunset provider. The sunset provider is used to create a ScheduleHelper:

```c#
    public Schedule(string filename, ISunsetProvider sunsetProvider)
    {
        this.scheduleHelper = new ScheduleHelper(sunsetProvider);
        this.filename = filename;
        LoadSchedule();
    }
```

**ScheduleHelper** (in the HouseControl.Library project, Schedules folder)

This class has a number of methods to help with the schedule, including determining if a time is in the past and rolling an event forward to the next applicable date.

The constructor requires a sunset provider:

```c#
    public ScheduleHelper(ISunsetProvider sunsetProvider)
    {
        this.SunsetProvider = sunsetProvider;
    }
```

**ISunsetProvider** (in the HouseControl.Sunset project)

This interface contains the following methods:

```c#
    public interface ISunsetProvider
    {
        DateTimeOffset GetSunset(DateTime date);
        DateTimeOffset GetSunrise(DateTime date);
    }
```

These methods use "DateTimeOffset" instead of "DateTime". The difference is that "DateTimeOffset" also includes timezone information (specifically the number of hours from UTC).

There are 2 implementations for this interface:

**SolarServiceSunsetProvider** uses a (local) web service to retrieve sunrise & sunset times.

**SolarTimesSunsetProvider** uses a NuGet package (SolarCalculator by Daniel M. Porrey) to calculate sunrise & sunset times locally -- no web service is required.

These are the objects that make up the system. These are used in the main (HouseControlAgent) project.

**HouseControlAgent Project**

This project is the production application, which also has some test code it for our purposes. This lab will involve making modifications to this project.

The "Program.cs" file contains an "InitializeHouseController" method to put all of the pieces together. This is the **composition root** for the application:

```c#
    private static HouseController InitializeHouseController()
    {
        var sunsetProvider = new SolarServiceSunsetProvider();

        var sunset = sunsetProvider.GetSunset(DateTime.Today.AddDays(1));
        Console.WriteLine($"Sunset Tomorrow: {sunset:G}");

        var schedule = new Schedule(
            AppDomain.CurrentDomain.BaseDirectory + "ScheduleData",
            sunsetProvider);

        var controller = new HouseController(schedule);
        controller.Commander = new FakeCommander();

        return controller;
    }
```

This creates a sunset provider (SolarServiceSunsetProvider) that uses a web service.

Then it outputs the sunset time for tomorrow.

Then it creates a schedule by passing in a file name and the sunset provider.

Then it creates a controller by passing in the schedule.

The "Commander" property is set to a new "FakeCommander" instance. The default commander object opens a serial port to communicate with the home automation hardware. This throws an exception if the hardware is not present. Using property injection, we can swap this out for a fake object. This lets us test the schedule operation and event messaging when the hardware is not available.

Running the Application
------------------------
The service will need to be started before this application will run. The service is in the **SolarCalculator.Service** project. 

**Visual Studio Code & Visual Studio 2022:** To start the service, navigate to the location on the command line.

```
    [WorkshopLocation]\Lab03\Starter\SolarCalculator.Service
```

Type ```dotnet run``` to start the service.

With the service running, start the HouseControlAgent application.

**Visual Studio Code:** Open a command prompt and navigate to the "HouseControlAgent" project folder.  

```
    [WorkshopLocation]\Lab03\Starter\HouseControlAgent
```

Type ```dotnet run``` to start the console application.

**Visual Studio 2022:** Make sure that the **HouseControlAgent** project is set at the startup project in Visual Studio. Press "F5" to run the application.

 You should get output similar to the following.

```
Starting Test
Sunset Tomorrow: 8/17/2022 8:20:14 PM
1101010110101010011000000100000010101101
8/16/2022 5:04:00 PM - Device: 5, Command: On
1101010110101010011000000110000010101101
8/16/2022 5:04:00 PM - Device: 5, Command: Off
Test Completed
```

***Note:** The application reports the "local" time based on the system. The location used in the test application is in the Eastern timezone. If your computer is set to a different timezone, the results will be shifted to the "local" time for your machine.*

If you leave the application running, it will process schedules every minute and show output. Pressing "s" and hitting Enter will show a current schedule. Pressing "q" and Enter will close the application.

```
1101010110101010011000000000000010101101
8/16/2022 5:05:00 PM - Device: 1, Command: On
Schedule Items Processed: 1 - Total Items: 18 - Active Items: 9
s
8/17/2022 12:00:00 AM - Standard (00:00:00), Device: 4, Command: Off
8/17/2022 7:50:14 PM - Sunset (-00:30:00), Device: 2, Command: On
8/17/2022 9:20:14 PM - Sunset (01:00:00), Device: 4, Command: On
8/17/2022 9:40:00 PM - Standard (00:00:00), Device: 2, Command: Off
8/17/2022 10:15:00 PM - Standard (00:00:00), Device: 4, Command: On
8/16/2022 5:05:00 PM - Standard (00:00:00), Device: 1, Command: On
8/16/2022 5:06:00 PM - Standard (00:00:00), Device: 2, Command: On
8/16/2022 5:07:00 PM - Standard (00:00:00), Device: 1, Command: Off
8/16/2022 5:08:00 PM - Standard (00:00:00), Device: 2, Command: Off
1101010110101010011000000001000010101101
8/16/2022 5:06:00 PM - Device: 2, Command: On
Schedule Items Processed: 1 - Total Items: 17 - Active Items: 8
q
```

Lab Goals
----------

The project is already using dependency injection in several places. This lab will take things a bit further. The goals are split up into 5 main topics.

* Goal #1 - Change the data provider
* Goal #2 - Add a data cache
* Goal #3 - Add the Ninject DI container
* Goal #4 - Add location parameters to the sunset provider
* Goal #5 - Add parameter objects to make configuration easier

Goals #1 and #2 mirror code from previous examples and labs, so they should go pretty quickly. The remaining goals have some more involved code changes.

Goal #1 Hints
--------------
The first goal is to change from the service-based data provider to the local calculation data provider.

* The manual composition in the "Program.cs" file of the test application needs to be updated.

* The result can be tested by running the test application *without* starting the service first.

Step-by-step instructions are included below (after all of the Goal Hints).

Goal #2 Hints
--------------
The second goal is to add caching functionality to the data provider. This will greatly reduce the number of service calls or complex calculations (depending on the provider).

* Caching can be added by using the Decorator pattern with the "ISunsetProvider" interface.

* Data for sunrise/sunset does not change per date.

* The following fields can be helpful for implementing a cache:

```c#
        private DateTime dataDate;
        private DateTimeOffset sunrise;
        private DateTimeOffset sunset;
```

* To use the cache, update the composition root of the application.

Step-by-step instructions are included below (after all of the Goal Hints).

Goal #3 Hints
--------------
The third goal is to add the Ninject dependency injection container to the test application.

* "Ninject" is available as a NuGet package.

* The container only needs to be used in the "InitializeController" method in the test application.

* To explicitly pass parameters to a constructor, look at the "ToMethod" method in Ninject.

```c#
    container.Bind<MyType>().ToMethod(
        c => new MyType(param1, param2));
```

* The "HouseController" object uses property injection for the test application. Here is one way to configure Ninject to use the FakeCommander object:

```c#
    Container.Bind<HouseController>().ToSelf()
        .WithPropertyValue("Commander", new FakeCommander());
```

Step-by-step instructions are included below (after all of the Goal Hints).

Goal #4 Hints
--------------
The fourth goal is to add the location information to the sunset provider. Currently the location (latitude and longitude) are hard-coded inside the provider. Adding parameters will make this more flexible.

* Latitude and longitude should be represented as "double" values.

* The location of Louisville, KY is latitude 38.1884 / longitude -85.9569. You can get your location by going to maps.google.com, selecting a location, and looking at the address bar. For example, if you pick The Gateway Arch in St. Louis, the URL will be similar to this:  ```
https://www.google.com/maps/place/The+Gateway+Arch/@38.6249983,-90.1874555,17z```   
"@38.6249983,-90.1874555" is the location.  

* Initially, specific constructor arguments will be used. This will be changed as part of Goal #5 below. The ".WithConstructorArgument()" is one way to set the parameter values.

```c#
    container.Bind<IMyInterface>()
        .To<MyType>()
        .WithConstructorArgument("paramName", 123);
```

Step-by-step instructions are included below (after all of the Goal Hints).

Goal #5 Hints
--------------
The last goal is to change the injected constructor parameters from primitive types (string & double) to custom types. This will enable easier configuration with the dependency injection container.

* The latitude and longitude parameters (from Goal #4) can be described as a single "Location" type.

* The "Schedule" class has a string parameter for the file name. Changing this to a custom type will allow for easier configuration.

* When complete, there should be no factory methods used with Ninject.

Step-by-step instructions are included below. 

If you need more assistance, keep reading. Otherwise, **STOP READING NOW**

Goal #1 - Step-by-Step: Changing the Data Provider
---------------------------------------------------
The test application currently uses the data provider created in the previous lab. This uses a web service to get sunrise/sunset data. We will change this to use a data provider that calculates the times locally.

*Note: This is based on a real situation where a 3rd-party service started throwing 403 errors, and I had to find another source for data. Read more here: [Loose Coupling FTW!](https://jeremybytes.blogspot.com/2015/05/loose-coupling-ftw.html)*

1. Start the web service.

One route to start the service: right-click on the "SolarCalculator.Service" project and select "Open in File Explorer". In the file explorer window, right-click on an empty spot, and choose "Open in Terminal". Then in the terminal window, type "dotnet run".

2. Run the "HouseControlAgent" application from Visual Studio 2022 or Visual Studio Code.

The output should be similar to the following:

```
Starting Test
Sunset Tomorrow: 8/17/2022 8:20:14 PM
1101010110101010011000000100000010101101
8/16/2022 5:04:00 PM - Device: 5, Command: On
1101010110101010011000000110000010101101
8/16/2022 5:04:00 PM - Device: 5, Command: Off
Test Completed
```

Note the value for "Sunset Tomorrow" on the second line.

3. Press "q" + Enter (or close the window) to stop running.

4. Open the "Program.cs" file in the "HouseControlAgent" project.

5. In the "InitializeHouseController" method, change from using "SolarServiceSunsetProvider" to "SolarTimesSunsetProvider".

```c#
    private static HouseController InitializeHouseController()
    {
        var sunsetProvider = new SolarTimesSunsetProvider();
        ...
    }
```

6. In the terminal window with the service, stop the web service by pressing "Ctrl+C".

7. Run the "HouseControlAgent" application.

*Note that the application runs even when the service is not running.*

The output should be similar to the following:

```
Starting Test
Sunset Tomorrow: 8/17/2022 7:36:03 PM
1101010110101010011000000100000010101101
8/16/2022 5:12:51 PM - Device: 5, Command: On
1101010110101010011000000110000010101101
8/16/2022 5:12:51 PM - Device: 5, Command: Off
Test Completed
```

Note the value for "Sunset Tomorrow". This is significantly different from the previous value.

The difference is because the local data provider uses a different location: Louisville, KY. The service data provider uses Atlanta, GA.

The location will be parameterized later on in this lab.

Goal #2 - Step-by-Step: Data Caching
-------------------------------------
When the application runs, it uses the sunrise & sunset times for the current date repeatedly throughout the day. Since this data doesn't change, it makes sense to cache the data rather than going to a service or re-calculating it each time.

By using the Decorator pattern, we can create a cache that can be used by either of the data providers.

This will use the "ISunsetProvider" interface from the "HouseControl.Sunset" project.

```c#
    public interface ISunsetProvider
    {
        DateTimeOffset GetSunset(DateTime date);
        DateTimeOffset GetSunrise(DateTime date);
    }
```

1. Create a new class in the "HouseControl.Sunset" project named "CachingSunsetProvider".

```c#
    internal class CachingSunsetProvider
    {
    }
```

2. Make the class "public" and declare that it implements the "ISunsetProvider" interface.

```c#
    public class CachingSunsetProvider : ISunsetProvider
    {
    }
```

3. Implement the interface.

Click on "ISunsetProvider", the press "Ctrl+." to bring up the actions menu, then choose "Implement interface".

```c#
    public class CachingSunsetProvider : ISunsetProvider
    {
        public DateTimeOffset GetSunrise(DateTime date)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetSunset(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
```

4. Create a field for a "wrapped" data provider, and inject it through the constructor.

```c#
    public class CachingSunsetProvider : ISunsetProvider
    {
        ISunsetProvider wrappedProvider;

        public CachingSunsetProvider(ISunsetProvider wrappedSunsetProvider)
        {
            wrappedProvider = wrappedSunsetProvider;
        }
        ...
    }
```

This is the real data provider that supplies the sunrise & sunset values.

5. Add fields to hold the cached data.

```c#
    private DateTime dataDate;
    private DateTimeOffset sunrise;
    private DateTimeOffset sunset;
```

6. Write a "ValidateCache" method. This will check to see if the cache is still valid. If it is *not* valid, it will refresh the data using the wrapped data provider.

```c#
    private void ValidateCache(DateTime date)
    {
        if (dataDate != date)
        {
            sunrise = wrappedProvider.GetSunrise(date);
            sunset = wrappedProvider.GetSunset(date);
            dataDate = date;
        }
    }
```

This creates a fairly naive implementation of a client-side cache. If the date is the same as what is in the cache, then the cached data is used. If the date is not the same, then the data is updated (along with the data date).

7. Implement the interface methods.

These methods call the "ValidateData" method and then return the local data.

```c#
        public DateTimeOffset GetSunrise(DateTime date)
        {
            ValidateCache(date);
            return sunrise;
        }

        public DateTimeOffset GetSunset(DateTime date)
        {
            ValidateCache(date);
            return sunset;
        }
```

8. Use the new cache by changing the composition root of the "HouseControlAgent" project.

The composition root is the "InitializeHouseController" method.

```c#
    private static HouseController InitializeHouseController()
    {
        var wrappedProvider = new SolarTimesSunsetProvider();
        var sunsetProvider = new CachingSunsetProvider(wrappedProvider);
        ...
    }
```

9. Build and run the application.

The output will be the same as before, but the values for sunrise and sunset will only be calculate one time (each).

*You can experiment with this by putting "Console.WriteLine"s into the local data provider to show each time the "GetSunset" method is called. Alternately, you can switch back to using the service data provider, start the service, run the application, stop the service, and wait. The application will continue to run without errors.*

Goal #3 - Step-by-Step: Adding Ninject
---------------------------------------
The application uses manual object composition (which is perfectly fine for small numbers of objects like we have here). In larger applications, dependency injection containers make a lot of sense.

In this section, we will add Ninject to the test application and explore some configuration options.

1. Add the "Ninject" package to the "HouseControlAgent" project.

**Visual Studio Code:** Open a command prompt and navigate to the HouseControlAgent project folder. Type the following to add the "Ninject" NugetPackage:

```
dotnet add package Ninject
```

**Visual Studio 2022:** In the "HouseControlAgent" project, right-click on the Dependencies and select "Manage NuGet Packages". Then use the search box to locate "Ninject" and install it.

2. In the "Program.cs" file, add a "using" statement for Ninject;

```c#
    using Ninject;
```

3. Create a Ninject container in the "InitializeController" method.

*As noted, we will only reference the Ninject container inside the composition root, which is inside this method.*

```c#
    private static HouseController InitializeHouseController()
    {
        IKernel container = new StandardKernel();
        ...
    }
```

4. Configure the container by binding the "ISunsetProvider" interface to the "SolarTimesSunsetProvider" class.

```c#
    IKernel container = new StandardKernel();

    container.Bind<ISunsetProvider>()
        .To<SolarTimesSunsetProvider>()
        .InSingletonScope();
```

*Note: we'll add the cache back in later on.*

5. Remove the manual creation of the data provider(s).

```c#
    //var wrappedProvider = new SolarTimesSunsetProvider();
    //var sunsetProvider = new CachingSunsetProvider(wrappedProvider);
```

6. Update the "Sunset Tomorrow" output to use the container.

We've been skipping over the following lines that output "Sunset Tomorrow" to the console.

```c#
    var sunset = sunsetProvider.GetSunset(DateTime.Today.AddDays(1));
    Console.WriteLine($"Sunset Tomorrow: {sunset:G}");
```

These lines will stop working because the "sunsetProvider" local variable is no longer valid.

Update these lines to get the sunset data from the container object.

```c#
    var sunset = container.Get<ISunsetProvider>()
        .GetSunset(DateTime.Today.AddDays(1));
    Console.WriteLine($"Sunset Tomorrow: {sunset:G}");
```

7. Configure the container for the "Schedule" object.

To start with, we'll use a factory method to configure the Schedule object.

```c#
    string schedulePath = AppDomain.CurrentDomain.BaseDirectory + "ScheduleData";
    container.Bind<Schedule>().ToMethod(
        c => new Schedule(schedulePath, 
            container.Get<ISunsetProvider>()));
```

This code has a local variable for the schedule file path. Then it binds the "Schedule" type to the container using a factory method.

The factory method "new"s up the Schedule and passes in the path and the sunset provider. We ask the container for the sunset provider to make sure we are always using the correct one (and also let the container manage instances and lifetimes).

*Note: We use "new" here, which we generally try to avoid when we're using containers to let the container do the work. We will update this configuration in a later section and find that explicit config for "Schedule" can go away completely.*

8a. Get the "HouseController" object from the Ninject container.

```c#
    var controller = container.Get<HouseController>();
    controller.Commander = new FakeCommander();

    return controller;
```

Here we ask the container for an instance of the "HouseController" class. Then we use property injection to replace the default "Commander" with the FakeCommander.

8b. Alternately, we can configure the container to use property injection for the "HouseController".

```c#
    container.Bind<HouseController>().ToSelf()
        .WithPropertyValue("Commander", new FakeCommander());

    var controller = container.Get<HouseController>();

    return controller;
```

The "Bind<...>().ToSelf()" lets us put a binding into the Ninject container so that we can add things like constructor parameters, properties, and other items without having an abstraction (like an interface).

In this case, it lets us add "WithPropertyValue" so that we can inject the fake object for the property.

9. Build and run the application.

At this point, the application should run as before. The code is a bit more complex than before, but we have seen container techniques that are useful in larger applications.

We will continue by adding some parameters to the data providers. Then we'll loop back to make the container easier to configure.

Goal #4 - Step-by-Step: Location Parameters
--------------------------------------------
The current application uses a hard-coded location. This works fine for a single installation (since home automation software runs at a single location). We'll make it easier to change the location by adding parameters to the sunset data providers.

*Note: we will only parameterize the local sunset provider, but the same steps can be used for the service data provider.*

1. Open the "SolarTimesSunsetProvider" in the "HouseControl.Sunset" project.

```c#
    public class SolarTimesSunsetProvider : ISunsetProvider
    {
        public DateTimeOffset GetSunset(DateTime date)
        {
            var solarTimes = new SolarTimes(date, 33.8361, -117.8897);
            return new DateTimeOffset(solarTimes.Sunset);
        }

        public DateTimeOffset GetSunrise(DateTime date)
        {
            var solarTimes = new SolarTimes(date, 33.8361, -117.8897);
            return new DateTimeOffset(solarTimes.Sunrise);
        }
    }
```

Note that the location (latitude / longitude) is hard-coded inside each of the methods.

2. Add local fields for the latitude and longitude.

```c#
    private readonly double latitude;
    private readonly double longitude;
```

3. Inject the values through a constructor.

```c#
    public SolarTimesSunsetProvider(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }
```

4. Update the "GetSunset" and "GetSunrise" methods to use those values.

```c#
    public DateTimeOffset GetSunset(DateTime date)
    {
        var solarTimes = new SolarTimes(date, latitude, longitude);
        return new DateTimeOffset(solarTimes.Sunset);
    }

    public DateTimeOffset GetSunrise(DateTime date)
    {
        var solarTimes = new SolarTimes(date, latitude, longitude);
        return new DateTimeOffset(solarTimes.Sunrise);
    }
```

5. Build and run the "HouseControlAgent" application.

At this point, the application builds successfully, but generates an error at runtime.

```
Ninject.ActivationException: 'Error activating double
No matching bindings are available, and the type is not self-bindable.
Activation path:
  2) Injection of dependency double into parameter latitude of constructor of type SolarTimesSunsetProvider
  1) Request for ISunsetProvider

Suggestions:
  1) Ensure that you have defined a binding for double.
  2) If the binding was defined in a module, ensure that the module has been loaded into the kernel.
  3) Ensure you have not accidentally created more than one kernel.
  4) If you are using constructor arguments, ensure that the parameter name matches the constructors parameter name.
  5) If you are using automatic module loading, ensure the search path and filters are correct.
```

Ninject is telling us that it does not know how to create the "SolarTimesSunsetProvider" because it does not know what to do with the constructor parameters.

6. Add explicit constructor parameters to the sunset data provider configuration.

In the composition root of the application (the "InitializeHouseController" method), update the binding with the following code:

```c#
    container.Bind<ISunsetProvider>()
        .To<SolarTimesSunsetProvider>()
        .InSingletonScope()
        .WithConstructorArgument("latitude", 38.1884)
        .WithConstructorArgument("longitude", -85.9569);
```

This adds explicit values for the "latitude" and "longitude" parameters. Note that the location is set to Louisville, KY. Feel free to set this to your own location using the method described in the "Hints" section above.

7. Build and run the application.

The application now builds and runs successfully, and we have the "Sunset Tomorrow" value for the correct location.

```
Starting Test
Sunset Tomorrow: 8/17/2022 8:20:14 PM
1101010110101010011000000100000010101101
8/16/2022 5:34:41 PM - Device: 5, Command: On
1101010110101010011000000110000010101101
8/16/2022 5:34:41 PM - Device: 5, Command: Off
Test Completed
```

This code works, but the configuration seems a bit "heavy" for the data provider. With the last section of the lab, we will make things easier for the container to automatically configure the parameters.

Goal #5 - Step-by-Step: Custom Parameter Types
-----------------------------------------------
The current application is working with the Ninject container, but the configuration seems a bit specific, particularly for the "SolarTimesSunsetProvider" and the "Schedule" objects.

```c#
    container.Bind<ISunsetProvider>()
        .To<SolarTimesSunsetProvider>()
        .InSingletonScope()
        .WithConstructorArgument("latitude", 38.1884)
        .WithConstructorArgument("longitude", -85.9569);
    ...
    string schedulePath = AppDomain.CurrentDomain.BaseDirectory + "ScheduleData";
    container.Bind<Schedule>().ToMethod(
        c => new Schedule(schedulePath, 
            container.Get<ISunsetProvider>()));
```

**Let the Container do the Work**

The general idea is that we want the container to do the work for us. Instead of providing specifics on how to construct an object, we want the container to figure it out for us.

The SolarTimesSunsetProvider requires 2 parameters.

```c#
    public SolarTimesSunsetProvider(
        double latitude, double longitude)
```

The HouseController constructor also has a parameter.

```c#
    public HouseController(Schedule schedule)
```

The container has a binding for "Schedule" so it automatically fills it in. We do not need to do any explicit configuration for that parameter.

This means that if we can figure out how to put the latitude and longitude values into the Ninject container, we will not need to explicitly configure them; Ninject will figure it out.

**The Problem**

The problem is that the latitude and longitude values are both "double". So a standard configuration will not work.

```c#
    container.Bind<double>().ToConstant(38.1884);
    container.Bind<double>().ToConstant(-85.9569);
```

This puts 2 values into the container, but there is no way to differentiate them. We could "name" them in the container, but that would lead to a similar explicitness in the configuration.

Instead, we will create a custom type that can be easily configured in the DI container.

**Updating the Code**
Instead of using 2 double values, we will use a single custom type to hold the location.

1. In the "HouseControl.Sunset" project, create a new class / type called "LatLongLocation".

```c#
namespace HouseControl.Sunset;
internal class LatLongLocation
{

}
```

2. Make the class "public", add 2 read-only properties (for latitude and longitude), and initialize them in the constructor.

```c#
    public class LatLongLocation
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public LatLongLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
```

3. Update the constructor for "SolarTimesSunsetProvider" to use the new "LatLongLocation" type instead of separate values for latitude and longitude.

```c#
    public class SolarTimesSunsetProvider : ISunsetProvider
    {
        private readonly double latitude;
        private readonly double longitude;

        public SolarTimesSunsetProvider(LatLongLocation latLong)
        {
            this.latitude = latLong.Latitude;
            this.longitude = latLong.Longitude;
        }
        ...
    }
```

*Note: Internally, we can still keep the separate "double" values. We only need the new type for the constructor parameter.*

4. Configure the "LatLongLocation" in the Ninject container.

In the composition root (the "InitializeHouseController" method of the "HouseControlAgent" project), add a binding for the new location object.

```c#
    container.Bind<LatLongLocation>()
        .ToConstant(new LatLongLocation(38.1884, -85.9569));
```

5. Remove the "WithConstructorArgument" calls in the sunset data provider configuration.

```c#
    container.Bind<ISunsetProvider>()
        .To<SolarTimesSunsetProvider>()
        .InSingletonScope();
```

Now the binding for the "SolarTimesProvider" looks the same as before we added the parameters.

6. Build and run the application.

The application will run with the same output as before. Ninject has all of the information it needs to create the objects.

We will do something similar for the "Schedule" object. But first, let's put caching back in.

7. Configure the container to use the caching decorator.

Now that we have a simplified configuration for the "SolarTimesProvider", we'll add caching functionality.

Here's the code:

```c#
    container.Bind<ISunsetProvider>()
        .To<CachingSunsetProvider>()
        .InSingletonScope()
        .WithConstructorArgument<ISunsetProvider>(
            container.Get<SolarTimesSunsetProvider>());
```

In this case, we bind the interface to the caching provider. The problem is that the caching provider needs an "ISunsetProvider" as a parameter. To avoid a circular reference, we can add a constructor argument, then ask for the local data provider ("SolarTimesSunsetProvider").

Note that we ask the container for the local data provider. This way the container can include the location values that the constructor needs. Let the container do the work.

8. Review the constructor for "Schedule" (in the "HouseControl.Library" project, "Schedules" folder).

```c#
    public Schedule(string filename, 
        ISunsetProvider sunsetProvider)
    {
        this.scheduleHelper = new ScheduleHelper(sunsetProvider);
        this.filename = filename;
        LoadSchedule();
    }
```

The constructor needs a file name and an ISunsetProvider. The filename is a string. Configuring a "string" in a dependency injection container could cause the same types of problems we saw when trying to configure a "double", particularly if there are other objects that also take "string" parameters.

To resolve this, we will create a custom type for the string parameter.

9. Create a custom type to hold the file name.

Since this type will not be shared, we can put it into the same file as the "Schedule" class ("Schedule.cs").

Add the following to the top of the "Schedule.cs" file (inside the namespace, but before the class declaration):

```c#
    public class ScheduleFileName
    {
        public string FileName { get; init; }
        public ScheduleFileName(string fileName)
        {
            FileName = fileName;
        }
    }
```

This is similar to the "LatLongLocation" type created above.

10. Update the "Schedule" class to use the new type.

```c#
    public Schedule(ScheduleFileName filename, 
        ISunsetProvider sunsetProvider)
    {
        this.scheduleHelper = new ScheduleHelper(sunsetProvider);
        this.filename = filename.FileName;
        LoadSchedule();
    }
```

11. Build the application.

There will be multiple build errors: the unit tests, the production application, and the test application.

The next steps will fix these one at at time.

12. Update the unit tests.

If we try to build at this point, a number of unit tests no longer build in the "ScheduleTests.cs" file.

At the top of the class, update the "fileName" field.

```c#
    //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\ScheduleData";
    ScheduleFileName fileName = new(AppDomain.CurrentDomain.BaseDirectory + "\\ScheduleData");
```

Since all of the tests use this value, they will now build and run without error.

13. Fix the application ("HouseControlAgent").

No we'll head back to the "HouseControlAgent" project (where we have the Ninject container), and fix that.

```c#
    // broken
    string schedulePath = AppDomain.CurrentDomain.BaseDirectory + "ScheduleData";
    container.Bind<Schedule>().ToMethod(
        c => new Schedule(schedulePath, 
            container.Get<ISunsetProvider>()));
```

To fix this, change the "schedulePath" variable from a "string" to the new "ScheduleFileName".

```c#
    // fixed
    ScheduleFileName schedulePath = 
        new(AppDomain.CurrentDomain.BaseDirectory + "ScheduleData");

    container.Bind<Schedule>().ToMethod(
        c => new Schedule(schedulePath, 
            container.Get<ISunsetProvider>()));
```

14. Build and run the application.

The application now builds and runs as before. But the configuration for the "Schedule" is just as complex.

15. Add the "ScheduleFileName" to the container.

Just like with the location, we will add the schedule file name to the container.

```c#
    container.Bind<ScheduleFileName>().ToConstant(
        new ScheduleFileName(
            AppDomain.CurrentDomain.BaseDirectory + "ScheduleData"));
```

16. Remove the "Schedule" configuration.

Comment out the separate "ScheduleFileName" variable and the binding for the "Schedule" object.

```c#
    //ScheduleFileName schedulePath = new ScheduleFileName(
    //    AppDomain.CurrentDomain.BaseDirectory + "ScheduleData");

    //container.Bind<Schedule>().ToMethod(
    //    c => new Schedule(schedulePath, 
    //        container.Get<ISunsetProvider>()));
```

We no longer need this binding because the Ninject container can figure out the parameters. The first parameter is a "ScheduleFileName", which has the binding we just created. The second parameter is an "ISunsetProvider", and we have a binding for that as well.

Because the container can figure out the parameters, we no longer need the explicit binding for the schedule.

*Note: Feel free to delete the commented code*

17. Build and run the application.

The application runs as before.

**Review**

Here is the completed "InitializeHouseController" method.

```c#
private static HouseController InitializeHouseController()
{
    IKernel container = new StandardKernel();

    container.Bind<LatLongLocation>()
        .ToConstant(new LatLongLocation(38.1884, -85.9569));

    container.Bind<ISunsetProvider>()
        .To<CachingSunsetProvider>()
        .InSingletonScope()
        .WithConstructorArgument<ISunsetProvider>(
            container.Get<SolarTimesSunsetProvider>());

    var sunset = container.Get<ISunsetProvider>()
        .GetSunset(DateTime.Today.AddDays(1));
    Console.WriteLine($"Sunset Tomorrow: {sunset:G}");

    container.Bind<ScheduleFileName>().ToConstant(
        new ScheduleFileName(
            AppDomain.CurrentDomain.BaseDirectory + "ScheduleData"));

    container.Bind<HouseController>().ToSelf()
        .WithPropertyValue("Commander", new FakeCommander());

    var controller = container.Get<HouseController>();

    return controller;
}
```

This method shows several techniques that are useful when we use dependency injection in our own applications.

* The dependency injection container is only used in the composition root (the "InitializeHouseController" method).

* Primitive parameter types (such as "double" and "string") can be replaced with custom types that are easy to put into a container.

* The Decorator pattern can be used to wrap objects and add functionality. DI containers provide various ways to configure these.

* Factory methods can be used for explicit configuration with a container. Containers also allow us to provide parameters (such as ".WithConstructorArguments()").

* Property injection can be used when we need to override a default value.

These techniques are useful in various scenarios and environments.

When we have a small set of objects, manual binding can be easier, but when applications start to grow, a dependency injection container can be a great benefit.

***
*Lab 03 - Decorators, Parameters, and Ninject*
***