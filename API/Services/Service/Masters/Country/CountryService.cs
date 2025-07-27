using DTO.Common.Response;
using DTO.Masters.Country.Response;
using Repositories.Interface.Masters.Country;
using Services.Interface.Masters.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.Masters.Country
{
    public class CountryService : ICountryService
    {

        private readonly ICountryRepository _iCountryRepository;

        public CountryService(ICountryRepository iCountryRepository)
        {
            this._iCountryRepository = iCountryRepository;
        }

        /// <summary>
        /// Gets a list of all countries for a dropdown.
        /// </summary>
        /// <returns>A response model containing a list of countries.</returns>
        public async Task<AddEditResponseModel<List<CountryDropdownModel>>> GetAllCountries()
        {
            return await _iCountryRepository.GetAllCountries();
        }
    }
}
