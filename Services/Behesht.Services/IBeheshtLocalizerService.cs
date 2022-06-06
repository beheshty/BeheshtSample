using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services
{
    public interface IBeheshtLocalizerService
    {
        /// <summary>
        /// translate the given key to the povided culture
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <param name="key">the key to translate</param>
        /// <returns>the translated text if exist, otherwise the key itself</returns>
        string TranslateByKey(string cultureName, string key);
        /// <summary>
        /// translate the given key to the default culture
        /// </summary>
        /// <param name="key">the key to translate</param>
        /// <returns>the translated text if exist, otherwise the key itself</returns>
        string TranslateByKey(string key);
        /// <summary>
        /// translate the given keys to provided culture
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <param name="keys">the keys to translate</param>
        /// <returns>a dictionary of translated texts or the keys itself if the translation didn't exist</returns>
        IDictionary<string, string> TranslateByKeys(string cultureName, params string[] keys);
        IDictionary<string, string> GetDictionaryByCulture(string cultureName);
    }
}
