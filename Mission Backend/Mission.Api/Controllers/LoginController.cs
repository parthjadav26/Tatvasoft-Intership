using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.ViewModels;
using Mission.Entities.ViewModels.Login;
using Mission.Entities.ViewModels.User;
using Mission.Services.IService;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        [AllowAnonymous]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser(UserLoginRequestModel model)
        {
            var response = await _userService.LogiUser(model);

            if (response.Result == ResponseStatus.Error)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(AddUserRequestModel model)
        {
            var response = await _userService.RegisterUserAsync(model);
            var result = new ResponseResult();

            if (!response)
            {
                result.Message = "User already exists with same email address";
                result.Result = ResponseStatus.Error;
                return BadRequest(result);
            }

            result.Result = ResponseStatus.Success;
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("LoginUserDetailById/{userId:int}")]
        public async Task<IActionResult> GetLoginUserDetailById(int userId)
        {
            var response = await _userService.GetLoginUserDetailById(userId);
            var result = new ResponseResult();

            if (response == null)
            {
                result.Message = "User not found";
                result.Result = ResponseStatus.Error;
                return NotFound(result);
            }

            result.data = response;
            result.Result = ResponseStatus.Success;
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestModel model)
        {
            var response = await _userService.UpdateUserAsync(model);

            if (response.Message == "User not found")
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
