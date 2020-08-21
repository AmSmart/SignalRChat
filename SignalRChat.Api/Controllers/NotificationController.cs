using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Api.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("posttoall")]
        public async Task<IActionResult> PostToAll([FromQuery] string userName, [FromQuery] string message)
        {
            await _hubContext.Clients.All.SendAsync("notification", userName, message);
            return Ok("Notification sent");
        }

        [HttpPost("joingroup")]
        public async Task<IActionResult> AddToGroup([FromQuery] string connectionId, [FromQuery] string groupName, [FromQuery] string userName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            await _hubContext.Clients.Group(groupName).SendAsync(groupName, $"{userName} has joined {groupName}");
            return Ok($"{userName} has joined {groupName}");
        }

        [HttpPost("posttochannel")]
        public async Task<IActionResult> PostToGroup([FromQuery] string groupName, [FromQuery] string userName, [FromQuery] string message)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(groupName, userName, message);
            return Ok("Notification sent");
        }
    }
}
