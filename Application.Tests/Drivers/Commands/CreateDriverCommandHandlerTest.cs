using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Features.Drivers.Commands.CreateDriver;
using Moq;
using Shouldly;

namespace Application.UnitTests.Features.Drivers.Commands
{
    public class CreateDriverCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessApiResponse()
        {
            // Arrange
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var driverFactoryMock = new Mock<IDriverFactory>();

            var expectedId = Guid.NewGuid();
            driverRepositoryMock.Setup(repo => repo.CreateDriver(It.IsAny<CreateDriverCommand>()))
                                .ReturnsAsync(expectedId);

            var handler = new CreateDriverCommandHandler(driverRepositoryMock.Object, driverFactoryMock.Object);
            var command = new CreateDriverCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeTrue(),
                () => result.Data.ShouldNotBeNull(),
                () => result.Data?.Id.ShouldBe(expectedId)
            );
        }
        [Fact]
        public async Task Handle_ValidRequest_CallsRepositoryCreateDriverOnce()
        {
            // Arrange
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var driverFactoryMock = new Mock<IDriverFactory>();

            driverRepositoryMock.Setup(repo => repo.CreateDriver(It.IsAny<CreateDriverCommand>()))
                                .ReturnsAsync(Guid.NewGuid());

            var handler = new CreateDriverCommandHandler(driverRepositoryMock.Object, driverFactoryMock.Object);
            var command = new CreateDriverCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            driverRepositoryMock.Verify(repo => repo.CreateDriver(It.IsAny<CreateDriverCommand>()), Times.Once);
        }
    }
}
