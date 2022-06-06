using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Models
{
    public class BaseApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public string MessageLocalizationKey { get; set; }
        public object Data { get; set; }
        public int Status { get; set; }
    }
}
