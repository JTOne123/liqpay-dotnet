using System;
using System.Security.Cryptography;
using System.Text;

namespace LiqPaySDK
{
    public static class LiqPayUtil
    {
        public static string ToBase64String(this string text)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = Encoding.UTF8.GetBytes(text);
            return ToBase64String(textAsBytes);
        }

        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(this string encodedText)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(textAsBytes);
        }

        public static byte[] SHA1Hash(this string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            }
        }
    }
}