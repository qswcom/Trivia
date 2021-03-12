using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Com.Qsw.TriviaClient.ConsoleClient.Main.Extension;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class GameService : IGameService
    {
        private const string UrlPath = "api/game";
        private readonly IServerInfoService serviceInfoService;
        private readonly IUserInfoService userInfoService;

        public GameService(IServerInfoService serviceInfoService, IUserInfoService userInfoService)
        {
            this.serviceInfoService = serviceInfoService;
            this.userInfoService = userInfoService;
        }

        public async Task<GameInfo> RetrieveQuestion(long gameId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = 
                await httpClient.PostAsJsonAsync<string>($"{UrlPath}/retrieve/{gameId}", null);
            httpResponseMessage.EnsureSuccessResponse();
            return await httpResponseMessage.Content.ReadFromJsonAsync<GameInfo>();
        }

        public async Task SubmitAnswer(long gameId, string answer)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = 
                await httpClient.PostAsJsonAsync($"{UrlPath}/submit-answer/{gameId}", answer);
            httpResponseMessage.EnsureSuccessResponse();
        }

        public async Task LeftGame(long gameId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = 
                await httpClient.PostAsJsonAsync<string>($"{UrlPath}/leave/{gameId}", null);
            httpResponseMessage.EnsureSuccessResponse();
        }
    }
}