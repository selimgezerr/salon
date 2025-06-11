using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages staff CRUD operations.
/// </summary>
public class StaffViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<Staff> StaffList { get; } = new();

    private Staff? _selectedStaff;
    public Staff? SelectedStaff
    {
        get => _selectedStaff;
        set
        {
            if (SetProperty(ref _selectedStaff, value))
            {
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public StaffViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        LoadStaff();

        NewCommand = new RelayCommand(NewStaff);
        SaveCommand = new RelayCommand(SaveStaff, () => SelectedStaff != null);
        DeleteCommand = new RelayCommand(DeleteStaff, () => SelectedStaff != null && SelectedStaff.Id > 0);
    }

    /// <summary>
    /// Loads staff records from the database.
    /// </summary>
    private void LoadStaff()
    {
        StaffList.Clear();
        foreach (var s in _context.Staff)
            StaffList.Add(s);
    }

    private void NewStaff()
    {
        SelectedStaff = new Staff();
    }

    private void SaveStaff()
    {
        if (SelectedStaff == null)
            return;

        if (SelectedStaff.Id == 0)
            _context.Staff.Add(SelectedStaff);
        else
            _context.Staff.Update(SelectedStaff);

        _context.SaveChanges();
        LoadStaff();
    }

    private void DeleteStaff()
    {
        if (SelectedStaff == null || SelectedStaff.Id == 0)
            return;

        _context.Staff.Remove(SelectedStaff);
        _context.SaveChanges();
        LoadStaff();
        SelectedStaff = null;
    }
}
