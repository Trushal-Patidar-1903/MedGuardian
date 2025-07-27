using DTO.User.ResponseModel;

namespace Helper.SharedResource.Interface.Jwt
{
    public interface IJwtTokenServices
    {
        #region GenerateJsonWebToken
        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <param name="UserMasterModel">The user model containing user details.</param>
        /// <returns>The generated JWT as a string.</returns>
        string GenerateJsonWebToken(UserMasterCredentialsModel userMaster);
        #endregion
    }
}
