using DemoLib.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoLib.Services
{
    public class EmailService
    {
        private readonly AuthService _authService;
        private readonly HttpClient _httpClient;
        private readonly string mailId = "";

        // GraphClient使うのが一番楽なんだけど別サービス想定してそれぞれ別の認可を取得する構造にしたいため未実装
        public EmailService(
            AuthService authService,
            IConfiguration config
        )
        {
            _authService = authService;
            _httpClient = new HttpClient();
            mailId = config["EmailId1"];
        }

        public async Task<string> GetEmailMessage()
        {
            var token = await _authService.GetToken(new List<string> { "Mail.Read" });

            var req = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/me/messages/{mailId}");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(req);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStreamAsync();
                var data = JsonSerializer.Deserialize<Email>(res);
                if (data == null) return "";
                return data.BodyPreview;
            }
            return "";
        }
    }
}
