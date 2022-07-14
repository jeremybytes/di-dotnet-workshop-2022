namespace PeopleLibrary;

public class DefaultPersonFormatter : IPersonFormatter
{
    public string DisplayName => "Default";

    public string Format(Person person)
    {
        return person.ToString();
    }
}
