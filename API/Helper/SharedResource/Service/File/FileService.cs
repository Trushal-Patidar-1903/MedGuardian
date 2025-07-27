using Microsoft.Extensions.Options; // To read configuration
using System.IO;                  // For file operations
using Microsoft.AspNetCore.Http;
using QRCoder;
using System.Text.Json;
using System;
using System.Threading.Tasks;

// Assuming this is the correct interface path
using Helper.SharedResource.Interface.File;
using DTO.Common.Request;

namespace Helper.SharedResource.Service.File
{
    public class FileService : IFileService
    {
        private readonly string _basePath;

        // Inject IOptions<FileStorageSettings> instead of IWebHostEnvironment
        public FileService(IOptions<FileStorageSettings> fileStorageSettings)
        {
            _basePath = fileStorageSettings.Value.basePath;
            if (string.IsNullOrEmpty(_basePath))
            {
                throw new InvalidOperationException("FileStorageSettings:BasePath is not configured in appsettings.json.");
            }
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return null;
            }

            // Construct full physical path: e.g., C:\MedGuardianFiles\profile_pictures
            string targetFolder = Path.Combine(_basePath, "profile_pictures");
            Directory.CreateDirectory(targetFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
            string fullFilePath = Path.Combine(targetFolder, uniqueFileName);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(fileStream);
            }

            // Return the full physical path to be stored in the database
            return fullFilePath;
        }

        public async Task<string> GenerateAndSaveQrCodeAsync(int userId, string email, int userType)
        {
            var qrData = new { idUser = userId, email = email, userType = userType };
            string jsonPayload = JsonSerializer.Serialize(qrData);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonPayload, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImageBytes = qrCode.GetGraphic(20);

            // Construct full physical path: e.g., C:\MedGuardianFiles\qr_codes
            string targetFolder = Path.Combine(_basePath, "qr_codes");
            Directory.CreateDirectory(targetFolder);

            string uniqueFileName = $"{userId}_{Guid.NewGuid()}.png";
            string fullFilePath = Path.Combine(targetFolder, uniqueFileName);

            await System.IO.File.WriteAllBytesAsync(fullFilePath, qrCodeImageBytes);

            // Return the full physical path
            return fullFilePath;
        }
    }
}
