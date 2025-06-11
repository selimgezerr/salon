using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages payment records and filtering.
/// </summary>
public class PaymentViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<Payment> Payments { get; } = new();
    public ObservableCollection<Payment> DebtPayments { get; } = new();

    private Payment? _selectedPayment;
    public Payment? SelectedPayment
    {
        get => _selectedPayment;
        set
        {
            if (SetProperty(ref _selectedPayment, value))
            {
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    private string? _filterCustomer;
    public string? FilterCustomer
    {
        get => _filterCustomer;
        set { if (SetProperty(ref _filterCustomer, value)) LoadPayments(); }
    }

    private PaymentType? _filterType;
    public PaymentType? FilterType
    {
        get => _filterType;
        set { if (SetProperty(ref _filterType, value)) LoadPayments(); }
    }

    private DateTime _filterStart = DateTime.Today.AddMonths(-1);
    public DateTime FilterStart
    {
        get => _filterStart;
        set { if (SetProperty(ref _filterStart, value)) LoadPayments(); }
    }

    private DateTime _filterEnd = DateTime.Today.AddDays(1);
    public DateTime FilterEnd
    {
        get => _filterEnd;
        set { if (SetProperty(ref _filterEnd, value)) LoadPayments(); }
    }

    private decimal? _minAmount;
    public decimal? MinAmount
    {
        get => _minAmount;
        set { if (SetProperty(ref _minAmount, value)) LoadPayments(); }
    }

    // TODO: Expose chart data once LiveCharts is added

    public ICommand NewCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public PaymentViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        LoadPayments();

        NewCommand = new RelayCommand(NewPayment);
        SaveCommand = new RelayCommand(SavePayment, () => SelectedPayment != null);
        DeleteCommand = new RelayCommand(DeletePayment, () => SelectedPayment != null && SelectedPayment.Id > 0);
    }

    /// <summary>
    /// Loads payments from the database according to the filters.
    /// </summary>
    private void LoadPayments()
    {
        Payments.Clear();
        DebtPayments.Clear();

        var query = _context.Payments
            .Include(p => p.Customer)
            .Where(p => p.Date >= FilterStart && p.Date <= FilterEnd);

        if (!string.IsNullOrWhiteSpace(FilterCustomer))
            query = query.Where(p => (p.Customer!.FirstName + " " + p.Customer!.LastName).Contains(FilterCustomer));

        if (FilterType.HasValue)
            query = query.Where(p => p.Type == FilterType.Value);

        if (MinAmount.HasValue)
            query = query.Where(p => p.Amount >= MinAmount.Value);

        foreach (var p in query.OrderByDescending(p => p.Date))
            Payments.Add(p);

        foreach (var dp in query.Where(p => p.Type == PaymentType.Debt))
            DebtPayments.Add(dp);
    }

    private void NewPayment()
    {
        SelectedPayment = new Payment { Date = DateTime.Today };
    }

    private void SavePayment()
    {
        if (SelectedPayment == null)
            return;

        if (SelectedPayment.Id == 0)
            _context.Payments.Add(SelectedPayment);
        else
            _context.Payments.Update(SelectedPayment);

        _context.SaveChanges();
        LoadPayments();
    }

    private void DeletePayment()
    {
        if (SelectedPayment == null || SelectedPayment.Id == 0)
            return;

        _context.Payments.Remove(SelectedPayment);
        _context.SaveChanges();
        LoadPayments();
        SelectedPayment = null;
    }
}
