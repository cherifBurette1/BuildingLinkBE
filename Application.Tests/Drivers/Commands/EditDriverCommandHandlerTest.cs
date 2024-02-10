using Application.Contracts.Repositories;
using Application.Features.Drivers.Commands.EditDriver;
using Moq;
using Shouldly;
using System.Net;

namespace Application.Tests.Drivers.Commands
{
    public class EditDriverCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingDriver_ReturnsSuccessApiResponse()
        {
            // Arrange
            var command = new EditDriverCommand { Id = Guid.NewGuid() };

            var driverRepositoryMock = new Mock<IDriverRepository>();

            driverRepositoryMock.Setup(repo => repo.IsDriverExist(command.Id))
                                .ReturnsAsync(true);

            driverRepositoryMock.Setup(repo => repo.UpdateDriver(command))
                                .ReturnsAsync(true);

            var handler = new EditDriverCommandHandler(driverRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeTrue(),
                () => result.Data.ShouldNotBeNull()
            );
        }

        [Fact]
        public async Task Handle_NonExistingDriver_ReturnsNotFoundApiResponse()
        {
            // Arrange
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var handler = new EditDriverCommandHandler(driverRepositoryMock.Object);
            var command = new EditDriverCommand { Id = Guid.NewGuid() };

            driverRepositoryMock.Setup(repo => repo.IsDriverExist(command.Id))
                                .ReturnsAsync(false);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeFalse(),
                () => result.StatusCode.ShouldBe((int)HttpStatusCode.NotFound),
                () => result.Errors.ShouldContain("driver is not found")
            );
        }

        [Fact]
        public async Task Handle_RepositoryFailure_ReturnsNotModifiedApiResponse()
        {
            // Arrange
            var driverRepositoryMock = new Mock<IDriverRepository>();
            var handler = new EditDriverCommandHandler(driverRepositoryMock.Object);
            var command = new EditDriverCommand { Id = Guid.NewGuid() };

            driverRepositoryMock.Setup(repo => repo.IsDriverExist(command.Id))
                                .ReturnsAsync(true);
            driverRepositoryMock.Setup(repo => repo.UpdateDriver(command))
                                .ReturnsAsync(false);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsSuccessStatusCode.ShouldBeFalse(),
                () => result.Data.ShouldNotBeNull(),
                () => result.StatusCode.ShouldBe((int)HttpStatusCode.NotModified),
                () => result.Data?.IsSuccess.ShouldBeFalse()
            );
        }
    }
}
