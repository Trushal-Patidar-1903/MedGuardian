using DTO.Common.Response;
using DTO.Masters.BloodGroup.Response;
using Repositories.Interface.Masters.BloodGroup;
using Services.Interface.Masters.BloodGroup;

namespace Services.Service.BloodGroup
{
    public class BloodGroupService : IBloodGroupService
    {
        private readonly IBloodGroupRepository _iBloodGroupRepository;
        public BloodGroupService(IBloodGroupRepository _iBloodGroupRepository)
        {
            this._iBloodGroupRepository = _iBloodGroupRepository;
        }

        public async Task<AddEditResponseModel<List<BloodGroupDropdownModel>>> GetAllBloodGroups()
        {
            return await _iBloodGroupRepository.GetAllBloodGroups();
        }
    }
}
