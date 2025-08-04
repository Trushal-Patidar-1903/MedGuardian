using DTO.Common.Response;
using DTO.Masters.City.Response;

namespace Repositories.Interface.Masters.City
{
    public interface ICityRepository
    {
        /// <summary>
        /// Gets a list of cities for a specific state ID.
        /// </summary>
        /// <param name="stateId">The ID of the state.</param>
        /// <returns>A response model containing a list of cities.</returns>
        Task<AddEditResponseModel<List<CityDropdownModel>>> GetCitiesByStateId(int stateId);
    }
}
