
using System;

namespace MyFirstEShop.Repositories
{
    public interface IHasher
    {
        string HashGenerate(string password);
        bool VerifyPassword(string password, string hashPassword);
        string GenerateToken();
    }

    public class Hash : IHasher
    {
        private Scrypt.ScryptEncoder scryptEncoder = new Scrypt.ScryptEncoder();

        public string HashGenerate(string password)
        {
            return scryptEncoder.Encode(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return scryptEncoder.Compare(password, hashPassword);
        }
        public string GenerateToken()
        {

            string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_=";
            string pass = "";

            Random random = new Random();

            for (int i = 0; i < 150; i++)
            {
                int count = random.Next(0, alphabet.Length);
                pass += alphabet[count];
            }

            return pass;


        }
    }
}
