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

        public bool CheckPassword(string plainText, string hashed)
        {
            //Easy-peasy
            return BCryptHelper.CheckPassword(plainText, hashed);
        }
    }
}
