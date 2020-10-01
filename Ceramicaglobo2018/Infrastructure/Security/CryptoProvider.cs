using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace WebSite.Infrastructure.Security
{
    public static class CryptoProvider
    {
        static string pk = "wglZhGehsEb7FlGwpUOjBFYBL5LUD8lynJ6bde1Ql3tZT7JCm1";

        static Byte[] TruncateHash(string key,int length)
        {
            // Hash the key.
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            Byte[] keyBytes= System.Text.Encoding.Unicode.GetBytes(key);
            Byte[] hash = sha1.ComputeHash(keyBytes);

            Array.Resize(ref hash, length);
            return hash;

        }

      public static string Encrypt(string plaintext,string key)
        {
            RijndaelManaged rj = new RijndaelManaged();
            // Initialize the crypto provider
            rj.Key = TruncateHash(key, rj.KeySize / 8);
            rj.IV = TruncateHash("ShAaH05FCpZE", rj.BlockSize / 8); //vettore;

            Byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(plaintext);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the encoder to write to the stream.
            CryptoStream encStream =new CryptoStream(ms, rj.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();

        //' Convert the encrypted stream to a printable string.
        return Convert.ToBase64String(ms.ToArray());

        }

        public static string Encrypt(string plaintext)
        {
            return Encrypt(plaintext, pk);
        }

        public static string DecryptData(string encryptedtext, string key = "") {
            if (key == "")
            {
                return DecryptData(encryptedtext, pk);
            }
        
            RijndaelManaged rj = new RijndaelManaged();

            rj.Key = TruncateHash(key, rj.KeySize / 8);
            rj.IV = TruncateHash("ShAaH05FCpZE", rj.BlockSize  /8); //vettore;

            try
            {
                Byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

                // Create the stream.
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                // Create the decoder to write to the stream
                CryptoStream decStream = new CryptoStream(ms, rj.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

                // Use the crypto stream to write the byte array to the stream.
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();

                // Convert the plaintext stream to a string.
                return System.Text.Encoding.Unicode.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }


        public static string setToken(string[] values)
        {
            string result = "";

            foreach(string s in values)
            {
                result += "|" + s;
            }
            
            result = Encrypt(result.Substring(1, result.Length));
            return result;
        }

        public static string[] getToken(string value = "")
        {
            string tempstring = DecryptData(value);
            return tempstring.Split('|');
        }

    }
}