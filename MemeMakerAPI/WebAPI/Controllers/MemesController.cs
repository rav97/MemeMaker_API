using Application.DTO;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for operation relates with Generated Memes
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiKeyAuthorisation]
    public class MemesController : ControllerBase
    {
        private readonly IMemeManager _memeManager;

        public MemesController(IMemeManager memeManager)
        {
            _memeManager = memeManager;
        }

        /// <summary>
        /// Get all memes
        /// </summary>
        /// <remarks>Gets content od GeneratedMeme table</remarks>
        /// <response code="200">Returns all data</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MemeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _memeManager.GetAllMemesAsync();

                return Ok(result);
            }
            catch(Exception e)
            {
                Log.Error(e, $"Exception in {nameof(Get)}()");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Upload meme
        /// </summary>
        /// <remarks>Upload generated meme to GeneratedMeme table, and save image on disk</remarks>
        /// <param name="templateId">Id of template used for creation of meme</param>
        /// <param name="memeName">Name of meme</param>
        /// <param name="image">PNG or IMG file containing meme</param>
        /// <response code="200">Upload succesful, data saved properly</response>
        /// <response code="400">Error during saving data</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMeme(int templateId, [SqlInjectionFilter]string memeName, IFormFile image)
        {
            try
            {
                var result = await _memeManager.AddMemeAsync(templateId, image);

                if (result)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch(Exception e)
            {
                Log.Error(e, $"Exception in {nameof(AddMeme)}() templateId = {templateId}, memeName = {memeName}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get random meme
        /// </summary>
        /// <remarks>Get random meme from GeneratedMeme table</remarks>
        /// <response code="200">Returns meme data and image</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("random")]
        [ProducesResponseType(typeof(MemeDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRandom()
        {
            try
            {
                var result = await _memeManager.GetRandomMemeAsync();

                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetRandom)}()");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get subset of memes (without image)
        /// </summary>
        /// <remarks>Returns subset of memes (without image data) that correspond to skip and take parameters</remarks>
        /// <param name="skip">How many position from GeneratedMeme to skip</param>
        /// <param name="take">How many position from GeneratedMeme to take</param>
        /// <response code="200">Returns list of memes. Possible to return empty list.</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("recent")]
        [ProducesResponseType(typeof(IEnumerable<MemeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMemes(int skip, int take)
        {
            try
            {
                var result = await _memeManager.GetRecentMemesAsync(skip, take);

                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetMemes)}() skip = {skip}, take = {take}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get subset of memes (with image)
        /// </summary>
        /// <remarks>Returns subset of memes (with image data) that correspond to skip and take parameters</remarks>
        /// <param name="skip">How many position from GeneratedMeme to skip</param>
        /// <param name="take">How many position from GeneratedMeme to take</param>
        /// <response code="200">Returns list of memes with image data. Possible to return empty list.</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("recent-images")]
        [ProducesResponseType(typeof(IEnumerable<MemeDataDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMemesImages(int skip, int take)
        {
            try
            {
                var result = await _memeManager.GetRecentMemesDataAsync(skip, take);

                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetMemesImages)}() skip = {skip}, take = {take}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get list of memes created from given template
        /// </summary>
        /// <remarks>Returns list of memes that was created from template with given ID</remarks>
        /// <param name="id">Template Id</param>
        /// <response code="200">Returns list of memes. Possible to return empty list.</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("template/{id}")]
        [ProducesResponseType(typeof(IEnumerable<MemeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMemesOfTemplate(int id)
        {
            try
            {
                var result = await _memeManager.GetMemesFromTemplateAsync(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetMemesOfTemplate)}() id = {id}");
                return StatusCode(500, false);
            }
        }
    }
}
