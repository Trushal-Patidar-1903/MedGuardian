using DTO.Common.Response;
using DTO.Masters.Country.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface.Masters.Country
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Gets a list of all countries for a dropdown.
        /// </summary>
        /// <returns>A response model containing a list of countries.</returns>
        Task<AddEditResponseModel<List<CountryDropdownModel>>> GetAllCountries();

    }
}
