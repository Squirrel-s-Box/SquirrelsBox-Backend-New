using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;

namespace SquirrelsBox.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemSpecRelationshipController : ControllerBase
    {
        private readonly IGenericServiceWithMassive<Spec, ItemSpecRelationshipResponse> _service;
        private readonly IGenericReadService<Spec, ItemSpecRelationshipResponse> _readService;
        private readonly IMapper _mapper;

        public ItemSpecRelationshipController(IGenericServiceWithMassive<Spec, ItemSpecRelationshipResponse> service, IGenericReadService<Spec, ItemSpecRelationshipResponse> readService, IMapper mapper)
        {
            _service = service;
            _readService = readService;
            _mapper = mapper;
        }

        [HttpGet("sectionlist/{itemId}")]
        public async Task<IActionResult> GetAllByIdCodeAsync(int itemId)
        {
            var model = await _readService.ListAllByIdCodeAsync(itemId);
            var list = model.Select(response => _mapper.Map<Spec, ReadItemSpecRelationshipResource>(response.Resource));
            return Ok(new { SectionList = list });
        }

        [HttpPost("PostMassiveAsync")]
        public async Task<IActionResult> PostAsync([FromBody] SaveSpecMassiveResource data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<ICollection<SaveSpecResource>, ICollection<Spec>>(data.Specs);

            var result = await _service.SaveMassiveAsync(model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(_mapper.Map<BaseResponse<Spec>, ValidationResource>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateSpecResource data, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<UpdateSpecResource, Spec>(data);
            var result = await _service.UpdateAsync(id, model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<Spec>, ValidationResource>(result);
            return Ok(itemResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<Spec>, ValidationResource>(result);
            return Ok(itemResource);
        }
    }
}
