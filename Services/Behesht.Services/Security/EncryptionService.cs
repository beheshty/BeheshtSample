using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Services.Security
{
    public class EncryptionService : IEncryptionService
    {
        public EncryptionService()
        {

        }

        public string GenerateSalt(int bufferSize)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            return salt;
        }

        public string Encrypt(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }


        public string Encrypt(string text, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, salt);
        }

      
    }
}
