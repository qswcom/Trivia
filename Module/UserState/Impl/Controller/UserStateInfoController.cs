using System.Threading.Tasks;
using Com.Qsw.Framework.Context.Web;
using Com.Qsw.Module.UserState.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Com.Qsw.Module.UserState.Impl
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserStateInfoController : Controller
    {
        private readonly IUserStateInfoService userStateInfoService;
        private readonly ICallContextService callContextService;

        public UserStateInfoController(IUserStateInfoService userStateInfoService,
            ICallContextService callContextService)
        {
            this.userStateInfoService = userStateInfoService;
            this.callContextService = callContextService;
        }

        [HttpPost("get-or-create")]
        public async Task<IActionResult> GetOrCreate()
        {
            string userId = callContextService.UserId;
            UserStateInfo userStateInfo = await userStateInfoService.GetOrCreate(userId);
            return new ObjectResult(userStateInfo);
        }
    }
}