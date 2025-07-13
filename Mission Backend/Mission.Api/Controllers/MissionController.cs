using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.ViewModels;
using Mission.Entities.ViewModels.Mission;
using Mission.Entities.ViewModels.MissionApplication;
using Mission.Services.IService;

namespace Mission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class MissionController(IMissionService missionService) : ControllerBase
    {
        private readonly IMissionService _missionService = missionService;
        ResponseResult result = new ResponseResult();

        [HttpPost]
        [Route("AddMission")]
        public async Task<IActionResult> AddMission(AddMissionRequestModel model)
        {
            await _missionService.AddMissionRequestAsync(model);
            var result = new ResponseResult() { Result = ResponseStatus.Success, Message = "Mission Added Successfully" };
            return Ok(result);
        }

        [HttpGet]
        [Route("MissionList")]
        public async Task<IActionResult> GetMissionList()
        {
            var response = await _missionService.GetMissionList();
            var result = new ResponseResult() { Result = ResponseStatus.Success, data = response };
            return Ok(result);
        }

        [HttpGet]
        [Route("MissionDetailById/{id:int}")]
        public async Task<IActionResult> GetMissionById(int id)
        {
            var response = await _missionService.GetMissionById(id);
            return Ok(new ResponseResult() { data = response, Result = ResponseStatus.Success, Message = "" });
        }

        [HttpPost]
        [Route("UpdateMission")]
        public async Task<IActionResult> UpdateMission(MissionRequestViewModel model)
        {
            var response = await _missionService.UpdateMission(model);
            return Ok(new ResponseResult() { data = response, Result = ResponseStatus.Success, Message = "" });
        }

        [HttpDelete]
        [Route("DeleteMission/{missionId:int}")]
        public async Task<IActionResult> DeleteMission(int missionId)
        {
            var response = await _missionService.DeleteMission(missionId);
            return Ok(new ResponseResult() { data = response, Result = ResponseStatus.Success, Message = "" });
        }

        [HttpGet]
        [Route("GetMissionThemeList")]
        public ResponseResult MissionThemeList()
        {
            try
            {
                result.data = _missionService.MissionThemeList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("GetMissionSkillList")]
        public ResponseResult MissionSkillList()
        {
            try
            {
                result.data = _missionService.MissionSkillList();
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("MissionApplicationList")]

        public async Task<IActionResult> GetMissionApplicationList()
        {
            var response = await _missionService.GetMissionApplicationList();
            return Ok(new ResponseResult() { data = response, Result = ResponseStatus.Success, Message = "" });
        }

        [HttpPost]
        [Route("MissionApplicationApprove")]
        public async Task<IActionResult> MissionApplicationApprove(MissionApplicationResponseModel model)
        {
            var response = await _missionService.MissionApplicationApprove(model);

            if (!response)
            {
                return NotFound(new ResponseResult()
                {
                    Result = ResponseStatus.Error,
                    Message = "Mission Application Not Found"
                });
            }

            return Ok(new ResponseResult()
            {
                data = response,
                Result = ResponseStatus.Success,
                Message = ""
            });
        }

        [HttpPost]
        [Route("MissionApplicationDelete")]
        public async Task<IActionResult> MissionApplicationDelete(MissionApplicationResponseModel model)
        {
            var response = await _missionService.MissionApplicationDelete(model);

            if (!response)
            {
                return NotFound(new ResponseResult()
                {
                    Result = ResponseStatus.Error,
                    Message = "Mission Application Not Found"
                });
            }

            return Ok(new ResponseResult()
            {
                data = response,
                Result = ResponseStatus.Success,
                Message = ""
            });
        }

    }
}
