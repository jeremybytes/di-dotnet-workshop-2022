namespace PeopleLibrary;

public class FamilyNamePersonFormatter : IPersonFormatter
{
    public string DisplayName => "Family Name";

    public string Format(Person person)
    {
        return person.FamilyName.ToUpper();
    }
}
