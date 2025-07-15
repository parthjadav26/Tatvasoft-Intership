namespace Mission.Entities.ViewModels.User
{
    public class AddUserRequestModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string UserType { get; set; }

        public string Password { get; set; }


    }
}
