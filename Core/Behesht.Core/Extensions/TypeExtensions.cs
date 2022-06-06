using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {

        public static bool IsSearchable(this Type type)
        {
            //var typeInfo = type.GetTypeInfo();
            //if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            //{
            //    // nullable type, check if the nested type is simple.
            //    return IsSimple(typeInfo.GetGenericArguments()[0]);
            //}
            return 
                 type.Equals(typeof(string))
                || type.Equals(typeof(int))
                || type.Equals(typeof(long))
                || type.Equals(typeof(decimal))
                || type.Equals(typeof(float))
                || type.Equals(typeof(double))
                //|| type.Equals(typeof(DateTime))
                ;
        }

        public static bool IsGenericList(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
