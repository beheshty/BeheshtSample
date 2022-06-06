using Behesht.Core.Localization.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtensions
    {
        public static string ToPersianDate(this DateTime date)
        {
            return PersianDateTime.ToPersianDateTime(date);
        }
    }
}
