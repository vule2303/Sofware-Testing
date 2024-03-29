using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using TestBuilder.Data;
using TestBuilder.Models;
using WpfMath;
using WpfMath.Parsers;
using Xceed.Words.NET;
using Rectangle = System.Windows.Shapes.Rectangle;

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
    private readonly int _questionsCount;
    private ObservableCollection<Models.Question> _questions = [];
    private int _currentIndex = -1;

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex OnlyNumber();

    public ManagerTest()
    {
        try
        {
            InitializeComponent();
            LoadData();
            _listQuestion = _dbContext.Questions.ToList();
            TestQuestionsListBox.ItemsSource = new ObservableCollection<string>(_listQuestion.Select(q => q.Content));
            _questionsCount = _listQuestion.Count;
            QuestionLabel.Text = "Số lượng câu hỏi trong ngân hàng: ";
            QuestionCount.Text = _questionsCount.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show(e.Message);
            throw;
        }
    }

    private void LoadData()
    {
        _listTest.Clear();
        var tests = _dbContext.Tests.Include(t => t.TestQuestions).ToList();
        tests.ForEach(t =>
        {
            if (t.TestQuestions is not null)
                _listTest.Add(new TestDto
                {
                    TestId = t.TestId,
                    Title = t.Title,
                    QuestionCount = t.TestQuestions.Count
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

        if (CheckBoxAutoGenerateTest.IsChecked == true)
        {
            if (int.Parse(NumberOfQuestions.Text) == 0)
            {
                MessageBox.Show("Số lượng câu hỏi không thể bằng 0",
                    "Thông báo",
                    MessageBoxButton.OK);
                return;
            }

            AutoGenerateTest();

            return;
        }

        var _ = _dbContext.Tests.Add(new Models.Test
        {
            Title = TestName.Text,
            TestQuestions = [],
            SubjectId = 1
        }).Entity;

        foreach (var selectedItem in TestQuestionsListBox.SelectedItems)
        {
            var question = _dbContext.Questions.First(q => q.Content.Equals(selectedItem.ToString()));
            question.TestId = _.TestId;
            _.TestQuestions!.Add(new TestQuestions
            {
                QuestionId = question.QuestionId,
                Test = _,
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

    private void TestDataGrid_Edit(object sender, RoutedEventArgs routedEventArgs)
    {
        _currentIndex = TestDataGrid.SelectedIndex;
        TestQuestionsListBox.SelectedItems.Clear();
        var test = _listTest[_currentIndex];
        TestName.Text = test.Title;
        var list = _dbContext.Questions
            .Where(q => q.TestId == test.TestId)
            .ToList();

        list.ForEach(q => { TestQuestionsListBox.SelectedItems.Add(q.Content); });

        ToggleButton();
    }

    private void ButtonUpdate(object sender, RoutedEventArgs e)
    {
        var _ = _dbContext.Tests.Find(_listTest[_currentIndex].TestId);
        if (_ is null)
        {
            MessageBox.Show("Không tìm thấy bộ đề");
            TestQuestionsListBox.SelectedItems.Clear();
            ToggleButton();
            return;
        }

        _.TestQuestions!.Clear();
        foreach (var selectedItem in TestQuestionsListBox.SelectedItems)
        {
            var question = _dbContext.Questions.First(q => q.Content.Equals(selectedItem));
            question.TestId = _.TestId;
            _.TestQuestions.Add(new TestQuestions
            {
                QuestionId = question.QuestionId,
                Question = question,
                TestId = _.TestId,
                Test = _
            });
        }

        _dbContext.SaveChanges();

        TestQuestionsListBox.SelectedItems.Clear();
        TestName.Text = "";
        ToggleButton();
        LoadData();
    }

    private void ToggleButton()
    {
        if (Button.Content.Equals("Thêm bộ đề"))
        {
            Button.Content = "Cập nhật";
            Button.Click -= ButtonAdd;
            Button.Click += ButtonUpdate;
        }
        else
        {
            Button.Content = "Thêm bộ đề";
            Button.Click -= ButtonUpdate;
            Button.Click += ButtonAdd;
        }

        CheckBoxAutoGenerateTest.IsChecked = false;
        NumberOfQuestions.Text = "0";
    }

    private void NumberOfQuestions_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var _ = OnlyNumber();
        e.Handled = _.IsMatch(e.Text);
    }

    private void ButtonExportToWord(object sender, RoutedEventArgs e)
    {
        var _ = (TestDto)TestDataGrid.SelectedItem;
        var questions = _dbContext.TestQuestions
            .Include(ts => ts.Question)
            .ThenInclude(question => question.Options)
            .Where(ts => ts.TestId == _.TestId)
            .ToList();

        var saveDialog = new SaveFileDialog
        {
            Filter = "Word documents (*.docx)|*.docx",
            DefaultExt = ".docx"
        };

        var result = saveDialog.ShowDialog();
        if (result == false) return;

        var index = 1;
        var document = DocX.Create(saveDialog.FileName);

        document.InsertParagraph(_.Title);

        foreach (var ts in questions)
        {
            // Add question content.
            document.InsertParagraph($"Câu {index++}: {ts.Question.Content}");

            if (!string.IsNullOrEmpty(ts.Question.Formula))
            {
                var parser = WpfTeXFormulaParser.Instance;
                var formula = parser.Parse(ts.Question.Formula);
                var pngBytes = formula.RenderToPng(50.0, 0.0, 0.0, "Arial");
                using var stream = new MemoryStream(pngBytes);
                var image = document.AddImage(stream, "image/png");
                var picture = image.CreatePicture();
                document.InsertParagraph().AppendPicture(picture);
            }

            if (!string.IsNullOrEmpty(ts.Question.Image))
            {
                var image = document.AddImage(ts.Question.Image);
                var picture = image.CreatePicture(100, 100);
                document.InsertParagraph().AppendPicture(picture);
            }

            var currentChar = 'A';
            if (ts.Question.Options is null) continue;

            foreach (var o in ts.Question.Options)
            {
                var p = document.InsertParagraph($"{currentChar}: {o.Text}\n");

                if (!string.IsNullOrEmpty(o.Image))
                {
                    var image = document.AddImage(o.Image);
                    var picture = image.CreatePicture(100, 100);
                    p.AppendPicture(picture);
                }

                currentChar++;
            }
        }

        document.Save();
        MessageBox.Show("Đã xuất file thành công");
    }

    private void TestQuestionsListBox_OnSelected(object sender, RoutedEventArgs e)
    {
        NumberOfQuestions.Text = TestQuestionsListBox.SelectedItems.Count.ToString();
    }

    private void View(object sender, RoutedEventArgs e)
    {
        ManagePage.Visibility = Visibility.Hidden;
        TestQuestionPage.Visibility = Visibility.Visible;
        var _ = (TestDto)TestDataGrid.SelectedItem;
        var listQuestion = _dbContext.TestQuestions.Include(question => question.Question)
            .Include(testQuestions => testQuestions.Question.Options).Where(tq => tq.TestId == _.TestId).ToList();
        _questions = new ObservableCollection<Models.Question>(listQuestion
            .Select(q => q.Question)
            .ToList());
        TestLabel.Text = _.Title;
        ListQuestionLabel.Text = "Số câu hỏi trong đề: ";
        Amount.Text = _questions.Count.ToString();
        QuestionsViewBox.ItemsSource = _questions;
    }

    private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;

        var sourceListViewItem = (ListViewItem)sender;
        DragDrop.DoDragDrop(QuestionsViewBox, sourceListViewItem, DragDropEffects.Move);
    }

    private void ListBoxItem_DragEnter(object sender, DragEventArgs e)
    {
        var targetListViewItem = (ListViewItem)sender;
        var sourceListViewItem = (ListViewItem)e.Data!.GetData("System.Windows.Controls.ListViewItem");
        if (sourceListViewItem == targetListViewItem)
            return;
        var targetItem = (Models.Question)targetListViewItem.Content;
        var sourceItem = (Models.Question)sourceListViewItem.Content;

        if (targetItem == null) return;
        var targetItemIndex = _questions.IndexOf(targetItem);

        if (sourceItem == null) return;
        var sourceItemIndex = _questions.IndexOf(sourceItem);

        var topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
        var bottomRectangle =
            (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);

        if (targetItemIndex < sourceItemIndex)
            topRectangle.Visibility = Visibility.Visible;
        else
            bottomRectangle.Visibility = Visibility.Visible;
    }

    private void ListBoxItem_DragLeave(object sender, DragEventArgs e)
    {
        var targetListViewItem = (ListViewItem)sender;
        var topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
        var bottomRectangle = (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);

        topRectangle.Visibility = Visibility.Collapsed;
        bottomRectangle.Visibility = Visibility.Collapsed;
    }

    private void ListBoxItem_Drop(object sender, DragEventArgs e)
    {
        var targetListViewItem = (ListViewItem)sender;
        var sourceListViewItem = (ListViewItem)e.Data.GetData("System.Windows.Controls.ListViewItem");

        var targetItem = (Models.Question)targetListViewItem.Content;
        var sourceItem = (Models.Question)sourceListViewItem.Content;

        var targetItemIndex = _questions.IndexOf(targetItem);
        var sourceItemIndex = _questions.IndexOf(sourceItem);

        var topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
        var bottomRectangle = (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);

        topRectangle.Visibility = Visibility.Collapsed;
        bottomRectangle.Visibility = Visibility.Collapsed;

        _questions.Move(sourceItemIndex, targetItemIndex);
    }

    private void MixQuestions(object sender, RoutedEventArgs e)
    {
        var random = new Random();

        var n = _questions.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (_questions[k], _questions[n]) = (_questions[n], _questions[k]);
        }

        QuestionsViewBox.ItemsSource = _questions;
    }

    private void ExportToWord(object sender, RoutedEventArgs e)
    {
        var questions = _questions.ToList();

        var saveDialog = new SaveFileDialog
        {
            Filter = "Word documents (*.docx)|*.docx",
            DefaultExt = ".docx"
        };

        var result = saveDialog.ShowDialog();
        if (result == false) return;

        var index = 1;
        var document = DocX.Create(saveDialog.FileName);

        document.InsertParagraph(TestLabel.Text);

        foreach (var ts in questions)
        {
            document.InsertParagraph($"Câu {index++}: {ts.Content}");
            if (!string.IsNullOrEmpty(ts.Image))
            {
                var image = document.AddImage(ts.Image);
                var picture = image.CreatePicture(100, 100);
                document.InsertParagraph().AppendPicture(picture);
            }

            var currentChar = 'A';
            if (ts.Options is null) continue;

            foreach (var o in ts.Options)
            {
                var p = document.InsertParagraph($"{currentChar}: {o.Text}\n");

                if (!string.IsNullOrEmpty(o.Image))
                {
                    var image = document.AddImage(o.Image);
                    var picture = image.CreatePicture(100, 100);
                    p.AppendPicture(picture);
                }

                currentChar++;
            }
        }

        document.Save();
        MessageBox.Show("Đã xuất file thành công");
    }

    private void ButtonBack(object sender, RoutedEventArgs e)
    {
        ManagePage.Visibility = Visibility.Visible;
        TestQuestionPage.Visibility = Visibility.Hidden;
    }

    private void CheckBoxAutoGenerateTest_OnChecked(object sender, RoutedEventArgs e)
    {
        NumberOfQuestions.IsEnabled = true;
        TestQuestionsListBox.IsEnabled = false;
        NumberOfQuestions.Text = "0";
    }

    private void CheckBoxAutoGenerateTest_OnUnchecked(object sender, RoutedEventArgs e)
    {
        NumberOfQuestions.IsEnabled = false;
        TestQuestionsListBox.IsEnabled = true;
        NumberOfQuestions.Text = "0";
    }
}