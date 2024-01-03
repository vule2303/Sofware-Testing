using System.Configuration;
using System.Data;
using System.Windows;
using TestBuilder.Data;

namespace TestBuilder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var context = new TestDbContext();
        context.Database.EnsureCreated();
    }
}