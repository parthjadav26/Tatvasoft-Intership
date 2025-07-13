using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Entities.ViewModels.Mission
{
    public class UpsertMissionThemeRequestModel
    {
        public int Id { get; set; }

        public string ThemeName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
