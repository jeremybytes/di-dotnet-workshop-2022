namespace PeopleLibrary;

public class GivenNamePersonFormatter : IPersonFormatter
{
    public string DisplayName => "Given Name";

    public string Format(Person person)
    {
        return person.GivenName.ToLower();
    }
}
