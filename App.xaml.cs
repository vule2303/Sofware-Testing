using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TestBuilder.Data;

namespace TestBuilder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        var services = new ServiceCollection();
        services.AddDbContext<TestDbContext>();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var context = new TestDbContext();
        context.Database.EnsureCreated();
    }
}