using Microsoft.AspNetCore.Mvc;
using PeopleViewer.Common;

namespace PeopleViewer.Controllers;

public class PeopleController : Controller
{
    IPersonReader reader;

    public PeopleController(IPersonReader reader)
    {
        this.reader = reader;
    }

    public async Task<IActionResult> UseInjectedReader()
    {
        ViewData["Title"] = "Using Injected Reader";
        ViewData["ReaderType"] = reader.GetTypeName();

        List<Person> people = (await reader.GetPeople()).ToList();
        return View("Index", people);
    }
}
