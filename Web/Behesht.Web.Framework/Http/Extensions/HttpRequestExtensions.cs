using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Http.Extensions
{
    public static class HttpRequestExtensions
    {
        public static T GetFromQueryString<T>(this HttpRequest httpRequest) where T : new()
        {
            return (T)CreateObejct(httpRequest, typeof(T), string.Empty);

            //var valueAsString = httpRequest.Query[paramName];
            //if (valueAsString == StringValues.Empty)
            //{
            //    return new T();
            //}
            //try
            //{
            //    return JsonSerializer.Deserialize<T>(valueAsString, CaseInsensitiveJsonSerializerOptions.JsonSerializerOptions);
            //}
            //catch (Exception)
            //{
            //    return new T();
            //}
        }

        private static object CreateObejct(HttpRequest httpRequest, Type type, string className)
        {
            var obj = Activator.CreateInstance(type);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyName = className + property.Name;
                if (property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System."))
                {
                    property.SetValue(obj, CreateObejct(httpRequest, property.PropertyType, propertyName));
                }
                else
                {
                    var valueAsString = httpRequest.Query[propertyName.ToCamelCase()];
                    if (valueAsString == StringValues.Empty)
                    {
                        valueAsString = httpRequest.Query[propertyName];
                        if (valueAsString == StringValues.Empty)
                        {
                            valueAsString = httpRequest.Query[propertyName.ToLower()];
                            if (valueAsString == StringValues.Empty)
                            {
                                continue;
                            }
                        }
                    }

                    var value = Parse(valueAsString, property.PropertyType);

                    if (value == null)
                    {
                        continue;
                    }

                    property.SetValue(obj, value, null);
                }
            }
            return obj;
        }

        private static object Parse(string valueToConvert, Type dataType)
        {
            TypeConverter obj = TypeDescriptor.GetConverter(dataType);
            object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, valueToConvert);
            return value;
        }

    }
}
