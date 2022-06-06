using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum en)
        {
            var member = en.GetType().GetMember(en.ToString()).First();
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            return attr == null ? member.Name : attr.Name;
        }
    }
}
