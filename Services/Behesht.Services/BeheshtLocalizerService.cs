using Behesht.Core.Caching;
using Behesht.Core.Infrastructure.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;
using Behesht.Core;

namespace Behesht.Services
{
    public class BeheshtLocalizerService : IBeheshtLocalizerService
    {
        private readonly string _localizationFilesPath;
        private readonly IFileService _fileService;
        private readonly IBeheshtCacheManager _cacheManager;
        private const string _translateFilePrefixCacheKey = "dic-";

        public BeheshtLocalizerService(string localizationFilesPath,
            IFileService fileService,
            IBeheshtCacheManager cacheManager)
        {
            _localizationFilesPath = localizationFilesPath;
            _fileService = fileService;
            _cacheManager = cacheManager;
        }

        public string TranslateByKey(string key)
        {
            return TranslateByKey(ServicesCommonHelper.DefaultCulture, key);
        }

        public string TranslateByKey(string cultureName, string key)
        {
            var fileContent = GetFileContentByCulture(cultureName);
            var jsonDic = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
            if (jsonDic.ContainsKey(key))
            {
                return jsonDic[key];
            }
            return key;
        }

        public IDictionary<string, string> TranslateByKeys(string cultureName, params string[] keys)
        {
            var resultDic = new Dictionary<string, string>(keys.ToDictionary(k => k, v => ""));
            var fileContent = GetFileContentByCulture(cultureName);
            var jsonDic = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
            foreach (var item in resultDic)
            {
                if (jsonDic.ContainsKey(item.Key))
                {
                    resultDic[item.Key] = jsonDic[item.Key];
                }
                else
                {
                    resultDic[item.Key] = item.Key;
                }
            }
            return resultDic;
        }

        private string GetFileContentByCulture(string cultureName)
        {
            return _cacheManager.GetOrCreate(
                _translateFilePrefixCacheKey + cultureName,
                () => _fileService.ReadAllText(
                    _fileService.CombinPaths(_localizationFilesPath, _translateFilePrefixCacheKey + cultureName + ".json"),
                    Encoding.UTF8));
        }

        public IDictionary<string, string> GetDictionaryByCulture(string cultureName)
        {
            var fileContent = GetFileContentByCulture(cultureName);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(fileContent);
        }
    }
}
