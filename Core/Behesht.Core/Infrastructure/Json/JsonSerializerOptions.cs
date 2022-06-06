using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Text.Json
{
    public static class CaseInsensitiveJsonSerializerOptions 
    {
        private static JsonSerializerOptions _jsonSerializerOptions;
        public static JsonSerializerOptions JsonSerializerOptions
        { 
            get
            {
                if(_jsonSerializerOptions == null)
                {
                    _jsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    };
                }
                return _jsonSerializerOptions;
            } 
        }
    }

}
