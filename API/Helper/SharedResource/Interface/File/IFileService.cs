using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.SharedResource.Interface.File
{
    public interface IFileService
    {
        Task<string> SaveProfilePictureAsync(IFormFile profilePicture);
        Task<string> GenerateAndSaveQrCodeAsync(int userId, string email, int userType);
    }
}
