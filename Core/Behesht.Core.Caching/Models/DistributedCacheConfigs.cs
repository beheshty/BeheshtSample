using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Core.Caching.Models
{
    public class DistributedCacheConfigs
    {
        public string KeyPrefix { get; set; }
        public string Endpoint { get; set; }
        public int Port { get; set; }
    }
}
