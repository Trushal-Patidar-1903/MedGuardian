using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Helper.Cryptography
{
    /// <summary>
    /// Provides methods for hashing, encrypting, and decrypting passwords.
    /// </summary>
    public class CryptographyProcessor
    {
        #region Constants and Fields
        private const int HashIteration = 1000;
        private const int Supporter = 8;
        private static readonly int Balancer = Supporter + (int)Math.Floor(Math.Sqrt(Supporter));
        private static readonly int Maximum = HashIteration * Supporter * Balancer;
        private static readonly int Minimum = HashIteration * Supporter * Balancer / (int)Math.Floor(Math.Sqrt(Supporter));
        private static readonly byte[] StaticSaltKey = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];
        private static string EncryptionKey = string.Empty;
        #endregion

        #region Constructor
        public CryptographyProcessor(IConfiguration configuration)
        {
            EncryptionKey = configuration["JwtSettings:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "JwtSettings:SecretKey not found in configuration");
        }
        #endregion

        #region Hashing Methods
        public static string HashPassword(string password, string saltBase64 = "")
        {
            password = string.IsNullOrWhiteSpace(password) ? throw new ArgumentNullException(nameof(password), "Password cannot be empty.") : password;

            password = password.Trim();
            byte[] salt = !string.IsNullOrEmpty(saltBase64) ? Convert.FromBase64String(saltBase64) : GenerateSalt(password);
            string combinedPassword = string.Concat(password, Convert.ToBase64String(salt));
            int iteration = GetIteration(password);

            using Rfc2898DeriveBytes rfc2898 = new(combinedPassword, salt, iteration, HashAlgorithmName.SHA256);
            byte[] hash = rfc2898.GetBytes(32);
            return $"{Convert.ToBase64String(hash)}{Common.Strings.Separator}{Convert.ToBase64String(salt)}";
        }

        public static int GetIteration(string password)
        {
            password = string.IsNullOrEmpty(password) ? throw new ArgumentException(nameof(password), "Password cannot be null or empty") : password;

            int iteration = password.Length * Supporter * HashIteration;

            if (iteration < Minimum)
            {
                iteration *= (int)Math.Floor(Math.Sqrt(Supporter));
                if (iteration < Minimum)
                {
                    iteration *= (int)Math.Floor(Math.Sqrt(Supporter));
                }
            }

            if (iteration > Maximum)
            {
                iteration /= (int)Math.Floor(Math.Sqrt(Supporter));
                if (iteration > Maximum)
                {
                    iteration /= (int)Math.Floor(Math.Sqrt(Supporter));
                }
            }

            if (!(iteration >= Minimum && iteration <= Maximum))
            {
                iteration = (Maximum + Minimum) / (int)Math.Floor(Math.Sqrt(Supporter));
            }

            return iteration;
        }

        private static byte[] GenerateSalt(string password)
        {
            int saltLength = Math.Max(16, password.Length * Supporter);
            byte[] salt = new byte[saltLength];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
        #endregion

        #region Encryption and Decryption Methods
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            }

            byte[] key = GenerateKey();
            byte[] iv = GenerateIV();
            string hashedPassword = HashPassword(password);
            string cypherText = EncryptData(hashedPassword, key, iv);
            return $"{cypherText}{Common.Strings.Separator}{Convert.ToBase64String(key)}{Common.Strings.Separator}{Convert.ToBase64String(iv)}";
        }

        public static string DecryptPassword(string Password)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException(nameof(Password), "Encrypted password cannot be null or empty");
            }

            try
            {
                string[] parts = Password.Split(Common.Strings.Separator);
                byte[] key = Convert.FromBase64String(parts[1]);
                byte[] iv = Convert.FromBase64String(parts[2]);
                return DecryptData(parts[0], key, iv);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to decrypt the password. Data might be corrupt.", ex);
            }
        }

        private static string EncryptData(string plainText, byte[] key, byte[] iv)
        {
            return string.IsNullOrWhiteSpace(plainText)
                ? throw new ArgumentNullException(nameof(plainText), "Data cannot be empty")
                : Convert.ToBase64String(PerformCryptography(Encoding.Unicode.GetBytes(plainText), key, iv, true));
        }

        private static string DecryptData(string cipherText, byte[] key, byte[] iv)
        {
            cipherText = string.IsNullOrWhiteSpace(cipherText) ? throw new ArgumentNullException(nameof(cipherText), "Cipher text cannot be null or empty") : cipherText;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = PerformCryptography(cipherBytes, key, iv, false);
            return Encoding.Unicode.GetString(decryptedBytes);
        }

        private static byte[] PerformCryptography(byte[] data, byte[] key, byte[] iv, bool encrypt)
        {
            using Aes aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            using MemoryStream memoryStream = new();
            ICryptoTransform cryptoTransform = encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor();

            using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }
        #endregion

        #region Key and IV Generation
        private static byte[] GenerateKey()
        {
            return GenerateRandomBytes(32);
        }

        private static byte[] GenerateIV()
        {
            return GenerateRandomBytes(16);
        }

        private static byte[] GenerateRandomBytes(int size)
        {
            byte[] bytes = new byte[size];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
        #endregion

        #region Password Verification
        public static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            if (string.IsNullOrWhiteSpace(enteredPassword) || string.IsNullOrWhiteSpace(storedPassword))
            {
                throw new ArgumentNullException(nameof(storedPassword), "Both passwords must be provided");
            }

            try
            {
                string[] storedParts = DecryptPassword(storedPassword).Split(Common.Strings.Separator);
                if (storedParts.Length != 2)
                {
                    throw new FormatException("Stored password format is invalid.");
                }

                string storedHash = storedParts[0];
                string salt = storedParts[1];
                string enteredHash = HashPassword(enteredPassword, salt).Split(Common.Strings.Separator)[0];

                return storedHash == enteredHash;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Password verification failed.", ex);
            }
        }
        #endregion

        #region Encrypt String
        public static string Encrypt(string clearText)
        {
            clearText = string.IsNullOrWhiteSpace(clearText) ? throw new ArgumentNullException(nameof(clearText), "Text cannot be empty") : clearText;

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using Aes aesEncryptor = Aes.Create();
            using Rfc2898DeriveBytes pdb = new(EncryptionKey, StaticSaltKey, HashIteration, HashAlgorithmName.SHA256);

            aesEncryptor.Key = pdb.GetBytes(32);
            aesEncryptor.IV = pdb.GetBytes(16);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aesEncryptor.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion Encrypt String

        #region Decrypt String
        public static string Decrypt(string cipherText)
        {
            cipherText = string.IsNullOrWhiteSpace(cipherText) ? throw new ArgumentNullException(nameof(cipherText), "Cipher text cannot be empty") : cipherText;

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using Aes aesEncryptor = Aes.Create();
            using Rfc2898DeriveBytes pdb = new(EncryptionKey, StaticSaltKey, HashIteration, HashAlgorithmName.SHA256);

            aesEncryptor.Key = pdb.GetBytes(32);
            aesEncryptor.IV = pdb.GetBytes(16);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aesEncryptor.CreateDecryptor(aesEncryptor.Key, aesEncryptor.IV), CryptoStreamMode.Write);

            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();

            byte[] decryptedBytes = ms.ToArray();
            return Encoding.Unicode.GetString(decryptedBytes);
        }
        #endregion Decrypt String
    }
}
