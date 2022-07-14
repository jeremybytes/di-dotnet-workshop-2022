using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HouseControl.Library.Test
{
    [TestClass]
    public class ScheduleTests
    {
        //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\ScheduleData";
        ScheduleFileName fileName = new ScheduleFileName(AppDomain.CurrentDomain.BaseDirectory + "\\ScheduleData");

        [TestCleanup]
        public void Teardown()
        {
            ScheduleHelper.TimeProvider = null;
        }

        [TestMethod]
        public void ScheduleItems_OnCreation_IsPopulated()
        {
            // Arrange / Act
            var schedule = new Schedule(fileName, new FakeSunsetProvider());

            // Assert
            Assert.IsTrue(schedule.Count > 0);
        }

        [TestMethod]
        public void ScheduleItems_OnCreation_AreInFuture()
        {
            // Arrange / Act
            var schedule = new Schedule(fileName, new FakeSunsetProvider());
            var currentTime = DateTimeOffset.Now;

            // Assert
            foreach (var item in schedule)
            {
                Assert.IsTrue(item.Info.EventTime > currentTime);
            }
        }

        [TestMethod]
        public void ScheduleItems_AfterRoll_AreInFuture()
        {
            // Arrange
            var schedule = new Schedule(fileName, new FakeSunsetProvider());
            var currentTime = DateTimeOffset.Now;
            foreach (var item in schedule)
            {
                if (item.Info.EventTime > currentTime)
                    item.Info.EventTime = item.Info.EventTime - TimeSpan.FromDays(7);
                Assert.IsTrue(item.Info.EventTime < currentTime);
            }

            // Act
            schedule.RollSchedule();

            // Assert
            foreach (var item in schedule)
            {
                Assert.IsTrue(item.Info.EventTime > currentTime);
            }
        }

        [TestMethod]
        public void OneTimeItemInPast_AfterRoll_IsRemoved()
        {
            // Arrange
            var schedule = new Schedule(fileName, new FakeSunsetProvider());
            var originalCount = schedule.Count;
            var scheduleItem = new ScheduleItem()
            {
                Device = 1,
                Command = DeviceCommands.On,
                Info = new ScheduleInfo()
                {
                    EventTime = DateTimeOffset.Now.AddMinutes(-2),
                    Type = ScheduleType.Once,
                },
                IsEnabled = true,
                ScheduleSet = "",
            };
            schedule.Add(scheduleItem);
            var newCount = schedule.Count;
            Assert.AreEqual(originalCount + 1, newCount);

            // Act
            schedule.RollSchedule();

            // Assert
            Assert.AreEqual(originalCount, schedule.Count);
        }

        [TestMethod]
        public void OneTimeItemInFuture_AfterRoll_IsStillThere()
        {
            // Arrange
            var schedule = new Schedule(fileName, new FakeSunsetProvider());
            var originalCount = schedule.Count;
            var scheduleItem = new ScheduleItem()
            {
                Device = 1,
                Command = DeviceCommands.On,
                Info = new ScheduleInfo()
                {
                    EventTime = DateTimeOffset.Now.AddMinutes(2),
                    Type = ScheduleType.Once,
                },
                IsEnabled = true,
                ScheduleSet = "",
            };
            schedule.Add(scheduleItem);
            var newCount = schedule.Count;
            Assert.AreEqual(originalCount + 1, newCount);

            // Act
            schedule.RollSchedule();

            // Assert
            Assert.AreEqual(newCount, schedule.Count);
        }
    }
}
