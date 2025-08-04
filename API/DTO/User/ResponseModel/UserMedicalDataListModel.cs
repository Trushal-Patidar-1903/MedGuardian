namespace DTO.User.ResponseModel
{
    public class UserMedicalDataListModel
    {
        public int idUserMedicalData { get; set; }
        public int idUser { get; set; }
        public string? UserName { get; set; } // Joined from Users table
        public string? BloodGroupName { get; set; } // Joined from BloodGroup table
        public int? age { get; set; }
        public decimal? weight { get; set; }
        public decimal? height { get; set; }
        public bool? isActive { get; set; }
    }
}
