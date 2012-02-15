using System;
using System.Security.Cryptography;
using System.Text;

namespace MVC.Utilities.Encryption
{
    public class HMACSHA1Service : ICryptoService
    {
        private readonly string _validationKey;

        public HMACSHA1Service(string validationKey)
        {
            _validationKey = validationKey;
        }

        public string HashPassword(string originalStr)
        {
            //Use the validation key as the default salt
            return HashPassword(originalStr, _validationKey);
        }

        public string HashPassword(string originalStr, string salt)
        {
            return HashPassword(originalStr, Encoding.Unicode.GetBytes(salt));
        }

        public string HashPassword(string originalStr, byte[] salt)
        {
            var hash = new HMACSHA1 { Key = salt };
            var encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(originalStr)));
            return encodedPassword;
        }

        public bool CheckPassword(string plainText, string hashed)
        {
            //Use the validation key as the default salt (consistent with previous implementations)
            return CheckPassword(plainText, hashed, _validationKey);
        }

        public bool CheckPassword(string plainText, string hashed, string salt)
        {
            var pass1 = HashPassword(plainText, salt);
            return pass1.Equals(hashed);
        }

        //   Converts a hexadecimal string to a byte array. Used to convert encryption key values from the validation key.    
        public static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
