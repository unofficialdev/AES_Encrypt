using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AES_Encrypt
{
    class Program
    {
        private const string KEY_ENC = "584ff9dmskcdrovk";
        static void Main(string[] args)
        {
            // encrypt
            string inputText = "{\"status\":true,\"key\":\"808ffc0a-0cda-4358-9f01-a28d2a3x90db\",\"name\":\"unoficialdev\",\"add_date\":\"2021-04-20\",\"token\":\"Y79Y3SOhBwSruNwWPvo2XQbBv9s9eypfK9gioTuLhxnZWtW4cS8uGYj98cS2mqWr\"}";
            Console.WriteLine(inputText);
            Console.WriteLine("");


            string encryptText = Encryptor(inputText, KEY_ENC);
            Console.WriteLine(encryptText);
            Console.WriteLine("");
            // decrypt
            string decryptText = Decrypt(encryptText, KEY_ENC);
            Console.WriteLine(decryptText);
            Console.ReadKey();
        }


        static string Encryptor(string text, string key)
        {
            byte[] btkey = Encoding.ASCII.GetBytes(key);
            RijndaelManaged aes128 = new()
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform encryptor = aes128.CreateEncryptor(btkey, null);
            MemoryStream msEncrypt = new();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        static string Decrypt(string text, string key)
        {
            byte[] cipher = Convert.FromBase64String(text);
            byte[] btkey = Encoding.ASCII.GetBytes(key);
            RijndaelManaged aes128 = new()
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform decryptor = aes128.CreateDecryptor(btkey, null);
            MemoryStream ms = new(cipher);
            CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            byte[] plain = new byte[cipher.Length];
            int decryptcount = cs.Read(plain, 0, plain.Length);
            ms.Close();
            cs.Close();
            return Encoding.UTF8.GetString(plain, 0, decryptcount);
        }
    }
}
