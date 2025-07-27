using DTO.Common.Response;
using Helper.SharedResource.Interface.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Configurations.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _RequestDelegate;
        private readonly IServiceScopeFactory _iServiceScopeFactory;
        private readonly IUtilityServices _iUtilityServices;

        public AuthenticationMiddleware(RequestDelegate _RequestDelegate, IServiceScopeFactory _iServiceScopeFactory, IUtilityServices _iUtilityServices)
        {
            this._RequestDelegate = _RequestDelegate;
            this._iServiceScopeFactory = _iServiceScopeFactory;
            this._iUtilityServices = _iUtilityServices;
        }

        #region Invoke Method

        /// <summary>
        /// Middleware invocation method to handle request logging and token validation.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            // Bypass middleware if the request comes from specific domains
            var referer = httpContext.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                var allowedDomains = new HashSet<string>
                {
                    "https://test-brmdemo.simpliworks.co.in",
                    "https://preprod-brmdemo.simpliworks.co.in",
                    "https://brmdemo.simpliworks.co.in"
                };

                if (allowedDomains.Any(domain => referer.StartsWith(domain)))
                {
                    await _RequestDelegate(httpContext);
                    return;
                }
            }

            // Bypass middleware for Swagger requests
            if (httpContext.Request.Path.HasValue)
            {
                //_iLogger.LogInformation("3. RequestLoggingMiddleware: httpContext.Request.Path - " + httpContext.Request.Path.Value);

                if (httpContext.Request.Path.Value.Contains("swagger"))
                {
                    await _RequestDelegate(httpContext);
                    return;
                }

                // Bypass middleware for Swagger requests
                if (httpContext.Request.Path.StartsWithSegments("/swagger") || httpContext.Request.Path.StartsWithSegments("/swagger/index.html") || httpContext.Request.Path.StartsWithSegments("/swagger/v1/swagger.json"))
                {
                    await _RequestDelegate(httpContext);
                    return;
                }
            }

            //if (httpContext.Request.Host.HasValue && httpContext.Request.Host.Value.Contains("localhost"))
            //{
            //    await _RequestDelegate(httpContext);
            //    return;
            //}

            // Check if the current request should bypass middleware
            if (await ShouldBypassMiddlewareAsync(httpContext))
            {
                await _RequestDelegate(httpContext);
                return;
            }

            // Get token from Authorization header
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //if (!string.IsNullOrEmpty(token))
            //{
            //    _iLogger.LogInformation("4. RequestLoggingMiddleware: token - " + token);
            //}

            if (string.IsNullOrEmpty(token))
            {
                await WriteResponseAsync(httpContext, StatusCodes.Status401Unauthorized, "Authorization token is missing.");
                return;
            }

            try
            {
                if (!ValidateToken(token, out var userId))
                {
                    await WriteResponseAsync(httpContext, StatusCodes.Status403Forbidden, "Access denied: Invalid or expired token.");
                    return;
                }

                //using var scope = _iServiceScopeFactory.CreateScope();
                //var userSessionService = scope.ServiceProvider.GetRequiredService<IUserSessionService>();
                //var userSession = await userSessionService.GetUserSessionByJwtToken(token);

                //if (userSession is null && userSession.Count <= 0)
                //{
                //    await WriteResponseAsync(httpContext, StatusCodes.Status403Forbidden, "Access denied: User session is invalid or expired.");
                //    return;
                //}

                // Add userId to HttpContext for downstream use
                httpContext.Items["userId"] = userId;
            }
            catch (Exception ex)
            {
                AddEditResponseModel<object> responseException = _iUtilityServices.CreateExceptionResponseModel<object>(ex);
                await WriteResponseAsync(httpContext, StatusCodes.Status500InternalServerError, $"Server Error: ${responseException.message}");
                return;
            }

            // Proceed to the next middleware
            await _RequestDelegate(httpContext);
        }

        #endregion Invoke Method

        #region ValidateToken Method

        /// <summary>
        /// Validates the JWT token and extracts the userId.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <param name="userId">The extracted userId.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        private static bool ValidateToken(string token, out int userId)
        {
            userId = 0;
            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
                return false;

            var jwtToken = jwtHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
                return false;

            return true;
        }

        #endregion ValidateToken Method

        #region WriteResponseAsync Method

        /// <summary>
        /// Writes a standardized JSON response to the HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="statusCode">The status code to set in the response.</param>
        /// <param name="message">The message to include in the response.</param>
        private async Task WriteResponseAsync(HttpContext context, int statusCode, string message)
        {
            var response = new GlobalResponseModel<object>
            {
                status = statusCode == StatusCodes.Status200OK ? true : false,
                statusCode = statusCode,
                message = message,
                data = GlobalResponseModel<object>.blankArray
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }

        #endregion WriteResponseAsync Method

        #region Middleware Bypass Helper

        /// <summary>
        /// Determines if the current request matches any of the specified method names for bypassing middleware.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>True if the request matches one of the specified method names, otherwise false.</returns>
        private async Task<bool> ShouldBypassMiddlewareAsync(HttpContext httpContext)
        {
            var methodNames = new List<string> {
                "PostLogIn",
                "GetAllActiveAlertList",
                "GetPlaceDetails",
                "GetRolesAndDepartmentsByUserId",
                "GetLatestVersion"
            };

            try
            {
                // Check if the action name in route values matches any of the provided method names
                if (httpContext.Request.RouteValues.TryGetValue("action", out var action) && action is string actionName)
                {
                    if (methodNames.Any(method => actionName.Equals(method, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }

                // Ensure request path is not null and contains any of the method names
                var requestPath = httpContext.Request.Path.Value;
                if (!string.IsNullOrEmpty(requestPath) && methodNames.Any(method => requestPath.Contains(method, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                AddEditResponseModel<object> responseException = _iUtilityServices.CreateExceptionResponseModel<object>(ex);
                await WriteResponseAsync(httpContext, StatusCodes.Status500InternalServerError, $"Error while determining request method: {responseException.message}");
                return false; // Default to not bypassing middleware in case of errors
            }
        }

        #endregion Middleware Bypass Helper
    }
}