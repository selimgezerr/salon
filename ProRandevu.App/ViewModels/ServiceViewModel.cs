using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages services CRUD operations.
/// </summary>
public class ServiceViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<Service> Services { get; } = new();

    private Service? _selectedService;
    public Service? SelectedService
    {
        get => _selectedService;
        set
        {
            if (SetProperty(ref _selectedService, value))
            {
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    private string? _filterCategory;
    public string? FilterCategory
    {
        get => _filterCategory;
        set
        {
            if (SetProperty(ref _filterCategory, value))
                LoadServices();
        }
    }

    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public ServiceViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        LoadServices();

        NewCommand = new RelayCommand(NewService);
        SaveCommand = new RelayCommand(SaveService, () => SelectedService != null);
        DeleteCommand = new RelayCommand(DeleteService, () => SelectedService != null && SelectedService.Id > 0);
    }

    /// <summary>
    /// Loads services from the database with optional category filtering.
    /// </summary>
    private void LoadServices()
    {
        Services.Clear();
        var query = _context.Services.AsQueryable();
        if (!string.IsNullOrWhiteSpace(FilterCategory))
            query = query.Where(s => s.Category == FilterCategory);

        foreach (var s in query)
            Services.Add(s);
    }

    private void NewService()
    {
        SelectedService = new Service();
    }

    private void SaveService()
    {
        if (SelectedService == null)
            return;

        if (SelectedService.Id == 0)
            _context.Services.Add(SelectedService);
        else
            _context.Services.Update(SelectedService);

        _context.SaveChanges();
        LoadServices();
    }

    private void DeleteService()
    {
        if (SelectedService == null || SelectedService.Id == 0)
            return;

        _context.Services.Remove(SelectedService);
        _context.SaveChanges();
        LoadServices();
        SelectedService = null;
    }
}
