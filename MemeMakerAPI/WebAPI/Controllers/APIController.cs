using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        public APIController()
        {

        }

        [HttpGet("IsAlive")]
        public IActionResult IsAlive()
        {
            return Ok(true);
        }
    }
}
