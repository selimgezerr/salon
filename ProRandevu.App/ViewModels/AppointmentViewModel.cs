using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages appointment operations and filtering.
/// </summary>
public class AppointmentViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<Appointment> Appointments { get; } = new();

    private Appointment? _selectedAppointment;
    public Appointment? SelectedAppointment
    {
        get => _selectedAppointment;
        set
        {
            if (SetProperty(ref _selectedAppointment, value))
            {
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    private DateTime _filterStart = DateTime.Today;
    public DateTime FilterStart
    {
        get => _filterStart;
        set
        {
            if (SetProperty(ref _filterStart, value))
                LoadAppointments();
        }
    }

    private DateTime _filterEnd = DateTime.Today.AddDays(1);
    public DateTime FilterEnd
    {
        get => _filterEnd;
        set
        {
            if (SetProperty(ref _filterEnd, value))
                LoadAppointments();
        }
    }

    public ObservableCollection<string> ViewModes { get; } = new(new[] { "Günlük", "Haftalık" });

    private string _selectedViewMode = "Günlük";
    public string SelectedViewMode
    {
        get => _selectedViewMode;
        set
        {
            if (SetProperty(ref _selectedViewMode, value))
                ApplyViewMode();
        }
    }

    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public AppointmentViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        LoadAppointments();

        NewCommand = new RelayCommand(NewAppointment);
        SaveCommand = new RelayCommand(SaveAppointment, () => SelectedAppointment != null);
        DeleteCommand = new RelayCommand(DeleteAppointment, () => SelectedAppointment != null && SelectedAppointment.Id > 0);
    }

    /// <summary>
    /// Loads appointments from the database according to the filter.
    /// </summary>
    private void LoadAppointments()
    {
        Appointments.Clear();
        var list = _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Service)
            .Include(a => a.Staff)
            .Where(a => a.StartTime >= FilterStart && a.StartTime < FilterEnd)
            .OrderBy(a => a.StartTime);
        foreach (var ap in list)
            Appointments.Add(ap);
    }

    private void ApplyViewMode()
    {
        if (SelectedViewMode == "Günlük")
        {
            FilterEnd = FilterStart.AddDays(1);
        }
        else // Haftalık
        {
            FilterEnd = FilterStart.AddDays(7);
        }
    }

    private void NewAppointment()
    {
        // TODO: Open modal window to create a new appointment
    }

    private void SaveAppointment()
    {
        if (SelectedAppointment == null)
            return;

        if (SelectedAppointment.Id == 0)
            _context.Appointments.Add(SelectedAppointment);
        else
            _context.Appointments.Update(SelectedAppointment);

        _context.SaveChanges();
        LoadAppointments();
    }

    private void DeleteAppointment()
    {
        if (SelectedAppointment == null || SelectedAppointment.Id == 0)
            return;

        _context.Appointments.Remove(SelectedAppointment);
        _context.SaveChanges();
        LoadAppointments();
        SelectedAppointment = null;
    }
}
