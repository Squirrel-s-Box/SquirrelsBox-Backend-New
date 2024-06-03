using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Resource;
using AutoMapper;

namespace SquirrelsBox.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignedPermissionController : ControllerBase
    {
        private readonly IGenericReadService<AssignedPermission, AssignedPermissionResponse> _service;
        private readonly IGenericService<AssignedPermission, AssignedPermissionResponse> _genericService;
        private readonly IMapper _mapper;

        public AssignedPermissionController(IGenericReadService<AssignedPermission, AssignedPermissionResponse> service, IGenericService<AssignedPermission, AssignedPermissionResponse> genericService, IMapper mapper)
        {
            _service = service;
            _genericService = genericService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllByUserCodeAsync(string userCode)
        {
            var model = await _service.ListAllByUserCodeAsync(userCode);
            var permissions = model.Select(response => new { code = response.PermissionResource.Value });
            return Ok(new { Permissions = permissions });
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveAssignedPermissionResource model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var data = _mapper.Map<SaveAssignedPermissionResource, AssignedPermission>(model);
            var result = await _genericService.SaveAsync(data);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<AssignedPermission>, ValidationResource>(result);

            return Ok(itemResource);
        }
    }
}
