using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Test;

public class TestDto
{
    public int TestId { get; set; }
    public string Title { get; set; } = "";
    public int QuestionCount { get; set; }
}

public partial class ManagerTest
{
    private readonly TestDbContext _dbContext = new();
    private readonly ObservableCollection<TestDto> _listTest = [];

    public ManagerTest()
    {
        InitializeComponent();
        LoadData();
        TestQuestionsListBox.ItemsSource = new ObservableCollection<string>(
            _dbContext.Questions.Select(q => q.QuestionId.ToString()));
    }

    private void LoadData()
    {
        _listTest.Clear();
        var tests = _dbContext.Tests.Include(t => t.TestQuestions).ToList();
        tests.ForEach(t =>
        {
            _listTest.Add(new TestDto
            {
                TestId = t.TestId,
                Title = t.Title,
                QuestionCount = t.TestQuestions!.Count
            });
        });
        TestDataGrid.ItemsSource = _listTest;
    }

    private void ButtonAdd(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TestName.Text))
        {
            MessageBox.Show("Tên bộ đề không được để trống!", "Thông báo", MessageBoxButton.OK);
            return;
        }

        if (TestQuestionsListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Bộ đề cần phải có câu hỏi", "Thông báo", MessageBoxButton.OK);
            return;
        }

        var test = new Models.Test
        {
            Title = TestName.Text,
            TestQuestions = [],
            SubjectId = 1
        };
        foreach (var selectedItem in TestQuestionsListBox.SelectedItems)
        {
            var temp = new Guid(selectedItem.ToString()!);
            test.TestQuestions.Add(new TestQuestions
            {
                QuestionId = temp,
                Test = test,
                Question = _dbContext.Questions.Find(temp)!,
            });
        }

        var _ = _dbContext.Tests.Add(test).Entity;
        _dbContext.SaveChanges();

        _listTest.Add(new TestDto
        {
            TestId = _.TestId,
            Title = _.Title,
            QuestionCount = _.TestQuestions!.Count
        });

        TestName.Text = "";
        TestQuestionsListBox.SelectedItems.Clear();
    }

    private void OnDelete(object sender, ExecutedRoutedEventArgs e)
    {
        var test = (TestDto)TestDataGrid.SelectedItem;
        _dbContext.Tests.Remove(_dbContext.Tests.Find(test.TestId)!);
        _dbContext.SaveChanges();
        LoadData();
    }
}