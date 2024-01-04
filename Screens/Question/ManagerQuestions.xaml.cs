using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestBuilder.Data;

namespace TestBuilder.Screens.Question;

public partial class ManagerQuestions
{
    private readonly ObservableCollection<Models.Question> _listQuestions;
    private readonly TestDbContext _context = new();

    public ManagerQuestions()
    {
        InitializeComponent();
        _listQuestions = new ObservableCollection<Models.Question>(_context.Questions.ToList());
        DataGridQuestion.ItemsSource = _listQuestions;
        DataContext = this;
    }

    private async void OnDelete(object sender, ExecutedRoutedEventArgs? e)
    {
        var question = (Models.Question)DataGridQuestion.CurrentItem;
        _listQuestions.Remove(question);
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }

    private void ButtonRemoveAnswer(object sender, RoutedEventArgs e)
    {
        OnDelete(this, null);
    }

    private void ButtonCreateQuestion(object sender, RoutedEventArgs e)
    {
        var createQuestion = new CreateQuestion();
        createQuestion.Show();
        Close();
        createQuestion.Closed += (o, args) =>
        {
            var _ = new ManagerQuestions();
            _.Show();
        };
    }
}