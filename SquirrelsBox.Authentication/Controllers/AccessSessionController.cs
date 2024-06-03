using AutoMapper;
using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.Data;
using SquirrelsBox.Authentication.Resource;
using Base.Security;
using Microsoft.AspNetCore.Authorization;
using SquirrelsBox.Authentication.Domain.Interfaces;

namespace SquirrelsBox.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessSessionController : ControllerBase
    {
        private readonly IGenericService<Domain.Models.AccessSession, AccessSessionResponse> _service;
        private readonly IAccessSessionService _accessSessionService;
        private readonly IMapper _mapper;

        public AccessSessionController(IGenericService<AccessSession, AccessSessionResponse> service, IAccessSessionService accessSessionService, IMapper mapper)
        {
            _service = service;
            _accessSessionService = accessSessionService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("ProveToken")]
        public async Task<IActionResult> ProveToken()
        {
            var result = true;
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Login request)
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

            Domain.Models.AccessSession model = new Domain.Models.AccessSession();
            model.Username = request.Username;
            byte[] hashedPassword = Sha256Enc.HashPassword(request.Password);
            model.Password = hashedPassword;
            var result = await _service.SaveAsync(model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<AccessSession>, ValidationResource>(result);

            var res = new { 
                itemResource, 
                code = model.Code,
                RefreshToken = model.RefreshToken,
                token = result.Token};
            return Ok(res);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] AccessRefreshTokenRequest request)
        {
            try
            {
                var response = await _accessSessionService.VerifyRefreshToken(request.RefreshToken, request.Code);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> PutAsync(string code)
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

            var findUser = await _service.FindByCodeAsync(code);

            if (findUser.Resource != null)
            {
                Domain.Models.AccessSession model = new Domain.Models.AccessSession();
                var result = await _service.UpdateAsync(findUser.Resource.Id, model);

                if (!result.Success)
                    return BadRequest(result.Message);

                var itemResource = _mapper.Map<BaseResponse<AccessSession>, ValidationResource>(result);
                return Ok(itemResource);
            }
            else
            {
                return BadRequest(findUser.Message);
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteAsync(string code)
        {
            var findUser = await _service.FindByCodeAsync(code);

            if (findUser.Resource != null)
            {
                var result = await _service.DeleteAsync(findUser.Resource.Id);

                if (!result.Success)
                    return BadRequest(result.Message);

                var itemResource = _mapper.Map<BaseResponse<AccessSession>, ValidationResource>(result);
                return Ok(itemResource);
            }
            else
            {
                return BadRequest(findUser.Message);
            }
        }
    }
}
