using System.Security.Cryptography;
using System.Text;

namespace Miltha.Tech.Core.Helper.Cryptography
{
    public static class CryptographyHelper
    {
        public static string GetEncryptedString(string key, string value)
        {
            var hash = SHA1.HashData(Encoding.Unicode.GetBytes(key + "|" + value));
            var digest = new StringBuilder(hash.Length * 2);

            foreach (var n in hash)
                digest.Append(n.ToString("X2"));

            return digest.ToString();
        }
    }
}