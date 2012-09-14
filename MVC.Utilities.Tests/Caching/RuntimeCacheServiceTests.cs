using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using MVC.Utilities.Caching;
using MVC.Utilities.Tests.TestData;
using NUnit.Framework;

namespace MVC.Utilities.Tests.Caching
{
    /// <summary>
    /// Unit testing harness for testing the Runtime Cache provider
    /// </summary>
    [TestFixture]
    public class RuntimeCacheProviderTests
    {
        private ICacheService _cacheService;
        private IList<string> _cachedObjects;

        [SetUp]
        public void Setup()
        {
            //Create a new in-memory cache with a default sliding expiration of 20 minutes
            _cacheService = new RuntimeCacheService(MemoryCache.Default, new TimeSpan(0, 0, 20));

            //Create a dictionary we can use to clean-up items from the cache.
            _cachedObjects = new List<string>();
        }

        /// <summary>
        /// Can we save an item to the cache and verify that it exists?
        /// </summary>
        [Test]
        public void CanSaveItemToCacheAndVerify()
        {
            //Create a new test user to save to the cache
            var user = TestDataHelper.CreateNewUser();

            //Add the cached object to our clean-up list
            _cachedObjects.Add(user.UserName);

            //Save the user to the cache
            _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(_cacheService.Exists(user.UserName));
        }

        /// <summary>
        /// Can we save an item from the cache and then retrieve it?
        /// </summary>
        [Test]
        public void CanSaveAndRetrieveItemFromCache()
        {
            //Create a new test user to save to the cache
            var user = TestDataHelper.CreateNewUser();

            //Add the cached object to our clean-up list
            _cachedObjects.Add(user.UserName);

            //Save the user to the cache
            _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(_cacheService.Exists(user.UserName));

            //Retrieve the object from the cache
            var userInCache = _cacheService.Get(user.UserName) as UserAccount;

            //Assert that the object isn't null
            Assert.IsNotNull(userInCache);

            //Assert that the object in the cache is the same instance as our existing object
            Assert.AreSame(user, userInCache);
        }

        /// <summary>
        /// Can we successfully delete an existing item from the cache?
        /// </summary>
        [Test]
        public void CanDeleteItemFromCache()
        {
            //Create a new test user to save to the cache
            var user = TestDataHelper.CreateNewUser();

            //Add the cached object to our clean-up list
            _cachedObjects.Add(user.UserName);

            //Save the user to the cache
            _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(_cacheService.Exists(user.UserName));

            //Attempt to delete the object from the cache...
            var removeResult = _cacheService.Remove(user.UserName);

            //Assert that the item was indeed removed from the cache..
            Assert.IsTrue(removeResult);

            //Verify that the item was removed
            Assert.IsFalse(_cacheService.Exists(user.UserName));
        }

        /// <summary>
        /// Can we save an item from the cache and then retrieve it?
        /// </summary>
        [Test]
        public void CanUpdateItemInCache()
        {
            //Create a new test user to save to the cache
            var user = TestDataHelper.CreateNewUser();

            //Add the cached object to our clean-up list
            _cachedObjects.Add(user.UserName);

            //Save the user to the cache
            _cacheService.Save(user.UserName, user);

            //Verify that the user is in the cache
            Assert.IsTrue(_cacheService.Exists(user.UserName));

            //Clone the user's email address and save a copy for future comparison
            var oldUserEmail = user.EmailAddress.Clone() as string;

            //Create a new email address
            var newUserEmail = System.Guid.NewGuid().ToString();

            user.EmailAddress = newUserEmail;

            //Assert that the two emails aren't equal (reference checking, not Guid collisions)
            Assert.AreNotEqual(oldUserEmail, newUserEmail);

            //Update the user in the cache
            _cacheService.Save(user.UserName, user);

            //Retrieve the object from the cache
            var userInCache = _cacheService.Get(user.UserName) as UserAccount;

            //Assert that the object isn't null
            Assert.IsNotNull(userInCache);

            //Assert that the object in the cache is the same instance as our existing object
            Assert.AreNotEqual(oldUserEmail, userInCache.EmailAddress);
        }

        [Test]
        public void CleanUp()
        {
            //Go through the list of cached items and delete everything that doesn't belong
            foreach (var item in _cachedObjects)
            {
                //Remove the item from the cache
                _cacheService.Remove(item);
            }

            //Clear the list of objects
            _cachedObjects.Clear();
        }

    }
}
