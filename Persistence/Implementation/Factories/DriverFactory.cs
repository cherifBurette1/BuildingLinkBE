using Application.Contracts.Factories;
using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Features.Drivers.Queries.GetAllDriversSortedNames;
using Application.Features.Drivers.Queries.GetDriver;
using Domain.Entities;

namespace Persistence.Implementation.Factories.Implementations
{
    public class DriverFactory : IDriverFactory
    {
        public GetAllDriversQueryResponse GetAllDriversQueryResponse(Driver driver)
        {
            return new GetAllDriversQueryResponse
            {
                Id = driver.Id,
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                PhoneNumber = driver.PhoneNumber
            };
        }
        public GetAllDriversSortedNamesQueryResponse GetAllDriversSortedNamesQueryResponse(GetAllDriversQueryResponse driver)
        {
            return new GetAllDriversSortedNamesQueryResponse
            {
                Email = driver.Email,
                FirstName = new string(driver.FirstName.OrderBy(firstName => firstName).ToArray()),
                LastName = new string(driver.LastName.OrderBy(lastName => lastName).ToArray()),
                Id = driver.Id,
                PhoneNumber = driver.PhoneNumber
            };
        }
        public GetDriverQueryResponse GetDriverQueryResponse(Driver driver)
        {
            return new GetDriverQueryResponse
            {
                Id = driver.Id,
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                PhoneNumber = driver.PhoneNumber
            };
        }
    }
}
