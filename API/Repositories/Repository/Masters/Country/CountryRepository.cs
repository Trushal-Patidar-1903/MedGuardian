using Dapper;
using DTO.Common.Response;
using DTO.Masters.Country.Response;
using Helper.Database;
using Helper.SharedResource.Interface.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories.Interface.Common.Sql;
using Repositories.Interface.Masters.Country;
using System.Data;

namespace Repositories.Repository.Masters.Country
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IConfiguration _iConfiguration;
        private readonly IBaseSqlManager _iBaseSqlManager;
        private readonly IUtilityServices _iUtilityServices;

        public CountryRepository(IConfiguration _iConfiguration, IBaseSqlManager _iBaseSqlManager, IUtilityServices _iUtilityServices)
        {
            this._iConfiguration = _iConfiguration;
            this._iBaseSqlManager = _iBaseSqlManager;
            this._iUtilityServices = _iUtilityServices;
        }

        /// <summary>
        /// Gets a list of all countries for a dropdown.
        /// </summary>
        /// <returns>A response model containing a list of countries.</returns>
        public async Task<AddEditResponseModel<List<CountryDropdownModel>>> GetAllCountries()
        {
            var responseModel = new AddEditResponseModel<List<CountryDropdownModel>>
            {
                data = null,
                status = false,
                message = Helper.Common.Messages.NotFound("Country")
            };

            using (SqlConnection sqlConnection = await _iBaseSqlManager.OpenSqlConnectionAsync())
            {
                try
                {
                    var result = await sqlConnection.QueryAsync<CountryDropdownModel>(
                        StoredProcedures.sp_GetAllCountries,
                        commandType: CommandType.StoredProcedure
                    );

                    var countryList = result?.ToList();

                    if (countryList != null && countryList.Any())
                    {
                        responseModel.data = countryList;
                        responseModel.status = true;
                        responseModel.message = Helper.Common.Messages.RetrievedSuccess("Country");
                    }
                }
                catch (SqlException sqlEx)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<CountryDropdownModel>>(sqlEx);
                }
                catch (Exception ex)
                {
                    responseModel = _iUtilityServices.CreateExceptionResponseModel<List<CountryDropdownModel>>(ex);
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
