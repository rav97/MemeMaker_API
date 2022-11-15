using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Attributes
{
    /// <summary>
    /// I cannot use dependency injection on attribute, but TypeFilter can, so I created wrapper for ApiKeyAuthorisation
    /// </summary>
    public class ApiKeyAuthorisationAttribute : TypeFilterAttribute
    {
        public ApiKeyAuthorisationAttribute():base(typeof(ApiKeyAuthorisationBaseAttribute))
        {

        }
    }
}
