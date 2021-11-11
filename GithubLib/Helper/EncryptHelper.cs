using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GithubLib.Helper
{
    public class EncryptHelper
    {
        static TripleDESCryptoServiceProvider instance_crypt(string key)
        {
            var r = new TripleDESCryptoServiceProvider();
            r.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            r.Mode = CipherMode.ECB;
            r.Padding = PaddingMode.PKCS7;
            return r;
        }

        public static string Decrypt(string text, string key)
        {
            var byte_text = Convert.FromBase64String(text);

            TripleDESCryptoServiceProvider tdes = instance_crypt(key);

            byte[] resultArray = tdes.CreateDecryptor().TransformFinalBlock(byte_text, 0, byte_text.Length);

            return Encoding.UTF8.GetString(resultArray);
        }


        public static string Encrypt(string text, string key)
        {
            var byte_text = Encoding.UTF8.GetBytes(text);

            TripleDESCryptoServiceProvider tdes = instance_crypt(key);

            byte[] resultArray = tdes.CreateEncryptor().TransformFinalBlock(byte_text, 0, byte_text.Length);

            return Convert.ToBase64String(resultArray);
        }
    }
}

