using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Services.Models
{
    public class BaseServiceResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public string MessageLocalizationKey { get; set; }
        public int Status { get; set; }
    }

    public class BaseServiceResult<TData> : BaseServiceResult where TData : class
    {
        public TData Data { get; set; }
    }
}
