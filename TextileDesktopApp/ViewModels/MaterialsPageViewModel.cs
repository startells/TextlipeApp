using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TextileDesktopApp.Services;

namespace TextileDesktopApp.ViewModels;

public partial class MaterialsPageViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;

    public MaterialsPageViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void OpenMaterial()
    {
        
    }
}