using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.ViewModels;
using Mission.Entities.ViewModels.MissionApplication;
using Mission.Services.IService;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "user,admin")]
    public class ClientMissionController(IMissionService missionService) : Controller
    {
        private readonly IMissionService _missionService = missionService;

        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [Route("ClientSideMissionList/{id:int}")]
        public async Task<IActionResult> GetClientSideMissionList(int id)
        {
            var response = await _missionService.GetClientSideMissionList(id);
            var result = new ResponseResult() { Result = ResponseStatus.Success, data = response };
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("ApplyMission")]
        public async Task<IActionResult> ApplyMission(ApplyMissionRequestModel model)
        {
            var (response, message) = await _missionService.ApplyMission(model);
            var result = new ResponseResult() { Message = message };

            if (!response)
            {
                result.Result = ResponseStatus.Error;
                if (message == "Not Found")
                {
                    return NotFound(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            result.Result = ResponseStatus.Success;
            return Ok(result);
        }
    }
}
