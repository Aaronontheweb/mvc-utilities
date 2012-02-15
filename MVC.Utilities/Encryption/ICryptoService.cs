namespace MVC.Utilities.Encryption
{
    /// <summary>
    /// Interface for abstracing all of the cryptography features we need to secure user information
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Hashes the original string used whatever mechanism is implemented by the interface
        /// </summary>
        /// <param name="originalStr">a plaintext string</param>
        /// <returns>a hashed string</returns>
        string HashPassword(string originalStr);

        /// <summary>
        /// Hashes the original string plus its salt used whatever mechanism is implemented by the interface
        /// </summary>
        /// <param name="originalStr">a plaintext string</param>
        /// <param name="salt">a string containing a salt used to create the hash</param>
        /// <returns>a hashed string</returns>
        string HashPassword(string originalStr, string salt);

        /// <summary>
        /// Checks the two strings to see if the hashes are equivalent
        /// </summary>
        /// <param name="plainText">The first value to check</param>
        /// <param name="hashed">The second value to check</param>
        /// <returns>Returns true if the hashes are equivalent in accordance with the crypto implementation</returns>
        bool CheckPassword(string plainText, string hashed);


        /// <summary>
        /// Checks the two strings to see if the hashes are equivalent
        /// </summary>
        /// <param name="plainText">The first value to check</param>
        /// <param name="hashed">The second value to check</param>
        /// <param name="salt">a string containing the salt used to create the hash</param>
        /// <returns>Returns true if the hashes are equivalent in accordance with the crypto implementation</returns>
        bool CheckPassword(string plainText, string hashed, string salt);
    }
}
