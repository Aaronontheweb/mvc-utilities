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
        /// <param name="originalStr">the original string</param>
        /// <returns>a hashed string</returns>
        string HashPassword(string originalStr);

        /// <summary>
        /// Checks the two strings to see if the hashes are equivalent
        /// </summary>
        /// <param name="plainText">The first value to check</param>
        /// <param name="hashed">The second value to check</param>
        /// <returns>Returns true if the hashes are equivalent in accordance with the crypto implementation</returns>
        bool CheckPassword(string plainText, string hashed);
    }
}
