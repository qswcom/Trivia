using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Com.Qsw.TriviaClient.ConsoleClient.Main.Extension;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class UserStateInfoService : IUserStateInfoService
    {
        private const string UrlPath = "api/userstateinfo";
        private readonly IServerInfoService serviceInfoService;
        private readonly IUserInfoService userInfoService;

        public UserStateInfoService(IServerInfoService serviceInfoService, IUserInfoService userInfoService)
        {
            this.serviceInfoService = serviceInfoService;
            this.userInfoService = userInfoService;
        }

        public async Task<UserStateInfo> GetOrCreate()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage =
                await httpClient.PostAsJsonAsync<string>($"{UrlPath}/get-or-create", null);
            httpResponseMessage.EnsureSuccessResponse();
            return await httpResponseMessage.Content.ReadFromJsonAsync<UserStateInfo>();
        }
    }
}