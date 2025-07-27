namespace DTO.User.RequestModel
{
    public class UserContactModel
    {
        public int idUserContact { get; set; }
        public int idRelationType { get; set; }
        public bool isDefault { get; set; }
        public string? contactNo { get; set; }
    }
}
