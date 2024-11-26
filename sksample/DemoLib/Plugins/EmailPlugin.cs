using DemoLib.Model;
using DemoLib.Services;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoLib.Plugins
{
#pragma warning disable SKEXP0010 // 種類は、評価の目的でのみ提供されています。将来の更新で変更または削除されることがあります。続行するには、この診断を非表示にします。
    public class EmailPlugin
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailPlugin> _logger;

        public EmailPlugin(
            EmailService emailService,
            ILogger<EmailPlugin> logger
        )
        {
            _emailService = emailService;
            _logger = logger;
        }

        [KernelFunction("get_email")]
        [Description("デモで利用するメールを取得します")]
        [return: Description("メールの内容を返却します。データが取得できなかったらNOT ACCESSを返却します。")]
        public async Task<string> GetEmail()
        {
            _logger.LogInformation("プラグイン実行（メールの取得）");
            var res = await _emailService.GetEmailMessage();
            if (res != "")
            {
                return res;
            }
            return "NOT ACCESS";
        }

        [KernelFunction("change_strategy")]
        [Description("メールの内容から予定情報を取得し構造体に変換します")]
        [return: Description("変換された予定構造体")]
        public async Task<EventsResult> ChangeStrategy(
            Kernel kernel,
            [Description("メールの内容")]string emailContent
        )
        {
            _logger.LogInformation("プラグイン実行（メールの内容から予定情報を取得）");
            var settings = new OpenAIPromptExecutionSettings
            {
                ResponseFormat = typeof(EventsResult),
            };
            var prompt = @"
            下記のメールの内容から予定情報を取得してください
            イベントの構成情報はタイトルのSubject、本文のBody、開始日時のStart、終了日時のEndを含む必要があります。
            [メールの内容]
            " + emailContent;
            var result = await kernel.InvokePromptAsync(prompt, new (settings));
            var eventsResult = JsonSerializer.Deserialize<EventsResult>(result.ToString());

            return eventsResult;
        }
    }
}
#pragma warning restore SKEXP0010 // 種類は、評価の目的でのみ提供されています。将来の更新で変更または削除されることがあります。続行するには、この診断を非表示にします。