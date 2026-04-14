using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TextileApp.AvaloniaClient.Services;
using TextileApp.AvaloniaClient.ViewModels;

namespace TextileApp.AvaloniaClient.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<NavigationService>();
            services.AddSingleton<ShellViewModel>();
            
            services.AddTransient<LoginPageViewModel>();
            services.AddTransient<MaterialsPageViewModel>();
            services.AddTransient<ClientsPageViewModel>();
            
            return services;
        }
    }
}
