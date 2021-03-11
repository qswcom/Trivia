using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Module.Game.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Com.Qsw.Module.Game.Action
{
    [Route("api/[Controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly ICallContextService callContextService;
        private readonly IGameActionService gameActionService;

        public GameController(ICallContextService callContextService, IGameActionService gameActionService)
        {
            this.callContextService = callContextService;
            this.gameActionService = gameActionService;
        }

        [HttpPost("retrieve/{gameId}")]
        public async Task<IActionResult> RetrieveGame([FromRoute] long gameId)
        {
            string userId = callContextService.UserId;
            GameInfo gameInfo = await gameActionService.RetrieveQuestion(gameId, userId);
            return new ObjectResult(gameInfo);
        }

        [HttpPost("submit-answer/{gameId}")]
        public async Task<IActionResult> RetrieveGame([FromRoute] long gameId, [FromBody] string answer)
        {
            string userId = callContextService.UserId;
            await gameActionService.SubmitAnswer(gameId, userId, answer);
            return Ok();
        }

        [HttpPost("leave/{gameId}")]
        public async Task<IActionResult> LeaveGame([FromRoute] long gameId)
        {
            string userId = callContextService.UserId;
            await gameActionService.LeftGame(gameId, userId);
            return Ok();
        }
    }
}