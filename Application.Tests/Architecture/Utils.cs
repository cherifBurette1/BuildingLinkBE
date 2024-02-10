using System.Reflection;

namespace Application.Tests.Architecture
{
    public static class Utils
    {
        public static IEnumerable<Type> GetApplicationsClassess(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => t.IsClass)
                .Where(t => !t.IsSealed)
                .Where(t => t.Namespace != null);
        }
    }
}
