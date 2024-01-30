using ForumUsers.Model;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;

namespace ForumUsers.Authentication
{
    internal class Cryptography
    {
        internal (string salt, string hash) GenerateEncryptedKeys(string password)
        {
            try
            {
                //Generate Salt
                byte[] salt = new byte[8];
                RandomNumberGenerator.Fill(salt);
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations: 5000, HashAlgorithmName.SHA256);
                string hash = Convert.ToBase64String(pbkdf2.GetBytes(20));
                string saltedString = Convert.ToBase64String(salt);
                return (saltedString, hash);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return (null, ex.ToString());
            }
        }

        internal bool ConfrontKeys(string password, string salt, string storedHash)
        {
            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations: 5000, HashAlgorithmName.SHA256);
                string hash = Convert.ToBase64String(pbkdf2.GetBytes(20));
                return hash == storedHash;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return false;
            }
        }
    }
}
