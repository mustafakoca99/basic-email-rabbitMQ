using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Api.Services.Abstract;
using RabbitMQ.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailNotificationController : ControllerBase
    {
        private IPublishService _publishService;

        public MailNotificationController(IPublishService publishService)
        {
            _publishService = publishService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMail(EmailSendRequestDto emailSendRequestDto)
        {
            await _publishService.PublishQuee(emailSendRequestDto);
            return Ok("Send Email");
        }
    }
}
