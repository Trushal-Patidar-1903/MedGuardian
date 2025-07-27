using DTO.Common.Response;
using DTO.Masters.Gender.Response;
using Repositories.Interface.Masters.Gender;
using Services.Interface.Masters.Gender;

namespace Services.Service.Gender
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _iGenderRepository;
        public GenderService(IGenderRepository _iGenderRepository)
        {
            this._iGenderRepository = _iGenderRepository;
        }

        public async Task<AddEditResponseModel<List<GenderDropdownModel>>> GetAllGenders()
        {
            return await _iGenderRepository.GetAllGenders();
        }
    }
}
