using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.SemanticKernel.Skills.MsGraph.Connectors;
using Microsoft.SemanticKernel.Skills.MsGraph;
using Microsoft.SemanticKernel.Skills.MsGraph.Connectors.Client;
using Microsoft.SemanticKernel.Skills.MsGraph.Connectors.CredentialManagers;
using System.Net.Http.Headers;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace MsGraphSkillsSample
{
    internal class Program
    {

        private static readonly string InitSystemMessage = "あなたの予定コンシェルジュです。\n指定された日付とその日のTodoリストから最適と思われるスケジュールを組み立てます";
        private static IPublicClientApplication authClient;

        static async Task Main(string[] args)
        {

            // 設定情報諸々取得
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: "appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();

            // Skillに食わせたりするLoggerを生成
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
            var logger = loggerFactory.CreateLogger<Program>();

            logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Initialize Start");

            // GraphClientまわりの設定
            var graphConfig = configuration.GetRequiredSection("MsGraph").Get<MsGraphConfiguration>();
            authClient = PublicClientApplicationBuilder.Create(graphConfig.ClientId).WithTenantId(graphConfig.TenantId).WithRedirectUri(graphConfig.RedirectUri.OriginalString).Build();

            // めんどいから都度Login
            var authRes = await authClient.AcquireTokenInteractive(graphConfig.Scopes).ExecuteAsync();
            var account = authRes.Account;

            // Skillに食わせるHttpClientとGraphClient作成
            var handlers = GraphClientFactory.CreateDefaultHandlers(
                CreateAuthenticationProvider(authClient, graphConfig, account));

            using MsGraphClientLoggingHandler loggingHandler = new(logger);
            handlers.Add(loggingHandler);
            using HttpClient httpClient = GraphClientFactory.Create(handlers);
            var graphServiceClient = new GraphServiceClient(httpClient);

            // Skill生成
            var todoSkill = new TaskListSkill(new MicrosoftToDoConnector(graphServiceClient), loggerFactory.CreateLogger<TaskListSkill>());

            // SemanticKernel生成
            var skbuilder = Kernel.Builder.WithLogger(loggerFactory.CreateLogger<IKernel>());
            var deploymentName = configuration.GetValue<string>("AzureOpenAI:DeploymentName");
            var endpoint = configuration.GetValue<string>("AzureOpenAI:Endpoint");
            var key = configuration.GetValue<string>("AzureOpenAI:ApiKey");
            skbuilder.WithAzureChatCompletionService(deploymentName, endpoint, key);

            var sk = skbuilder.Build();
            var todo = sk.ImportSkill(todoSkill, "todo");

            var chatCompletion = sk.GetService<IChatCompletion>();
            logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Initialize End");

            while (true)
            {
                var chatHistory = chatCompletion.CreateNewChat(InitSystemMessage);
                Console.WriteLine($"{InitSystemMessage}\nお望みの日付を入力してください。");

                var daySt = Console.ReadLine();
                if (!DateOnly.TryParse(daySt, out var date))
                {
                    Console.WriteLine("日付の形式で入力お願いします");
                    continue;
                }

                var taskMemory = new ContextVariables("すべてのToDo");

                // ToDoを取得
                var res = await sk.RunAsync(taskMemory, todo["GetDefaultTasks"]);
                string task = res.Result;

                // JSONで落ちてくるのでコントロールしやすい形に
                var tasks = JsonSerializer.Deserialize<IEnumerable<ToDoData>>(task);

                // Markdownチックな形式に整形ののち1この文字列にする（日付は日本時間を考慮し＋9時間する…超雑）
                var taskList = tasks.Select(s => {
                    return $"* {s.Title}(実行日: {s.Due.AddHours(9)})";
                });
                var taskString = String.Join("\n", taskList);

                // Todoから予定を引っ張ってきてそれをベースにプロンプトを作成。
                var prompt = "/n/nあなたは予定コンシェルジュです。\n" +
                    "以下のタスクリストの{DATE}の実行日のものをすべて選択し、各予定で一般的に必要とされている作業工程を提示してください。\n" +
                    "{TASKLIST}";

                // Skillから取得したもろもろをpromptに反映
                var s = prompt.Replace("{DATE}", daySt).Replace("{TASKLIST}", taskString);

                // ChatGptになげる
                chatHistory.AddUserMessage(s);
                var settings = new ChatRequestSettings(){ MaxTokens = 2000 };
                await foreach (string message in chatCompletion.GenerateMessageStreamAsync(chatHistory, settings))
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        Console.Write(message); 
                    }
                }
                continue;
            }

        }

        // 参考 https://github.com/microsoft/semantic-kernel/blob/main/samples/dotnet/graph-api-skills/Program.cs
        private static DelegateAuthenticationProvider CreateAuthenticationProvider(
            IPublicClientApplication client,
            MsGraphConfiguration config,
            IAccount account
        )
        => new(
            async (requestMessage) =>
            {
                var res = await client.AcquireTokenSilent(config.Scopes, account).ExecuteAsync();

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    scheme: "bearer",
                    parameter: res.AccessToken);
            });
    }
}