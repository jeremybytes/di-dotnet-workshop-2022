using PeopleLibrary;
using System.Windows;

namespace PeopleViewer;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var formatters = new List<IPersonFormatter>()
        {
            new DefaultPersonFormatter(),
            new FamilyNamePersonFormatter(),
            new GivenNamePersonFormatter(),
            new FullNamePersonFormatter(),
            new CompositePersonFormatter(
                new List<IPersonFormatter>() {new GivenNamePersonFormatter(), new FamilyNamePersonFormatter()}),
        };
        Application.Current.MainWindow = new MainWindow(formatters);
        Application.Current.MainWindow.Show();
    }
}

