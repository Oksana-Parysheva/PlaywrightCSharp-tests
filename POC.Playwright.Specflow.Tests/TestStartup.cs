using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Reqnroll.Autofac;
using POC.Playwright.Specflow.Tests.Pages;
using POC.Playwright.Specflow.Tests.Services;
using POC.Playwright.Specflow.Tests.Settings;
using POC.Playwright.Specflow.Tests.Steps;
using POC.Playwright.Specflow.Tests.Hooks;

namespace POC.Playwright.Specflow.Tests
{
    public static class TestStartup
    {
        private static string _testArtifactsFolder = "TestArtifacts";

        [ScenarioDependencies]
        public static void CreateServices(ContainerBuilder builder)
        {
            builder.RegisterConfiguration();
            builder.RegisterPlaywright();
            builder.RegisterAppSettings();
            builder.RegisterHooks();
            builder.RegisterPages();
            builder.RegisterPagesHandler();
            builder.RegisterPageDependencyService();
            builder.RegisterSteps();
        }

        private static void RegisterConfiguration(this ContainerBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Settings/appsettings.json", false, true)
                .Build();

            builder.RegisterInstance(configuration)
                .As<IConfiguration>()
                .SingleInstance();
        }

        private static void RegisterAppSettings(this ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var appSettings = new AppSettings();
                configuration.Bind(appSettings);
                return Options.Create(appSettings);
            }).As<IOptions<AppSettings>>();
        }

        private static void RegisterSteps(this ContainerBuilder builder)
        {
            builder.RegisterType<StepsDefinishion>().InstancePerDependency();
        }

        private static void RegisterPlaywright(this ContainerBuilder builder)
        {
            builder.Register(async _ =>
            {
                var playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
                var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                    SlowMo = 200
                }).ConfigureAwait(false);

                var browserOptions = new BrowserNewContextOptions
                {
                    Locale = "en-US",
                    ColorScheme = ColorScheme.Light,
                    ViewportSize = new() { Width = 1920, Height = 1080 },
                    RecordVideoDir = $"{_testArtifactsFolder}",
                    RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 }
                };

                var context = await browser.NewContextAsync(browserOptions).ConfigureAwait(false);
                return await context.NewPageAsync().ConfigureAwait(false);
            }).As<Task<IPage>>().InstancePerDependency();
        }

        private static void RegisterPages(this ContainerBuilder builder)
        {
            builder.RegisterType<HomePage>().AsSelf().InstancePerDependency();
            builder.RegisterType<CartPage>().AsSelf().InstancePerDependency();
            builder.RegisterType<ProductPage>().AsSelf().InstancePerDependency();
        }

        private static void RegisterPagesHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<PagesService>().As<IPageService>().InstancePerLifetimeScope();
        }

        private static void RegisterPageDependencyService(this ContainerBuilder builder)
        {
            builder.RegisterType<PageDependencyService>().As<IPageDependencyService>().InstancePerLifetimeScope();
        }

        private static void RegisterHooks(this ContainerBuilder builder)
        {
            builder.RegisterType<TestFixture>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
