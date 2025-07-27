using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Helper.Cryptography;
using Helper.SharedResource.Interface.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface.User;

namespace MedGuardianWebApi.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;
        private readonly IJwtTokenServices _iJwtTokenServices;
        public UserController(IUserService _iUserService, IJwtTokenServices _iJwtTokenServices)
        {
            this._iUserService = _iUserService;
            this._iJwtTokenServices = _iJwtTokenServices;
        }

        #region GetAllUsers

        /// <summary>
        /// Retrieves the list of all users.
        /// </summary>
        /// <returns>A response containing the list of all users with accessible file URLs.</returns>
        [AllowAnonymous]
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _iUserService.GetAllUsers();

            if (result is not null && result.status && result.data != null)
            {
                // Construct the base URL for file endpoints
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                // Loop through each user and transform the physical paths into URLs
                foreach (var user in result.data)
                {
                    // If a profile picture path exists, replace it with the API URL
                    if (!string.IsNullOrEmpty(user.profilePicturePath))
                    {
                        user.profilePicturePath = $"{baseUrl}/api/File/user-profile-picture/{user.idUser}";
                    }

                    // If a QR code path exists, replace it with the API URL
                    if (!string.IsNullOrEmpty(user.qrCode))
                    {
                        // Assuming you will create a 'user-qr-code' endpoint similar to the profile picture one
                        user.qrCode = $"{baseUrl}/api/File/user-qr-code/{user.idUser}";
                    }
                }

                var successResponse = new GlobalResponseModel<List<UserMasterListModel>>
                {
                    status = true,
                    statusCode = StatusCodes.Status200OK,
                    message = result.message,
                    data = result.data // The data now contains the modified URLs
                };

                return Ok(successResponse);
            }
            else
            {
                // Your existing error handling logic
                var errorResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status404NotFound,
                    message = result?.message ?? "User list not found.",
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }

        #endregion

        #region GetUserById

        /// <summary>
        /// Retrieves user details by user ID.
        /// </summary>
        /// <param name="idUser">The ID of the user to retrieve.</param>
        /// <returns>A response containing the user details with accessible file URLs.</returns>
        [AllowAnonymous]
        [HttpGet("get-user-by-id")] // Changed route for clarity
        public async Task<IActionResult> GetUserById([FromQuery] int idUser)
        {
            if (idUser <= 0)
            {
                // ... (Your existing bad request handling)
            }

            var result = await _iUserService.GetUserById(idUser);

            if (result is not null && result.status && result.data != null)
            {
                // Construct the base URL for file endpoints
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var user = result.data;

                // Transform the physical path into a full URL
                if (!string.IsNullOrEmpty(user.profilePicturePath))
                {
                    user.profilePicturePath = $"{baseUrl}/api/File/user-profile-picture/{user.idUser}";
                }
                if (!string.IsNullOrEmpty(user.qrCode))
                {
                    user.qrCode = $"{baseUrl}/api/File/user-qr-code/{user.idUser}";
                }

                var successResponse = new GlobalResponseModel<UserMasterListModel>
                {
                    status = true,
                    statusCode = StatusCodes.Status200OK,
                    message = result.message,
                    data = user // Return the modified user object
                };

                return Ok(successResponse);
            }
            else
            {
                // Your existing error handling logic
                var errorResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status404NotFound,
                    message = result?.message ?? "User not found.",
                    data = GlobalResponseModel<object>.blankArray
                };
                return Ok(errorResponse);
            }
        }

        #endregion

        #region UserType

        /// <summary>
        /// Retrieves the list of user types for dropdown binding.
        /// </summary>
        /// <returns>A response containing the list of user types.</returns>
        [AllowAnonymous]
        [HttpGet("get-all-usertype")]
        public async Task<IActionResult> GetAllUserTypes()
        {
            var result = await _iUserService.GetAllUserTypes();

            if (result is not null && result.status)
            {
                var successResponse = new GlobalResponseModel<List<UserTypeDropdownModel>>
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
                        message = result?.message ?? "User type list not found.",
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
                        message = "User type list not found.",
                        exception = null,
                        data = GlobalResponseModel<object>.blankArray
                    };

                    return Ok(notFoundResponse);
                }
            }
        }

        #endregion

        #region UserLogIn

        /// <summary>
        /// Generates a JWT token for an authenticated user.
        /// </summary>
        /// <param name="userAuthentication">The user Email and Password used to verify authentication.</param>
        /// <returns>A response containing the JWT token if the user is authenticated, or an invalid response if not.</returns>
        [AllowAnonymous]
        [HttpPost("post-user-logIn")]
        public async Task<IActionResult> UserLogIn([FromBody] UserAuthenticationModel userAuthentication)
        {
            // Validate the incoming model
            if (userAuthentication == null || string.IsNullOrWhiteSpace(userAuthentication.email) || string.IsNullOrWhiteSpace(userAuthentication.password))
            {
                var invalidRequestResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Invalid request: Missing Email or Password.",
                    exception = null,
                    data = null
                };

                return BadRequest(invalidRequestResponse);
            }

            // Get user credentials from DB
            var objUserCredentials = await _iUserService.GetUserCredentials(userAuthentication.email);

            if (objUserCredentials?.data == null || string.IsNullOrEmpty(objUserCredentials.data.password))
            {
                var invalidCredentialResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status401Unauthorized,
                    message = "Invalid email or password.",
                    exception = null,
                    data = null
                };

                return Unauthorized(invalidCredentialResponse);
            }

            var userData = objUserCredentials.data;

            // Verify the provided password
            if (!CryptographyProcessor.VerifyPassword(userAuthentication.password, userData.password))
            {
                var invalidPasswordResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status401Unauthorized,
                    message = "Invalid password.",
                    exception = null,
                    data = null
                };

                return Unauthorized(invalidPasswordResponse);
            }

            // Generate JWT token
            string jwtToken = _iJwtTokenServices.GenerateJsonWebToken(userData);

            var loginSuccessData = new
            {
                JwtToken = $"Bearer {jwtToken}",
                userData.idUser,
                userData.email,
                userData.idUserType
            };

            var successResponse = new GlobalResponseModel<object>
            {
                status = true,
                statusCode = StatusCodes.Status200OK,
                message = "Login successful.",
                exception = null,
                data = loginSuccessData
            };

            return Ok(successResponse);
        }

        #endregion

        #region RegisterOrUpdateUser

        /// <summary>
        /// Registers a new user or updates an existing user's details.
        /// </summary>
        /// <param name="userRegistration">User registration details.</param>
        /// <returns>A response containing the user ID if successful.</returns>
        [AllowAnonymous]
        [HttpPost("post-user-registeration")]
        public async Task<IActionResult> RegisterOrUpdateUser([FromForm] UserRegistrationModel userRegistration)
        {
            // Validate incoming model
            if (userRegistration == null || string.IsNullOrWhiteSpace(userRegistration.email) || string.IsNullOrWhiteSpace(userRegistration.password))
            {
                var invalidRequestResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status400BadRequest,
                    message = Helper.Common.Messages.CannotBeEmpty("User model, Email and Password", "Empty"),
                    exception = null,
                    data = GlobalResponseModel<object>.blankArray
                };

                return BadRequest(invalidRequestResponse);
            }

            // Encrypt password before saving
            userRegistration.password = CryptographyProcessor.EncryptPassword(userRegistration.password);

            // Save or update user
            var result = await _iUserService.AddEditUserDetails(userRegistration);

            if (!result.status || result.data <= 0)
            {
                var failureResponse = new GlobalResponseModel<object>
                {
                    status = false,
                    statusCode = StatusCodes.Status500InternalServerError,
                    message = "Failed to save user details.",
                    exception = null,
                    data = GlobalResponseModel<object>.blankArray
                };

                return StatusCode(StatusCodes.Status500InternalServerError, failureResponse);
            }

            var successResponse = new GlobalResponseModel<object>
            {
                status = true,
                statusCode = StatusCodes.Status200OK,
                message = "User details saved successfully.",
                exception = null,
                data = new { userId = result.data }
            };

            return Ok(successResponse);
        }

        #endregion
    }
}
