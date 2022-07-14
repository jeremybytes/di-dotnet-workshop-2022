using PeopleViewer.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PeopleViewer.Presentation;

public class PeopleViewModel : INotifyPropertyChanged
{
    protected readonly IPersonReader DataReader;

    private IEnumerable<Person> _people = new List<Person>();

    public IEnumerable<Person> People
    {
        get => _people;
        set { _people = value; RaisePropertyChanged(); }
    }

    public PeopleViewModel(IPersonReader dataReader)
    {
        // Guard Clause
        ArgumentNullException.ThrowIfNull(dataReader);

        DataReader = dataReader;
    }

    public async Task RefreshPeople()
    {
        People = await DataReader.GetPeople();
    }

    public void ClearPeople()
    {
        People = new List<Person>();
    }

    public string DataReaderType
    {
        get { return DataReader.GetTypeName(); }
    }


    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler? PropertyChanged;
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}