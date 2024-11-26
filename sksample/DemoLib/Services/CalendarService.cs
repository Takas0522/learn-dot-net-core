using DemoLib.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoLib.Services
{
    public class CalendarService
    {
        private readonly AuthService _authService;
        private readonly HttpClient _httpClient;

        // GraphClient使うのが一番楽なんだけど別サービス想定してそれぞれ別の認可を取得する構造にしたいため未実装
        public CalendarService(
            AuthService authService
        )
        {
            _authService = authService;
            _httpClient = new HttpClient();
        }

        public async Task<string> PostEvent(Event data)
        {
            DateTime.TryParse(data.Start, out var startDate);
            DateTime.TryParse(data.End, out var endDate);

            var postdata = new GraphEvent() {
                Subject = data.Subject,
                Body = new Body() { Content = data.Body },
                Start = new DateTimeSet() { DateTime = startDate },
                End = new DateTimeSet() { DateTime = endDate }
            };

            var token = await _authService.GetToken(new List<string> { "Calendars.ReadWrite" });
            var req = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/v1.0/me/events");
            req.Content = new StringContent(JsonSerializer.Serialize(postdata), Encoding.UTF8, "application/json");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(req);
            if (response.IsSuccessStatusCode)
            {
                return "予定の登録が成功しました";
            }
            return "予定の登録が失敗しました";
        }
    }
}
