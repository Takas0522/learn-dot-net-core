using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLib.Services
{
    public class AuthService
    {
        private readonly IPublicClientApplication _app;
        private readonly IConfidentialClientApplication _confidentialApp;

        private string token = "";

        public AuthService(
            IConfiguration config    
        )
        {
            _app = PublicClientApplicationBuilder.Create(config["AzureAD:ClientId"])
                .WithAuthority(AzureCloudInstance.AzurePublic, config["AzureAD:TenantId"])
                .WithRedirectUri("http://localhost")
                .Build();

            _confidentialApp = ConfidentialClientApplicationBuilder.Create(config["AzureAD:ClientId"])
                .WithTenantId(config["AzureAD:TenantId"])
                .WithClientSecret(config["AzureAD:Secret"])
                .Build();
        }

        public async Task Login()
        {
            var accounts = await _app.GetAccountsAsync();
            if (accounts == null || !accounts.Any())
            {
                var res = await _app.AcquireTokenInteractive(["api://b9c8d056-e227-461e-bc56-d6d2fc00c79d/access"])
                    .ExecuteAsync();
                token = res.AccessToken;
            }
        }

        public async Task<string> GetToken(IEnumerable<string> scopes)
        {
            // 既知のトークンからOnbehalfofを使用してサービス専用のアクセストークンを取得する。
            var result = await _confidentialApp.AcquireTokenOnBehalfOf(scopes, new UserAssertion(token))
                .ExecuteAsync();
            return result.AccessToken;
        }
    }
}
