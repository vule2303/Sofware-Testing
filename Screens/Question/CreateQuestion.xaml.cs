using System.Collections.ObjectModel;
using System.Windows;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Question;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public string? Image { get; set; }
}

public partial class CreateQuestion
{
    private readonly TestDbContext _context = new();
    private readonly ObservableCollection<Answer> _listAnswer;

    public CreateQuestion()
    {
        InitializeComponent();
        _listAnswer = new ObservableCollection<Answer>
        {
            new()
            {
                Id = 0,
                Content = "",
                Image = null
            }
        };
        AnswerDataGrid.ItemsSource = _listAnswer;
        DataContext = this;
    }

    private void ButtonClose(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void ButtonAddAnswer(object sender, RoutedEventArgs e)
    {
        var questionId = new Guid();
        
        var newQuestion = new Models.Question
        {
            QuestionId = questionId,
            Text = ContentQuestion.TextBoxArea.Text,
            Image = null,
            Options = [],
            TestQuestions = null
        };

        foreach (var answer in _listAnswer)
        {
            newQuestion.Options.Add(new Option
            {
                QuestionId = questionId,
                Text = answer.Content,
                Image = answer.Image,
                Question = newQuestion
            });
        }
        
        _context.Questions.Add(newQuestion);

        await _context.SaveChangesAsync();
    }

    private void ButtonRemoveAnswer(object sender, RoutedEventArgs e)
    {
        _listAnswer.RemoveAt(AnswerDataGrid.SelectedIndex);
    }
}