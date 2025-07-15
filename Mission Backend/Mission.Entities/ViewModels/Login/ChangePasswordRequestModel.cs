namespace Mission.Entities.ViewModels.Login
{
    public class ChangePasswordRequestModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string confirmPassword { get; set; }
    }
}
