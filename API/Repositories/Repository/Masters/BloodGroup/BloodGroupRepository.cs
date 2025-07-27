using Dapper;
using DTO.Common.Response;
using DTO.Masters.BloodGroup.Response;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.BloodGroup;
using System.Data;

namespace Repositories.Repository.Masters.BloodGroup
{
    public class BloodGroupRepository : IBloodGroupRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public BloodGroupRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        public async Task<AddEditResponseModel<List<BloodGroupDropdownModel>>> GetAllBloodGroups()
        {
            var responseModel = new AddEditResponseModel<List<BloodGroupDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("Blood Group")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<BloodGroupDropdownModel>(
                        StoredProcedures.sp_GetAllBloodGroups,
                        commandType: CommandType.StoredProcedure
                    );

                    var bloodGroupList = result?.ToList();

                    if (bloodGroupList != null && bloodGroupList.Any())
                    {
                        responseModel.data = bloodGroupList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("Blood Group");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<BloodGroupDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<BloodGroupDropdownModel>>(ex);
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
