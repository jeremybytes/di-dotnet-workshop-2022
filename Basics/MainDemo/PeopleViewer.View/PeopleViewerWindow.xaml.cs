using PeopleViewer.Presentation;
using System.Windows;

namespace PeopleViewer;

public partial class PeopleViewerWindow : Window
{
    PeopleViewModel viewModel { get; }

    public PeopleViewerWindow(PeopleViewModel peopleViewModel)
    {
        InitializeComponent();
        ArgumentNullException.ThrowIfNull(peopleViewModel);
        viewModel = peopleViewModel;
        this.DataContext = viewModel;
    }

    private void FetchButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.RefreshPeople();
        ShowRepositoryType();
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.ClearPeople();
        ClearRepositoryType();
    }

    private void ShowRepositoryType()
    {
        RepositoryTypeTextBlock.Text = viewModel.DataReaderType;
    }

    private void ClearRepositoryType()
    {
        RepositoryTypeTextBlock.Text = string.Empty;
    }
}
