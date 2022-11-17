using Application.DTO;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiKeyAuthorisation]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateManager _templateManager;

        public TemplateController(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        /// <summary>
        /// Get template by id
        /// </summary>
        /// <remarks>Returns template data and image with given Id</remarks>
        /// <param name="id">Id of template</param>
        /// <response code="200">Image was found and is returned</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="404">Template with given Id was not found</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TemplateDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTemplate(int id)
        {
            try
            {
                var template = await _templateManager.GetTemplateByIdAsync(id);

                if (template != null)
                    return Ok(template);
                else
                    return NotFound(false);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetTemplate)}() id = {id}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get popular templates
        /// </summary>
        /// <remarks>Returns list of most popular templates limited to specified number</remarks>
        /// <param name="limit">Limit number of templates to return</param>
        /// <response code="200">Returns list of templates data</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(IEnumerable<TemplateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPopularTemplates(int limit)
        {
            try
            {
                var templates = await _templateManager.GetPopularTemplatesAsync(limit);
                return Ok(templates);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetPopularTemplates)}() limit = {limit}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Get list of templates containing given phrase
        /// </summary>
        /// <remarks>Returns list of templates data which name or filename contains given phrase</remarks>
        /// <param name="phrase">Phrase to search template</param>
        /// <response code="200">List of templates data. Possible empty list.</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpGet("contains")]
        [ProducesResponseType(typeof(IEnumerable<TemplateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTemplatesWithName([SqlInjectionFilter] string phrase)
        {
            try
            {
                var templates = await _templateManager.GetTemplatesContainingPhraseAsync(phrase);
                return Ok(templates);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(GetTemplatesWithName)}() phrase = {phrase}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Upload new template
        /// </summary>
        /// <remarks>Upload template with given name</remarks>
        /// <param name="templateName">Name of meme template</param>
        /// <param name="image">PNG or JPG file with image</param>
        /// <response code="200">Upload succesful</response>
        /// <response code="400">Error during saving data</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        /// <exception cref="ArgumentNullException">Throw exception if image is null</exception>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTemplate([SqlInjectionFilter]string templateName, IFormFile image)
        {
            try
            {
                if (image == null)
                    throw new ArgumentNullException();

                var result = await _templateManager.AddTemplateAsync(templateName, image);

                if (result)
                    return Ok(true);
                else
                    return BadRequest(false);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(AddTemplate)}() templateName = {templateName}");
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Save usage of template
        /// </summary>
        /// <remarks>Saves id of template that was used for generation of meme. This data allows to suggest most popular memes.</remarks>
        /// <param name="templateId">Id of template which was used</param>
        /// <response code="200">Status is returned always if there is no authorisations issues or critical errors</response>
        /// <response code="401">ApiKey is not provided, invalid or expired</response>
        /// <response code="500">Error during execution of request</response>
        [HttpPost("usage")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveMemeUsage(int templateId)
        {
            try
            {
                var result = await _templateManager.SaveTemplateUsageAsync(templateId);

                //This functionality is not necessary to succed every time
                return Ok(true);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception in {nameof(SaveMemeUsage)}() templateId = {templateId}");
                return StatusCode(500, false);
            }
        }
    }

}
