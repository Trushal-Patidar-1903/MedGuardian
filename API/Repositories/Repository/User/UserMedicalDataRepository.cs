using Dapper;
using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.User;
using System.Data;

namespace Repositories.Repository.User
{
    public class UserMedicalDataRepository : IUserMedicalDataRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public UserMedicalDataRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        /// <summary>
        /// Retrieves a list of all user medical data records.
        /// </summary>
        /// <returns>A response model containing a list of user medical data summaries.</returns>
        public async Task<AddEditResponseModel<List<UserMedicalDataListModel>>> GetAllUserMedicalData()
        {
            var responseModel = new AddEditResponseModel<List<UserMedicalDataListModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User Medical Data")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<UserMedicalDataListModel>(
                        StoredProcedures.sp_GetAllUserMedicalData,
                        commandType: CommandType.StoredProcedure
                    );

                    var dataList = result?.ToList();

                    if (dataList != null && dataList.Any())
                    {
                        responseModel.data = dataList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("User Medical Data");
                    }
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<UserMedicalDataListModel>>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }
            return responseModel;
        }

        /// <summary>
        /// Retrieves a complete user medical profile by its ID.
        /// </summary>
        /// <param name="idUserMedicalData">The primary key of the UserMedicalData record.</param>
        /// <returns>A response model containing the full user medical profile.</returns>
        public async Task<AddEditResponseModel<UserMedicalDataViewModel>> GetUserMedicalDataById(int idUserMedicalData)
        {
            var responseModel = new AddEditResponseModel<UserMedicalDataViewModel>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User Medical Data")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var parameters = new { idUserMedicalData };

                    using (var multi = await sqlConnection.QueryMultipleAsync(
                        StoredProcedures.sp_GetUserMedicalDataById,
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    {
                        // Read the first result set (the main medical data)
                        var medicalData = await multi.ReadSingleOrDefaultAsync<UserMedicalDataViewModel>();

                        if (medicalData != null)
                        {
                            // Read the subsequent result sets and map to the child collections
                            medicalData.userContacts = (await multi.ReadAsync<UserContactModel>()).ToList();
                            medicalData.allergies = (await multi.ReadAsync<UserAllergyModel>()).ToList();
                            medicalData.surgicalHistories = (await multi.ReadAsync<UserSurgicalHistoryModel>()).ToList();
                            medicalData.medicalHistories = (await multi.ReadAsync<UserMedicalHistoryModel>()).ToList();

                            responseModel.data = medicalData;
                            responseModel.status = true;
                            responseModel.message = Helper.Common.Messages.RetrievedSuccess("User Medical Data");
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<UserMedicalDataViewModel>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }
            return responseModel;
        }

        public async Task<AddEditResponseModel<int>> AddOrUpdateUserMedicalData(UserMedicalDataModel model)
        {
            var response = new AddEditResponseModel<int>
            {
                status = false,
                message = "Failed to save user medical data"
            };

            using (var sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Convert Lists to DataTables
                        DataTable contactTable = model.userContacts?.Any() == true
                            ? _iUtilityServices.ConvertListToDataTable(model.userContacts)
                            : _iUtilityServices.CreateEmptyTable<UserContactModel>();

                        DataTable allergyTable = model.allergies?.Any() == true
                            ? _iUtilityServices.ConvertListToDataTable(model.allergies)
                            : _iUtilityServices.CreateEmptyTable<UserAllergyModel>();

                        DataTable surgicalTable = model.surgicalHistories?.Any() == true
                            ? _iUtilityServices.ConvertListToDataTable(model.surgicalHistories)
                            : _iUtilityServices.CreateEmptyTable<UserSurgicalHistoryModel>();

                        DataTable medicalTable = model.medicalHistories?.Any() == true
                            ? _iUtilityServices.ConvertListToDataTable(model.medicalHistories)
                            : _iUtilityServices.CreateEmptyTable<UserMedicalHistoryModel>();

                        var parameters = new DynamicParameters();
                        parameters.Add("@idUserMedicalData", model.idUserMedicalData, DbType.Int32, ParameterDirection.InputOutput);
                        parameters.Add("@createdBy", model.createdBy);
                        parameters.Add("@modifiedBy", model.modifiedBy);
                        parameters.Add("@idUser", model.idUser);
                        parameters.Add("@idBloodGroup", model.idBloodGroup);
                        parameters.Add("@age", model.age);
                        parameters.Add("@weight", model.weight);
                        parameters.Add("@height", model.height);
                        parameters.Add("@isActive", model.isActive);
                        parameters.Add("@isDeleted", model.isDeleted);

                        // Table-valued parameters
                        parameters.Add("@typeUserContacts", contactTable.AsTableValuedParameter("typeUserContact"));
                        parameters.Add("@typeUserAllergies", allergyTable.AsTableValuedParameter("typeUserAllergy"));
                        parameters.Add("@typeUserSurgicalHistories", surgicalTable.AsTableValuedParameter("typeUserSurgicalHistory"));
                        parameters.Add("@typeUserMedicalHistories", medicalTable.AsTableValuedParameter("typeUserMedicalHistory"));

                        // Execute SP
                        await sqlConnection.ExecuteAsync(
                            StoredProcedures.sp_AddOrUpdateUserMedicalData,
                            parameters,
                            commandType: CommandType.StoredProcedure,
                            transaction: sqlTransaction
                        );

                        int idUserMedicalData = parameters.Get<int>("@idUserMedicalData");

                        sqlTransaction.Commit();

                        response.status = true;
                        response.data = idUserMedicalData;
                        response.message = "User medical data saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        response = _iUtilityServices.CreateExceptionResponseModel<int>(ex);
                    }
                    finally
                    {
                        await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                    }
                }
            }

            return response;
        }
    }
}
