using PeopleViewer.Common;
using PeopleViewer.Logging;
using PersonDataReader.CSV;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using PersonDataReader.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(5000));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IPersonReader>(s => GetPersonReader());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


static IPersonReader GetPersonReader()
{
    // Data Reader
    var readerUri = new ServiceReaderUri("http://localhost:9874");
    //var reader = new ServiceReader(readerUri);
    var reader = new CSVReader(AppDomain.CurrentDomain.BaseDirectory + "People.txt");
    //var reader = new SQLReaderProxy(AppDomain.CurrentDomain.BaseDirectory + "People.db");

    // Retry Function
    var delay = new TimeSpan(0, 0, 3);
    var retryReader = new RetryReader(reader, delay);

    // Exception Logging Function
    var logFilePath = AppDomain.CurrentDomain.BaseDirectory + "ExceptionLog.txt";
    var logger = new FileLogger(logFilePath);
    var loggingReader = new ExceptionLoggingReader(retryReader, logger);

    // Caching Function
    var cachingReader = new CachingReader(loggingReader);

    return cachingReader;
}

static IPersonReader AlternateGetPersonReader()
{
    return new CachingReader(
        new ExceptionLoggingReader(
            new RetryReader(
                new ServiceReader(
                    new ServiceReaderUri("http://localhost:9874")),
                new TimeSpan(0, 0, 3)
                ),
            new FileLogger(
                AppDomain.CurrentDomain.BaseDirectory + "ExceptionLog.txt")
        )
    );
}

