using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Queries.GetAllDrivers
{
    internal class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, ApiResponse<List<GetAllDriversQueryResponse>>>
    {
        private readonly IDriverRepository _diverRepository;
        private readonly IDriverFactory _driverFactory;

        public GetAllDriversQueryHandler(IDriverRepository diverRepository, IDriverFactory driverFactory)
        {
            _diverRepository = diverRepository;
            _driverFactory = driverFactory;
        }

        public async Task<ApiResponse<List<GetAllDriversQueryResponse>>> Handle(GetAllDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _diverRepository.GetAllDrivers();
            if (!drivers.Any())
            {
                return ApiResponse<List<GetAllDriversQueryResponse>>.GetNotFoundApiResponse();
            }

            var dto = drivers.Select(driver => _driverFactory.GetAllDriversQueryResponse(driver));
            var sortedDto = request.AlphabetizedSort ? dto.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList() : dto.ToList();

            return ApiResponse<List<GetAllDriversQueryResponse>>.GetSuccessApiResponse(sortedDto);

        }
    }
}
