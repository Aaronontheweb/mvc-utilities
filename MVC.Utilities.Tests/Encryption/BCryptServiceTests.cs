using MVC.Utilities.Encryption;
using NUnit.Framework;

namespace MVC.Utilities.Tests.Encryption
{
    /// <summary>
    /// Summary description for BCryptServiceTest
    /// </summary>
    [TestFixture]
    public class BCryptServiceTest
    {
        private readonly ICryptoService _crypto;

        public BCryptServiceTest()
        {
            _crypto = new BCryptService();
        }

        /// <summary>
        /// Can we hash and check a simple password?
        /// </summary>
        [Test]
        public void CanBCryptHashAndCheckSimplePassword()
        {
            //Create a basic password...
            var originalPass = "password";

            //Hash it...
            var hash = _crypto.HashPassword(originalPass);

            //Assert that they're actually different...
            Assert.AreNotEqual(originalPass, hash);

            //And assert that they're not instances of the same object (I'm being anal here)
            Assert.AreNotSame(originalPass, hash);

            //Now use the validation function to check to see if they're equivalent

            Assert.IsTrue(_crypto.CheckPassword(originalPass, hash));
        }

        /// <summary>
        /// Assert that the hashes for our passwords are the same length
        /// </summary>
        [Test]
        public void AreBCryptHashesTheSameLength()
        {
            //Create our passwords...
            var pass1 = "password";
            var pass2 = "passwordpassword";
            var pass3 = "passwordpasswordpasswordpassword";
            var pass4 = "passwordpasswordpasswordpasswordpasswordpasswordpasswordpassword";

            //Hash our passwords...
            var hash1 = _crypto.HashPassword(pass1);
            var hash2 = _crypto.HashPassword(pass2);
            var hash3 = _crypto.HashPassword(pass3);
            var hash4 = _crypto.HashPassword(pass4);

            //Assert that the lengths are equivalent
            Assert.AreEqual(hash1.Length, hash2.Length);
            Assert.AreEqual(hash1.Length, hash3.Length);
            Assert.AreEqual(hash1.Length, hash4.Length);
        }

        /// <summary>
        /// Assert that the hashes for our passwords are the same length
        /// </summary>
        [Test]
        public void AreBCryptHashesWithDifferentSaltsTheSameLength()
        {
            //Create our passwords...
            var pass1 = "password";
            var pass2 = "passwordpassword";
            var pass3 = "passwordpasswordpasswordpassword";
            var pass4 = "passwordpasswordpasswordpasswordpasswordpasswordpasswordpassword";

            //Create our salts
            var salt1 = "magic";
            var salt2 = "moarmagic";
            var salt3 = "doublemagic";
            var salt4 = "triplemagic";

            //Hash our passwords...
            var hash1 = _crypto.HashPassword(pass1, salt1);
            var hash2 = _crypto.HashPassword(pass2, salt2);
            var hash3 = _crypto.HashPassword(pass3, salt3);
            var hash4 = _crypto.HashPassword(pass4, salt4);

            //Assert that the lengths are equivalent
            Assert.AreEqual(hash1.Length, hash2.Length);
            Assert.AreEqual(hash1.Length, hash3.Length);
            Assert.AreEqual(hash1.Length, hash4.Length);
        }

    }
}
