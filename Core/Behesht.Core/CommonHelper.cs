using Behesht.Core.Infrastructure.IO;
using Behesht.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Behesht.Core
{
    public partial class CoreCommonHelper
    {
        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public static IFileService DefaultFileService { get; set; }

        public static List<EnumData> GetEnumList<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).OfType<Enum>().OrderBy(p => Convert.ToInt32(p))
                .Select(p => new EnumData() { DisplayName = p.GetDisplayName(), Value = Convert.ToInt32(p), Name = p.ToString() })
                .Where(p => !string.IsNullOrEmpty(p.DisplayName)).ToList();
        }

    }
}
