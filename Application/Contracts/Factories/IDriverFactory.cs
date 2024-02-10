using Application.Features.Drivers.Commands.CreateDriver;
using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Features.Drivers.Queries.GetAllDriversSortedNames;
using Application.Features.Drivers.Queries.GetDriver;
using Domain.Entities;

namespace Application.Contracts.Factories
{
    public interface IDriverFactory
    {
        GetAllDriversQueryResponse GetAllDriversQueryResponse(Driver driver);
        GetAllDriversSortedNamesQueryResponse GetAllDriversSortedNamesQueryResponse(GetAllDriversQueryResponse driver);
        GetDriverQueryResponse GetDriverQueryResponse(Driver driver);
    }
}
