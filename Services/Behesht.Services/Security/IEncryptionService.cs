namespace Behesht.Services.Security
{
    public interface IEncryptionService
    {

        string GenerateSalt(int bufferSize);
        /// <summary>
        /// hash the input text with provided salt
        /// </summary>
        /// <param name="text"></param>
        /// <param name="salt"></param>
        /// <returns>the encrypted text</returns>
        string Encrypt(string text, string salt);
        /// <summary>
        /// hash the input text
        /// </summary>
        /// <param name="text"></param>
        /// <returns>the encrypted text</returns>
        string Encrypt(string text);
    }
}