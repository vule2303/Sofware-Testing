using Microsoft.Extensions.DependencyInjection;
using TestBuilder.View;


namespace TestBuilder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindow>();
        services.BuildServiceProvider();
    }
}