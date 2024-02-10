using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Queries.GetDriver
{
    public class GetDriverQueryHandler : IRequestHandler<GetDriverQuery, ApiResponse<GetDriverQueryResponse>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IDriverFactory _driverFactory;

        public GetDriverQueryHandler(IDriverRepository driverRepository, IDriverFactory driverFactory)
        {
            _driverRepository = driverRepository;
            _driverFactory = driverFactory;
        }

        public async Task<ApiResponse<GetDriverQueryResponse>> Handle(GetDriverQuery request, CancellationToken cancellationToken)
        {
            var isDriverExist = await _driverRepository.IsDriverExist(request.Id);
            if (!isDriverExist)
                return ApiResponse<GetDriverQueryResponse>.GetNotFoundApiResponse(
                    new List<string>()
                    {
                        "driver is not found"
                    });

            var driver = await _driverRepository.GetDriver(request.Id);
            var dto = _driverFactory.GetDriverQueryResponse(driver);

            return ApiResponse<GetDriverQueryResponse>.GetSuccessApiResponse(dto);
        }
    }
}
