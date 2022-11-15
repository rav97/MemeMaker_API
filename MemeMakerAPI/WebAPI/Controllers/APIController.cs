using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for general API purposes
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IApiKeyManager _apiKeyManager;

        public APIController(IApiKeyManager keyManager)
        {
            _apiKeyManager = keyManager;
        }

        /// <summary>
        /// Check API connection
        /// </summary>
        /// <remarks>If there is connection to API, this endpoint returns true</remarks>
        /// <response code="200">Connection to API exists</response>
        [HttpGet("IsAlive")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public IActionResult IsAlive()
        {
            return Ok(true);
        }

        /// <summary>
        /// Generate ApiKey
        /// </summary>
        /// <remarks>Generate and save new API key that is used to authorize API Requests. NOTE: This endpoint should not be publicly accessed in production enviroment</remarks>
        /// <response code="200">Returns API key</response>
        /// <response code="500">An error occured during execution of method</response>
        /// <returns>ApiKey</returns>
        [HttpGet("ApiKey")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public IActionResult GetApiKey()
        {
            try
            {
                var key = _apiKeyManager.GenerateNewApiKey();
                return Ok(key);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, false);
            }
        }

        /// <summary>
        /// Check ApiKey
        /// </summary>
        /// <remarks>Check if given API Key is valid and not expired</remarks>
        /// <response code="200">Key is valid and active - returns true</response>
        /// <response code="400">Key is invalid or expired - returns message</response>
        /// <response code="500">An error occured during execution of method</response>
        /// <returns>ApiKey</returns>
        [HttpPut("CheckApiKey")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public IActionResult CheckApiKey(string key)
        {
            try
            {
                var result = _apiKeyManager.CheckApiKey(key);
                if (result)
                    return Ok(true);
                else
                    return BadRequest("Key is invalid or expired");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, false);
            }
        }
    }
}
