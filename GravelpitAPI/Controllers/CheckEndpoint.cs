using Microsoft.AspNetCore.Mvc;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckEndpoint : ControllerBase
    {
        public string Get()
        {
            return "200 OK";
        }
    }
}
