using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Week2Queue.Core.Interfaces;
using Week2Queue.Core.Models;
using Week2Queue.Core.DTO;

namespace Week2Queue.Controllers
{
    [Route("queue")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository _repository;
        public QueueController(IQueueRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add")]
        public async Task<ActionResult> Add(MessageDTO message)
        {
            Message mes = new Message()
            {
                Id = Guid.NewGuid(),
                Type = message.Type,
                Handled = false,
                JsonContent = message.JsonContent,
                AddedAt = DateTime.Now
            };
            if (await _repository.AddMessage(mes))
            {
                return Ok("A new message was added succesfully!");
            }
            return BadRequest("Error");

        }

        [HttpGet("handled/{messageId}")]
        public async Task<ActionResult> SetHandled(Guid messageId)
        {
            if(await _repository.SetHandled(messageId))
            {
                return Ok("Message was set to handled!");
            }
            return BadRequest("Error");
        }

        [HttpGet("retrieve/email")]
        public async Task<ActionResult> GetUnhandledEmail()
        {
            var messages = await _repository.GetUnhandledEmailMessage();
            return Ok(messages);
        }

        [HttpGet("retrieve/log")]
        public async Task<ActionResult> GetUnhandledLog()
        {
            var messages = await _repository.GetUnhandledLoggingMessage();
            return Ok(messages);
        }
    }
}
