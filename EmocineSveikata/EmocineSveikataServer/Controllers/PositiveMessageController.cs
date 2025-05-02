using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Services.PositiveMessageService;

namespace EmocineSveikataServer.Controllers
{
    [Route("api/positiveMessages")]
    [ApiController]
    public class PositiveMessageController : ControllerBase
    {
        private readonly IPositiveMessageService _positiveMessageService;

        public PositiveMessageController(IPositiveMessageService positiveMessageService)
        {
            _positiveMessageService = positiveMessageService;
        }

        [HttpGet("random")]
        public IActionResult GetRandomPositiveMessage()
        {
            var positiveMessageDto = _positiveMessageService.GetRandomMessage();

            return Ok(positiveMessageDto);
        }
    }
}
