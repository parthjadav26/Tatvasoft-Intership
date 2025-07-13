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
    public class MissionSkillController(IMissionSkillService missionSkillService) : Controller
    {
        private readonly IMissionSkillService _missionSkillService = missionSkillService;

        [HttpPost]
        [Route("AddMissionSkill")]
        public async Task<IActionResult> AddMissionSkill(UpsertMissionSkillRequestModel model)
        {
            await _missionSkillService.AddMissionSkillAsync(model);

            var result = new ResponseResult()
            {
                Result = ResponseStatus.Success,
                Message = "New Mission Skill Added Successfully"
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMissionSkillList")]
        public async Task<IActionResult> GetMissionSkill()
        {
            var response = await _missionSkillService.GetMissionSkillAsync();

            var result = new ResponseResult()
            {
                data = response,
                Result = ResponseStatus.Success
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMissionSkillById/{id:int}")]
        public async Task<IActionResult> GetMissionSkillById(int id)
        {
            var response = await _missionSkillService.GetMissionSkillByIdAsync(id);

            var result = new ResponseResult();

            if (response == null)
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Skill Not Found";
                return NotFound(result);
            }

            result.Result = ResponseStatus.Success;
            result.data = response;
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateMissionSkill")]

        public async Task<IActionResult> UpdateMissionSkill(UpsertMissionSkillRequestModel model)
        {
            var response = await _missionSkillService.UpdateMissionSkillAsync(model);

            var result = new ResponseResult();

            if (response)
            {
                result.Result = ResponseStatus.Success;
                return Ok(result);
            }
            else
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Skill Record not Found";
                return NotFound(result);
            }
        }

        [HttpDelete]
        [Route("DeleteMissionSkill/{id:int}")]
        public async Task<IActionResult> DeleteMissionSkill(int id)
        {
            var response = await _missionSkillService.DeleteMissionSkillAsync(id);

            var result = new ResponseResult();

            if (response)
            {
                result.Result = ResponseStatus.Success;
                return Ok(result);
            }
            else
            {
                result.Result = ResponseStatus.Error;
                result.Message = "Mission Skill not Found";
                return NotFound(result);
            }
        }

    }
}
