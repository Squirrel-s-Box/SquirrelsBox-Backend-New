using Azure.Storage.Blobs;
using Base.AzureServices.BlobStorage;
using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ASearchController : ControllerBase
    {
        private readonly IGenericSearchService _service;

        public ASearchController(IGenericSearchService service)
        {
            _service = service;
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
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Invalid image file.");
            }

            // Upload to Blob Storage
            var blobUrl = await ContainerService.UploadImageToBlobStorageAsync(image);

            // Return the URL to the frontend
            return Ok(new { url = blobUrl });
        }
    }
}
