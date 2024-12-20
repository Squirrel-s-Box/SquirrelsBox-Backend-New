﻿using Base.Generic.Domain.Services.Communication;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Base.Generic.Resources;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace SquirrelsBox.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoxController : ControllerBase
    {
        private readonly IGenericServiceWithCascade<Box, BoxResponse> _service;
        private readonly IGenericReadService<Box, BoxResponse> _readService;
        private readonly IMapper _mapper;

        public BoxController(IGenericServiceWithCascade<Box, BoxResponse> service, IGenericReadService<Box, BoxResponse> readService, IMapper mapper)
        {
            _service = service;
            _readService = readService;
            _mapper = mapper;
        }

        [HttpGet("boxlist/{userCode}")]
        public async Task<IActionResult> GetAllByUserCodeAsync(string userCode)
        {

            var model = await _readService.ListAllByUserCodeAsync(userCode);
            var list = model.Select(response => new { box = _mapper.Map<Box, ReadBoxResource>(response.Resource) });
            return Ok(new { BoxList = list });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.FindByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("{userCode}")]
        public async Task<IActionResult> PostAsync([FromBody] SaveBoxResource data, string userCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<SaveBoxResource, Box>(data);
            model.UserCodeOwner = userCode;
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            model.UserCodeLog = token;

            var result = await _service.SaveAsync(model);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(_mapper.Map<BaseResponse<Box>, ValidationResource>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateBoxResource data, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var model = _mapper.Map<UpdateBoxResource, Box>(data);
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            model.UserCodeLog = token;

            var result = await _service.UpdateAsync(id, model);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<Box>, ValidationResource>(result);
            return Ok(itemResource);
        }

        [HttpDelete("{id}/{cascade}")]
        public async Task<IActionResult> DeleteAsync(int id, bool cascade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _service.DeleteCascadeAsync(id, token, cascade);

            if (!result.Success)
                return BadRequest(result.Message);

            var itemResource = _mapper.Map<BaseResponse<Box>, ValidationResource>(result);
            return Ok(itemResource);
        }
    }
}
