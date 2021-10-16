using HostedWpf;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using opal.Models;
using opal.Services;
using opal.ViewModels;
using opal.Views;

using System;
using System.IO;
using System.Net.Sockets;
using System.Windows;

using yapsi;
using yapsi.Extensions.DependencyInjection;

namespace opal
{
    public partial class App
    {
        public static new App Current => BaseApp.Current as App ?? throw new ApplicationException("Current Application is not an App instance!");

        /// <inheritdoc />
        protected override IHostBuilder ConfigureHost(IHostBuilder builder) => builder
            .ConfigureLogging(builder => builder.AddSimpleConsole())
            .ConfigureAppConfiguration(builder => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
            )
            .ConfigureServices(services =>
            {
                services.AddOptions<SocketServiceOptions>()
                        .BindConfiguration("Socket");

                services.AddYapsi<ServerCommand>()
                        .AddYapsi<ServerState>()
                        .AddYapsi<Exception>()
                        .AddSingleSubscribeYapsi<Socket>()
                        .AddTransient<MainWindowViewModel>()
                        .AddTransient<MainWindow>()
                        .AddTransient<DashboardViewModel>()
                        .AddTransient<DashboardPage>()
                        .AddTransient<SocketService>()
                        .AddHostedService<SocketControllerService>();
            });

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var logger = Services.GetRequiredService<ILogger<App>>();

            var exceptionSubscription = Services.GetRequiredService<ISubscription<Exception>>();
            exceptionSubscription.Published += (sender, exception) =>
            {
                logger.LogError(exception, "Exception published");
            };

            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
