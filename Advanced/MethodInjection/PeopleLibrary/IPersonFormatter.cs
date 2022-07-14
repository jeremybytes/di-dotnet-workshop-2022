namespace PeopleLibrary;

public interface IPersonFormatter
{
    string DisplayName { get; }
    string Format(Person person);
}
