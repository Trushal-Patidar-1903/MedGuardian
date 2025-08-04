using DTO.Common.Response;
using DTO.Masters.Country.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Masters.Country;

namespace MedGuardianWebApi.Controllers.Masters.Country
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _iCountryService;

        public CountryController(ICountryService _iCountryService)
        {
            this._iCountryService = _iCountryService;
        }

        /// <summary>
        /// Retrieves the list of all countries for dropdown binding.
        /// </summary>
        /// <returns>A response containing the list of countries.</returns>
        [AllowAnonymous]
        [HttpGet("get-all-countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            var result = await _iCountryService.GetAllCountries();

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<CountryDropdownModel>>
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
                    message = result?.message ?? "Country list not found.",
                    exception = result?.exception,
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }
    }
}
