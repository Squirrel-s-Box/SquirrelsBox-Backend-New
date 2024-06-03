using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Resource;

namespace SquirrelsBox.Authentication.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly IGenericService<UserData, UserDataResponse> _service;
        private readonly IMapper _mapper;

        public UserDataController(IGenericService<UserData, UserDataResponse> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{userCode}")]
        public async Task<UserDataResource> GetByCodeAsync(string userCode)
        {
            var model = await _service.FindByCodeAsync(userCode);
            var resources = _mapper.Map<UserData, UserDataResource>(model.Resource);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveUserDataResource resource)
        {
            if (!ModelState.IsValid)
            {
                var errorState = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

                var errorMessages = ErrorMessagesExtensions.GetErrorMessages(errorState);

                return BadRequest(errorMessages);
            }

            var model = _mapper.Map<SaveUserDataResource, UserData>(resource);
            var result = await _service.SaveAsync(model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<UserData>, ValidationResource>(result);
            return Ok(itemResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveUserDataResource resource)
        {
            if (!ModelState.IsValid)
            {
                var errorState = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

                var errorMessages = ErrorMessagesExtensions.GetErrorMessages(errorState);

                return BadRequest(errorMessages);
            }

            var model = _mapper.Map<SaveUserDataResource, UserData>(resource);
            var result = await _service.UpdateAsync(id, model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<UserData>, ValidationResource>(result);
            return Ok(itemResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<UserData>, ValidationResource>(result);
            return Ok(itemResource);
        }
    }
}
