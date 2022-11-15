using Application.Managers;
using Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Attributes
{

    [Obsolete("This attribute can not be used on its own because of need of dependency injection. Use ApiKeyAuthorisationAttribute instead")]
    public class ApiKeyAuthorisationBaseAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _header = "X-Api-Key";
        private readonly IApiKeyManager _apiKeyManager;

        public ApiKeyAuthorisationBaseAttribute(IApiKeyManager keyManager)
        {
            _apiKeyManager = keyManager;
            Console.WriteLine();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool canExtract = true;

            if(_header == "")
                canExtract = false;

            if (!context.HttpContext.Request.Headers.ContainsKey(_header))
                canExtract = false;

            if(canExtract)
            {
                var apiKey = context.HttpContext.Request.Headers[_header].ToString();
                canExtract = _apiKeyManager.CheckApiKey(apiKey);
            }

            if(!canExtract)
            {
                context.Result = new JsonResult(false) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
