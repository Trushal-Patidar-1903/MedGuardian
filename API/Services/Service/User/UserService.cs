using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Repositories.Interface.User;
using Services.Interface.User;

namespace Services.Service.User
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _iUserRepository;
        public UserService(IUserRepository _iUserRepository)
        {
            this._iUserRepository = _iUserRepository;
        }

        public async Task<AddEditResponseModel<List<UserMasterListModel>>> GetAllUsers()
        {
            return await _iUserRepository.GetAllUsers();
        }

        public async Task<AddEditResponseModel<UserMasterListModel>> GetUserById(int idUser)
        {
            return await _iUserRepository.GetUserById(idUser);
        }

        public async Task<AddEditResponseModel<List<UserTypeDropdownModel>>> GetAllUserTypes()
        {
            return await _iUserRepository.GetAllUserTypes();
        }

        public async Task<AddEditResponseModel<UserMasterCredentialsModel>> GetUserCredentials(string email)
        {
            return await _iUserRepository.GetUserCredentials(email);
        }

        public async Task<AddEditResponseModel<int>> AddEditUserDetails(UserRegistrationModel userRegDetails)
        {
            return await _iUserRepository.AddEditUserDetails(userRegDetails);
        }
    }
}
