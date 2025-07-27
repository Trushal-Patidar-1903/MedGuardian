using System.ComponentModel.DataAnnotations;

namespace DTO.User.RequestModel
{
    public class UserAuthenticationModel
    {
        [Required]
        public string email { get; set; } = "";
        [Required]
        public string password { get; set; } = "";
    }
}
