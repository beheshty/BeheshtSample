using Behesht.Web.Framework.Models;
using System.Collections.Generic;
using System.Reflection;

namespace Behesht.Web.Framework.Reflections
{
    public interface IWebTypeFinder
    {
        IEnumerable<ActionMethodInfoModel> GetAllActions(Assembly assembly);

    }
}