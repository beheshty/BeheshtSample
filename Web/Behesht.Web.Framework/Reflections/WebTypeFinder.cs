using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Behesht.Core.Infrastructure.Types;
using Behesht.Web.Framework.Controllers;
using Behesht.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Web.Framework.Reflections
{
    public class WebTypeFinder : IWebTypeFinder
    {

        public IEnumerable<ActionMethodInfoModel> GetAllActions(Assembly assembly)
        {
            var baseControllerMethods = typeof(BaseController).GetMethods();
            var methods = assembly.GetTypes()
              .Where(type => typeof(BaseApiController).IsAssignableFrom(type))
              .SelectMany(type => type.GetMethods())
              .Where(p => !baseControllerMethods.Any(n => n.ToString() == p.ToString()))
              .Where(p => p.ReflectedType.Namespace.StartsWith("Behesht") && !p.ReflectedType.Namespace.StartsWith("Behesht.Web.Framework"))
              .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)) && !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
              .Select(x =>
              {
                  var parameters = $"({string.Join(", ", x.GetParameters().Select(p => p.ParameterType.Name))})";
                  return new ActionMethodInfoModel
                  {
                      FullName = $"{x.ReflectedType.Namespace}.{x.ReflectedType.Name}.{x.Name}{parameters}",
                      Controller = x.ReflectedType.Name.Replace("Controller", ""),
                      Action = $"{x.Name}{parameters}",
                      Verb = x.GetCustomAttribute<HttpMethodAttribute>(true)?.HttpMethods.FirstOrDefault(),
                      IsAllowAnonymous = x.IsDefined(typeof(AllowAnonymousAttribute)),
                      ActionName = $"{x.Name}"
                  };
              }
              ).OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return methods;
        }

    }
}
