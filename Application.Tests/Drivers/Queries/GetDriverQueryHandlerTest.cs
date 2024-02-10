using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Features.Drivers.Queries.GetDriver;
using Domain.Entities;
using Moq;
using Persistence.Implementation.Factories.Implementations;
using Shouldly;

namespace Application.Tests.Drivers.Queries
{
    public class GetDriverQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingDriver_ReturnsSuccessApiResponse()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var driverRepositoryMock = new Mock<IDriverRepository>();

            var handler = new GetDriverQueryHandler(driverRepositoryMock.Object, new DriverFactory());

            var query = new GetDriverQuery { Id = driverId };

            var expectedDriver = new Driver { Id = driverId, Email = "test@example.com", FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" };

            driverRepositoryMock.Setup(repo => repo.IsDriverExist(driverId))
                                .ReturnsAsync(true);

            driverRepositoryMock.Setup(repo => repo.GetDriver(driverId))
                                .ReturnsAsync(expectedDriver);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeTrue(),
                () => result.Data.ShouldNotBeNull(),
                () => result.Data.Id.ShouldBe(expectedDriver.Id),
                () => result.Data.Email.ShouldBe(expectedDriver.Email),
                () => result.Data.FirstName.ShouldBe(expectedDriver.FirstName),
                () => result.Data.LastName.ShouldBe(expectedDriver.LastName),
                () => result.Data.PhoneNumber.ShouldBe(expectedDriver.PhoneNumber)
            );
        }

        [Fact]
        public async Task Handle_NonExistingDriver_ReturnsBadRequestApiResponse()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var driverFactoryMock = new Mock<IDriverFactory>();

            driverRepositoryMock.Setup(repo => repo.IsDriverExist(driverId))
                    .ReturnsAsync(false);

            driverFactoryMock.Setup(factory => factory.GetAllDriversQueryResponse(It.IsAny<Driver>()))
                .Returns((Driver a) => new Features.Drivers.Queries.GetAllDrivers.GetAllDriversQueryResponse
                {
                    Email = a.Email,
                    FirstName = a.FirstName,
                    Id = a.Id,
                    LastName = a.LastName,
                    PhoneNumber = a.PhoneNumber,
                });

            var handler = new GetDriverQueryHandler(driverRepositoryMock.Object, driverFactoryMock.Object);

            var query = new GetDriverQuery { Id = driverId };



            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeFalse(),
                () => result.Errors.ShouldContain("driver is not found")
            );
        }
    }
}
