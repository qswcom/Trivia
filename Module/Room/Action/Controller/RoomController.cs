using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Module.Room.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Com.Qsw.Module.Room.Action
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly ICallContextService callContextService;
        private readonly IRoomActionService roomActionService;

        public RoomController(ICallContextService callContextService, IRoomActionService roomActionService)
        {
            this.callContextService = callContextService;
            this.roomActionService = roomActionService;
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> Get([FromRoute] long roomId)
        {
            RoomInfo roomInfo = await roomActionService.Get(roomId);
            return new ObjectResult(roomInfo);
        }

        [HttpPut("leave/{roomId}")]
        public async Task<IActionResult> LeaveRoom([FromRoute] long roomId)
        {
            string userId = callContextService.UserId;
            await roomActionService.LeaveRoom(userId, roomId);
            return Ok();
        }

        [HttpPut("start/{roomId}")]
        public async Task<IActionResult> StartGame([FromRoute] long roomId)
        {
            string userId = callContextService.UserId;
            await roomActionService.StartGame(userId, roomId);
            return Ok();
        }
    }
}