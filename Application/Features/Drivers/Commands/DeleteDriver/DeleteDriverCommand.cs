using Application.Response;
using MediatR;

namespace Application.Features.Drivers.Commands.DeleteDriver
{
    public class DeleteDriverCommand : IRequest<ApiResponse<DeleteDriverCommandResponse>>
    {
        public Guid Id { get; set; }
    }
}
