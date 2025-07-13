using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mission.Entities;
using Mission.Entities.Models;
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

        public async Task<(bool response,string message)> UpdateUserAsync(UpdateUserRequestModel model)
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


    }
}
