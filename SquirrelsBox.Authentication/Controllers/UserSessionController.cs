using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Resource;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSessionController : ControllerBase
    {
        private readonly IGenericService<UserSession, UserSessionResponse> _service;
        private readonly IGenericService<AccessSession, AccessSessionResponse> _accessSessionService;
        private readonly IMapper _mapper;

        public UserSessionController(IGenericService<UserSession, UserSessionResponse> service, IGenericService<AccessSession, AccessSessionResponse> userService, IMapper mapper)
        {
            _service = service;
            _accessSessionService = userService;
            _mapper = mapper;
        }

        [HttpPost("{userCode}")]
        public async Task<IActionResult> PostAsync([FromBody] SaveTokenResource data, string userCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var response = await _accessSessionService.FindByCodeAsync(userCode);
            if (!response.Success)
                return BadRequest(response.Message);

            var model = _mapper.Map<SaveTokenResource, SaveFoundTokenByUserIdResource>(data);
            model.UserId = response.Resource.Id;

            var result = await _service.SaveAsync(_mapper.Map<SaveFoundTokenByUserIdResource, UserSession>(model));
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(_mapper.Map<BaseResponse<UserSession>, ValidationResource>(result));
        }

        [HttpPut("{userCode}")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateUserSessionResource data, string userCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var findUser = await _accessSessionService.FindByCodeAsync(userCode);

            if (findUser.Resource != null)
            {
                var model = _mapper.Map<UpdateUserSessionResource, UserSession>(data);
                var result = await _service.UpdateAsync(findUser.Resource.Id, model);

                if (!result.Success)
                    return BadRequest(result.Message);

                var itemResource = _mapper.Map<BaseResponse<UserSession>, ValidationResource>(result);
                return Ok(itemResource);
            }
            else
            {
                return BadRequest(findUser.Message);
            }
        }

        [HttpDelete("{userCode}")]
        public async Task<IActionResult> DeleteAsync(string userCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var findUser = await _accessSessionService.FindByCodeAsync(userCode);

            if (findUser.Resource != null)
            {
                var result = await _service.DeleteAsync(findUser.Resource.Id);

                if (!result.Success)
                    return BadRequest(result.Message);

                var itemResource = _mapper.Map<BaseResponse<UserSession>, ValidationResource>(result);
                return Ok(itemResource);
            }
            else
            {
                return BadRequest(findUser.Message);
            }
        }
    }
}
