using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Com.Qsw.TriviaClient.ConsoleClient.Main.Extension;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class RoomService : IRoomService
    {
        private const string UrlPath = "api/room";
        private readonly IServerInfoService serviceInfoService;
        private readonly IUserInfoService userInfoService;

        public RoomService(IServerInfoService serviceInfoService, IUserInfoService userInfoService)
        {
            this.serviceInfoService = serviceInfoService;
            this.userInfoService = userInfoService;
        }
        
        public async Task<RoomInfo> Get(long roomId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{UrlPath}/{roomId}");
            httpResponseMessage.EnsureSuccessResponse();
            return await httpResponseMessage.Content.ReadFromJsonAsync<RoomInfo>();
        }

        public async Task LeaveRoom(long roomId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsJsonAsync<string>($"{UrlPath}/leave/{roomId}", null);
            httpResponseMessage.EnsureSuccessResponse();
        }

        public async Task StartGame(long roomId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsJsonAsync<string>($"{UrlPath}/start/{roomId}", null);
            httpResponseMessage.EnsureSuccessResponse();
        }
    }
}