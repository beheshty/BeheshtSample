using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Behesht.Core.Infrastructure.Types
{
    public interface ITypeFinder
    {
        /// <summary>
        /// Find classes of type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="onlyConcreteClasses">A value indicating whether to find only concrete classes</param>
        /// <returns>Result</returns>
        IEnumerable<System.Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// Find classes of type
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="onlyConcreteClasses">A value indicating whether to find only concrete classes</param>
        /// <returns>Result</returns>
        /// <returns></returns>
        IEnumerable<System.Type> FindClassesOfType(System.Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// Find classes of type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">A value indicating whether to find only concrete classes</param>
        /// <returns>Result</returns>
        IEnumerable<System.Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// Find classes of type
        /// </summary>
        /// <param name="assignTypeFrom">Assign type from</param>
        /// <param name="assemblies">Assemblies</param>
        /// <param name="onlyConcreteClasses">A value indicating whether to find only concrete classes</param>
        /// <returns>Result</returns>
        IEnumerable<System.Type> FindClassesOfType(System.Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// Gets the assemblies related to the current implementation.
        /// </summary>
        /// <returns>A list of assemblies</returns>
        IList<Assembly> GetAssemblies();

        string GetBinDirectory();
    }
}
