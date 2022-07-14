namespace PeopleLibrary;

public class FullNamePersonFormatter : IPersonFormatter
{
    public string DisplayName => "Full Name";

    public string Format(Person person)
    {
        return $"{person.FamilyName}, {person.GivenName}";
    }
}
