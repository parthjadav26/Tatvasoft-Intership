using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.ViewModels;
using Mission.Services.IService;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;
        [HttpGet]
        [Route("UserDetailList")]
        public async Task<IActionResult> GetUserDetailList()
        {
            var response = await _userService.GetUsersAsync();

            var result = new ResponseResult()
            {
                data = response,
                Result = ResponseStatus.Success
            };

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteUser/{userId:int}")]

        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = new ResponseResult();

            var user = await _userService.GetLoginUserDetailById(userId);
            if (user == null)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "User not found";
                return NotFound(result);
            }

            if (user.UserType == "admin")
            {
                result.Result = ResponseStatus.Error;
                result.Message = "You can't delete admin record";
                return Ok(result);
            }

            var response = await _userService.DeleteUser(userId);
            if (response)
            {
                result.Result = ResponseStatus.Success;
                return Ok(result);
            }

            result.Result = ResponseStatus.Error;
            result.Message = "Delete operation failed";
            return BadRequest(result);
        }

    }
}
