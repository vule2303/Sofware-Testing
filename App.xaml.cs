using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TestBuilder.Data;
using TestBuilder.Models;
using TestBuilder.Utils;
using TestBuilder.ViewModel;


namespace TestBuilder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        services.AddDbContext<TestDbContext>();

        IServiceCollection svc = new ServiceCollection();
        services.AddSingleton<MainWindow> (provider => new MainWindow()
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<HomeViewModel> ();
        services.AddSingleton<SubjectViewModel> ();
        services.AddSingleton<INavigationService, Utils.NavigationService>();
        
        
        services.AddSingleton<Func<Type, Core.ViewModel>>(serviceProvider => viewModelType =>
            (Core.ViewModel)serviceProvider.GetRequiredService(viewModelType));
        _serviceProvider = services.BuildServiceProvider();
    }
    
    
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);

        using var context = new TestDbContext();
        context.Database.EnsureCreated();
    }
}