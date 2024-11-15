﻿//using System.Security.Cryptography;
//using System.Text;

//namespace studentDetails_Api.Services
//{
//    /// <summary>
//    /// Cryptographic services
//    /// </summary>
//    public class CryptoServices
//    {
//        private readonly IConfiguration _config;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly string _signingKey = string.Empty;
//        private readonly string _encyptionKey = string.Empty;

//        public CryptoServices(IConfiguration config, IHttpContextAccessor httpContextAccessor)
//        {
//            _config = config;
//            _httpContextAccessor = httpContextAccessor;
//            _signingKey = _config["JWT:SigningKey"] ?? "SignSecRetK3y$End!nE@PikEy!nS3ckEy";
//            _encyptionKey = _config["JWT:EncryptionKey"] ?? "EncSecRetK3y$vEnd!nE@PikEy!nS3ckEy";
//        }

//        public string EncryptStringToBytes_Aes(string plainText)
//        {
//            byte[] encrypted;
//            string encryptedStr;

//            using (Aes aesAlg = Aes.Create())
//            {
//                aesAlg.Key = GetKeyBytes(_encyptionKey, aesAlg.KeySize / 8);
//                aesAlg.IV = GetKeyBytes(_signingKey, aesAlg.BlockSize / 8);

//                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

//                using (MemoryStream msEncrypt = new MemoryStream())
//                {
//                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
//                    {
//                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
//                        {
//                            swEncrypt.Write(plainText);
//                        }
//                    }
//                    encrypted = msEncrypt.ToArray();
//                }
//            }

//            encryptedStr = Convert.ToBase64String(encrypted);
//            return encryptedStr;
//        }

//        private byte[] GetKeyBytes(string key, int bytes)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                byte[] keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
//                Array.Resize(ref keyBytes, bytes); // Truncate or pad with zeros if necessary
//                return keyBytes;
//            }
//        }
//    }
//}