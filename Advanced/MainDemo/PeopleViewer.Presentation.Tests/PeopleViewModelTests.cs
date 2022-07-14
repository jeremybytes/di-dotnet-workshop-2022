using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleViewer.Common;

namespace PeopleViewer.Presentation.Tests;

[TestClass]
public class PeopleViewModelTests
{
    private IPersonReader GetTestReader() =>  new FakeReader();

    [TestMethod]
    public async Task People_OnRefreshPeople_IsPopulated()
    {
        // Arrange
        var reader = GetTestReader();
        var viewModel = new PeopleViewModel(reader);

        // Act
        await viewModel.RefreshPeople();

        // Assert
        Assert.IsNotNull(viewModel.People);
        Assert.AreEqual(2, viewModel.People.Count());
    }

    [TestMethod]
    public async Task People_OnClearPeople_IsEmpty()
    {
        // Arrange
        var reader = GetTestReader();
        var viewModel = new PeopleViewModel(reader);
        await viewModel.RefreshPeople();
        Assert.AreNotEqual(0, viewModel.People.Count(),
            "Invalid arrangement");

        // Act
        viewModel.ClearPeople();

        // Assert
        Assert.AreEqual(0, viewModel.People.Count());
    }
}
