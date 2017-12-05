using System;
using System.IO;
using System.Security.Cryptography;


namespace aescrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter path to the file: ");
                string file_path = Console.ReadLine();

                Console.WriteLine("Enter AES Key: ");
                string key = Console.ReadLine();

                //for example key = 5f4dcc3b5aa765d61d8327deb882cf99

                byte[] key_byte = new byte[key.Length / 2];

                for (int i = 0; i < key.Length; i += 2)
                {
                    key_byte[i / 2] = Convert.ToByte(key.Substring(i, 2), 16);
                }

                using (var myAes = Aes.Create())
                {
                    myAes.Key = key_byte;
                    myAes.IV = key_byte;

                    Console.WriteLine("\nWaiting...\n");

                    EncryptFileToByteAes(file_path, myAes.Key, myAes.IV);
                    DecryptFileToByteAes("_encrypt_" + file_path, myAes.Key, myAes.IV);

                    Console.WriteLine("Done\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            Console.ReadKey();
        }

        private static void EncryptFileToByteAes(string file_path, byte[] Key, byte[] IV) //encrypt
        {
            using (FileStream fsFileIn = File.OpenRead(file_path))
            using (FileStream fsFileEncrypt = File.Create("_encrypt_" + file_path))
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                AES.Key = Key;
                AES.IV = IV;
                using (CryptoStream csEncrypt = new CryptoStream(fsFileEncrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    fsFileIn.CopyTo(csEncrypt);
                }
            }
        }

        private static void DecryptFileToByteAes(string file_path, byte[] Key, byte[] IV) //decrypt
        {
            using (FileStream fsFileIn = File.OpenRead(file_path))
            using (FileStream fsFileOut = File.Create("_decrypt" + file_path))
            using (AesCryptoServiceProvider cryptAlgorithm = new AesCryptoServiceProvider())
            {
                cryptAlgorithm.Key = Key;
                cryptAlgorithm.IV = IV;
                using (CryptoStream csEncrypt = new CryptoStream(fsFileIn, cryptAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    csEncrypt.CopyTo(fsFileOut);
                }
            }
        }
    }
}
