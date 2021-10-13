using HostedWpf;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using opal.Models;
using opal.Services;
using opal.ViewModels;
using opal.Views;

using System;
using System.Threading.Tasks;
using System.Windows;

using yapsi;
using yapsi.Extensions.DependencyInjection;

namespace opal
{
    public partial class App
    {
        public static new App Current => BaseApp.Current as App ?? throw new ApplicationException("Current Application is not an App instance!");

        /// <inheritdoc />
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection collection)
        {
            _ = collection
                .AddYapsi<ServerCommand>()
                .AddYapsi<ServerState>()
                .AddYapsi<Exception>()
                .AddTransient<MainWindowViewModel>()
                .AddTransient<MainWindow>()
                .AddTransient<DashboardViewModel>()
                .AddTransient<DashboardPage>()
                .AddHostedService<SocketService>();
        }

        protected override void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddSimpleConsole();

            base.ConfigureLogging(context, builder);
        }

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

            var stateSubscription = Services.GetRequiredService<ISubscription<ServerState>>();
            stateSubscription.Published += (sender, state) =>
            {
                logger.LogInformation("New server state: {state}", state);
            };

            logger.LogInformation("Startup complete. Launching MainWindow");

            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
