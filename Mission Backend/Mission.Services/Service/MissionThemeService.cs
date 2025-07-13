using Mission.Entities.ViewModels.Mission;
using Mission.Entities.ViewModels.MissionTheme;
using Mission.Repositories.IRepository;
using Mission.Services.IService;
using System.Threading.Tasks;

namespace Mission.Services.Service
{
    public class MissionThemeService : IMissionThemeService
    {
        private readonly IMissionThemeRepository _missionThemeRepository;


        public MissionThemeService(IMissionThemeRepository missionThemeRepository)
        {
            _missionThemeRepository = missionThemeRepository;
        }

        public async Task AddMissionThemeAsync(UpsertMissionThemeRequestModel model)
        {
            await _missionThemeRepository.AddMissionThemeAsync(model);
        }

        public async Task<List<MissionThemeResponseModel>> GetMissionThemeAsync()
        {
            return await _missionThemeRepository.GetMissionThemeAsync();
        }

        public async Task<MissionThemeResponseModel?> GetMissionThemeByIdAsync(int missionThemeId)
        {
            return await _missionThemeRepository.GetMissionThemeByIdAsync(missionThemeId);
        }

        public async Task<bool> UpdateMissionThemeAsync(UpsertMissionThemeRequestModel model)
        {
            return await _missionThemeRepository.UpdateMissionThemeAsync(model);
        }

        public async Task<bool> DeleteMissionThemeAsync(int missionThemeId)
        {
            return await _missionThemeRepository.DeleteMissionThemeAsync(missionThemeId);
        }
    }
}
