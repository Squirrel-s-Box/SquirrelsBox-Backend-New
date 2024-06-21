using Base.Generic.Domain.Services;
using Base.Generic.Extensions;
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

        [HttpGet]
        public async Task<IActionResult> GetAsync(string text, int type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessagesExtensions.GetErrorMessages(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList())));

            var result = await _service.ListFinderAsync(text, type);

            return Ok(result);
        }
    }
}
