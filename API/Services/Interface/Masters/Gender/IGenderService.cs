using DTO.Common.Response;
using DTO.Masters.Gender.Response;

namespace Services.Interface.Masters.Gender
{
    public interface IGenderService
    {
        Task<AddEditResponseModel<List<GenderDropdownModel>>> GetAllGenders();
    }
}
