namespace PeopleLibrary;

public class CompositePersonFormatter : IPersonFormatter
{
    private List<IPersonFormatter> formatters;

    public string DisplayName => "given FAMILY";

    public CompositePersonFormatter(List<IPersonFormatter> formatters)
    {
        this.formatters = formatters;
    }

    public string Format(Person person)
    {
        string output = string.Empty;
        foreach(var formatter in formatters)
        {
            output += $"{formatter.Format(person)} ";
        }
        return output;
    }
}
