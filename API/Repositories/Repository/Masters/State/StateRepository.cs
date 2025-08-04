using Dapper;
using DTO.Common.Response;
using DTO.Masters.State.Response;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.State;
using System.Data;

namespace Repositories.Repository.Masters.State
{
    public class StateRepository : IStateRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public StateRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        /// <summary>
        /// Gets a list of states for a specific country ID.
        /// </summary>
        /// <param name="countryId">The ID of the country.</param>
        /// <returns>A response model containing a list of states.</returns>
        public async Task<AddEditResponseModel<List<StateDropdownModel>>> GetStatesByCountryId(int countryId)
        {
            var responseModel = new AddEditResponseModel<List<StateDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("State")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var parameters = new { CountryID = countryId };
                    var result = await sqlConnection.QueryAsync<StateDropdownModel>(
                        StoredProcedures.sp_GetStatesByCountryID,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var stateList = result?.ToList();

                    if (stateList != null && stateList.Any())
                    {
                        responseModel.data = stateList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("State");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<StateDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<StateDropdownModel>>(ex);
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
