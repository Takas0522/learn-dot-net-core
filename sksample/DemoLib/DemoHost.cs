using DemoLib.Plugins;
using DemoLib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLib
{
    public static class DemoHost
    {
        public static IHostBuilder ConfigureHostBuilder()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(s =>
                {
                    s.AddSingleton<AuthService>();
                    s.AddSingleton<EmailService>();
                    s.AddSingleton<CalendarService>();

                    s.AddAzureOpenAIChatCompletion(
                        deploymentName: config["AzureOpenAIDeploymentName"],
                        endpoint: config["AzureOpenAIEndpoint"],
                        apiKey: config["AzureOpenAIKey"]
                    );

                    s.AddKernel();
                    // Plugin追加
                    s.AddSingleton(sp =>
                    {
                        return KernelPluginFactory.CreateFromType<EmailPlugin>(serviceProvider: sp);
                    });
                    s.AddSingleton(sp =>
                    {
                        return KernelPluginFactory.CreateFromType<CalendarPlugin>(serviceProvider: sp);
                    });
                    s.AddSingleton(sp =>
                    {
                        return KernelPluginFactory.CreateFromType<TodayPlugin>(serviceProvider: sp);
                    });
                });
            return hostBuilder;
        }
    }
}
