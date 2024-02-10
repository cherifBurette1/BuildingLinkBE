using Application.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Drivers.Queries.GetDriver
{
    public class GetDriverQuery : IRequest<ApiResponse<GetDriverQueryResponse>>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
