using DevOne.Security.Cryptography.BCrypt;

namespace MVC.Utilities.Encryption
{
    /// <summary>
    /// Uses Blowfish Encryption to hash and check passwords
    /// </summary>
    public class BCryptService : ICryptoService
    {
        public string HashPassword(string originalStr)
        {
            //Generate a salt for our hash (makes hackers sad :( )
            var salt = BCryptHelper.GenerateSalt();

            //Generate a hash using our salt
            var hash = BCryptHelper.HashPassword(originalStr, salt);

            //Return the hash back to the caller
            return hash;
        }

        /// <summary>
        /// This is a bit of an ugly hack, but BCrypt requires its own salt given the implementation we're using,
        /// so user-defined salts are not used in any of our hashes.
        /// </summary>
        /// <param name="originalStr">The plaintext string</param>
        /// <param name="salt">An UNUSED salt</param>
        /// <returns>The BCrypt-hashed string</returns>
        public string HashPassword(string originalStr, string salt)
        {
            return HashPassword(originalStr);
        }

        public bool CheckPassword(string plainText, string hashed)
        {
            //Easy-peasy
            return BCryptHelper.CheckPassword(plainText, hashed);
        }

        public bool CheckPassword(string plainText, string hashed, string salt)
        {
            //Easy-peasy
            return BCryptHelper.CheckPassword(plainText, hashed);
        }
    }
}
