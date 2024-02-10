using Application.Response;
using MediatR;

namespace Application.Features.Drivers.Queries.GetAllDriversSortedNames
{
    public class GetAllDriversSortedNamesQuery : IRequest<ApiResponse<List<GetAllDriversSortedNamesQueryResponse>>>
    {
    }
}
