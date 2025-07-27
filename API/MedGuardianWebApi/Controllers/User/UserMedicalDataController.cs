using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Helper.SharedResource.Interface.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.User;
using Services.Service.User;

namespace MedGuardianWebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMedicalDataController : ControllerBase
    {
        private readonly IUserMedicalDataService _iUserMedicalDataService;
        public UserMedicalDataController(IUserMedicalDataService _iUserMedicalDataService)
        {
            this._iUserMedicalDataService = _iUserMedicalDataService;
        }

        /// <summary>
        /// Retrieves a list of all user medical data records.
        /// </summary>
        /// <returns>A response containing the list of user medical data.</returns>
        [AllowAnonymous] // Or your preferred authorization
        [HttpGet("get-all-user-medical-data")]
        public async Task<IActionResult> GetAllUserMedicalData()
        {
            var result = await _iUserMedicalDataService.GetAllUserMedicalData();

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<UserMedicalDataListModel>>
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
                    message = result?.message ?? "User medical data list not found.",
                    exception = result?.exception,
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }

        /// <summary>
        /// Retrieves a specific user's medical data by its ID.
        /// </summary>
        /// <param name="id">The ID of the user medical data record.</param>
        /// <returns>A response containing the detailed user medical profile.</returns>
        [AllowAnonymous] // Or your preferred authorization
        [HttpGet("get-user-medical-data-by-id/{id:int}")]
        public async Task<IActionResult> GetUserMedicalDataById(int id)
        {
            var result = await _iUserMedicalDataService.GetUserMedicalDataById(id);

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<UserMedicalDataViewModel>
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
                    message = result?.message ?? $"User medical data not found for ID: {id}.",
                    exception = result?.exception,
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }

        #region AddOrUpdateUserMedicalData
        /// <summary>
        /// Adds or updates user medical data including contacts, allergies, histories.
        /// </summary>
        /// <param name="userMedicalData">User medical data with related details.</param>
        /// <returns>A response containing the medical data ID if successful.</returns>
        [HttpPost("post-user-medical-data")]
        public async Task<IActionResult> AddOrUpdateUserMedicalData([FromBody] UserMedicalDataModel userMedicalData)
        {
            // Validate the input model
            if (userMedicalData == null || userMedicalData.idUser <= 0)
            {
                var invalidRequestResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status400BadRequest,
                    message = Helper.Common.Messages.CannotBeEmpty("UserMedicalData model or UserId", "Empty"),
                    exception = null,
                    data = GlobalResponseModel<object>.blankArray
                };

                return BadRequest(invalidRequestResponse);
            }

            // Call the repository/service layer
            var result = await _iUserMedicalDataService.AddOrUpdateUserMedicalData(userMedicalData);

            if (!result.status || result.data <= 0)
            {
                var failureResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status500InternalServerError,
                    message = "Failed to save user medical data.",
                    exception = null,
                    data = GlobalResponseModel<object>.blankArray
                };

                return StatusCode(StatusCodes.Status500InternalServerError, failureResponse);
            }

            var successResponse = new GlobalResponseModel<object>
            {
                status = true,
                statusCode = StatusCodes.Status200OK,
                message = "User medical data saved successfully.",
                exception = null,
                data = new { idUserMedicalData = result.data }
            };

            return Ok(successResponse);
        }
        #endregion
    }
}
