using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using TextileDesktopApp.ViewModels;

namespace TextileDesktopApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var vmType = param.GetType();
        var name = vmType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type is not null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = $"View not found for: {name}" };
    }

    public bool Match(object? data) => data is ViewModelBase;
}