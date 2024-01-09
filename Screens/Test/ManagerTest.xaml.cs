using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TestBuilder.Data;
using TestBuilder.Models;

namespace TestBuilder.Screens.Test;

public class TestDto
{
    public int TestId { get; init; }
    public string Title { get; init; } = "";
    public int QuestionCount { get; set; }
}

public partial class ManagerTest
{
    private readonly TestDbContext _dbContext = new();
    private readonly ObservableCollection<TestDto> _listTest = [];
    private readonly List<Models.Question> _listQuestion;
    private readonly int _questionsCount = 0;

    public ManagerTest()
    {
        InitializeComponent();
        LoadData();
        _listQuestion = _dbContext.Questions.ToList();
        TestQuestionsListBox.ItemsSource = new ObservableCollection<string>(_listQuestion.Select(q => q.Content));
        _questionsCount = _listQuestion.Count;
        QuestionLabel.Text = "Số lượng câu hỏi trong ngân hàng: ";
        QuestionCount.Text = _questionsCount.ToString();
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

        if (int.Parse(NumberOfQuestions.Text) > _questionsCount)
        {
            MessageBox.Show("Số lượng câu hỏi không thể lớn hơn số câu hỏi trong ngân hàng",
                "Thông báo",
                MessageBoxButton.OK);
            return;
        }

        if (TestQuestionsListBox.SelectedItems.Count == 0)
        {
            var result = MessageBox.Show("bạn có muốn tự động tạo câu hỏi?",
                "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information,
                MessageBoxResult.Cancel);

            if (int.Parse(NumberOfQuestions.Text) == 0)
            {
                MessageBox.Show("Số lượng câu hỏi không thể bằng 0",
                    "Thông báo",
                    MessageBoxButton.OK);
                return;
            }

            if (result == MessageBoxResult.OK) AutoGenerateTest();

            return;
        }

        var test = new Models.Test
        {
            Title = TestName.Text,
            TestQuestions = [],
            SubjectId = 1
        };
        var _ = _dbContext.Tests.Add(test).Entity;

        foreach (var selectedItem in TestQuestionsListBox.SelectedItems)
        {
            var temp = new Guid(selectedItem.ToString()!);
            var question = _dbContext.Questions.Find(temp)!;
            question.TestId = _.TestId;
            test.TestQuestions.Add(new TestQuestions
            {
                QuestionId = temp,
                Test = test,
                Question = question
            });
        }

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

    private void AutoGenerateTest()
    {
        var test = _dbContext.Tests.Add(new Models.Test
        {
            Title = TestName.Text,
            TestExams = [],
            TestQuestions = []
        }).Entity;

        var _ = _listQuestion;
        while (test.TestQuestions!.Count != int.Parse(NumberOfQuestions.Text))
        {
            var index = new Random().Next(0, _.Count);
            test.TestQuestions!.Add(new TestQuestions
            {
                TestId = 0,
                QuestionId = _[index].QuestionId
            });
            _.RemoveAt(index);
        }

        TestName.Text = string.Empty;
        _dbContext.SaveChanges();
        LoadData();
    }

    private void OnDelete(object sender, ExecutedRoutedEventArgs e)
    {
        var test = (TestDto)TestDataGrid.SelectedItem;
        _dbContext.Tests.Remove(_dbContext.Tests.Find(test.TestId)!);
        _dbContext.SaveChanges();
        LoadData();
    }

    private void TestDataGrid_Edit(object sender, MouseButtonEventArgs e)
    {
        TestQuestionsListBox.SelectedItems.Clear();
        var test = (TestDto)TestDataGrid.SelectedItem;
        TestName.Text = test.Title;
        var list = _dbContext.Questions
            .Where(q => q.TestId == test.TestId)
            .ToList();

        list.ForEach(q => { TestQuestionsListBox.SelectedItems.Add(q.QuestionId.ToString()); });

        Button.Content = "Cập nhật";

        Button.Click -= ButtonAdd;
        Button.Click += ButtonUpdate;
    }

    private void ButtonUpdate(object sender, RoutedEventArgs e)
    {
        var _ = _dbContext.Tests.Find(((TestDto)TestDataGrid.SelectedItem).TestId);
        if (_ is null)
        {
            MessageBox.Show("Không tìm thấy bộ đề");
            TestQuestionsListBox.SelectedItems.Clear();
            Button.Content = "Thêm bộ đề";
            Button.Click -= ButtonUpdate;
            Button.Click += ButtonAdd;
            return;
        }

        _.TestQuestions!.Clear();
        foreach (var selectedItem in TestQuestionsListBox.SelectedItems)
        {
            var temp = new Guid(selectedItem.ToString()!);
            var question = _dbContext.Questions.Find(temp)!;
            question.TestId = _.TestId;
            _.TestQuestions.Add(new TestQuestions
            {
                QuestionId = temp,
                Question = question,
                TestId = _.TestId,
                Test = _
            });
        }

        _dbContext.SaveChanges();

        TestQuestionsListBox.SelectedItems.Clear();
        TestName.Text = "";
        Button.Content = "Thêm bộ đề";
        Button.Click -= ButtonUpdate;
        Button.Click += ButtonAdd;
        LoadData();
    }

    private void NumberOfQuestions_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var _ = new Regex("[^0-9]+");
        e.Handled = _.IsMatch(e.Text);
    }
}