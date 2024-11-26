using DemoLib.Model;
using DemoLib.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable SKEXP0001 // 種類は、評価の目的でのみ提供されています。将来の更新で変更または削除されることがあります。続行するには、この診断を非表示にします。
internal class Program
{
    static async Task Main(string[] args)
    {
        var hostBuilder = DemoLib.DemoHost.ConfigureHostBuilder();
        var host = hostBuilder.Build();
        var kernel = host.Services.GetRequiredService<Kernel>();
        var chatCom = kernel.GetRequiredService<IChatCompletionService>();

        // まずはログインをする
        var authService = host.Services.GetRequiredService<AuthService>();
        await authService.Login();

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        // 普通のチャットサービスみたいな感じで構成してみる
        var history = new ChatHistory();
        history.AddSystemMessage(@"
            あなたはチャットアシスタントです。
            ユーザーからの質問に答えることができます。
            まずはユーザーからの質問を待ってください。
            登録処理を行う場合はユーザーに登録内容の確認を行い、許可を得たときにのみ登録を行ってください。
        ");

        var init = await chatCom.GetChatMessageContentsAsync(history, executionSettings: settings, kernel: kernel);
        Console.Write("Assistant >");
        Console.WriteLine(init.First().ToString());

        string userInput = "";

        do
        {
            Console.Write("User >");
            userInput = Console.ReadLine();

            history.AddUserMessage(userInput);

            var res = await chatCom.GetChatMessageContentsAsync(history, executionSettings: settings, kernel: kernel);
            Console.Write("Assistant >");
            Console.WriteLine(res.First().ToString());
        } while (userInput != "");
    }
}
#pragma warning restore SKEXP0001 // 種類は、評価の目的でのみ提供されています。将来の更新で変更または削除されることがあります。続行するには、この診断を非表示にします。
