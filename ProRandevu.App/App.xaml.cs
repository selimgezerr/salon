using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;
using System.Linq;
using System.Windows;

namespace ProRandevu.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=pro.db")
            .Options;

        using var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        DemoVeriSeeder.Seed(context);

        var setting = context.Settings.FirstOrDefault();
        if (setting != null)
        {
            // apply saved theme
            TryApplyTheme(setting.Theme);

            if (string.IsNullOrWhiteSpace(setting.LicenseKey))
            {
                MessageBox.Show("Uygulama lisanss\u0131z modda \u00e7al\u0131\u015f\u0131yor.", "Uyar\u0131");
            }
        }
    }

    private static void TryApplyTheme(string theme)
    {
        try
        {
            var dict = new ResourceDictionary
            {
                Source = new System.Uri($"/ProRandevu.App;component/Themes/{theme}Theme.xaml", System.UriKind.RelativeOrAbsolute)
            };
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(dict);
        }
        catch
        {
            // ignore theme load errors
        }
    }
}
