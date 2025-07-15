using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mission.Entities;
using Mission.Entities.Helper;
using Mission.Entities.Models;
using Mission.Entities.ViewModels;
using Mission.Entities.ViewModels.Login;
using Mission.Entities.ViewModels.User;
using Mission.Repositories.IRepository;

namespace Mission.Repositories.Repository
{
    public class UserRepository(MissionDbContext dbContext) : IUserRepository
    {
         private readonly MissionDbContext _context = dbContext;
  


        public async Task<(UserLoginResponseModel? response, string message)> LogiUser(UserLoginRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.EmailAddress) || string.IsNullOrWhiteSpace(model.Password))
            {
                return (null, "Email or Password cannot be empty.");
            }

            var user = await _context.Users
                .Where(u => u.EmailAddress.ToLower() == model.EmailAddress.ToLower())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return (null, "User doesn't exist for the given email address.");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                return (null, "Password doesn't match.");
            }

            // Create response model
            var response = new UserLoginResponseModel()
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                UserImage = user.UserImage,
            };
       

            return (response, "Login Successfully");
        }


        public async Task<List<UserResponseModel>> GetUsersAsync()
        {
            return await _context.Users.Select(u => new UserResponseModel()
            {
                Id = u.Id,
                EmailAddress = u.EmailAddress,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                UserType = u.UserType,
                ProfileImage = u.UserImage

            }).ToListAsync();

        }

        public async Task<bool> RegisterUserAsync(AddUserRequestModel model)
       {
            var userInDb = _context.Users.Where(u => u.EmailAddress.ToLower() == model.EmailAddress.ToLower()).FirstOrDefault();

            if (userInDb != null)
            {
                return false;
            }


            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType,
            };

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
       }

        public async Task<UserResponseModel?> GetLoginUserDetailById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                return null;
            }

            return new UserResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                ProfileImage = user.UserImage,
            };
        }

        public async Task<(bool response,string message)> UpdateUserAsync(UpdateUserRequestModel model,string imageUploadPath)
        {
            var userInDb = _context.Users.Where(u =>u.Id != model.Id && u.EmailAddress.ToLower() == model.EmailAddress.ToLower()).FirstOrDefault();

            if (userInDb != null)
            {
                return (false,"User already exists with same email address");
            }

            var user = _context.Users.Find(model.Id);

            if (user == null) 
            {
                return (false, "User not found");
            }

            if(model.RemoveImage && string.IsNullOrEmpty(user.UserImage))
            {
                string oldImageFullPath = Path.Combine(imageUploadPath, user.UserImage.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if(File.Exists(oldImageFullPath))
                    File.Delete(oldImageFullPath);
            }

            if (model.ProfileImage != null) 
            {
                string newImagePath = await UploadFile.SaveImageAsync(model.ProfileImage,"UploadMissionImage/Images",imageUploadPath);
                user.UserImage = newImagePath;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmailAddress = model.EmailAddress;
            user.PhoneNumber = model.PhoneNumber;
            

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return (true,"User Update Successfully");
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user == null) 
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResponseResult> ChangePasswordAsync(ChangePasswordRequestModel model)
        {
            var result = new ResponseResult();

            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "User not found";
                return result;
            }

            if(model.NewPassword != model.confirmPassword)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "New password and Confirm password does not match";
                return result;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, model.OldPassword) == PasswordVerificationResult.Failed)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Old password does not match";
                return result;

            }

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, model.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            result.Result = ResponseStatus.Success;
            result.Message = "Password updated successfully";
            return result;
        }


        public async Task<UserResponseModel?> GetUserProfileDetailById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                ProfileImage = user.UserImage,
            };
        }
    }
}
