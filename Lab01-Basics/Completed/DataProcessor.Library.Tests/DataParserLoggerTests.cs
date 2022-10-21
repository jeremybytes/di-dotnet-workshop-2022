using Moq;

namespace DataProcessor.Library.Tests;

public class DataParserLoggerTests
{
    [Test]
    public void ParseData_WithGoodRecord_LoggerIsNotCalled()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        parser.ParseData(TestData.GoodRecord);

        // Assert
        mockLogger.Verify(m =>
            m.Log(It.IsAny<string>(), TestData.GoodRecord[0]), Times.Never());
    }

    [Test]
    public void ParseData_WithBadRecord_LoggerIsCalledOnce()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        parser.ParseData(TestData.BadRecord);

        // Assert
        mockLogger.Verify(m =>
            m.Log(It.IsAny<string>(), TestData.BadRecord[0]), Times.Once());
    }

    [Test]
    public void ParseData_WithBadStartData_LoggerIsCalledOnce()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        parser.ParseData(TestData.BadStartDate);

        // Assert
        mockLogger.Verify(m =>
            m.Log(It.IsAny<string>(), TestData.BadStartDate[0]), Times.Once());
    }

    [Test]
    public void ParseData_WithBadRating_LoggerIsCalledOnce()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        parser.ParseData(TestData.BadRating);

        // Assert
        mockLogger.Verify(m =>
            m.Log(It.IsAny<string>(), TestData.BadRating[0]), Times.Once());
    }

    [Test]
    public void ParseData_WithMixedRecords_LoggerIsCalled4Times()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var parser = new DataParser(mockLogger.Object);

        // Act
        parser.ParseData(TestData.Data);

        // Assert
        mockLogger.Verify(m =>
            m.Log(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
    }
}
