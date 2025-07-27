using Dapper;
using DTO.Common.Response;
using DTO.Masters.City.Response;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.City;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Masters.City
{
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public CityRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        /// <summary>
        /// Gets a list of cities for a specific state ID.
        /// </summary>
        /// <param name="stateId">The ID of the state.</param>
        /// <returns>A response model containing a list of cities.</returns>
        public async Task<AddEditResponseModel<List<CityDropdownModel>>> GetCitiesByStateId(int stateId)
        {
            var responseModel = new AddEditResponseModel<List<CityDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("City")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var parameters = new { StateID = stateId };
                    var result = await sqlConnection.QueryAsync<CityDropdownModel>(
                        StoredProcedures.sp_GetCitiesByStateID,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var cityList = result?.ToList();

                    if (cityList != null && cityList.Any())
                    {
                        responseModel.data = cityList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("City");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<CityDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<CityDropdownModel>>(ex);
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
