using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Commands.CreateDriver
{
    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, ApiResponse<CreateDriverCommandResponse>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IDriverFactory _driverFactory;
        public CreateDriverCommandHandler(IDriverRepository driverRepository, IDriverFactory driverFactory)
        {
            _driverRepository = driverRepository;
            _driverFactory = driverFactory;

        }

        public async Task<ApiResponse<CreateDriverCommandResponse>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var createdDriverId = await _driverRepository.CreateDriver(request);

            return ApiResponse<CreateDriverCommandResponse>.GetSuccessApiResponse(
                new CreateDriverCommandResponse
                {
                    Id = createdDriverId
                });
        }
    }
}
