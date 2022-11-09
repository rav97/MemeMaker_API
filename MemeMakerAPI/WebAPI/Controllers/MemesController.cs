using Application.DTO;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
            var result = _memeManager.GetAllMemes();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddMeme(string memeName, IFormFile file)
        {
            return Ok();
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

        [HttpGet("template/{id}")]
        public IActionResult GetMemesOfTemplate(int id)
        {
            var result = _memeManager.GetMemesFromTemplate(id);

            return Ok(result);
        }
    }
}
