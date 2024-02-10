using Application.Contracts.Repositories;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Commands.EditDriver
{
    public class EditDriverCommandHandler : IRequestHandler<EditDriverCommand, ApiResponse<EditDriverCommandResponse>>
    {
        private readonly IDriverRepository _driverRepository;

        public EditDriverCommandHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }
        public async Task<ApiResponse<EditDriverCommandResponse>> Handle(EditDriverCommand request, CancellationToken cancellationToken)
        {
            var isDriverExist = await _driverRepository.IsDriverExist(request.Id);
            if (!isDriverExist)
                return ApiResponse<EditDriverCommandResponse>.GetNotFoundApiResponse(
                    new List<string>()
                    {
                        "driver is not found"
                    });

            var isSuccess = await _driverRepository.UpdateDriver(request);
            if (!isSuccess)
            {
                return ApiResponse<EditDriverCommandResponse>.GetNotModifiedResponse(
                    new EditDriverCommandResponse
                    {
                        IsSuccess = false
                    });
            }

            return ApiResponse<EditDriverCommandResponse>.GetSuccessApiResponse(new EditDriverCommandResponse());
        }
    }
}
