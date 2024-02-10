using Application.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Drivers.Commands.CreateDriver
{
    public class CreateDriverCommand : IRequest<ApiResponse<CreateDriverCommandResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
