using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework
{
    public static class ServiceProviderStore
    {
        public static IServiceProvider Provider { get; private set; }

        public static void StoreProvider(IServiceProvider serviceProvider)
        {
            Provider = serviceProvider;
        }
    }
}
