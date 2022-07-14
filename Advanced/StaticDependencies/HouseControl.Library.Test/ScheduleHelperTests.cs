using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HouseControl.Library.Test
{
    [TestClass]
    public class ScheduleHelperTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Use current time provider as the default;
            // override in individual tests with "SetCurrentTime" if needed.
            ScheduleHelper.TimeProvider = new CurrentTimeProvider();
        }

        private static void SetCurrentTime(DateTimeOffset currentTime)
        {
            var mockTime = new Mock<ITimeProvider>();
            mockTime.Setup(t => t.Now()).Returns(currentTime);
            ScheduleHelper.TimeProvider = mockTime.Object;
        }

        [TestMethod]
        public void MondayItemInFuture_OnRollDay_IsUnchanged()
        {
            // Arrange
            var monday = new DateTimeOffset(2021, 01, 04, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);
            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };
            var scheduleHelper = new ScheduleHelper(new FakeSunsetProvider());

            // Act
            var newDate = scheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(monday, newDate);
        }

        [TestMethod]
        public void MondayItemInFuture_OnRollWeekdayDay_IsUnchanged()
        {
            // Arrange
            var monday = new DateTimeOffset(2021, 01, 04, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };
            var scheduleHelper = new ScheduleHelper(new FakeSunsetProvider());

            // Act
            var newDate = scheduleHelper.RollForwardToNextWeekdayDay(info);

            // Assert
            Assert.AreEqual(monday, newDate);
        }

        [TestMethod]
        public void SaturdayItemInFuture_OnRollDay_IsUnchanged()
        {
            // Arrange
            var saturday = new DateTimeOffset(2021, 01, 09, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            Assert.AreEqual(DayOfWeek.Saturday, saturday.DayOfWeek);
            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = saturday,
                TimeType = ScheduleTimeType.Standard,
            };
            var scheduleHelper = new ScheduleHelper(new FakeSunsetProvider());

            // Act
            var newDate = scheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(saturday, newDate);
        }

        [TestMethod]
        public void SaturdayItemInFuture_OnRollWeekendDay_IsUnchanged()
        {
            // Arrange
            var saturday = new DateTimeOffset(2021, 01, 09, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            Assert.AreEqual(DayOfWeek.Saturday, saturday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = saturday,
                TimeType = ScheduleTimeType.Standard,
            };
            var scheduleHelper = new ScheduleHelper(new FakeSunsetProvider());

            // Act
            var newDate = scheduleHelper.RollForwardToNextWeekendDay(info);

            // Assert
            Assert.AreEqual(saturday, newDate);
        }

        [TestMethod]
        public void MondayItemInPast_OnRollDay_IsTomorrow()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2015, 01, 12, 16, 35, 22,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            SetCurrentTime(currentTime);

            var monday = new DateTimeOffset(2015, 01, 12, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            var expectedTime = new DateTimeOffset(2015, 01, 13, 15, 32, 00,
                TimeZoneInfo.Local.GetUtcOffset(ScheduleHelper.Now()));
            Assert.AreEqual(DayOfWeek.Monday, monday.DayOfWeek);

            ScheduleInfo info = new ScheduleInfo()
            {
                EventTime = monday,
                TimeType = ScheduleTimeType.Standard,
            };
            var scheduleHelper = new ScheduleHelper(new FakeSunsetProvider());

            // Act
            var newDateTime = scheduleHelper.RollForwardToNextDay(info);

            // Assert
            Assert.AreEqual(expectedTime, newDateTime);
        }
    }
}
