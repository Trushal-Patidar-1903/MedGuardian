using DTO.Common.Response;
using DTO.Masters.City.Response;
using Repositories.Interface.Masters.BloodGroup;
using Repositories.Interface.Masters.City;
using Services.Interface.Masters.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.Masters.City
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _iCityRepository;
        public CityService(ICityRepository _iCityRepository)
        {
            this._iCityRepository = _iCityRepository;
        }

        /// <summary>
        /// Gets a list of cities for a specific state ID.
        /// </summary>
        /// <param name="stateId">The ID of the state.</param>
        /// <returns>A response model containing a list of cities.</returns>
        public async Task<AddEditResponseModel<List<CityDropdownModel>>> GetCitiesByStateId(int stateId)
        {
            return await _iCityRepository.GetCitiesByStateId(stateId);
        }
    }
}
