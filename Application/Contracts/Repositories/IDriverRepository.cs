using Application.Features.Drivers.Commands.CreateDriver;
using Application.Features.Drivers.Commands.EditDriver;
using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IDriverRepository
    {
        Task<Guid> CreateDriver(CreateDriverCommand driver);
        Task<bool> UpdateDriver(EditDriverCommand driver);
        Task<List<Driver>> GetAllDrivers();
        Task<Driver> GetDriver(Guid Id);
        Task<bool> DeleteDriver(Guid id);
        Task<bool> IsDriverExist(Guid Id);
    }
}
