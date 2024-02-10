using Application.Contracts.Repositories;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Tests.Architecture
{
    public class ClassPrivateVariablesNamingConventionsTest
    {
        const string ClassPrivateVariablesNameRegex = @"^_[a-z]+([A-Z]([a-z]|\d)*)*(`\d+)?$";

        private void ClassPrivateVariablesNamesShouldBeInCamelCase(Assembly assembly)
        {
            var result = Utils.GetApplicationsClassess(assembly)
                .SelectMany(t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                .Where(m => m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.IsPrivate)
                .Where(v => !Regex.IsMatch(v.Name, ClassPrivateVariablesNameRegex))
                .Select(p => p.DeclaringType.FullName + " > " + p.Name)
                .ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Persistence_ClassPrivateVariablesNamesShouldBeInCamelCase()
        {
            ClassPrivateVariablesNamesShouldBeInCamelCase(typeof(IDriverRepository).Assembly);
        }
    }
}
