using Behesht.Core.Infrastructure.IO;
using Behesht.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Behesht.Core
{
    public partial class ServicesCommonHelper
    {

        /// <summary>
        /// Gets or sets the default localizer service
        /// </summary>
        public static IBeheshtLocalizerService DefaultLocalizer { get; set; }

        public static string DefaultCulture { get; set; } = "en-US";
    }
}
