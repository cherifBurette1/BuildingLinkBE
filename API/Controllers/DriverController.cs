using Application.Features.Drivers.Commands.CreateDriver;
using Application.Features.Drivers.Commands.DeleteDriver;
using Application.Features.Drivers.Commands.EditDriver;
using Application.Features.Drivers.Queries.GetAllDrivers;
using Application.Features.Drivers.Queries.GetAllDriversSortedNames;
using Application.Features.Drivers.Queries.GetDriver;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DriverController : BaseController
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateDriverCommandResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<CreateDriverCommandResponse>> CreateDriver(CreateDriverCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(EditDriverCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status304NotModified)]

        public async Task<ActionResult<EditDriverCommandResponse>> EditDriver(EditDriverCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }

        [HttpGet("drivers-list")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDriverQueryResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetAllDriversQueryResponse>>> GetAllDriver([FromQuery] GetAllDriversQuery query)
        {
            var result = await _mediator.Send(query);

            return GetApiResponse(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDriverQueryResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetDriverQueryResponse>> GetDriver([FromQuery] GetDriverQuery query)
        {
            var result = await _mediator.Send(query);

            return GetApiResponse(result);
        }

        [HttpGet("sorted-names")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDriverQueryResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllDriversSortedNamesQueryResponse>> GetDriversSortedNames()
        {
            var result = await _mediator.Send(new GetAllDriversSortedNamesQuery());

            return GetApiResponse(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status304NotModified)]
        public async Task<ActionResult> DeleteDriver(DeleteDriverCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }
    }
}
