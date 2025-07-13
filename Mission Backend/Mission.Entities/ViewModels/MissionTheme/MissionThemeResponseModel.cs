using Mission.Entities.ViewModels.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.ViewModels.MissionTheme
{
    public class MissionThemeResponseModel : UpsertMissionThemeRequestModel
    {
        public MissionThemeResponseModel() { }

        public MissionThemeResponseModel(Models.MissionTheme missionTheme)
        {
            Id = missionTheme.Id;
            ThemeName = missionTheme.ThemeName;
            Status = missionTheme.Status;
        }


    }
}
