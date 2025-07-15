using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Mission.Entities.ViewModels.User
{
    public class UpdateUserRequestModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty; 
        public string UserType { get; set; } = string.Empty;
        public bool RemoveImage { get; set; }

        [JsonIgnore]

        public IFormFile? ProfileImage { get; set; } = null;


    }
}
