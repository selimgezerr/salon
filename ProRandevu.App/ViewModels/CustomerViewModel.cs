using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages customer operations.
/// </summary>
public class CustomerViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<Customer> Customers { get; } = new();

    private Customer? _selectedCustomer;
    public Customer? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            if (SetProperty(ref _selectedCustomer, value))
            {
                // Notify commands that depend on SelectedCustomer
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public CustomerViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        LoadCustomers();

        NewCommand = new RelayCommand(NewCustomer);
        SaveCommand = new RelayCommand(SaveCustomer, () => SelectedCustomer != null);
        DeleteCommand = new RelayCommand(DeleteCustomer, () => SelectedCustomer != null && SelectedCustomer.Id > 0);
    }

    /// <summary>
    /// Loads customers from the database.
    /// </summary>
    private void LoadCustomers()
    {
        Customers.Clear();
        foreach (var c in _context.Customers)
            Customers.Add(c);
    }

    private void NewCustomer()
    {
        SelectedCustomer = new Customer();
    }

    private void SaveCustomer()
    {
        if (SelectedCustomer == null)
            return;

        if (SelectedCustomer.Id == 0)
            _context.Customers.Add(SelectedCustomer);
        else
            _context.Customers.Update(SelectedCustomer);

        _context.SaveChanges();
        LoadCustomers();
    }

    private void DeleteCustomer()
    {
        if (SelectedCustomer == null || SelectedCustomer.Id == 0)
            return;

        _context.Customers.Remove(SelectedCustomer);
        _context.SaveChanges();
        LoadCustomers();
        SelectedCustomer = null;
    }
}
