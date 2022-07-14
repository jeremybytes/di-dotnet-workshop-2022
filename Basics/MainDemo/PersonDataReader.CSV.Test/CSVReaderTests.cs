using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PersonDataReader.CSV.Test
{
    [TestClass]
    public class CSVReaderTests
    {
        [TestMethod]
        public async Task GetPeople_WithEmptyFile_ReturnsEmptyList()
        {
            var reader = new CSVReader("");
            reader.FileLoader = new FakeFileLoader("Empty");

            var result = await reader.GetPeople();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetPeople_WithNoFile_ThrowsFileNotFoundException()
        {
            var reader = new CSVReader("BadFileName.txt");

            await Assert.ThrowsExceptionAsync<FileNotFoundException>(
                async () => await reader.GetPeople());
        }

        [TestMethod]
        public async Task GetPeople_WithGoodRecords_ReturnsAllRecords()
        {
            var reader = new CSVReader("");
            reader.FileLoader = new FakeFileLoader("Good");

            var result = await reader.GetPeople();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetPeople_WithGoodAndBadRecords_ReturnsGoodRecords()
        {
            var reader = new CSVReader("");
            reader.FileLoader = new FakeFileLoader("Mixed");

            var result = await reader.GetPeople();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetPeople_WithOnlyBadRecords_ReturnsEmptyList()
        {
            var reader = new CSVReader("");
            reader.FileLoader = new FakeFileLoader("Bad");

            var result = await reader.GetPeople();

            Assert.AreEqual(0, result.Count());
        }

    }
}