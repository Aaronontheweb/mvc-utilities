using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using MVC.Utilities.Caching;
using MVC.Utilities.Memcached.Tests.TestData;
using MVC.Utilities.Tests.TestData;
using NUnit.Framework;

namespace MVC.Utilities.Memcached.Tests.Memcached
{
    [TestFixture(Description = "Test fixture to ensure that our Memcached service")]
    public class MemcachedServiceTests
    {
        private ICacheService _cacheService;

        private Process memcacheProcess;
        private int port = 11211;

        #region Setup / Teardown

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _cacheService = new MemcachedService(new TimeSpan(0, 10, 0));

            ProcessStartInfo memcacheInfo;
            memcacheInfo = new ProcessStartInfo(MemcacheProcessHelper.MemcachePath);
            memcacheInfo.WorkingDirectory = Path.GetDirectoryName(MemcacheProcessHelper.MemcachePath);
            memcacheInfo.Arguments = string.Format("-p {0} -vvv", port);
            memcacheInfo.UseShellExecute = true;
            memcacheInfo.CreateNoWindow = false;
            memcacheInfo.RedirectStandardError = false;
            memcacheInfo.RedirectStandardOutput = false;
            memcacheProcess = Process.Start(memcacheInfo);

            //Spin until memcached can accept requests
            while(!memcacheProcess.Responding){}

            var config = new MemcachedClientConfiguration();
            config.Servers.Add(new IPEndPoint(IPAddress.Loopback, port));
            config.Protocol = MemcachedProtocol.Binary;

            MemcachedService.Initialize(config);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            memcacheProcess.Kill();
        }

        #endregion

        #region Tests

        [Test(Description = "Should be able to get a cache item with no problems")]
        public void Should_Get_False_on_Exists_for_Nonexistant_Cache_Item()
        {
            var isInCache = _cacheService.Exists(Guid.NewGuid().ToString());
            Assert.IsFalse(isInCache);
        }

        [Test(Description = "Should be able to save a serializable POCO class to the cache")]
        public void Should_Save_Item_To_Cache()
        {
            //Create a new test user to save to the cache
            var user = TestDataHelper.CreateNewUser();

            //Save the user to the cache
            var result = _cacheService.Save(user.UserName, user);


            //Verify that the user is in the cache
            Assert.IsTrue(result);
        }

        [Test(Description = "Should be able to use the Exists method to find an item we saved to the cache")]
        public void Should_Find_Item_in_Cache()
        {
            //Save data to the cache
            var result = _cacheService.Save("animal", "bunny");

            //Verify that the user is in the cache
            Assert.IsTrue(result);

            var itemInCache = _cacheService.Exists("animal");
            Assert.IsTrue(itemInCache);
        }

        [Test(Description = "Should be able to use the Get method to retrieve an item we saved to the cache")]
        public void Should_Get_Item_From_Cache()
        {
            //Save data to the cache
            var result = _cacheService.Save("person", "Elvis");

            //Verify that the user is in the cache
            Assert.IsTrue(result);

            var itemInCache = _cacheService.Get("person");
            Assert.IsInstanceOf<string>(itemInCache);
            Assert.AreEqual("Elvis", (string)itemInCache);
        }

        [Test(Description = "Should be able to use the Get method to retrieve a .NET class instance we saved to the cache")]
        public void Should_Get_Poco_Item_From_Cache()
        {
            var user = TestDataHelper.CreateNewUser();

            //Save data to the cache
            var result = _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(result);

            var itemInCache = _cacheService.Get(user.UserName);
            Assert.IsInstanceOf<UserAccount>(itemInCache);
            Assert.AreEqual(user.EmailAddress, ((UserAccount)itemInCache).EmailAddress);
        }

        [Test(Description = "Should be able to overwrite an item in the cache")]
        public void Should_Overwrite_Item_in_Cache()
        {
            var user = TestDataHelper.CreateNewUser();

            //Save data to the cache
            var result = _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(result);

            //Verify that the user is actually in the database
            var itemInCache = _cacheService.Get(user.UserName);
            Assert.IsInstanceOf<UserAccount>(itemInCache);
            Assert.AreEqual(user.EmailAddress, ((UserAccount)itemInCache).EmailAddress);

            var originalName = user.UserName;
            var newName = "Bears";
            user.UserName = newName;

            Assert.AreNotEqual(originalName, user.UserName);
            Assert.IsTrue(_cacheService.Save(originalName, user));

            itemInCache = _cacheService.Get(originalName);
            Assert.IsInstanceOf<UserAccount>(itemInCache);
            Assert.AreEqual(user.EmailAddress, ((UserAccount)itemInCache).EmailAddress);
            Assert.AreNotEqual(originalName, ((UserAccount)itemInCache).UserName);
            Assert.AreEqual(newName, ((UserAccount)itemInCache).UserName);
        }

        #endregion
    }
}
