using System.Windows;
using TestBuilder.Screens.Question;

namespace TestBuilder;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonManagerQuestion(object sender, RoutedEventArgs e)
    {
        var managerQuestions = new ManagerQuestions();
        managerQuestions.Show();
    }
}