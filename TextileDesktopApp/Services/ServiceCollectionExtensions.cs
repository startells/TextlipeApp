using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TextileDesktopApp.Services;
using TextileDesktopApp.ViewModels;

namespace TextileDesktopApp.Services
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
