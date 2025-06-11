using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// Main window view model that handles navigation.
/// </summary>
public class MainViewModel : BaseViewModel
{
    private BaseViewModel? _currentView;
    public BaseViewModel? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public ICommand ShowCustomersCommand { get; }
    public ICommand ShowAppointmentsCommand { get; }
    public ICommand ShowServicesCommand { get; }
    public ICommand ShowStaffCommand { get; }
    public ICommand ShowPaymentsCommand { get; }
    public ICommand ShowReportsCommand { get; }
    public ICommand ShowSettingsCommand { get; }

    public MainViewModel()
    {
        ShowCustomersCommand = new RelayCommand(() => CurrentView = new CustomerViewModel());
        ShowAppointmentsCommand = new RelayCommand(() => CurrentView = new AppointmentViewModel());
        ShowServicesCommand = new RelayCommand(() => CurrentView = new ServiceViewModel());
        ShowStaffCommand = new RelayCommand(() => CurrentView = new StaffViewModel());
        ShowPaymentsCommand = new RelayCommand(() => CurrentView = new PaymentViewModel());
        ShowReportsCommand = new RelayCommand(() => CurrentView = new ReportViewModel());
        ShowSettingsCommand = new RelayCommand(() => CurrentView = new SettingsViewModel());
    }
}
