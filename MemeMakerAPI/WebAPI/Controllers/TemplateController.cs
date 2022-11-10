using Application.DTO;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateManager _templateManager;

        public TemplateController(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        [HttpGet("all")]
        public IActionResult GetAllTemplates()
        {
            var templates = _templateManager.GetAllTemplates();
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public IActionResult GetTemplate(int id)
        {
            var template = _templateManager.GetTemplateById(id);

            if (template != null)
                return Ok(template);
            else
                return NotFound();
        }

        [HttpGet("popular")]
        public IActionResult GetPopularTemplates(int limit)
        {
            var templates = _templateManager.GetPopularTemplates(limit);
            return Ok(templates);
        }

        [HttpGet("contains")]
        public IActionResult GetTemplatesWithName(string phrase)
        {
            var templates = _templateManager.GetTemplatesContainingPhrase(phrase);
            return Ok(templates);
        }

        [HttpPost]
        public IActionResult AddTemplate(string templateName, IFormFile image)
        {
            if (image == null)
                throw new ArgumentNullException();

            var result = _templateManager.AddTemplate(templateName, image);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("usage")]
        public IActionResult SaveMemeUsage(int templateId)
        {
            var result = _templateManager.SaveTemplateUsage(templateId);

            //This functionality is not necessary to succed every time
            return Ok();
        }

    }
}
