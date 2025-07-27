using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO.User.RequestModel
{
    public class UserRegistrationModel
    {
        public int idUser { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? dateOfBirth { get; set; } // Make nullable for flexibility
        public int idGender { get; set; }
        public int idUserType { get; set; }

        // For file upload from the client
        public IFormFile? profilePicture { get; set; }

        // These will hold the paths once files are saved, not for binding
        public string? profilePicturePath { get; set; }
        public string? qrCodePath { get; set; }

        public string? mobile { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }

}
