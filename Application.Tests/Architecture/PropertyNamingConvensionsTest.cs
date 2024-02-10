using Application.Contracts.Repositories;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Tests.Architecture
{
    public class PropertyNamingConventionsTest
    {
        // Should be PascalCase and can contain digits ex. ExampleOneMethod, Example2Method, GenericExampleMethod`1 
        const string PropertyNameRegex = @"^([A-Z]([a-z]|\d|_)*)+(`\d+)?$";

        private void PropertyNamesShouldBeInPascalCase(Assembly assembly)
        {
            var result = Utils.GetApplicationsClassess(assembly)
                .SelectMany(t => t.GetProperties())
                .Where(m => !Regex.IsMatch(m.Name, PropertyNameRegex))
                .Select(m => m.DeclaringType.FullName + " > " + m.Name)
                .ToList();

            Assert.Empty(result);
        }
        [Fact]
        public void Persistence_PropertyNamesShouldBeInPascalCase()
        {
            PropertyNamesShouldBeInPascalCase(typeof(IDriverRepository).Assembly);
        }
    }
}
