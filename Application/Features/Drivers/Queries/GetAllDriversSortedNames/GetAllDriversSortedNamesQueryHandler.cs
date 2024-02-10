using Application.Contracts.Factories;
using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Response;
using MediatR;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Application.Tests")]
namespace Application.Features.Drivers.Queries.GetAllDriversSortedNames
{
    internal class GetAllDriversSortedNamesQueryHandler : IRequestHandler<GetAllDriversSortedNamesQuery, ApiResponse<List<GetAllDriversSortedNamesQueryResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly IDriverFactory _driverFactory;

        public GetAllDriversSortedNamesQueryHandler(IMediator mediator, IDriverFactory driverFactory)
        {
            _mediator = mediator;
            _driverFactory = driverFactory;
        }

        public async Task<ApiResponse<List<GetAllDriversSortedNamesQueryResponse>>> Handle(GetAllDriversSortedNamesQuery request, CancellationToken cancellationToken)
        {
            var sortedDriversResult = await _mediator.Send(new GetAllDriversQuery { AlphabetizedSort = true });

            if (!sortedDriversResult.IsSuccessStatusCode)
                return ApiResponse<List<GetAllDriversSortedNamesQueryResponse>>
                .GetNotFoundApiResponse();

            var dto = sortedDriversResult.Data.Select(a =>
            _driverFactory.GetAllDriversSortedNamesQueryResponse(a)).ToList();

            return ApiResponse<List<GetAllDriversSortedNamesQueryResponse>>
                .GetSuccessApiResponse(dto);

        }
    }
}
