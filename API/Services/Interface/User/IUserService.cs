using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;

namespace Services.Interface.User
{
    public interface IUserService
    {
        Task<AddEditResponseModel<List<UserMasterListModel>>> GetAllUsers();
        Task<AddEditResponseModel<UserMasterListModel>> GetUserById(int idUser);
        Task<AddEditResponseModel<List<UserTypeDropdownModel>>> GetAllUserTypes();
        Task<AddEditResponseModel<UserMasterCredentialsModel>> GetUserCredentials(string email);
        Task<AddEditResponseModel<int>> AddEditUserDetails(UserRegistrationModel userRegDetails);
    }
}
