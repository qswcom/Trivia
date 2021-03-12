using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Com.Qsw.TriviaClient.ConsoleClient.Main.Extension;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class WaitingService : IWaitingService
    {
        private const string UrlPath = "api/waiting";
        private readonly IServerInfoService serviceInfoService;
        private readonly IUserInfoService userInfoService;

        public WaitingService(IServerInfoService serviceInfoService, IUserInfoService userInfoService)
        {
            this.serviceInfoService = serviceInfoService;
            this.userInfoService = userInfoService;
        }
        
        public async Task<IList<RoomInfo>> LoadAll()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"{UrlPath}/rooms");
            httpResponseMessage.EnsureSuccessResponse();
            return await httpResponseMessage.Content.ReadFromJsonAsync<IList<RoomInfo>>();
        }

        public async Task CreateRoom()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync<string>($"{UrlPath}/room", null);
            httpResponseMessage.EnsureSuccessResponse();
        }

        public async Task JoinRoom(long roomId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceInfoService.ServerHost);
            httpClient.DefaultRequestHeaders.Add("user_id", userInfoService.UserInfo.UserId);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsJsonAsync<string>($"{UrlPath}/room/{roomId}", null);
            httpResponseMessage.EnsureSuccessResponse();
        }
    }
}