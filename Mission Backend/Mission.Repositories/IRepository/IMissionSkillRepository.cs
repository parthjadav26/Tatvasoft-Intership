using Mission.Entities.ViewModels.Mission;
using Mission.Entities.ViewModels.MissionSkill;

namespace Mission.Repositories.IRepository
{
    public interface IMissionSkillRepository
    {
        Task AddMissionSkillAsync(UpsertMissionSkillRequestModel model);

        Task<List<MissionSkillResponseModel>> GetMissionSkillAsync();

        Task<MissionSkillResponseModel?> GetMissionSkillByIdAsync(int missionSkillId);

        Task<bool> UpdateMissionSkillAsync(UpsertMissionSkillRequestModel model);

        Task<bool> DeleteMissionSkillAsync(int missionSkillId);
    }
}
