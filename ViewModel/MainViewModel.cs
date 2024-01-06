using TestBuilder.Core;
using TestBuilder.Utils;

namespace TestBuilder.ViewModel;

public class MainViewModel : Core.ViewModel
{
    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }
    public RelayCommand NavigateToHomeCommand { get; set; }
    public RelayCommand NavigateToSubjectsViewCommand { get; set; }
    public MainViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o=>true);
        NavigateToSubjectsViewCommand = new RelayCommand(o => { Navigation.NavigateTo<SubjectViewModel>(); }, o=>true);
    }
}