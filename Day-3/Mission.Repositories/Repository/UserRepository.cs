﻿using Microsoft.EntityFrameworkCore;
using Mission.Entities;
using Mission.Entities.ViewModels.Login;
using Mission.Repositories.IRepository;

namespace Mission.Repositories.Repository
{
    public class UserRepository(MissionDbContext dbContext) : IUserRepository
    {
        private readonly MissionDbContext _context = dbContext;

        public async Task<(UserLoginResponseModel? response, string message)> LogiUser(UserLoginRequestModel model) 
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return (null, "Email or Password cannot be empty.");
            }

            var user = await _context.Users.Where(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefaultAsync();

            if (user == null)
            {
                return (null, "User doesn't exist for the given emailaddress");
            }

            if (user.Password != model.Password)
            {
                return (null, "Password doesn't matched");
            }

            var response = new UserLoginResponseModel() 
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                UserImage = user.UserImage,
            };

            return (response, "Login Successfully");
        }
    }
}
