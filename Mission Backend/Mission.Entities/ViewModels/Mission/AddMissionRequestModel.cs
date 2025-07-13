namespace Mission.Entities.ViewModels.Mission
{
    public class AddMissionRequestModel
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }

        public string MissionTitle { get; set; } = string.Empty;

        public int MissionThemeId { get; set; }

        public string MissionDescription { get; set; } = string.Empty;

        public int TotalSeats { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string MissionImages { get; set; } = string.Empty;

        public string MissionSkillId { get; set; } = string.Empty;
    }
}
