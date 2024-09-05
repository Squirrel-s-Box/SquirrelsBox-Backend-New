using Azure.Storage.Blobs;
using Base.AzureServices.BlobStorage;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Resources;

namespace SquirrelsBox.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ASearchController : ControllerBase
    {
        private readonly IGenericSearchService _service;
        private readonly IContainerService _imageUploadService;

        public ASearchController(IGenericSearchService service, IContainerService imageUploadService)
        {
            _service = service;
            _imageUploadService = imageUploadService;
        }

        [HttpGet("GetCounterAsync")]
        public async Task<IActionResult> GetCounterAsync(string userCode)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.CounterByUserCodeAsync(userCode);

            return Ok(result);
        }

        [HttpGet("SearchAsync")]
        public async Task<IActionResult> SearchAsync(string text, int type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.ListFinderAsync(text, type);

            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] SaveSectionItemResource data)
        {
            string? blobUrl = string.Empty;
            if (data.Image != null && data.Image.Length > 0)
            {
                blobUrl = await _imageUploadService.UploadImageToBlobStorageAsync(data.Item.Name, data.Image);
            }

            // Return the URL to the frontend
            return Ok(new { url = blobUrl });
        }
    }
}
