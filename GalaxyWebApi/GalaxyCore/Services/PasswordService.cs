using System.Security.Cryptography;
using System.Text;

namespace GalaxyCore.Services
{
    public class PasswordService
    {
        private static readonly Encoding DefaultEncoding = System.Text.Encoding.UTF8;

        //todo добавить соль к паролю

        public bool ComparePasswords(byte[] bytesA, string stringB)
        {
            var bytesB = GetHashedPasswordBytes(stringB);
            return ComparePasswords(bytesA, bytesB);
        }

        private string BytesToStringWithFormatting(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private bool ComparePasswords(byte[] bytesA, byte[] bytesB)
        {
            if (bytesA is null || bytesB is null)
                return false;

            if (bytesA.Length != bytesB.Length)
                return false;

            for (var i = 0; i < bytesA.Length; i++)
                if (bytesA[i] != bytesB[i])
                    return false;

            return true;
        }

        public byte[] GetHashedPasswordBytes(string password)
        {
            using var sha256Hash = SHA256.Create();

            return sha256Hash.ComputeHash(DefaultEncoding.GetBytes(password));
        }
    }
}