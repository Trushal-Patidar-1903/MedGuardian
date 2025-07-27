using DTO.User.ResponseModel;
using Helper.SharedResource.Interface.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper.SharedResource.Service.Jwt
{
    public class JwtTokenServices : IJwtTokenServices
    {
        private readonly IConfiguration _iConfiguration;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenServices"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration to access JWT settings.</param>
        public JwtTokenServices(IConfiguration _iConfiguration)
        {
            this._iConfiguration = _iConfiguration;
        }
        #endregion

        #region GenerateJsonWebToken
        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="UserMasterModel">The user model containing user details.</param>
        /// <returns>The generated JWT as a string.</returns>
        public string GenerateJsonWebToken(UserMasterCredentialsModel userMaster)
        {
            var JwtSettings = _iConfiguration.GetSection("JwtSettings");
            if (JwtSettings != null)
            {
                var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["SecretKey"] ?? Common.Strings.SecretKey));
                var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

                // Ensure UserMasterModel has a property or method to retrieve Email
                var Claims = new[]
                {
                    new Claim("email", userMaster.email ?? string.Empty), // Adjusted to use a method
                    new Claim("userID", Convert.ToString(userMaster.idUser)),
                    new Claim("firstName", userMaster.firstName ?? string.Empty),
                    new Claim("lastName", userMaster.lastName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var Token = new JwtSecurityToken(
                    issuer: JwtSettings["Issuer"],
                    audience: JwtSettings["Audience"],
                    claims: Claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(JwtSettings["ExpiryInMinutes"] ?? "0")),
                    signingCredentials: Credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(Token);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
