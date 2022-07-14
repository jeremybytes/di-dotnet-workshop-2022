using PeopleLibrary;
using System.Windows;
using System.Windows.Controls;

namespace PeopleViewer;

public partial class MainWindow : Window
{
    public MainWindow(IEnumerable<IPersonFormatter> formatters)
    {
        ArgumentNullException.ThrowIfNull(formatters);
        InitializeComponent();
        PopulateFormatters(formatters);
        PersonListBox.ItemsSource = People.GetPeople();
    }

    private void PopulateFormatters(IEnumerable<IPersonFormatter> formatters)
    {
        bool isDefault = true;
        foreach(var formatter in formatters)
        {
            var button = new RadioButton()
            {
                Content = formatter.DisplayName,
                VerticalContentAlignment = VerticalAlignment.Center,
                IsChecked = isDefault,
                Tag = formatter,
            };
            FormattersPanel.Children.Add(button);
            isDefault = false;
        }
    }

    private void ProcessDataButton_Click(object sender, RoutedEventArgs e)
    {
        OutputList.Items.Clear();
        var people = People.GetPeople();

        IPersonFormatter? selectedFormatter = SelectFormatter();

        foreach(var person in people)
        {
            AddToList(person.ToString(selectedFormatter));
        }
    }

    private IPersonFormatter? SelectFormatter()
    {
        foreach (RadioButton item in FormattersPanel.Children)
        {
            if (item.IsChecked!.Value)
                return item.Tag as IPersonFormatter;
        }

        return null;
    }

    private void AddToList(string item)
    {
        OutputList.Items.Add(item);
    }
}
