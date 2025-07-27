using DTO.Common.Response;
using DTO.Masters.City.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Masters.BloodGroup;
using Services.Interface.Masters.City;

namespace MedGuardianWebApi.Controllers.Masters.City
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _iCityService;

        public CityController(ICityService _iCityService)
        {
            this._iCityService = _iCityService;
        }

        /// <summary>
        /// Retrieves the list of cities for a given state ID.
        /// </summary>
        /// <param name="stateId">The ID of the state.</param>
        /// <returns>A response containing the list of cities.</returns>
        [AllowAnonymous]
        [HttpGet("get-cities-by-state/{stateId:int}")]
        public async Task<IActionResult> GetCitiesByStateIdAsync(int stateId)
        {
            var result = await _iCityService.GetCitiesByStateId(stateId);

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<CityDropdownModel>>
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
                    message = result?.message ?? "City list not found for the given state.",
                    exception = result?.exception,
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }
    }
}
