namespace DataProcessor.Library.Tests;

public class DataParserTests
{
    [Test]
    public void ParseData_WithMixedData_ReturnsGoodRecords()
    {
        // Arrange
        var logger = new FakeLogger();
        var parser = new DataParser(logger);

        // Act
        int processedRecords = parser.ParseData(TestData.Data);

        // Assert
        Assert.That(processedRecords, Is.EqualTo(7));
    }

    [Test]
    public void ParseData_WithGoodRecord_ReturnsOne()
    {
        // Arrange
        var logger = new FakeLogger();
        var parser = new DataParser(logger);

        // Act
        int processedRecords = parser.ParseData(TestData.GoodRecord);

        // Assert
        Assert.That(processedRecords, Is.EqualTo(1));
    }

    [Test]
    public void ParseData_WithBadRecord_ReturnsZero()
    {
        // Arrange
        var logger = new FakeLogger();
        var parser = new DataParser(logger);

        // Act
        int processedRecords = parser.ParseData(TestData.BadRecord);

        // Assert
        Assert.That(processedRecords, Is.EqualTo(0));
    }

    [Test]
    public void ParseData_WithBadStartDate_ReturnsZero()
    {
        // Arrange
        var logger = new FakeLogger();
        var parser = new DataParser(logger);

        // Act
        int processedRecords = parser.ParseData(TestData.BadStartDate);

        // Assert
        Assert.That(processedRecords, Is.EqualTo(0));
    }

    [Test]
    public void ParseData_WithBadRating_ReturnsZero()
    {
        // Arrange
        var logger = new FakeLogger();
        var parser = new DataParser(logger);

        // Act
        int processedRecords = parser.ParseData(TestData.BadRating);

        // Assert
        Assert.That(processedRecords, Is.EqualTo(0));
    }
}