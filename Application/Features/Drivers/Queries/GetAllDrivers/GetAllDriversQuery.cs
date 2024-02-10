using Application.Response;
using MediatR;

namespace Application.Features.Drivers.Queries.GetAllDrivers
{
    public class GetAllDriversQuery : IRequest<ApiResponse<List<GetAllDriversQueryResponse>>>
    {
        public bool AlphabetizedSort { get; set; } = false;
    }
}
