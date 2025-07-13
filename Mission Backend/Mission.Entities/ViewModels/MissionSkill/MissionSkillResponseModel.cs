using Mission.Entities.ViewModels.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.ViewModels.MissionSkill
{
    public class MissionSkillResponseModel : UpsertMissionSkillRequestModel
    {
        public MissionSkillResponseModel() { }

        public MissionSkillResponseModel(Models.MissionSkill missionSkill) 
        {
            Id = missionSkill.Id;
            SkillName = missionSkill.SkillName;
            Status = missionSkill.Status;
        }


    }
}
