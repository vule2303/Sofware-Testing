using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestBuilder.Data;
using EntityFrameworkQueryableExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace TestBuilder.Screens.Test;

public partial class TestQuestion
{
    private readonly TestDbContext _dbContext = new();
    private readonly TestDto _test;
    private List<Models.Question> _listQuestion = new();

    public TestQuestion(TestDto test)
    {
        InitializeComponent();
        _test = test;
        TestLabel.Text = _test.Title;
        LoadData();
    }

    private void LoadData()
    {
        var listQuestion = EntityFrameworkQueryableExtensions
            .Include(_dbContext.TestQuestions.Include(question => question.Question),
                testQuestions => testQuestions.Question).ToList();
        _listQuestion = listQuestion
            .Where(q => q.TestId == _test.TestId)
            .Select(q => q.Question)
            .ToList();
        TestQuestionsListBox.ItemsSource = new ObservableCollection<string>(_listQuestion.Select(q => q.Content));
        ListQuestionLabel.Text = "Số câu hỏi: ";
        Amount.Text = _listQuestion.Count.ToString();
    }


    private void TestQuestionsListBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        // drag listbox
        var item = TestQuestionsListBox.SelectedItems;
        DragDrop.DoDragDrop(TestQuestionsListBox, item, DragDropEffects.All);

    }
}