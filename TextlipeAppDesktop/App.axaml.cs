using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Threading.Tasks;
using TextlipeAppDesktop.Services;
using TextlipeAppDesktop.ViewModels;
using TextlipeAppDesktop.Views;

namespace TextlipeAppDesktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async Task OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        // We want to save our ToDoList before we actually shutdown the App. As File I/O is async, we need to wait until file is closed
        // before we can actually close this window

        private bool _canClose; // This flag is used to check if window is allowed to close
        private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
        {
            e.Cancel = !_canClose; // cancel closing event first time

            if (!_canClose)
            {
                // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
                var itemsToSave = _mainWindowViewModel.ToDoItems.Select(item => item.GetToDoItem());
                await ToDoListFileService.SaveToFileAsync(itemsToSave);

                // Set _canClose to true and Close this Window again
                _canClose = true;
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            }
        }

        private async Task InitMainViewModelAsync()
        {
            // get the items to load
            var itemsLoaded = await ToDoListFileService.LoadFromFileAsync();

            if (itemsLoaded is not null)
            {
                foreach (var item in itemsLoaded)
                {
                    _mainWindowViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
                }
            }
        }
    }
}