using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC.Utilities.Routing;
using NUnit.Framework;

namespace MVC.Utilities.Tests.Routing
{
    [TestFixture(Description = "Tests for validating that our SlugHelper behaves as expected")]
    public class SlugHelperTests
    {
        #region Tests

        [Test]
        public void ShouldProvideValidRepresentationOfTimeStampFromDate()
        {
            var targetDate = DateTime.UtcNow;
            var dateSlug = SlugHelper.GenerateSlug(targetDate);
            Assert.IsNotNullOrEmpty(dateSlug);
        }

        #endregion
    }
}
