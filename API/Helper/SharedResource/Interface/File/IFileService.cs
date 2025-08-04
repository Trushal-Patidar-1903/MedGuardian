using Microsoft.AspNetCore.Http;

namespace Helper.SharedResource.Interface.File
{
    public interface IFileService
    {
        Task<string> SaveProfilePictureAsync(IFormFile profilePicture);
        Task<string> GenerateAndSaveQrCodeAsync(int userId, string email, int userType);
    }
}
