using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User.ResponseModel
{
    public class UserMasterListModel
    {
        public int idUser { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public int? idGender { get; set; }
        public string? gender { get; set; }
        public int? idUserType { get; set; }
        public string? userType { get; set; }
        public string? qrCode { get; set; }
        public string? profilePicturePath { get; set; }
        public string? mobile { get; set; }
        public string? email { get; set; }
        public bool? isActive { get; set; }
        public bool? isDeleted { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? modifiedOn { get; set; }
    }
}
