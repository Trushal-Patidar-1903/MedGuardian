using DTO.Common.Response;
using DTO.Masters.BloodGroup.Response;

namespace Repositories.Interface.Masters.BloodGroup
{
    public interface IBloodGroupRepository
    {
        Task<AddEditResponseModel<List<BloodGroupDropdownModel>>> GetAllBloodGroups();
    }
}
