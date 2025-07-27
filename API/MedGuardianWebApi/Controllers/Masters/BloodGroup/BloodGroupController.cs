using DTO.Common.Response;
using DTO.Masters.BloodGroup.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.Masters.BloodGroup;

namespace MedGuardianWebApi.Controllers.Masters.BloodGroup
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodGroupController : ControllerBase
    {
        private readonly IBloodGroupService _iBloodGroupService;

        public BloodGroupController(IBloodGroupService _iBloodGroupService)
        {
            this._iBloodGroupService = _iBloodGroupService;
        }

        #region BloodGroup

        /// <summary>
        /// Retrieves the list of blood groups for dropdown binding.
        /// </summary>
        /// <returns>A response containing the list of blood groups.</returns>
        [AllowAnonymous]
        [HttpGet("get-all-bloodgroup")]
        public async Task<IActionResult> GetAllBloodGroupsAsync()
        {
            var result = await _iBloodGroupService.GetAllBloodGroups();

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<BloodGroupDropdownModel>>
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
                        message = result?.message ?? "Blood group list not found.",
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
                        message = "Blood group list not found.",
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
