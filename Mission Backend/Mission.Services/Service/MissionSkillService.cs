using Mission.Entities.ViewModels.Mission;
using Mission.Entities.ViewModels.MissionSkill;
using Mission.Repositories.IRepository;
using Mission.Services.IService;
using System.Threading.Tasks;

namespace Mission.Services.Service
{
    public class MissionSkillService : IMissionSkillService
    {
        private readonly IMissionSkillRepository _missionSkillRepository;


        public MissionSkillService(IMissionSkillRepository missionSkillRepository)
        {
            _missionSkillRepository = missionSkillRepository;
        }

        public async Task AddMissionSkillAsync(UpsertMissionSkillRequestModel model)
        {
            await _missionSkillRepository.AddMissionSkillAsync(model);
        }

        public async Task<List<MissionSkillResponseModel>> GetMissionSkillAsync()
        {
            return await _missionSkillRepository.GetMissionSkillAsync();
        }

        public async Task<MissionSkillResponseModel?> GetMissionSkillByIdAsync(int missionSkillId)
        {
            return await _missionSkillRepository.GetMissionSkillByIdAsync(missionSkillId);
        }

        public async Task<bool> UpdateMissionSkillAsync(UpsertMissionSkillRequestModel model)
        {
            return await _missionSkillRepository.UpdateMissionSkillAsync(model);
        }

        public async Task<bool> DeleteMissionSkillAsync(int missionSkillId)
        {
            return await _missionSkillRepository.DeleteMissionSkillAsync(missionSkillId); 
        }
    }
}
