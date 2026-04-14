using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TextileApp.AvaloniaClient.ViewModels;
using TextileApp.AvaloniaClient.Views;
using TextileApp.AvaloniaClient.Services;
using System;
using CommunityToolkit.Mvvm;

namespace TextileApp.AvaloniaClient
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ConfigureDi();

            var vm = Services.GetRequiredService<MainWindowViewModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Настраивает и устанавливает значение свойства ServiceProvider
        /// </summary>
        private void ConfigureDi()
        {
            var collection = new ServiceCollection();

            // Добавляет в DI контейнер элементы
            collection.AddCommonServices();

            Services = collection.BuildServiceProvider(validateScopes: true);
        }
    }
}