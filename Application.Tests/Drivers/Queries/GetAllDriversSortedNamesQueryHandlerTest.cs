using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Features.Drivers.Queries.GetAllDriversSortedNames;
using Application.Response;
using Domain.Entities;
using MediatR;
using Moq;
using Persistence.Implementation.Factories.Implementations;
using Shouldly;

namespace Application.Tests.Drivers.Queries
{
    public class GetAllDriversSortedNamesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsSortedApiResponse()
        {
            // Arrange
            var drivers = new List<Driver>
            {
                new Driver { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe"   ,Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" },
                new Driver { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith",Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" },
                new Driver { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Johnson",Email = "sherif@gmail.com", PhoneNumber = "123-456-7890" }
            };

            var driverFactory = new DriverFactory();

            var sortedDrivers = drivers
                .Select(driverFactory.GetAllDriversQueryResponse)
                .Select(driverFactory.GetAllDriversSortedNamesQueryResponse)
                .ToList();

            var mediatorMock = new Mock<IMediator>();

            var handler = new GetAllDriversSortedNamesQueryHandler(mediatorMock.Object, driverFactory);

            var query = new GetAllDriversSortedNamesQuery();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllDriversQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(ApiResponse<List<GetAllDriversQueryResponse>>
                        .GetSuccessApiResponse(drivers.Select(a => new GetAllDriversQueryResponse
                        {
                            Email = a.Email,
                            FirstName = a.FirstName,
                            Id = a.Id,
                            LastName = a.LastName,
                            PhoneNumber = a.PhoneNumber
                        })
                        .ToList()));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccessStatusCode.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBe(sortedDrivers.Count);
            ResultShouldMatch(sortedDrivers, result);
        }

        private static void ResultShouldMatch(List<GetAllDriversSortedNamesQueryResponse> sortedDrivers, ApiResponse<List<GetAllDriversSortedNamesQueryResponse>> result)
        {
            for (int i = 0; i < sortedDrivers.Count; i++)
            {
                var isMatch = IsMatch(sortedDrivers[i], result.Data[i]);
                isMatch.ShouldBeTrue();
            }
        }

        private static bool IsMatch(GetAllDriversSortedNamesQueryResponse expectedDriver, GetAllDriversSortedNamesQueryResponse resultDriver)
        {
            return expectedDriver.Email == resultDriver.Email
                                && expectedDriver.FirstName == resultDriver.FirstName
                                && expectedDriver.LastName == resultDriver.LastName
                                && expectedDriver.Id == resultDriver.Id
                                && expectedDriver.PhoneNumber == resultDriver.PhoneNumber;
        }
    }
}
