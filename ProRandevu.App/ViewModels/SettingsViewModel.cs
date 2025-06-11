using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using ProRandevu.Data.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProRandevu.App.ViewModels;

/// <summary>
/// ViewModel that manages application settings such as theme and backup path.
/// </summary>
public class SettingsViewModel : BaseViewModel
{
    private readonly AppDbContext _context;
    private readonly Setting _setting;

    private string _theme = "Light";
    public string Theme
    {
        get => _theme;
        set
        {
            if (SetProperty(ref _theme, value))
            {
                ApplyTheme();
            }
        }
    }

    private string? _backupPath;
    public string? BackupPath
    {
        get => _backupPath;
        set => SetProperty(ref _backupPath, value);
    }

    private int _defaultDuration;
    public int DefaultDuration
    {
        get => _defaultDuration;
        set => SetProperty(ref _defaultDuration, value);
    }

    private string? _licenseKey;
    public string? LicenseKey
    {
        get => _licenseKey;
        set => SetProperty(ref _licenseKey, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand BrowseCommand { get; }

    public SettingsViewModel()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        _context = new AppDbContext(options);
        _setting = _context.Settings.FirstOrDefault() ?? new Setting();
        if (_setting.Id == 0)
        {
            _context.Settings.Add(_setting);
            _context.SaveChanges();
        }

        _theme = _setting.Theme;
        _backupPath = _setting.BackupPath;
        _defaultDuration = _setting.DefaultAppointmentDuration;
        _licenseKey = _setting.LicenseKey;

        ApplyTheme();

        SaveCommand = new RelayCommand(SaveSettings);
        BrowseCommand = new RelayCommand(BrowseBackupPath);
    }

    private void BrowseBackupPath()
    {
        // TODO: Implement folder picker dialog for backup path selection
    }

    private void ApplyTheme()
    {
        try
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/ProRandevu.App;component/Themes/{Theme}Theme.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        catch
        {
            // ignored
        }
    }

    private void SaveSettings()
    {
        _setting.Theme = Theme;
        _setting.BackupPath = BackupPath;
        _setting.DefaultAppointmentDuration = DefaultDuration;
        _setting.LicenseKey = LicenseKey;
        _context.SaveChanges();
    }
}
