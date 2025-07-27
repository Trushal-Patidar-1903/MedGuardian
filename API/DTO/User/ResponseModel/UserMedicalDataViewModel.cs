using DTO.User.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User.ResponseModel
{
    public class UserMedicalDataViewModel
    {
        public UserMedicalDataViewModel()
        {
            userContacts = new List<UserContactModel>();
            allergies = new List<UserAllergyModel>();
            surgicalHistories = new List<UserSurgicalHistoryModel>();
            medicalHistories = new List<UserMedicalHistoryModel>();
        }

        public int idUserMedicalData { get; set; }
        public int idUser { get; set; }
        public int? idBloodGroup { get; set; }
        public int? age { get; set; }
        public decimal? weight { get; set; }
        public decimal? height { get; set; }
        public bool? isActive { get; set; }

        // Child collections
        public List<UserContactModel> userContacts { get; set; }
        public List<UserAllergyModel> allergies { get; set; }
        public List<UserSurgicalHistoryModel> surgicalHistories { get; set; }
        public List<UserMedicalHistoryModel> medicalHistories { get; set; }
    }
}
