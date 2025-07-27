namespace DTO.User.ResponseModel
{
    public class UserMasterCredentialsModel
    {
        public int idUser { get; set; }
        public int idUserType { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
