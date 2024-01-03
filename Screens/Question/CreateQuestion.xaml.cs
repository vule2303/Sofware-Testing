using System.Windows;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Question;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string? Image { get; set; }
}

public partial class CreateQuestion : Window
{
    private readonly TestDbContext _context = new();
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

    private void ButtonAddAnswer(object sender, RoutedEventArgs e)
    {
        var questionId = new Guid();
        
        var newQuestion = new Models.Question
        {
            QuestionId = questionId,
            Text = ContentQuestion.Content.ToString() ?? "",
            Image = null,
            Options = [],
            TestQuestions = null
        };
        
        _listAnswer.ForEach(answer =>
        {
            newQuestion.Options.Add(new Option
            {
                QuestionId = questionId,
                Text = answer.Content,
                Image = answer.Image,
                Question = newQuestion
            });
        });
        
        _context.Questions.Add(newQuestion);

        _context.SaveChanges();
        
        Console.WriteLine(_context.Questions.FirstOrDefault());
    }
}