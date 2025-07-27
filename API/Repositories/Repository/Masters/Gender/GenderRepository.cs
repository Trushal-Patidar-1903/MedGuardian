using Dapper;
using DTO.Common.Response;
using DTO.Masters.Gender.Response;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.Gender;
using System.Data;

namespace Repositories.Repository.Masters.Gender
{
    public class GenderRepository : IGenderRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public GenderRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        public async Task<AddEditResponseModel<List<GenderDropdownModel>>> GetAllGenders()
        {
            var responseModel = new AddEditResponseModel<List<GenderDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("Gender")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<GenderDropdownModel>(
                        StoredProcedures.sp_GetAllGenders,
                        commandType: CommandType.StoredProcedure
                    );

                    var genderList = result?.ToList();

                    if (genderList != null && genderList.Any())
                    {
                        responseModel.data = genderList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("Gender");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<GenderDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<GenderDropdownModel>>(ex);
                }
                finally
                {
                    await _iBaseSqlManager.CloseSqlConnectionAsync(sqlConnection);
                }
            }

            return responseModel;
        }
    }
}
