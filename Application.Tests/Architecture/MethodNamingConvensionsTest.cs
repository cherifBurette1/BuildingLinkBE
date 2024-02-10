using Application.Contracts.Repositories;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Tests.Architecture
{
    public class MethodNamingConventionsTest
    {
        // Should be PascalCase and can contain digits ex. ExampleOneMethod, Example2Method, GenericExampleMethod`1 
        const string MethodNameRegex = @"^([A-Z]([a-z]|\d)*)+(`\d+)?$";

        private void MethodNamesShouldBeInPascalCase(Assembly assembly)
        {
            var result = Utils.GetApplicationsClassess(assembly)
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => !Regex.IsMatch(m.Name, MethodNameRegex))
                .Select(m => m.DeclaringType.FullName + " > " + m.Name)
                .ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Persistence_MethodNamesShouldBeInPascalCase()
        {
            MethodNamesShouldBeInPascalCase(typeof(IDriverRepository).Assembly);
        }
    }
}
