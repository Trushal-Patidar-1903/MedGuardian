using DTO.Common.Response;
using DTO.Masters.State.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Masters.State;

namespace MedGuardianWebApi.Controllers.Masters.State
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _iStateService;

        public StateController(IStateService _iStateService)
        {
            this._iStateService = _iStateService;
        }

        /// <summary>
        /// Retrieves the list of states for a given country ID.
        /// </summary>
        /// <param name="countryId">The ID of the country.</param>
        /// <returns>A response containing the list of states.</returns>
        [AllowAnonymous]
        [HttpGet("get-states-by-country/{countryId:int}")]
        public async Task<IActionResult> GetStatesByCountryId(int countryId)
        {
            var result = await _iStateService.GetStatesByCountryId(countryId);

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<StateDropdownModel>>
                {
                    status = true,
                    statusCode = StatusCodes.Status200OK,
                    message = result.message,
                    data = result.data
                };
                return Ok(successResponse);
            }
            else
            {
                var errorResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status404NotFound,
                    message = result?.message ?? "State list not found for the given country.",
                    exception = result?.exception,
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }
    }
}
