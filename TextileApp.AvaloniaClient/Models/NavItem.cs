using System;
using CommunityToolkit.Mvvm.Input;

namespace TextileApp.AvaloniaClient.Models;

public class NavItem
{
    public string? Title { get; set; }
    public IRelayCommand Command { get; set; } = null!;
    public Func<bool> IsVisible { get; set; } = null!;
}