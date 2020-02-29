using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace amesensible.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OidcConfigurationController : ControllerBase
    {
        private readonly ILogger<OidcConfigurationController> logger;

        public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider, ILogger<OidcConfigurationController> _logger)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            logger = _logger;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetClientRequestParameters()
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, "amesensible");
            return Ok(parameters);
        }
    }
}
