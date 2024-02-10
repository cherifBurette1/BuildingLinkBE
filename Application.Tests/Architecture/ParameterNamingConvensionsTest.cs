using Application.Contracts.Repositories;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Tests.Architecture
{
    public class ParameterNamingConventionsTest
    {
        // Should be camelCase and can contain digits ex. exampleOneParam, exampleOneParam
        const string ParameterNameRegex = @"^[a-z]+([A-Z]([a-z]|\d)*)*(`\d+)?$";

        private void ParameterNamesShouldBeInCamelCase(Assembly assembly)
        {
            var result = Utils.GetApplicationsClassess(assembly)
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Length == 0)
                .Where(m => !m.IsSpecialName)
                .SelectMany(m => m.GetParameters())
                .Where(p => !Regex.IsMatch(p.Name, ParameterNameRegex))
                .Select(p => p.Member.DeclaringType.FullName + " > " + p.Member.Name + " > " + p.Name)
                .ToList();

            Assert.Empty(result);
        }
        [Fact]
        public void Persistence_ParameterNamesShouldBeInCamelCase()
        {
            ParameterNamesShouldBeInCamelCase(typeof(IDriverRepository).Assembly);
        }
    }
}
