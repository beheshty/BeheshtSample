using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Localization.Date
{
    public static class PersianDateTime
    {
        public static DateTime ToGeoDate(string date, string betweenChar = "/")
        {
            if (string.IsNullOrEmpty(betweenChar))
            {
                if (date.Length == 8 && date.All(char.IsDigit))
                {
                    date = date.Insert(3, "/");
                    date = date.Insert(6, "/");
                    betweenChar = "/";
                }
                else
                {
                    throw new ArgumentException("Persian date is not valid", nameof(date));
                }
            }
            var splited = date.Split('/');
            int Month = Convert.ToInt32(splited[1]), Day = Convert.ToInt32(splited[2]);

            DateTime dt = new DateTime(Convert.ToInt32(splited[0]), Month, Day, new PersianCalendar());
            return dt;
        }

        public static string ToPersianDateTime(DateTime dateTime)
        {
            try
            {
                PersianCalendar P = new PersianCalendar();
                var ddate = Convert.ToDateTime(dateTime);
                string str = "" + (P.GetDayOfMonth(ddate) >= 10 ? "" : "0") + P.GetDayOfMonth(ddate) + "/"
                    + (P.GetMonth(ddate) >= 10 ? "" : "0") + P.GetMonth(ddate) + "/" + P.GetYear(ddate);
                string temp = str[(str.LastIndexOf("/") + 1)..];
                temp += "/";
                temp += str.Substring(str.IndexOf("/") + 1, 2);
                temp += "/";
                temp += str.Substring(0, 2);
                str = temp;

                return str;

            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
