using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string text)
        {
            if (text.Length <= 1)
            {
                return text.ToLower();
            }

            if (text.Length > 1 && text[1] >= 'a' && text[1] <= 'z')
            {
                return text.Substring(0, 1).ToLower() + text[1..];
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= 'a' && text[i] <= 'z')
                {
                    return text.Substring(0, i - 1).ToLower() + text.Substring(i - 1, text.Length - i + 1);
                }
            }

            return text.ToLower();
        }



    }
}
