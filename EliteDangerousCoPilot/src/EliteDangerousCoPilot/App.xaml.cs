using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSW.EliteDangerous.API;
using NSW.Logging.Files;

namespace NSW.EliteDangerous.Copilot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider Services { get; }
        public static IConfiguration Configuration { get; private set; }

        private static ILogger Log { get; }

        static App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
            Log = Services.GetService<ILoggerFactory>().CreateLogger<App>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("EDCopilotSettings.json", optional: false, reloadOnChange: true);
 
            Configuration = builder.Build();

            services
                .AddLogging(cfg => cfg
                    .AddFileLogger(o =>
                    {
                        o.Folder = "logs";
                        o.Template.AddColumn("Category", 50, e => e.Category);
                    }))
                .AddEliteDangerousAPI()
                .AddScoped<MainWindow>();
        }

        public static T GetResource<T>(string key)
        {
            var result = Application.Current.TryFindResource(key);
            if (result != null)
                return (T)result;
            return default;
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture =
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
            var mainWindow = Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.LogCritical(e.Exception, e.Exception.Message);
            MessageBox.Show("Произошла непредвиденная ошибка: " + e.Exception.Message, "Ошибка программы", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
