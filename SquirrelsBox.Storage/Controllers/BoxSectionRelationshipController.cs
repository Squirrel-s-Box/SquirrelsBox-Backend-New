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
    public class BoxSectionRelationshipController : ControllerBase
    {
        private readonly IGenericServiceWithCascade<BoxSectionRelationship, BoxSectionRelationshipResponse> _service;
        private readonly IGenericReadService<BoxSectionRelationship, BoxSectionRelationshipResponse> _readService;
        private readonly IMapper _mapper;

        public BoxSectionRelationshipController(IGenericServiceWithCascade<BoxSectionRelationship, BoxSectionRelationshipResponse> service, IGenericReadService<BoxSectionRelationship, BoxSectionRelationshipResponse> readService, IMapper mapper)
        {
            _service = service;
            _readService = readService;
            _mapper = mapper;
        }

        [HttpGet("sectionlist/{boxId}")]
        public async Task<IActionResult> GetAllByIdCodeAsync(int boxId)
        {
            var model = await _readService.ListAllByIdCodeAsync(boxId);
            var list = model.Select(response => _mapper.Map<BoxSectionRelationship, ReadBoxSectionRelationshipResource>(response.Resource));
            return Ok(new { SectionList = list });
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveBoxSectionsListResource data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<SaveBoxSectionsListResource, BoxSectionRelationship>(data);

            var result = await _service.SaveAsync(model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(_mapper.Map<BaseResponse<BoxSectionRelationship>, ValidationResource>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateBoxSectionsListResource data, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<UpdateBoxSectionsListResource, BoxSectionRelationship>(data);
            model.Section.Id = id;
            var result = await _service.UpdateAsync(id, model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<BoxSectionRelationship>, ValidationResource>(result);
            return Ok(itemResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, bool cascade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.DeleteCascadeAsync(id, cascade);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<BoxSectionRelationship>, ValidationResource>(result);
            return Ok(itemResource);
        }
    }
}
