using DTO.Common.Response;
using DTO.Masters.Country.Response;

namespace Services.Interface.Masters.Country
{
    public interface ICountryService
    {
        /// <summary>
        /// Gets a list of all countries for a dropdown.
        /// </summary>
        /// <returns>A response model containing a list of countries.</returns>
        Task<AddEditResponseModel<List<CountryDropdownModel>>> GetAllCountries();
    }
}
