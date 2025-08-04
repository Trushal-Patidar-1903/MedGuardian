using DTO.Common.Response;
using DTO.Masters.State.Response;
using Repositories.Interface.Masters.State;
using Services.Interface.Masters.State;

namespace Services.Service.Masters.State
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _iStateRepository;

        public StateService(IStateRepository iStateRepository)
        {
            _iStateRepository = iStateRepository;
        }

        /// <summary>
        /// Gets a list of states for a specific country ID.
        /// </summary>
        /// <param name="countryId">The ID of the country.</param>
        /// <returns>A response model containing a list of states.</returns>
        public async Task<AddEditResponseModel<List<StateDropdownModel>>> GetStatesByCountryId(int countryId)
        {
            return await _iStateRepository.GetStatesByCountryId(countryId);
        }
    }
}
