using DTO.Common.Response;
using DTO.Masters.Gender.Response;

namespace Repositories.Interface.Masters.Gender
{
    public interface IGenderRepository
    {
        Task<AddEditResponseModel<List<GenderDropdownModel>>> GetAllGenders();
    }
}
