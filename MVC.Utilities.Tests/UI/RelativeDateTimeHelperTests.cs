using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC.Utilities.UI;
using NUnit.Framework;

namespace MVC.Utilities.Tests.UI
{
    [TestFixture(Description = "Tests as to whether or not our datetime helper extension method actually works")]
    public class RelativeDateTimeTests
    {

        #region Past Date Tests
        [Test(Description = "Can we get a 'x days ago' message?")]
        public void CanAccuratelyRenderPastTimeInDays()
        {
            var pastTime = DateTime.UtcNow.AddDays(-3d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("days"));
            Assert.That(dateTimeOutput.Contains("3"));
            Assert.AreEqual(dateTimeOutput, "3 days ago");
        }

        [Test(Description = "Can we get a 'x hours ago' message?")]
        public void CanAccuratelyRenderPastTimeInHours()
        {
            var pastTime = DateTime.UtcNow.AddHours(-14d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("hours"));
            Assert.That(dateTimeOutput.Contains("14"));
            Assert.AreEqual(dateTimeOutput, "14 hours ago");
        }

        [Test(Description = "Can we get a 'x minutes ago' message?")]
        public void CanAccuratelyRenderPastTimeInMinutes()
        {
            var pastTime = DateTime.UtcNow.AddMinutes(-30d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("minutes"));
            Assert.That(dateTimeOutput.Contains("30"));
            Assert.AreEqual(dateTimeOutput, "30 minutes ago");
        }

        [Test(Description = "Can we get a 'x seconds ago' message?")]
        public void CanAccuratelyRenderPastTimeInSeconds()
        {
            var pastTime = DateTime.UtcNow.AddSeconds(-1d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("second"));
            Assert.That(dateTimeOutput.Contains("1"));
            Assert.AreEqual(dateTimeOutput, "1 second ago");
        }
        #endregion

        #region Future Date Tests

        [Test(Description = "Can we get a 'x days ago' message?")]
        public void CanAccuratelyRenderFutureTimeInDays()
        {
            var pastTime = DateTime.UtcNow.AddDays(3d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("days"));
            Assert.That(dateTimeOutput.Contains("3"));
            Assert.AreEqual(dateTimeOutput, "in 3 days");
        }

        [Test(Description = "Can we get a 'x hours ago' message?")]
        public void CanAccuratelyRenderFutureTimeInHours()
        {
            var pastTime = DateTime.UtcNow.AddHours(14d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("hours"));
            Assert.That(dateTimeOutput.Contains("14"));
            Assert.AreEqual(dateTimeOutput, "in 14 hours");
        }

        [Test(Description = "Can we get a 'x minutes ago' message?")]
        public void CanAccuratelyRenderFutureTimeInMinutes()
        {
            var pastTime = DateTime.UtcNow.AddMinutes(30d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("minutes"));
            Assert.That(dateTimeOutput.Contains("30"));
            Assert.AreEqual(dateTimeOutput, "in 30 minutes");
        }

        [Test(Description = "Can we get a 'x seconds ago' message?")]
        public void CanAccuratelyRenderFutureTimeInSeconds()
        {
            var pastTime = DateTime.UtcNow.AddSeconds(1d);

            var dateTimeOutput = DateTimeLocalizationHelper.ToRelativeDateTime(pastTime);

            Assert.That(dateTimeOutput.Contains("second"));
            Assert.That(dateTimeOutput.Contains("1"));
            Assert.AreEqual(dateTimeOutput, "in 1 second");
        }

        #endregion
    }
}
