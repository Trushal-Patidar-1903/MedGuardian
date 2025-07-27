using DTO.Common.Response;
using DTO.Masters.Gender.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Masters.Gender;

namespace MedGuardianWebApi.Controllers.Masters.Gender
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        public readonly IGenderService _iGenderService;
        public GenderController(IGenderService _iGenderService)
        {
            this._iGenderService = _iGenderService;
        }

        #region Gender

        /// <summary>
        /// Retrieves the list of genders for dropdown binding.
        /// </summary>
        /// <returns>A response containing the list of genders.</returns>
        [AllowAnonymous]
        [HttpGet("get-all-gender")]
        public async Task<IActionResult> GetAllGenders()
        {
            var result = await _iGenderService.GetAllGenders();

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<GenderDropdownModel>>
                {
                    status = true,
                    statusCode = StatusCodes.Status200OK,
                    message = result.message,
                    exception = result.exception,
                    data = result.data
                };

                return Ok(successResponse);
            }
            else
            {
                if (result is not null && !result.status)
                {
                    var errorResponse = new GlobalResponseModel<object>
                    {
                        status = false,
                        statusCode = StatusCodes.Status404NotFound,
                        message = result?.message ?? "Gender list not found.",
                        exception = result?.exception,
                        data = result?.data
                    };

                    return Ok(errorResponse);
                }
                else
                {
                    var notFoundResponse = new GlobalResponseModel<object>
                    {
                        status = false,
                        statusCode = StatusCodes.Status404NotFound,
                        message = "Gender list not found.",
                        exception = null,
                        data = GlobalResponseModel<object>.blankArray
                    };

                    return Ok(notFoundResponse);
                }
            }
        }

        #endregion
    }
}
