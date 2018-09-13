using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Philips_Ultrasound_Patient_Form
{
    /* Encryption Class
     * This wrapper class provide support for encrypting data.
     */
    class Encryption
    {
        // Initialize Global Variable
   
        // Creates a Rijndael object for symmetric 
        // encryption of data
        RijndaelManaged rjndl;

        // Parameter for Encrypting and Decrypting Data 
        // keyName is used to determine entropy
        const string keyName = "PhilipsKey";
        // entropy is used as a scrambling factor in encryption
        byte[] entropy = Encoding.Unicode.GetBytes(keyName);

        // keySize and blockSize used for encryption
        const int keySize = 256;
        // blockSize is the basic unit of data that 
        // can be encrypted at one time
        const int blockSize = 256;

        //Default Constructor
        public Encryption()
        {
            // Create new instance of Rijndael for symmetric
            // encryption of the data. This creates a new key
            // and initialization vector (IV)
            rjndl = new RijndaelManaged();
            rjndl.KeySize = keySize;
            rjndl.BlockSize = blockSize;
            // Uses Cipher Block Chaining for processing data block
            rjndl.Mode = CipherMode.CBC;
            // For the case that data to be encrypted is less than 
            // specified block size, ISO10126 adds random data to 
            // the end of the data block to fill it up.
            rjndl.Padding = PaddingMode.ISO10126;
            LoadKeyFromConfig();
        }

        // GenerateNewKey Method
        // This function will generate a new AES key
        // and IV for the AES encryption.
        // It will also update the key stored in the application setting
        public void GenerateNewKey()
        {
            rjndl.GenerateIV();
            rjndl.GenerateKey();

            //Save the new key and IV into 
            SaveKeyToConfig();
        }

        // Encrypt method
        // This function accepts array of bytes containing the data to be encrypted
        // and return the encrypted data as array of bytes
        public byte[] Encrypt(byte[] inData)
        {
            // Create memorystream to hold the encrypted data.
            MemoryStream outFs = new MemoryStream();

            // Create encryption transform object
            ICryptoTransform transform = rjndl.CreateEncryptor();

            // Now write the cipher text using
            // a CryptoStream for encrypting.
            using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
            {
                int count = 0;
                int offset = 0;

                int blockSizeBytes = rjndl.BlockSize / 8;
                byte[] data = new byte[blockSizeBytes];
                int bytesRead = 0;

                // Encrypts array of bytes and outputs an 
                // array of bytes to the output stream
                using (MemoryStream dataStream = new MemoryStream(inData))
                {
                    do
                    {
                        count = dataStream.Read(data, 0, blockSizeBytes);
                        offset += count;
                        outStreamEncrypted.Write(data, 0, count);
                        bytesRead += blockSizeBytes;
                    }
                    while (count > 0);
                    dataStream.Close();
                }
                outStreamEncrypted.FlushFinalBlock();
                outStreamEncrypted.Close();
            }
            
            outFs.Close();
            return outFs.ToArray();
        }

        // Decrypt method
        // This function accepts array of bytes containing the data to be decrypted
        // and return the decrypted data as array of bytes
        public byte[] Decrypt(byte[] inData)
        {
            // Create Memory Stream to hold the decrypted 
            // and encrypted data
            MemoryStream outFs = new MemoryStream();
            MemoryStream inFs = new MemoryStream(inData);

            // Create decryption transform object
            ICryptoTransform transform = rjndl.CreateDecryptor();

            // blockSizeBytes can be any arbitrary size.
            int blockSizeBytes = rjndl.BlockSize / 8;
            byte[] data = new byte[blockSizeBytes];

            // Decrypt array of bytes and ouputs it as 
            // an array of bytes to output stream
            using (CryptoStream outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
            {
                int count = 0;
                int offset = 0;
                do
                {
                    count = inFs.Read(data, 0, blockSizeBytes);
                    offset += count;
                    outStreamDecrypted.Write(data, 0, count);
                }
                while (count > 0);

                outStreamDecrypted.FlushFinalBlock();
                outStreamDecrypted.Close();
            }
            outFs.Close();
            inFs.Close();
            return outFs.ToArray();
        }
            
        // ExportKey Method
        // Will export the AES key from the Key Container
        // into keyData stream
        public void ExportKey(Stream keyData)
        {
            // StreamWriter is for character output in a 
            // particular encoding
            StreamWriter sw = new StreamWriter(keyData);
            sw.WriteLine(Convert.ToBase64String(rjndl.Key));
            sw.WriteLine(Convert.ToBase64String(rjndl.IV));
            sw.Close();
        }

        // ImportKey Method
        // Will import the AES key in the input stream
        // into the Key Container
        public void ImportKey(Stream keyData)
        {
            // StreamReader is for character input in a 
            // particular encoding
            StreamReader sr = new StreamReader(keyData);
            rjndl.Key = Convert.FromBase64String(sr.ReadLine());
            rjndl.IV = Convert.FromBase64String(sr.ReadLine());
            sr.Close();
        }

        // LoadKeyFromConfig Method
        // Load Key and Initialization Vectors (IV) for Encryption/Decryption
        public void LoadKeyFromConfig()
        {
            Properties.Settings set = Properties.Settings.Default;
            if (set.AESKey != "0" && set.AESIV != "0")
            {
                //Load key from the Setting File
                rjndl.Key = DecryptKey(set.AESKey);
                rjndl.IV = DecryptKey(set.AESIV);
            }
            else
                GenerateNewKey();
        }

        // SaveKeyToConfig Method
        // Save Key and Initialization Vectors (IV) for Encryption/Decryption
        public void SaveKeyToConfig()
        {
            Properties.Settings set = Properties.Settings.Default;
            set.AESKey = EncryptKey(rjndl.Key);
            set.AESIV = EncryptKey(rjndl.IV);
            set.Save(); //Save the keys into application setting
        }

        // EncryptKey Method
        // Encrypt the AES key using DPAPI and returns the encrypted key
        private string EncryptKey(byte[] key)
        {
            return Convert.ToBase64String(ProtectedData.Protect(key, entropy, DataProtectionScope.LocalMachine));
        }

        // DecryptKey Method
        // Decrypts the AES key using DPAPI and returns the decrypted key
        private byte[] DecryptKey(string key)
        {
            return ProtectedData.Unprotect(Convert.FromBase64String(key), entropy, DataProtectionScope.LocalMachine);
        }

        //WILL BE REMOVED ON THE FINAL VERSION
        public string GetEncryptionKey()
        {
            return Convert.ToBase64String(rjndl.Key);
        }
    }
}
