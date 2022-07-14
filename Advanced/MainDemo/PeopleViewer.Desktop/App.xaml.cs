using PeopleViewer.Logging;
using PeopleViewer.Presentation;
using PersonDataReader.CSV;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using PersonDataReader.SQL;
using System.Windows;

namespace PeopleViewer.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ComposeObjects();
        Application.Current.MainWindow.Title = "With Dependency Injection";
        Application.Current.MainWindow.Show();
    }

    private static void ComposeObjects()
    {
        // Data Reader
        var readerUri = new ServiceReaderUri("http://localhost:9874");
        var reader = new ServiceReader(readerUri);
        //var reader = new CSVReader(AppDomain.CurrentDomain.BaseDirectory + "People.txt");
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

        var viewModel = new PeopleViewModel(cachingReader);
        Application.Current.MainWindow = new PeopleViewerWindow(viewModel);
    }

    private static void AlternateComposeObjects()
    {
        Application.Current.MainWindow = new PeopleViewerWindow(
           new PeopleViewModel(
               new CachingReader(
                   new ExceptionLoggingReader(
                       new RetryReader(
                           new ServiceReader(
                               new ServiceReaderUri("http://localhost:9874")),
                           new TimeSpan(0, 0, 3)
                           ),
                       new FileLogger(
                           AppDomain.CurrentDomain.BaseDirectory + "ExceptionLog.txt")
                       )
                   )
               )
            );
    }
}
