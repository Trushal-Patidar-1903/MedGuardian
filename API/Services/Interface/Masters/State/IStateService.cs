using DTO.Common.Response;
using DTO.Masters.State.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.Masters.State
{
    public interface IStateService
    {
        /// <summary>
        /// Gets a list of states for a specific country ID.
        /// </summary>
        /// <param name="countryId">The ID of the country.</param>
        /// <returns>A response model containing a list of states.</returns>
        Task<AddEditResponseModel<List<StateDropdownModel>>> GetStatesByCountryId(int countryId);
    }
}
