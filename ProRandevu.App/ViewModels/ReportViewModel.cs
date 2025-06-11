using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that calculates various reports for the salon.
/// </summary>
public class ReportViewModel : BaseViewModel
{
    private readonly AppDbContext _context;

    public ObservableCollection<KeyValuePair<string, int>> ServiceStats { get; } = new();
    public ObservableCollection<KeyValuePair<string, decimal>> RevenueStats { get; } = new();
    public ObservableCollection<KeyValuePair<string, int>> CustomerStats { get; } = new();

    private DateTime _startDate = DateTime.Today.AddMonths(-1);
    public DateTime StartDate
    {
        get => _startDate;
        set { if (SetProperty(ref _startDate, value)) LoadData(); }
    }

    private DateTime _endDate = DateTime.Today.AddDays(1);
    public DateTime EndDate
    {
        get => _endDate;
        set { if (SetProperty(ref _endDate, value)) LoadData(); }
    }

    // TODO: Expose LiveCharts series properties once library is added
    public object? ServiceSeries { get; private set; }
    public object? RevenueSeries { get; private set; }
    public object? CustomerSeries { get; private set; }

    public int TotalCustomers { get; private set; }
    public decimal TotalRevenue { get; private set; }
    public decimal TotalDebt { get; private set; }

    public ICommand RefreshCommand { get; }

    public ReportViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);

        RefreshCommand = new RelayCommand(LoadData);
        LoadData();
    }

    /// <summary>
    /// Loads all statistics from the database.
    /// </summary>
    private void LoadData()
    {
        LoadServiceStats();
        LoadRevenueStats();
        LoadCustomerStats();
        LoadSummaryStats();
    }

    private void LoadServiceStats()
    {
        ServiceStats.Clear();
        var query = _context.Appointments
            .Include(a => a.Service)
            .Where(a => a.StartTime >= StartDate && a.StartTime <= EndDate);
        var grouped = query
            .GroupBy(a => a.Service!.Name)
            .Select(g => new { Service = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();
        foreach (var item in grouped)
            ServiceStats.Add(new KeyValuePair<string, int>(item.Service, item.Count));
        // TODO: Map ServiceStats to LiveCharts series
    }

    private void LoadRevenueStats()
    {
        RevenueStats.Clear();
        var query = _context.Payments
            .Where(p => p.Date >= StartDate && p.Date <= EndDate && p.Type != PaymentType.Debt)
            .AsEnumerable()
            .GroupBy(p => new { p.Date.Year, p.Date.Month })
            .Select(g => new
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
                Total = g.Sum(x => x.Amount)
            })
            .OrderBy(g => g.Month)
            .ToList();
        foreach (var item in query)
            RevenueStats.Add(new KeyValuePair<string, decimal>(item.Month, item.Total));
        // TODO: Map RevenueStats to LiveCharts series
    }

    private void LoadCustomerStats()
    {
        CustomerStats.Clear();
        var query = _context.Customers
            .Where(c => c.CreatedAt >= StartDate && c.CreatedAt <= EndDate)
            .AsEnumerable()
            .GroupBy(c => new { c.CreatedAt.Year, c.CreatedAt.Month })
            .Select(g => new
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
                Count = g.Count()
            })
            .OrderBy(g => g.Month)
            .ToList();
        foreach (var item in query)
            CustomerStats.Add(new KeyValuePair<string, int>(item.Month, item.Count));
        // TODO: Map CustomerStats to LiveCharts series
    }

    private void LoadSummaryStats()
    {
        TotalCustomers = _context.Customers.Count();
        TotalRevenue = _context.Payments.Where(p => p.Type != PaymentType.Debt).Sum(p => p.Amount);
        TotalDebt = _context.Payments.Where(p => p.Type == PaymentType.Debt).Sum(p => p.Amount);

        OnPropertyChanged(nameof(TotalCustomers));
        OnPropertyChanged(nameof(TotalRevenue));
        OnPropertyChanged(nameof(TotalDebt));
    }
}
