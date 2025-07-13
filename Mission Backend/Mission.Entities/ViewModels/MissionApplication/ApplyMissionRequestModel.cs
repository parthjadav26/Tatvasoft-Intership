using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.ViewModels.MissionApplication
{
    public class ApplyMissionRequestModel
    {

        public int MissionId { get; set; }

        public DateTime AppliedDate { get; set; }
        public int UserId { get; set; }


    }
}
