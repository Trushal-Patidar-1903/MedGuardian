using DTO.Common.Response;
using DTO.Masters.BloodGroup.Response;

namespace Services.Interface.Masters.BloodGroup
{
    public interface IBloodGroupService
    {
        Task<AddEditResponseModel<List<BloodGroupDropdownModel>>> GetAllBloodGroups();
    }
}
