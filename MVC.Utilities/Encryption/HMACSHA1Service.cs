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
            var hash = new HMACSHA1 { Key = HexToByte(_validationKey) };
            var encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(originalStr)));
            return encodedPassword;
        }

        public bool CheckPassword(string plainText, string hashed)
        {
            var pass1 = HashPassword(plainText);

            return pass1.Equals(hashed);
        }

        //   Converts a hexadecimal string to a byte array. Used to convert encryption key values from the validation key.    
        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
