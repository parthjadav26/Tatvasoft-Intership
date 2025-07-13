using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.ViewModels.Mission
{
    public class UpsertMissionSkillRequestModel
    {
        public int Id { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
