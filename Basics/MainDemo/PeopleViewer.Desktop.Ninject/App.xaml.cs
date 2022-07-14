using Ninject;
using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonDataReader.CSV;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using PersonDataReader.SQL;
using System.Windows;

namespace PeopleViewer.Desktop.Ninject;

public partial class App : Application
{
    IKernel Container = new StandardKernel();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ConfigureContainer();
        ComposeObjects();
        Application.Current.MainWindow.Title = "With Dependency Injection - Ninject";
        Application.Current.MainWindow.Show();
    }

    private void ConfigureContainer()
    {
        Container.Bind<ServiceReaderUri>()
            .ToConstant(new ServiceReaderUri("http://localhost:9874"));

        Container.Bind<IPersonReader>().To<ServiceReader>()
            .InSingletonScope();
    }

    private void ComposeObjects()
    {
        Application.Current.MainWindow = Container.Get<PeopleViewerWindow>();
    }
}
