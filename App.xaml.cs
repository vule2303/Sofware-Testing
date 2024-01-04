
﻿using System.Windows;


namespace TestBuilder;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var context = new TestDbContext();
        context.Database.EnsureCreated();
    }
}