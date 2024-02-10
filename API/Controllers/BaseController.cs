using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status500InternalServerError)]
    public class BaseController : ControllerBase
    {
        protected ActionResult GetApiResponse<T>(ApiResponse<T> obj) where T : class
        {
            if (obj.IsSuccessStatusCode && obj.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                return StatusCode(obj.StatusCode);

            if (obj.IsSuccessStatusCode)
                return StatusCode(obj.StatusCode, obj.Data);

            if (!obj.IsSuccessStatusCode && obj.HttpStatusCode == System.Net.HttpStatusCode.Conflict)
                return StatusCode(obj.StatusCode, obj.Data);

            return StatusCode(obj.StatusCode, obj.Errors);
        }
    }
}
