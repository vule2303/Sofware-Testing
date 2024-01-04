using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TestBuilder.Data;

namespace TestBuilder.Screens.Question;

public partial class ManagerQuestion
{
    private ObservableCollection<Models.Question> _listQuestions = [];
    private readonly TestDbContext _context = new();

    public ManagerQuestion()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
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
        var createQuestion = new DetailsQuestion();
        createQuestion.Show();
        createQuestion.Closed += (_, _) =>
        {
            LoadData();
        };
    }

    private void DataGridQuestion_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataGridQuestion.SelectedItem is not Models.Question selectedQuestion) return;

        var detailsQuestion = new DetailsQuestion(selectedQuestion.QuestionId);
        detailsQuestion.Show();
    }
}