using AutoMapper;
using Base.Generic.Domain.Services;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;
using System.Collections.Generic;

namespace SquirrelsBox.Storage.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SharedBoxController : ControllerBase
    {
        private readonly IGenericService<SharedBox, SharedBoxResponse> _service;
        private readonly IGenericReadService<SharedBox, SharedBoxResponse> _readService;
        private readonly IMapper _mapper;

        public SharedBoxController(IGenericService<SharedBox, SharedBoxResponse> service, Base.Generic.Domain.Services.IGenericReadService<SharedBox, SharedBoxResponse> readService, IMapper mapper)
        {
            _service = service;
            _readService = readService;
            _mapper = mapper;
        }

        [HttpGet("sharedboxlist/{userCode}")]
        public async Task<IActionResult> GetAllByUserCodeAsync(string userCode)
        {

            var model = await _readService.ListAllByUserCodeAsync(userCode);
            var list = model.Select(response => new { box = _mapper.Map<SharedBox, ReadSharedBoxResource>(response.Resource) });
            return Ok(new { SharedBoxList = list });
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveSharedBoxResource data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<SaveSharedBoxResource, SharedBox>(data);

            var result = await _service.SaveAsync(model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(_mapper.Map<BaseResponse<SharedBox>, ValidationResource>(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<SharedBox>, ValidationResource>(result);
            return Ok(itemResource);
        }
    }
}
