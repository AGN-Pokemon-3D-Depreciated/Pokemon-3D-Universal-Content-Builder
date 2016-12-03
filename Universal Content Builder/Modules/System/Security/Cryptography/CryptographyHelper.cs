using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Universal_Content_Builder.Modules.System.Security.Cryptography
{
    public static class CryptographyHelper
    {
        /// <summary>
        /// This constant is used to determine the keysize of the encryption algorithm in bits. We divide this by 8 within the code below to get the equivalent number of bytes.
        /// </summary>
        private const int Keysize = 256;

        /// <summary>
        /// This constant determines the number of iterations for the password bytes generation function.
        /// </summary>
        private const int DerivationIterations = 1000;

        /// <summary>
        /// Convert a String to <see cref="MD5"/> checksum.
        /// </summary>
        /// <param name="Value">String to convert.</param>
        public static string ToMD5(this string Value)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Value));
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return Value; }
        }

        /// <summary>
        /// Convert a <see cref="Stream"/> to <see cref="MD5"/> checksum.
        /// </summary>
        /// <param name="Stream">Stream to convert.</param>
        public static string ToMD5(this Stream Stream)
        {
            try
            {
                Stream.Seek(0, SeekOrigin.Begin);
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Stream);
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Convert a String to <see cref="SHA1"/> checksum.
        /// </summary>
        /// <param name="Value">String to convert.</param>
        public static string ToSHA1(this string Value)
        {
            try
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Value));
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return Value; }
        }

        /// <summary>
        /// Convert a <see cref="Stream"/> to <see cref="SHA1"/> checksum.
        /// </summary>
        /// <param name="Stream">Stream to convert.</param>
        public static string ToSHA1(this Stream Stream)
        {
            try
            {
                Stream.Seek(0, SeekOrigin.Begin);
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hash = sha1.ComputeHash(Stream);
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Convert a String to <see cref="SHA256"/> checksum.
        /// </summary>
        /// <param name="Value">String to convert.</param>
        public static string ToSHA256(this string Value)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Value));
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return Value; }
        }

        /// <summary>
        /// Convert a <see cref="Stream"/> to <see cref="SHA256"/> checksum.
        /// </summary>
        /// <param name="Stream">Stream to convert.</param>
        public static string ToSHA256(this Stream Stream)
        {
            try
            {
                Stream.Seek(0, SeekOrigin.Begin);
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Stream);
                    return string.Join("", hash.Select(a => a.ToString("X2")).ToArray());
                }
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Encrypt a String with a password.
        /// </summary>
        /// <param name="plainText">String to convert.</param>
        /// <param name="passPhrase">Password.</param>
        public static string EncryptString(this string plainText, string passPhrase)
        {
            try
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = Generate256BitsOfRandomEntropy();
                var ivStringBytes = Generate256BitsOfRandomEntropy();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Decrypt a String with a password.
        /// </summary>
        /// <param name="cipherText">Encrypted string to convert.</param>
        /// <param name="passPhrase">Password.</param>
        public static string DecryptString(this string cipherText, string passPhrase)
        {
            try
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();
                var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
            catch (Exception) { return null; }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            var rngCsp = new RNGCryptoServiceProvider();
            // Fill the array with cryptographically secure random bytes.
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}