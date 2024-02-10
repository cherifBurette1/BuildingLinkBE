using Application.Contracts.Repositories;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Commands.DeleteDriver
{
    public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, ApiResponse<DeleteDriverCommandResponse>>
    {
        private readonly IDriverRepository _driverRepository;

        public DeleteDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<ApiResponse<DeleteDriverCommandResponse>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var isDriverExist = await _driverRepository.IsDriverExist(request.Id);
            if (!isDriverExist)
                return ApiResponse<DeleteDriverCommandResponse>.GetNotFoundApiResponse(
                    new List<string>()
                    {
                        "driver is not found"
                    });

            var isSuccess = await _driverRepository.DeleteDriver(request.Id);
            if (!isSuccess)
            {
                return ApiResponse<DeleteDriverCommandResponse>.GetNotModifiedResponse(
                    new DeleteDriverCommandResponse
                    {
                        IsSuccess = false
                    });
            }

            return ApiResponse<DeleteDriverCommandResponse>.GetSuccessApiResponse(
                new DeleteDriverCommandResponse { IsSuccess = true });
        }
    }
}
