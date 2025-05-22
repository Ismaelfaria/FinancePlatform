using Microsoft.AspNetCore.Mvc;
using SendNotification.Application.Email;
using SendNotification.Domain.Interfaces;

namespace SendNotification.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public NotificationController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            await _emailService.SendAsync(request.To, request.Subject, request.Body);
            return Ok("Email enviado com sucesso.");
        }
    }
}
