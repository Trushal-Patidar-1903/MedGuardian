using Dapper;
using DTO.Common.Response;
using DTO.User.RequestModel;
using DTO.User.ResponseModel;
using Helper.Database;
using Helper.SharedResource.Interface.File;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.User;
using System.Data;
using static Helper.Common.Enums;

namespace Repositories.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;
        private readonly IFileService _iFileService;

        public UserRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices, IFileService _iFileService)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
            this._iFileService = _iFileService;
        }

        public async Task<AddEditResponseModel<List<UserMasterListModel>>> GetAllUsers()
        {
            var responseModel = new AddEditResponseModel<List<UserMasterListModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<UserMasterListModel>(
                        StoredProcedures.sp_GetAllUsers,
                        commandType: CommandType.StoredProcedure
                    );

                    var userList = result?.ToList();

                    if (userList != null && userList.Any())
                    {
                        responseModel.data = userList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("User");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<UserMasterListModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<UserMasterListModel>>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }

            return responseModel;
        }

        public async Task<AddEditResponseModel<UserMasterListModel>> GetUserById(int idUser)
        {
            var responseModel = new AddEditResponseModel<UserMasterListModel>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryFirstOrDefaultAsync<UserMasterListModel>(
                        StoredProcedures.sp_GetUserById,
                        new { idUser },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        responseModel.data = result;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("User");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<UserMasterListModel>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<UserMasterListModel>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }

            return responseModel;
        }

        public async Task<AddEditResponseModel<List<UserTypeDropdownModel>>> GetAllUserTypes()
        {
            var responseModel = new AddEditResponseModel<List<UserTypeDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User Type")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<UserTypeDropdownModel>(
                        StoredProcedures.sp_GetAllUserTypes,
                        commandType: CommandType.StoredProcedure
                    );

                    var userTypeList = result?.ToList();

                    if (userTypeList != null && userTypeList.Any())
                    {
                        responseModel.data = userTypeList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("User Type");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<UserTypeDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<UserTypeDropdownModel>>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }

            return responseModel;
        }

        public async Task<AddEditResponseModel<UserMasterCredentialsModel>> GetUserCredentials(string email)
        {
            var responseModel = new AddEditResponseModel<UserMasterCredentialsModel>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("User Credentials")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@Email", email, DbType.String);

                    var result = await sqlConnection.QueryFirstOrDefaultAsync<UserMasterCredentialsModel>(
                        StoredProcedures.sp_GetUserCredentials,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        responseModel.data = result;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("User Credentials");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<UserMasterCredentialsModel>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<UserMasterCredentialsModel>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }

            return responseModel;
        }

        public async Task<AddEditResponseModel<int>> AddEditUserDetails(UserRegistrationModel userRegDetails)
        {
            var responseModel = new AddEditResponseModel<int>
            {
                data = 0,
                status = false,
                message = "Failed to save user details."
            };

            // ----- Step 1: Save core user data to get an ID -----
            int userId = 0;
            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUser", userRegDetails.idUser, DbType.Int32, ParameterDirection.InputOutput);
                    parameters.Add("@createdOn", _iUtilityServices.SqlNow(), DbType.DateTime);
                    parameters.Add("@modifiedOn", _iUtilityServices.SqlNow(), DbType.DateTime);
                    parameters.Add("@isActive", (int)RecordActiveStatus.Active, DbType.Boolean);
                    parameters.Add("@isDeleted", (int)RecordDeleteStatus.NotDeleted, DbType.Boolean);
                    parameters.Add("@firstName", userRegDetails.firstName);
                    parameters.Add("@lastName", userRegDetails.lastName);
                    parameters.Add("dateOfBirth", _iUtilityServices.ToDbValue(userRegDetails.dateOfBirth), DbType.DateTime);
                    parameters.Add("@idGender", userRegDetails.idGender, DbType.Int32);
                    parameters.Add("@idUserType", userRegDetails.idUserType, DbType.Int32);
                    parameters.Add("@email", userRegDetails.email);
                    parameters.Add("@password", userRegDetails.password); // Assumes already encrypted
                    parameters.Add("@mobile", _iUtilityServices.ToDbValue(userRegDetails.mobile), DbType.String);// Pass NULL for file paths initially
                    parameters.Add("@qrCode", null, DbType.String);
                    parameters.Add("@profilePicturePath", null, DbType.String);

                    await sqlConnection.ExecuteAsync(
                        StoredProcedures.sp_AddEditUserDetails,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    userId = parameters.Get<int>("@idUser");
                    if (userId <= 0) throw new Exception("Failed to retrieve user ID after save.");
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<int>(ex);
                    return responseModel; // Exit if the initial save fails
                }
            }

            // ----- Step 2: Handle file operations (outside DB transaction) -----
            string savedProfilePicPath = null;
            string savedQrCodePath = null;
            try
            {
                // Save profile picture if one was uploaded
                if (userRegDetails.profilePicture != null)
                {
                    savedProfilePicPath = await _iFileService.SaveProfilePictureAsync(userRegDetails.profilePicture);
                }

                // Generate and save the QR code
                savedQrCodePath = await _iFileService.GenerateAndSaveQrCodeAsync(userId, userRegDetails.email, userRegDetails.idUserType);
            }
            catch (Exception ex)
            {
                // Log the file save error, but the user is already created.
                // You might want to add more robust error handling here.
                Console.WriteLine($"File operation failed for user {userId}: {ex.Message}");
            }

            // ----- Step 3: Update the user record with file paths -----
            if (!string.IsNullOrEmpty(savedProfilePicPath) || !string.IsNullOrEmpty(savedQrCodePath))
            {
                using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
                {
                    try
                    {
                        var updateParams = new DynamicParameters();
                        updateParams.Add("@idUser", userId);
                        updateParams.Add("@qrCodePath", savedQrCodePath);
                        updateParams.Add("@profilePicturePath", savedProfilePicPath);

                        await sqlConnection.ExecuteAsync(
                            StoredProcedures.sp_UpdateUserFilePaths, // Use the new SP
                            updateParams,
                            commandType: CommandType.StoredProcedure
                        );
                    }
                    catch (Exception ex)
                    {
                        // The user was created, but paths couldn't be saved. Log this critical error.
                        responseModel = _iUtilityServices.CreateExceptionResponseModel<int>(ex);
                        responseModel.message = "User created, but failed to save file paths.";
                        return responseModel;
                    }
                }
            }

            responseModel.data = userId;
            responseModel.status = true;
            responseModel.message = "User details saved successfully.";
            return responseModel;
        }
    }
}