using System.Windows;
using TestBuilder.Screens.Question;

namespace TestBuilder;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonCreateQuestion(object sender, RoutedEventArgs e)
    {
        var createQuestion = new CreateQuestion();
        createQuestion.Show();
    }
}