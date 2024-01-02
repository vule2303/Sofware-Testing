using System.Windows;

namespace TestBuilder.Screens.Question;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string? Image { get; set; }
}

public partial class CreateQuestion : Window
{
    private readonly List<Answer> _listAnswer = [];

    public CreateQuestion()
    {
        InitializeComponent();
        _listAnswer.Add(new Answer
        {
            Id = 0,
            Content = "",
            Image = null
        });
        AnswerDataGrid.ItemsSource = _listAnswer;
        DataContext = this;
    }

    private void ButtonClose(object sender, RoutedEventArgs e)
    {
        Close();
    }
}