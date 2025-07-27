using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;

namespace Repositories.Interface.User
{
    public interface IUserRepository
    {
        Task<AddEditResponseModel<List<UserMasterListModel>>> GetAllUsers();
        Task<AddEditResponseModel<UserMasterListModel>> GetUserById(int idUser);
        Task<AddEditResponseModel<List<UserTypeDropdownModel>>> GetAllUserTypes();
        Task<AddEditResponseModel<UserMasterCredentialsModel>> GetUserCredentials(string email);
        Task<AddEditResponseModel<int>> AddEditUserDetails(UserRegistrationModel userRegDetails);
    }
}
