using System.ComponentModel.DataAnnotations;


namespace Mission.Entities.Models
{
    public class UserSkills
    {

            [Key]
            public int Id { get; set; }

            public string SkillName { get; set; } = string.Empty;

            public string Status { get; set; } = string.Empty;
        }
}

