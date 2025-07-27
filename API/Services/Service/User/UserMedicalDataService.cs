using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Repositories.Interface.User;
using Services.Interface.User;

namespace Services.Service.User
{
    public class UserMedicalDataService : IUserMedicalDataService
    {
        public readonly IUserMedicalDataRepository _iUserMedicalDataRepository;
        public UserMedicalDataService(IUserMedicalDataRepository _iUserMedicalDataRepository)
        {
            this._iUserMedicalDataRepository = _iUserMedicalDataRepository;
        }

        /// <summary>
        /// Retrieves a list of all user medical data records.
        /// </summary>
        /// <returns>A response model containing a list of user medical data summaries.</returns>
        public async Task<AddEditResponseModel<List<UserMedicalDataListModel>>> GetAllUserMedicalData()
        {
            return await _iUserMedicalDataRepository.GetAllUserMedicalData();
        }

        /// <summary>
        /// Retrieves a complete user medical profile by its ID.
        /// </summary>
        /// <param name="idUserMedicalData">The primary key of the UserMedicalData record.</param>
        /// <returns>A response model containing the full user medical profile.</returns>
        public async Task<AddEditResponseModel<UserMedicalDataViewModel>> GetUserMedicalDataById(int idUserMedicalData)
        {
            return await _iUserMedicalDataRepository.GetUserMedicalDataById(idUserMedicalData);
        }

        public async Task<AddEditResponseModel<int>> AddOrUpdateUserMedicalData(UserMedicalDataModel model)
        {
            return await _iUserMedicalDataRepository.AddOrUpdateUserMedicalData(model);
        }
    }
}
