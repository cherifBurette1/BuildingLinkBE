using Application.Contracts.Repositories;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Tests.Architecture
{
    public class ClassNamingConventionsTest
    {
        const string ClassNameRegex = @"^([A-Z]([a-z]|\d)*)+(`\d+)?$";

        private void ClassNamesShouldBeInPascalCase(Assembly assembly)
        {
            var result = Utils.GetApplicationsClassess(assembly)
                .Where(t => !Regex.IsMatch(t.Name, ClassNameRegex))
                .Select(t => t.FullName)
                .ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Persistence_ClassNamesShouldBeInPascalCase()
        {
            ClassNamesShouldBeInPascalCase(typeof(IDriverRepository).Assembly);
        }

    }
}
