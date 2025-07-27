using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;

namespace Repositories.Interface.User
{
    public interface IUserMedicalDataRepository
    {
        /// <summary>
        /// Retrieves a list of all user medical data records.
        /// </summary>
        /// <returns>A response model containing a list of user medical data summaries.</returns>
        Task<AddEditResponseModel<List<UserMedicalDataListModel>>> GetAllUserMedicalData();

        /// <summary>
        /// Retrieves a complete user medical profile by its ID.
        /// </summary>
        /// <param name="idUserMedicalData">The primary key of the UserMedicalData record.</param>
        /// <returns>A response model containing the full user medical profile.</returns>
        Task<AddEditResponseModel<UserMedicalDataViewModel>> GetUserMedicalDataById(int idUserMedicalData);

        Task<AddEditResponseModel<int>> AddOrUpdateUserMedicalData(UserMedicalDataModel model);
    }
}
