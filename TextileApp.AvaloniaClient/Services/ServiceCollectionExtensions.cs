using Microsoft.Extensions.DependencyInjection;
using System;
using TextileApp.AvaloniaClient.ViewModels;
using Microsoft.Extensions.Configuration;
using TextileApp.AvaloniaClient.API.Services;

namespace TextileApp.AvaloniaClient.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // HttpClient
            var apiSettings = configuration.GetSection("ApiSettings");
            var baseUrl = apiSettings["BaseUrl"] ?? "http://localhost:5295/api/";

            services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // Services
            services.AddSingleton<SessionService>();
            services.AddTransient<AuthService>();
            services.AddTransient<NavigationService>();

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<LoginPageViewModel>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<ClientsPageViewModel>();
            services.AddTransient<MaterialsPageViewModel>();

            return services;
        }
    }
}
