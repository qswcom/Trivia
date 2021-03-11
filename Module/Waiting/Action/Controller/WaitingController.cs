using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Module.Room.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Com.Qsw.Module.Waiting.Action
{
    [Route("api/[Controller]")]
    [ApiController]
    public class WaitingController : Controller
    {
        private readonly ICallContextService callContextService;
        private readonly IWaitingActionService waitingActionService;

        public WaitingController(ICallContextService callContextService, IWaitingActionService waitingActionService)
        {
            this.callContextService = callContextService;
            this.waitingActionService = waitingActionService;
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> LoadAllRooms()
        {
            string userId = callContextService.UserId;
            IList<RoomInfo> roomInfos = await waitingActionService.LoadAll(userId);
            return new ObjectResult(roomInfos);
        }

        [HttpPost("room")]
        public async Task<IActionResult> CreateNewRoom()
        {
            string userId = callContextService.UserId;

            await waitingActionService.CreateRoom(userId);
            return Ok();
        }

        [HttpPut("room/{roomId}")]
        public async Task<IActionResult> JoinRoom([FromRoute] long roomId)
        {
            string userId = callContextService.UserId;
            await waitingActionService.JoinRoom(userId, roomId);
            return Ok();
        }
    }
}