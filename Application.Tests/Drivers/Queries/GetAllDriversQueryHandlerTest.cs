using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Response;
using Domain.Entities;
using Moq;
using Shouldly;
using System.Net;

namespace Application.Tests.Queries
{
    public class GetAllDriversQueryHandlerTests
    {

        [Fact]
        public async Task Handle_NoDrivers_ReturnsNotFoundApiResponse()
        {
            // Arrange
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var driverFactoryMock = new Mock<IDriverFactory>();
            var handler = new GetAllDriversQueryHandler(driverRepositoryMock.Object, driverFactoryMock.Object);
            var query = new GetAllDriversQuery();

            driverRepositoryMock.Setup(repo => repo.GetAllDrivers())
                                .ReturnsAsync(new List<Driver>());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeFalse(),
                () => result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound)
            );
        }

        [Fact]
        public async Task Handle_DriversExist_ReturnsSortedApiResponse()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe"   ,Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" },
                new Driver { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith",Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" },
                new Driver { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Johnson",Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" }
            };
            var sortedDrivers = drivers.Select(a => new GetAllDriversQueryResponse
            {
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Id = a.Id,
                PhoneNumber = a.PhoneNumber
            })
            .OrderBy(d => d.FirstName)
            .ThenBy(d => d.LastName)
            .ToList();

            var driverRepositoryMock = new Mock<IDriverRepository>();
            var driverFactoryMock = new Mock<IDriverFactory>();
            var handler = new GetAllDriversQueryHandler(driverRepositoryMock.Object, driverFactoryMock.Object);
            var query = new GetAllDriversQuery { AlphabetizedSort = true };

            driverRepositoryMock.Setup(repo => repo.GetAllDrivers())
                                .ReturnsAsync(drivers);
            driverFactoryMock.Setup(factory => factory.GetAllDriversQueryResponse(It.IsAny<Driver>()))
                             .Returns<Driver>(driver => new GetAllDriversQueryResponse { Id = driver.Id, FirstName = driver.FirstName, LastName = driver.LastName, PhoneNumber = driver.PhoneNumber, Email = driver.Email });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert

            result.Data.ShouldNotBeNull();
            result.IsSuccessStatusCode.ShouldBeTrue();
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Data.Count.ShouldBe(sortedDrivers.Count);
            ResultShouldMatch(sortedDrivers, result);
        }

        private static void ResultShouldMatch(List<GetAllDriversQueryResponse> sortedDrivers, ApiResponse<List<GetAllDriversQueryResponse>> result)
        {
            for (int i = 0; i < sortedDrivers.Count; i++)
            {
                var isMatch = IsMatch(sortedDrivers[i], result.Data[i]);
                isMatch.ShouldBeTrue();
            }
        }

        private static bool IsMatch(GetAllDriversQueryResponse expectedDriver, GetAllDriversQueryResponse resultDriver)
        {
            return expectedDriver.Email == resultDriver.Email
                                && expectedDriver.FirstName == resultDriver.FirstName
                                && expectedDriver.LastName == resultDriver.LastName
                                && expectedDriver.Id == resultDriver.Id
                                && expectedDriver.PhoneNumber == resultDriver.PhoneNumber;
        }
    }
}
