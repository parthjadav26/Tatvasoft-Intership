using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.ViewModels;
using Mission.Entities.ViewModels.Mission;
using Mission.Services.IService;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class MissionThemeController(IMissionThemeService missionThemeService) : Controller
    {
        private readonly IMissionThemeService _missionThemeService = missionThemeService;

        [HttpPost]
        [Route("AddMissionTheme")]
        public async Task<IActionResult> AddMissionTheme(UpsertMissionThemeRequestModel model)
        {
            await _missionThemeService.AddMissionThemeAsync(model);

            var result = new ResponseResult()
            {
                Result = ResponseStatus.Success,
                Message = "New Mission Theme Added Successfully"
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMissionThemeList")]
        public async Task<IActionResult> GetMissionTheme()
        {
            var response = await _missionThemeService.GetMissionThemeAsync();

            var result = new ResponseResult()
            {
                data = response,
                Result = ResponseStatus.Success
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMissionThemeById/{id:int}")]
        public async Task<IActionResult> GetMissionThemeById(int id)
        {
            var response = await _missionThemeService.GetMissionThemeByIdAsync(id);

            var result = new ResponseResult();

            if (response == null)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Theme Not Found";
                return NotFound(result);
            }

            result.Result = ResponseStatus.Success;
            result.data = response;
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateMissionTheme")]

        public async Task<IActionResult> UpdateMissionTheme(UpsertMissionThemeRequestModel model)
        {
            var response = await _missionThemeService.UpdateMissionThemeAsync(model);

            var result = new ResponseResult();

            if (response)
            {
                result.Result = ResponseStatus.Success;
                return Ok(result);
            }
            else
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Theme Record not Found";
                return NotFound(result);
            }
        }

        [HttpDelete]
        [Route("DeleteMissionTheme/{id:int}")]
        public async Task<IActionResult> DeleteMissionTheme(int id)
        {
            var response = await _missionThemeService.DeleteMissionThemeAsync(id);

            var result = new ResponseResult();

            if (response)
            {
                result.Result = ResponseStatus.Success;
                return Ok(result);
            }
            else
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Theme not Found";
                return NotFound(result);
            }
        }

    }
}
