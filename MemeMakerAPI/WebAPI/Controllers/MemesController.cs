using Application.DTO;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _memeManager.GetAllMemes();

                return Ok(result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult AddMeme(int templateId, string memeName, IFormFile image)
        {
            var result = _memeManager.AddMeme(templateId, image);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("random")]
        public IActionResult GetRandom()
        {
            var result = _memeManager.GetRandomMeme();

            return Ok(result);
        }

        [HttpGet("recent")]
        public IActionResult GetMemes(int skip, int take)
        {
            var result = _memeManager.GetRecentMemes(skip, take);

            return Ok(result);
        }

        [HttpGet("recent-images")]
        public IActionResult GetMemesImages(int skip, int take)
        {
            var result = _memeManager.GetRecentMemesData(skip, take);

            return Ok(result);
        }

        [HttpGet("template/{id}")]
        public IActionResult GetMemesOfTemplate(int id)
        {
            var result = _memeManager.GetMemesFromTemplate(id);

            return Ok(result);
        }
    }
}
