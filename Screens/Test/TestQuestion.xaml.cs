using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Win32;
using TestBuilder.Data;
using Xceed.Words.NET;
using EntityFrameworkQueryableExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace TestBuilder.Screens.Test;

public partial class TestQuestion
{
    private readonly TestDbContext _dbContext = new();
    private readonly TestDto _test;
    private ObservableCollection<Models.Question> _listQuestion = new();

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
                testQuestions => testQuestions.Question.Options).ToList();
        _listQuestion = new ObservableCollection<Models.Question>(listQuestion
            .Where(q => q.TestId == _test.TestId)
            .Select(q => q.Question)
            .ToList());
        ListQuestionLabel.Text = "Số câu hỏi: ";
        Amount.Text = _listQuestion.Count.ToString();
        TestQuestionsListBox.ItemsSource = _listQuestion;
    }

    private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            ListViewItem sourceListViewItem = (ListViewItem)sender;
            DragDrop.DoDragDrop(TestQuestionsListBox, sourceListViewItem, DragDropEffects.Move);
        }
    }

    private void ListBoxItem_DragEnter(object sender, DragEventArgs e)
    {
        ListViewItem targetListViewItem = (ListViewItem)sender;
        ListViewItem sourceListViewItem = (ListViewItem)e.Data!.GetData("System.Windows.Controls.ListViewItem");
        if (sourceListViewItem == targetListViewItem)
            return;
        var targetItem = (Models.Question)targetListViewItem.Content;
        var sourceItem = (Models.Question)sourceListViewItem.Content;

        if (targetItem != null)
        {
            var targetItemIndex = _listQuestion.IndexOf(targetItem);
            if (sourceItem != null)
            {
                var sourceItemIndex = _listQuestion.IndexOf(sourceItem);

                Rectangle topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
                Rectangle bottomRectangle = (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);
            
                if (targetItemIndex < sourceItemIndex)
                {
                    topRectangle.Visibility = Visibility.Visible;
                }
                else
                {
                    bottomRectangle.Visibility = Visibility.Visible;
                }
            }
        }
    }

    private void ListBoxItem_DragLeave(object sender, DragEventArgs e)
    {
        ListViewItem targetListViewItem = (ListViewItem)sender;
        Rectangle topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
        Rectangle bottomRectangle = (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);

        topRectangle.Visibility = Visibility.Collapsed;
        bottomRectangle.Visibility = Visibility.Collapsed;
    }

    private void ListBoxItem_Drop(object sender, DragEventArgs e) 
    {
        ListViewItem targetListViewItem = (ListViewItem)sender;
        ListViewItem sourceListViewItem = (ListViewItem)e.Data!.GetData("System.Windows.Controls.ListViewItem");
            
        Models.Question targetItem = (Models.Question)targetListViewItem.Content;
        Models.Question sourceItem = (Models.Question)sourceListViewItem.Content;
            
        var targetItemIndex = _listQuestion.IndexOf(targetItem);
        var sourceItemIndex = _listQuestion.IndexOf(sourceItem);

        Rectangle topRectangle = (Rectangle)targetListViewItem.Template.FindName("TopRectangle", targetListViewItem);
        Rectangle bottomRectangle = (Rectangle)targetListViewItem.Template.FindName("BottomRectangle", targetListViewItem);
            
        topRectangle.Visibility = Visibility.Collapsed;
        bottomRectangle.Visibility = Visibility.Collapsed;
            
        _listQuestion.Move(sourceItemIndex, targetItemIndex);
    }

    private void MixQuestions(object sender, RoutedEventArgs e)
    {
        Random random = new Random();

        int n = _listQuestion.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (_listQuestion[k], _listQuestion[n]) = (_listQuestion[n], _listQuestion[k]);
        }
        TestQuestionsListBox.ItemsSource = _listQuestion;   
    }
    
    private void ExportToWord(object sender, RoutedEventArgs e)
    {
        var questions = _listQuestion.ToList();

        var saveDialog = new SaveFileDialog
        {
            Filter = "Word documents (*.docx)|*.docx",
            DefaultExt = ".docx"
        };

        var result = saveDialog.ShowDialog();
        if (result == false) return;

        var index = 1;
        var document = DocX.Create(saveDialog.FileName);

        document.InsertParagraph(_test.Title);

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
}