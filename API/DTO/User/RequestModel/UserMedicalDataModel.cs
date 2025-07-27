namespace DTO.User.RequestModel
{
    public class UserMedicalDataModel
    {
        public int idUserMedicalData { get; set; }
        public int? createdBy { get; set; }
        public int? modifiedBy { get; set; }
        public int idUser { get; set; }
        public int? idBloodGroup { get; set; }
        public int? age { get; set; }
        public decimal? weight { get; set; }
        public decimal? height { get; set; }
        public bool? isActive { get; set; }
        public bool? isDeleted { get; set; }
        public List<UserContactModel>? userContacts { get; set; }
        public List<UserAllergyModel>? allergies { get; set; }
        public List<UserSurgicalHistoryModel>? surgicalHistories { get; set; }
        public List<UserMedicalHistoryModel>? medicalHistories { get; set; }
    }
}
